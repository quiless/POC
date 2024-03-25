using System;
using System.Xml.Linq;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.SQL.Transactions.Interfaces;
using POC.Domain.Commands.Order;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Context;
using POC.Domain.Models.Entities;
using POC.Domain.Models.Enumerations;
using POC.Domain.Models.ModelSettings;
using POC.Domain.Queries.Deliverymans;
using POC.Infrastructure.SQLRepositories.Interfaces.Deliveryman;
using POC.Infrastructure.SQLRepositories.Interfaces.Order;
using POC.Infrastructure.SQLRepositories.Interfaces.RentMotorcycle;
using POC.Infrastructure.SQLRepositories.Order;
using PubnubApi;

namespace POC.Domain.CommandsHandlers
{
    public class OrderCommandHandler :
        IRequestHandler<RegisterOrderCommand, GenericCommandResult>,
        INotificationHandler<NotifyDelivermansCommand>,
        INotificationHandler<NotifyDeliveriesPendingOrdersCommand>,
        IRequestHandler<OrderAcceptedCommand, GenericCommandResult>,
        IRequestHandler<OrderFinishedCommand, GenericCommandResult>
    {
        PNConfiguration pnConfiguration;
        Pubnub pubnub;
        private PubnubSetting _pubNubSetting;
        private ApplicationContextBase _applicationContextBase;
        private IUnitOfWork _transaction;
        private IDomainNotificationContext _notifications;
        private IRentMotorcycleRepository _rentMotorcycleRepository;
        private IMediator _mediator;
        private IDeliverymanRepository _deliverymanRepository;
        private IOrderRepository _orderRepository;
        private IOrderNotificationRepository _orderNotificationRepository;
        private TelemetryClient _telemetryClient;

        public OrderCommandHandler(
            IOptions<PubnubSetting> pubNubSettings,
            IUnitOfWork transaction,
            ApplicationContextBase applicationContextBase,
            IDomainNotificationContext notifications,
            IOrderRepository orderRepository,
            IDeliverymanRepository deliverymanRepository,
            IRentMotorcycleRepository rentMotorcycleRepository,
            IOrderNotificationRepository orderNotificationRepository,
            IMediator mediator,
            TelemetryClient telemetryClient)
		{
            #region Pubnub

            this._pubNubSetting = pubNubSettings.Value;
            var user = new UserId(this._pubNubSetting.ApplicationKey);
            this.pnConfiguration = new PNConfiguration(user);
            pnConfiguration.SubscribeKey = this._pubNubSetting.SubscribeKey;
            pnConfiguration.PublishKey = this._pubNubSetting.PublishKey;
            pubnub = new Pubnub(pnConfiguration);


            #endregion

            _applicationContextBase = applicationContextBase;
            _orderNotificationRepository = orderNotificationRepository;
            _mediator = mediator;
            _transaction = transaction;
            _orderRepository = orderRepository;
            _telemetryClient = telemetryClient;
            _notifications = notifications;
            _rentMotorcycleRepository = rentMotorcycleRepository;
            _deliverymanRepository = deliverymanRepository;

        }

        public async Task<GenericCommandResult> Handle(RegisterOrderCommand request, CancellationToken cancellationToken)
        {
            Order _newOrder = new Order(request.OrderDate, request.Price, _applicationContextBase.CustomIdentity.UserId);

            await _transaction.ScopeAsync(async (Commit) =>
            {
                await this._orderRepository.InsertAsync(_newOrder);

                await _mediator.Publish(new NotifyDelivermansCommand(_newOrder.Id, _newOrder.Price, _newOrder.OrderDate));

                Commit();
            });

            return new GenericCommandResult(true, "Pedido registrado com sucesso. Assim que definirmos o entregador, iremos lhe notificar.", _newOrder);

        }

        public async Task Handle(NotifyDelivermansCommand notification, CancellationToken cancellationToken)
        {

            //Atualmente, a única regra aplicada é: motorista está disponível
            //TODO: O motorista pode ter um notificação em seu dispositivo, e isso não está sendo controlado nesse estágio.
            //TODO: Ideial: controlar motorista que estão com notificações pendente, para que não sejam sobrepostas.
            IEnumerable<DeliverymanAvailable> _deliverymansAvailable = await _mediator.Send(new GetDeliverymansAvailableQuery(notification.OrderDate));

            if (_deliverymansAvailable == null || _deliverymansAvailable.Count() == 0)
            {
                //Registra notificação no telemetry para acompanhamento.
                //Cenário ideial seria criar um painel, enviar alguma notificação para o time acompanhar e metrificar corridas que não possuímos entregadores disponíveis, que pode resultar em planos de ação para capturar entregadores na região.
                //Atualmente, como não há o endereço do local, não é possível metrificar os motivos de entregadores não disponíveis.

                _telemetryClient.TrackTrace(
                    new TraceTelemetry(
                        Newtonsoft.Json.JsonConvert.SerializeObject(
                            new { orderId = notification.OrderId, orderDate = notification.OrderDate, message = "Não foram encontrados motoristas disponíveis para serem notificados da entrega." }),
                        SeverityLevel.Critical)
                    );
            }
            else
            {
                foreach (var _deliveryman in _deliverymansAvailable)
                {
                    await _transaction.ScopeAsync(async (Commit) =>
                    {
                        OrderNotification _orderNotification = new OrderNotification()
                        {
                            CreateDate = DateTime.Now,
                            OrderId = notification.OrderId,
                            MotorcycleRentalId = _deliveryman.MotorcycleRentalId
                        };

                        await this._orderNotificationRepository.InsertAsync(_orderNotification);

                        var channel = $"{Domain.Models.Environments.Pubnub.Channels.DELIVERYMAN_CHANNEL}{ _deliveryman.DeliverymanUniqueId.ToString()}";
                        var publishResponse = await pubnub.Publish()
                            .Message(JsonConvert.SerializeObject(new {
                                OrderId = notification.OrderId,
                                OrderDate = notification.OrderDate,
                                Price = notification.Price,
                                NotificiationId = _orderNotification.Id
                            }))
                            .Channel(channel)
                            .ExecuteAsync();

                        PNPublishResult publishResult = publishResponse.Result;
                        PNStatus status = publishResponse.Status;

                        if (!status.Error)
                            Commit();
                    });
     
                }
            }
        }

        public async Task Handle(NotifyDeliveriesPendingOrdersCommand notification, CancellationToken cancellationToken)
        {
            IEnumerable<Order> _pendingOrders = await this._orderRepository.GetAvailableOrders();

            if (_pendingOrders.Any())
            {
                foreach (var _order in _pendingOrders)
                {
                    await _mediator.Publish(new NotifyDelivermansCommand(_order.Id, _order.Price, _order.OrderDate));
                }
            }
        }

        public async Task<GenericCommandResult> Handle(OrderAcceptedCommand request, CancellationToken cancellationToken)
        {
            var _order = this._orderRepository.GetById(request.OrderId);

            if (_order == null || _order.Id <= 0)
            {
                _notifications.NotifyError("Pedido inválido.");
                return new GenericCommandResult(false);
            }

            if (_order.OrderStatusId != OrderStatusEnum.Available.Id)
            {
                _notifications.NotifyError("O pedido não está disponível.");
                return new GenericCommandResult();
            }


            Deliveryman _deliveryman = await this._deliverymanRepository.GetDeliverymanLoggedByUserId(_applicationContextBase.CustomIdentity.UserId);


            if (_deliveryman == null || _deliveryman.Id <= 0)
            {
                _notifications.NotifyError("Não conseguimos obter os seus dados para alocar o pedido. Realize o login novamente.");
                return new GenericCommandResult();
            }


            if (!_deliveryman.HasOpenRent)
            {
                _notifications.NotifyError("Você não possuí locação ativa no momento. Não é possível aceitar o pedido.");
                return new GenericCommandResult();
            }

            MotorcycleRental _motorcycleRental = await this._rentMotorcycleRepository.GetMotorcycleRentalOpenByUserId(_applicationContextBase.CustomIdentity.UserId);


            if (_motorcycleRental == null || _motorcycleRental.Id <= 0)
            {
                _notifications.NotifyError("Não conseguimos obter os dados de sua alocação. Realize o login novamente.");
                return new GenericCommandResult();
            }


            if (_motorcycleRental.HasOpenOrder)
            {
                _notifications.NotifyError("Você pode realizar apenas um pedido por vez. Finalize o pedido atual antes de aceitar novos pedidos.");
                return new GenericCommandResult();
            }


            if (!await this._orderNotificationRepository.ValidateOrderNotification(_motorcycleRental.Id, request.OrderId))
            {
                _notifications.NotifyError("Você não possuí permissão para aceitar esse pedido.");
                return new GenericCommandResult();
            }

            await _transaction.ScopeAsync(async (Commit) =>
            {
                await _orderRepository.UpdateOrderAccepted(request.OrderId, _motorcycleRental.Id);

                await _rentMotorcycleRepository.UpdateMotorcycleRentalUnavaliable(_motorcycleRental.Id);

                Commit();
            });

            return new GenericCommandResult(true, "O pedido foi adicionada em sua rota!", _order);
        }

        public async Task<GenericCommandResult> Handle(OrderFinishedCommand request, CancellationToken cancellationToken)
        {
            var _order = this._orderRepository.GetById(request.OrderId);

            if (_order == null || _order.Id <= 0)
            {
                _notifications.NotifyError("Pedido inválido.");
                return new GenericCommandResult(false);
            }

            if (_order.OrderStatusId != OrderStatusEnum.Accepted.Id)
            {
                _notifications.NotifyError("O pedido não está em andamento.");
                return new GenericCommandResult();
            }


            Deliveryman _deliveryman = await this._deliverymanRepository.GetDeliverymanLoggedByUserId(_applicationContextBase.CustomIdentity.UserId);


            if (_deliveryman == null || _deliveryman.Id <= 0)
            {
                _notifications.NotifyError("Não conseguimos obter os seus dados para finalizar o pedido. Realize o login novamente.");
                return new GenericCommandResult();
            }


            MotorcycleRental _motorcycleRental = await this._rentMotorcycleRepository.GetMotorcycleRentalWithOpenOrderByUserId(_applicationContextBase.CustomIdentity.UserId);


            if (_motorcycleRental == null || _motorcycleRental.Id <= 0)
            {
                _notifications.NotifyError("Não conseguimos obter os seus dados para finalizar o pedido. Realize o login novamente.");
                return new GenericCommandResult();
            }

            if (_motorcycleRental.DeliverymanId != _deliveryman.Id)
            {
                _notifications.NotifyError("Desculpe, mas o pedido não está destinado a você.");
                return new GenericCommandResult();
            }


            await _transaction.ScopeAsync(async (Commit) =>
            {
                await _orderRepository.UpdateOrderFinished(request.OrderId);

                await _rentMotorcycleRepository.UpdateMotorcycleRentalAvaliable(_motorcycleRental.Id);

                await _rentMotorcycleRepository.SumTotalAmount(_order.Price, _motorcycleRental.Id);
             
                Commit();
            });

            return new GenericCommandResult(true, "Pedido finalizado!", _order);
        }
    }
}


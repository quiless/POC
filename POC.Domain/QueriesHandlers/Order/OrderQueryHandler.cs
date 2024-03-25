using System;
using MediatR;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Models;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Models.Entities;
using POC.Domain.Queries.Order;
using POC.Infrastructure.SQLRepositories.Interfaces.Order;

namespace POC.Domain.QueriesHandlers.Order
{
    public class OrderQueryHandler :
        IRequestHandler<SearchOrdersQuery, GenericCommandResult>,
        IRequestHandler<GetOrderNotificationsQuery, GenericCommandResult>
	{

        private IOrderRepository _orderRepository;
        private IOrderNotificationRepository _orderNotificationRepository;
        private IDomainNotificationContext _notifications;

        public OrderQueryHandler(
            IOrderRepository orderRepository,
            IDomainNotificationContext notifications,
            IOrderNotificationRepository orderNotificationRepository)
        {
            _orderRepository = orderRepository;
            _notifications = notifications;
            _orderNotificationRepository = orderNotificationRepository;
        }


        public async Task<GenericCommandResult> Handle(SearchOrdersQuery request, CancellationToken cancellationToken)
        {
            return new GenericCommandResult(
                true,
                "Busca realizada com sucesso!",
                await _orderRepository.GetOrders(request.OrderStatusId, request.CNPJ, request.StartDate, request.EndDate));
        }

        public async Task<GenericCommandResult> Handle(GetOrderNotificationsQuery request, CancellationToken cancellationToken)
        {

            Domain.Models.Entities.Order _order = _orderRepository.GetById(request.OrderId);

            if (_order == null || _order.Id <= 0)
            {
                _notifications.NotifyError($"O pedido {request.OrderId} não está registrado.");
                return new GenericCommandResult(false);
            }

            return new GenericCommandResult(
                true,
                "Busca realizada com sucesso!",
                await _orderNotificationRepository.GetNotificationsSent(request.OrderId));
        }
    }
}


using System;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Queries;
using POC.Domain.Queries.Deliverymans;
using POC.Domain.Models.Context;
using POC.Infrastructure.SQLRepositories.Interfaces;
using POC.Infrastructure.SQLRepositories.Interfaces.Deliveryman;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.AzureStorage.Inferfaces;
using POC.Domain.Models.Entities;
using POC.Domain.Models.Aggregators;

namespace POC.Domain.QueriesHandlers.Deliverymans
{
	public class DeliverymanQueryHandler:
		IRequestHandler<GetDriverLicenseTypeQuery, GenericCommandResult>,
        INotificationHandler<CheckDeliverymanByCNPJQuery>,
        INotificationHandler<CheckDeliverymanByDriverLicenseNumberQuery>,
        IRequestHandler<GeDeliverymanDriverLicenseFileQuery, GenericCommandResult>,
        IRequestHandler<GetDeliverymanInfoQuery, GenericCommandResult>,
        IRequestHandler<GetDeliverymansAvailableQuery, IEnumerable<DeliverymanAvailable>>
    {

        private IDriverLicenseRepository _driverLicenseRepository;
        private IDeliverymanRepository _deliverymanRepository;
        private IDomainNotificationContext _notifications;
        private readonly ApplicationContextBase _applicationContext;
        private IBlob _azureBlobService;

        public DeliverymanQueryHandler(
            IDriverLicenseRepository driverLicenseRepository,
            IDeliverymanRepository deliverymanRepository,
            IBlob azureBlobService,
            IDomainNotificationContext notifications,
            ApplicationContextBase applicationContext)
		{
			_driverLicenseRepository = driverLicenseRepository;
			_applicationContext = applicationContext;
            _deliverymanRepository = deliverymanRepository;
            _notifications = notifications;
            _azureBlobService = azureBlobService;
		}

        public async Task<GenericCommandResult> Handle(GetDriverLicenseTypeQuery request, CancellationToken cancellationToken)
        {
			return new GenericCommandResult(true, "Busca realizada com sucesso.", await this._driverLicenseRepository.GetAsync());

        }

        public async Task Handle(CheckDeliverymanByCNPJQuery notification, CancellationToken cancellationToken)
        {
            var _deliveryman = await this._deliverymanRepository.GetDeliverymanByCNPJ(notification.CNPJ);

            if (_deliveryman != null && _deliveryman.Id > 0)
                _notifications.NotifyError($"O CNPJ {notification.CNPJ} já está registrado em nossa base de dados.");

           
        }

        public async Task Handle(CheckDeliverymanByDriverLicenseNumberQuery notification, CancellationToken cancellationToken)
        {
            var _deliveryman = await this._deliverymanRepository.GetDeliverymanByDriverLicenseNumber(notification.DriverLicenseNumber);

            if (_deliveryman != null && _deliveryman.Id > 0)
                _notifications.NotifyError($"O número da carteira de motorista {notification.DriverLicenseNumber} já está registrado em nossa base de dados.");
        }

        public async Task<GenericCommandResult> Handle(Queries.Deliverymans.GeDeliverymanDriverLicenseFileQuery request, CancellationToken cancellationToken)
        {
            var _deliveryman = _deliverymanRepository.GetById(request.DeliverymanId);

            //Verifica se o entregar logado é o mesmo da solicitação da Request, ou o usuário é um administrador da plataforma
            if (_deliveryman.UserId != _applicationContext.CustomIdentity.UserId && !_applicationContext.CustomIdentity.IsAdmin)
            {
                _notifications.NotifyError("Você não possuí autorização para realizar essa ação.");
                return new GenericCommandResult();
            }

            return new GenericCommandResult(true, String.Empty, _azureBlobService.GenerateTempLinkToDownload(_deliveryman.DriverLicenseFilename));
        }

        public async Task<GenericCommandResult> Handle(GetDeliverymanInfoQuery request, CancellationToken cancellationToken)
        {
            // *** Usuários administradores podem não ser entregadores ***.


            Domain.Models.Entities.Deliveryman _loggedDeliveryman = await _deliverymanRepository.GetDeliverymanLoggedByUserId(_applicationContext.CustomIdentity.UserId);

            //Verifica se o entregar logado é o mesmo da solicitação da Request, ou o usuário é um administrador da plataforma
            if (_loggedDeliveryman.UserId != _applicationContext.CustomIdentity.UserId && !_applicationContext.CustomIdentity.IsAdmin)
            {
                _notifications.NotifyError("Você não possuí autorização para realizar essa ação.");
                return new GenericCommandResult();
            }

            return new GenericCommandResult(true, "Busca realizada com sucesso!", _loggedDeliveryman);
        }

        public async Task<IEnumerable<DeliverymanAvailable>> Handle(GetDeliverymansAvailableQuery request, CancellationToken cancellationToken)
        {
            return await _deliverymanRepository.GetDeliverymansAvailableOnDate(request.OrderDate);
        }
    }
}


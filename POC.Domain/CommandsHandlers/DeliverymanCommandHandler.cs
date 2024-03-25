using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using POC.Artifacts.AzureStorage.Inferfaces;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.SQL.Transactions.Interfaces;
using POC.Domain.Commands.Deliveryman;
using POC.Domain.Models;
using POC.Domain.Models.Entities;
using POC.Domain.Queries.Deliverymans;
using POC.Infrastructure.SQLRepositories.Interfaces;
using POC.Artifacts.Helpers;
using POC.Infrastructure.SQLRepositories.Interfaces.Deliveryman;
using POC.Domain.Models.Context;

namespace POC.Domain.CommandsHandlers
{
	public class DeliverymanCommandHandler:
        IRequestHandler<RegisterDeliverymanCommand, GenericCommandResult>,
        IRequestHandler<RefreshDeliverymanDriverLicenseFileCommand, GenericCommandResult>,
        INotificationHandler<SetDeliverymanOpenRentCommand>,
        INotificationHandler<SetDeliverymanCloseRentCommand>
    {
        private readonly IBlob _azureBlobService;
        private IMediator _mediator;
        private IDomainNotificationContext _notifications;
        private IUnitOfWork _transaction;
        private IDeliverymanRepository _deliverymanRepository;
        private IUserRepository _userRepository;
        private ApplicationContextBase _applicationContextBase;

        public DeliverymanCommandHandler(
            IBlob azureBlobService,
            IUnitOfWork transaction,
            IUserRepository userRepository,
            IDeliverymanRepository deliverymanRepository,
            ApplicationContextBase applicationContextBase,
            IDomainNotificationContext notifications,
            IMediator mediator)
        {
            _azureBlobService = azureBlobService;
            _mediator = mediator;
            _deliverymanRepository = deliverymanRepository;
            _notifications = notifications;
            _transaction = transaction;
            _userRepository = userRepository;
            _applicationContextBase = applicationContextBase;
        }

        public async Task<GenericCommandResult> Handle(RegisterDeliverymanCommand request, CancellationToken cancellationToken)
        {

          
            //Verifica se o CNPJ já está cadastrado na base de dados
            await _mediator.Publish(new CheckDeliverymanByCNPJQuery(request.CNPJ));

            //Verifica se o prestador de serviço já está cadastrado na base de dados
            await _mediator.Publish(new CheckDeliverymanByDriverLicenseNumberQuery(request.DriverLicenseNumber));

            if (_notifications.HasErrorNotifications)
            {
                return new GenericCommandResult(false);
            }


            var sharedTempFileName = $"Foto_Perfil_{request.CNPJ}";
            var refBlobFileName = Guid.NewGuid() + System.IO.Path.GetExtension(request.DriverLicenseFile.FileName);
            var tempPathFile = Path.Combine(Path.GetTempPath().ToString().Trim(), $"{request.DriverLicenseFile.FileName}");

            using (var fileStream = new FileStream(tempPathFile, FileMode.Create))
            {
                await request.DriverLicenseFile.CopyToAsync(fileStream);
            }

            await _azureBlobService.UploadFromFile(refBlobFileName, tempPathFile);

            var _oldPassword = request.Password;

            UserInfo _user = new UserInfo(request.CNPJ, request.Password, false);
    

            try
            {
                await _transaction.ScopeAsync(async (Commit) =>
                {
                    await this._userRepository.InsertAsync(_user);

                    Deliveryman _deliveryman = new Deliveryman(
                                        request.Name,
                                        request.CNPJ,
                                        request.BirthDate,
                                        request.DriverLicenseNumber,
                                        request.DriveLicenseTypeId,
                                        refBlobFileName,
                                        _user.Id,
                                        request.Identifier);

                    await this._deliverymanRepository.InsertAsync(_deliveryman);

                    Commit();
                });
            }
            catch (Exception ex)
            {
                await _azureBlobService.Delete(refBlobFileName);
                throw new Exception(ex.Message);
            }

            _user.Password = String.Empty;

            return new GenericCommandResult(true, "Cadastro realizado com sucesso! Você já pode fazer o seu login para alugar uma moto e receber serviços!", _user);
        }

        public async Task<GenericCommandResult> Handle(RefreshDeliverymanDriverLicenseFileCommand request, CancellationToken cancellationToken)
        {

            var _deliveryman = _deliverymanRepository.GetById(request.DeliverymanId);

            if (_deliveryman == null || _deliveryman.Id == 0)
            {
                _notifications.NotifyError("Não conseguimos obter os seus dados para realizar a troca da imagem da CNH. Realize o login novamente.");
                return new GenericCommandResult();
            }
           
            //Verifica se o entregar logado é o mesmo da solicitação da Request, ou o usuário é um administrador da plataforma
            if (_deliveryman.UserId != _applicationContextBase.CustomIdentity.UserId && !_applicationContextBase.CustomIdentity.IsAdmin)
            {
                _notifications.NotifyError("Você não possuí autorização para realizar essa ação.");
                return new GenericCommandResult();
            }


            var sharedTempFileName = $"Foto_Perfil_{_deliveryman.CNPJ}";
            var refBlobFileName = Guid.NewGuid() + System.IO.Path.GetExtension(request.DriverLicenseFile.FileName);
            var tempPathFile = Path.Combine(Path.GetTempPath().ToString().Trim(), $"{request.DriverLicenseFile.FileName}");

            using (var fileStream = new FileStream(tempPathFile, FileMode.Create))
            {
                await request.DriverLicenseFile.CopyToAsync(fileStream);
            }

            await _azureBlobService.UploadFromFile(refBlobFileName, tempPathFile);

            await _deliverymanRepository.RefreshDeliverymanDriverLicenseFile(refBlobFileName, request.DeliverymanId);

            //Remove a imagem antiga do blob
            await _azureBlobService.Delete(_deliveryman.DriverLicenseFilename);

            _deliveryman.DriverLicenseFilename = refBlobFileName;

            return new GenericCommandResult(true, "Imagem atualizada com sucesso!", _deliveryman);
        }

        public async Task Handle(SetDeliverymanOpenRentCommand notification, CancellationToken cancellationToken)
        {
            await this._deliverymanRepository.UpdateDeliverymanOpenRent(notification.DeliverymanId);
        }

        public async Task Handle(SetDeliverymanCloseRentCommand notification, CancellationToken cancellationToken)
        {
            await this._deliverymanRepository.UpdateDeliverymanCloseRent(notification.DeliverymanId);
        }
    }
}


using System;
using MediatR;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.SQL.Transactions.Interfaces;
using POC.Domain.Commands.Deliveryman;
using POC.Domain.Commands.Rental;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Context;
using POC.Domain.Models.Entities;
using POC.Domain.Models.Enumerations;
using POC.Domain.Queries.Deliverymans;
using POC.Infrastructure.SQLRepositories.Interfaces.Deliveryman;
using POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle;
using POC.Infrastructure.SQLRepositories.Interfaces.RentMotorcycle;
using POC.Infrastructure.SQLRepositories.RentMotorcycle;

namespace POC.Domain.CommandsHandlers
{
    public class RentMotorcycleCommandHandler :
        IRequestHandler<RentMotorcycleCommand, GenericCommandResult>,
        IRequestHandler<FinalizedRentMotorcycleCommand, GenericCommandResult>
    {
        private IMotorcycleRepository _motorcycleRepository;
        private IDomainNotificationContext _notifications;
        private IDeliverymanRepository _deliverymanRepository;
        private IMediator _mediator;
        private IUnitOfWork _transaction;
        private IRentMotorcycleRepository _motorcycleRentRepository;
        private ApplicationContextBase _applicationContextBase;

        public RentMotorcycleCommandHandler(
            IMotorcycleRepository motorcycleRepository,
            IDeliverymanRepository deliverymanRepository,
            IRentMotorcycleRepository motorcycleRentRepository,
            ApplicationContextBase applicationContextBase,
            IUnitOfWork transaction,
            IMediator mediator,
            IDomainNotificationContext notifications)
        {
            _motorcycleRepository = motorcycleRepository;
            _motorcycleRentRepository = motorcycleRentRepository;
            _notifications = notifications;
            _applicationContextBase = applicationContextBase;
            _mediator = mediator;
            _transaction = transaction;
            _deliverymanRepository = deliverymanRepository;
        }

        public async Task<GenericCommandResult> Handle(RentMotorcycleCommand request, CancellationToken cancellationToken)
        {

            var _getDeliverymanInfoQuery = await _mediator.Send(new GetDeliverymanInfoQuery());

            Domain.Models.Entities.Deliveryman _delivermanInfoLogged = (Domain.Models.Entities.Deliveryman)_getDeliverymanInfoQuery.Data;


            //Validar motorista encontrado
            if (_delivermanInfoLogged == null || _delivermanInfoLogged.Id == 0)
            {
                _notifications.NotifyError("Não conseguimos lhe identificar. Realize o login novamente.");
                return new GenericCommandResult(false);
            }


            //Validar se o motorista já possuí locação em andamento
            if (_delivermanInfoLogged.HasOpenRent)
            {
                _notifications.NotifyError("Você já possuí uma locação em andamento. Você pode alugar apenas uma moto. Para realizar uma nova locação, encerre a locação anterior.");
                return new GenericCommandResult(false);
            }

            //Validar carteira de motorista para locação
            if (_delivermanInfoLogged.DriverLicenseTypeId != DriverLicenseTypeEnum.A.Id &&
                _delivermanInfoLogged.DriverLicenseTypeId != DriverLicenseTypeEnum.AB.Id)
            {
                _notifications.NotifyError("Seu tipo de licensa de motorista não permite que você alugue motos.");
                return new GenericCommandResult(false);
            }


            var _motorcycleRentalPlan = await this._motorcycleRentRepository.GetActiveMotorcycleRentalPlanById(request.MotorcycleRentalPlanId);

            if (_motorcycleRentalPlan == null || _motorcycleRentalPlan.Id <= 0)
            {
                _notifications.NotifyError("O plano de locação escolhido está indisponível.");
                return new GenericCommandResult(false);
            }

            //Busca uma moto disponível. Defini que a moto alocado será escolhida de forma randômica.
            //Outra maneira, seria que o entregador pudesse filtrar motos pelo ano, modelo, marca e escolhesse a moto desejada.
            var _motorcyleToRent = await _motorcycleRepository.GetMotorcycleToRent();

            if (_motorcyleToRent == null || _motorcyleToRent.Id <= 0)
            {
                _notifications.NotifyError("Não há motos disponíveis para locação.");
                return new GenericCommandResult(false);
            }


            MotorcycleRental _rent = new MotorcycleRental(_motorcycleRentalPlan.TotalDays, _delivermanInfoLogged.Id, _motorcyleToRent.Id, _motorcycleRentalPlan.Id);

            try
            {

                await _transaction.ScopeAsync(async (Commit) =>
                {
                    await this._motorcycleRentRepository.RegisterRent(_rent);
                    await _mediator.Publish(new SetDeliverymanOpenRentCommand(_rent.DeliverymanId));

                    if (_notifications.HasErrorNotifications)
                    {
                        await this._motorcycleRepository.UpdateMotorcycleToAvailable(_motorcyleToRent.Id);
                        await _mediator.Publish(new SetDeliverymanCloseRentCommand(_rent.DeliverymanId));
                    }
                    else
                    {
                        Commit();
                    }
                });

            }
            catch (Exception ex)
            {
                await this._motorcycleRepository.UpdateMotorcycleToAvailable(_motorcyleToRent.Id);
                throw new Exception(ex.Message);
            }

            if (_notifications.HasErrorNotifications)
                return new GenericCommandResult(false);

            _motorcyleToRent.StartDate = _rent.StartDate;
            _motorcyleToRent.ExpectedEndDate = _rent.ExpectedEndDate;

            return new GenericCommandResult(true, "Locação realizada com sucesso.", _motorcyleToRent);

        }

        public async Task<GenericCommandResult> Handle(FinalizedRentMotorcycleCommand request, CancellationToken cancellationToken)
        {
            MotorcycleRental _motorcycleRental = await this._motorcycleRentRepository.GetMotorcycleRentalOpenByUserId(_applicationContextBase.CustomIdentity.UserId);

            if (_motorcycleRental == null || _motorcycleRental.Id <= 0)
            {
                _notifications.NotifyError("Você não possuí locação aberta.");
                return new GenericCommandResult();
            }

            if (_motorcycleRental.HasOpenOrder)
            {
                _notifications.NotifyError("Você possuí um pedido em andamento.Para encerrar a locação, você precisa finalizar o pedido.");
                return new GenericCommandResult();
            }


            var _motorcycleRentalPlan = await this._motorcycleRentRepository.GetMotorcycleRentalPlanById(_motorcycleRental.MotorcycleRentalPlanId);


            try
            {
                CheckMotorcycleRentPrice _calculatedValues = new CheckMotorcycleRentPrice(
                                                                    _motorcycleRental.StartDate,
                                                                    _motorcycleRental.ExpectedEndDate,
                                                                    DateTime.Now,
                                                                    _motorcycleRentalPlan.TotalDays,
                                                                    _motorcycleRentalPlan.DailyPrice,
                                                                    _motorcycleRentalPlan.AdditionalDailyPrice,
                                                                    _motorcycleRentalPlan.PercentageDailyNotEffective);

                await _transaction.ScopeAsync(async (Commit) =>
                {
                    await this._motorcycleRentRepository.UpdateMotorcycleRentalFinalized(_motorcycleRental.Id, _calculatedValues.TotalValue, _calculatedValues.HasFine);
                    await this._deliverymanRepository.UpdateDeliverymanCloseRent(_motorcycleRental.DeliverymanId);
                    await this._motorcycleRepository.UpdateMotorcycleToAvailable(_motorcycleRental.MotorcycleId);

                    Commit();

                });

            }
            catch (Exception ex)
            {
                _notifications.NotifyError("Não foi possível finalizar a locação. Estamos corrigindo o problema.");
                throw new Exception(ex.Message);
            }


            return new GenericCommandResult(true, "Locação finalizada com sucesso!", _motorcycleRental);
        }
    }
   
}


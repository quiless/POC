using System;
using MediatR;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Enumerations;
using POC.Domain.Queries.Deliverymans;
using POC.Domain.Queries.Rental;
using POC.Infrastructure.SQLRepositories.Interfaces.RentMotorcycle;

namespace POC.Domain.QueriesHandlers.Rental
{
	public class RentMotorcycleQueryHandler:
		IRequestHandler<CheckRentalPriceQuery, GenericCommandResult>,
        IRequestHandler<GetRentalMotorcyclePlansQuery, GenericCommandResult>
	{
		private IRentMotorcycleRepository _rentMotorcycleRepository;
        private IMediator _mediator;
        private IDomainNotificationContext _notifications;

		public RentMotorcycleQueryHandler(
            IRentMotorcycleRepository rentMotorcycleRepository,
            IDomainNotificationContext notifications,
            IMediator mediator)
		{
			_rentMotorcycleRepository = rentMotorcycleRepository;
            _mediator = mediator;
            _notifications = notifications;
		}



        public async Task<GenericCommandResult> Handle(CheckRentalPriceQuery request, CancellationToken cancellationToken)
        {

            var _getDeliverymanInfoQuery = await _mediator.Send(new GetDeliverymanInfoQuery());

            Domain.Models.Entities.Deliveryman _delivermanInfoLogged = (Domain.Models.Entities.Deliveryman)_getDeliverymanInfoQuery.Data;

            //Validar motorista encontrado
            if (_delivermanInfoLogged == null || _delivermanInfoLogged.Id == 0)
            {
                _notifications.NotifyError("Não conseguimos lhe identificar. Realize o login novamente.");
                return new GenericCommandResult(false);
            }

            //Validar se o motorista possuí local em aberto
            if (!_delivermanInfoLogged.HasOpenRent)
            {
                _notifications.NotifyError("Você não possuí locações em aberto.");
                return new GenericCommandResult(false);
            }

            var _activeRentMotorcycle = await _rentMotorcycleRepository.GetActiveRentMotorcycleUserLogged(_delivermanInfoLogged.Id);

            var _motorcycleRentalPlan = await this._rentMotorcycleRepository.GetMotorcycleRentalPlanById(_activeRentMotorcycle.MotorcycleRentalPlanId);

            if (_motorcycleRentalPlan == null || _motorcycleRentalPlan.Id <= 0)
            {
                _notifications.NotifyError("O plano de locação escolhido está indisponível.");
                return new GenericCommandResult(false);
            }

            try
            {
                //Visto que a data da locação é o dia seguinte a data de criação, caso o entregar consulte o valor no mesmo dia entrega, irei considerar o valor de multa no período total de locação; 
                CheckMotorcycleRentPrice _calculatedValues = new CheckMotorcycleRentPrice(
                                                                    _activeRentMotorcycle.StartDate,
                                                                    _activeRentMotorcycle.ExpectedEndDate,
                                                                    request.ReturnDate,
                                                                    _motorcycleRentalPlan.TotalDays,
                                                                    _motorcycleRentalPlan.DailyPrice,
                                                                    _motorcycleRentalPlan.AdditionalDailyPrice,
                                                                    _motorcycleRentalPlan.PercentageDailyNotEffective);

                return new GenericCommandResult(true, "Valor da locação calculado com sucesso.", _calculatedValues);

            }
            catch (Exception ex)
            {
                _notifications.NotifyError("Não foi possível realizar o valor da locação. Estamos corrigindo o problema.");
                throw new Exception(ex.Message);
            }
        }

        public async Task<GenericCommandResult> Handle(GetRentalMotorcyclePlansQuery request, CancellationToken cancellationToken)
        {
            return new GenericCommandResult(true, "Busca realizada com sucesso", await this._rentMotorcycleRepository.GetMotorcycleRentalPlans());
        }
    }
}


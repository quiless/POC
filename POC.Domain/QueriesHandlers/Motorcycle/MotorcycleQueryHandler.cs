using System;
using MediatR;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Queries.Motorcycle;
using POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle;

namespace POC.Domain.QueriesHandlers.Motorcycle
{
    public class MotorcycleQueryHandler :
        IRequestHandler<CheckMotorcycleByPlateQuery, Domain.Models.Entities.Motorcycle>,
        IRequestHandler<SearchMotorcyclesQuery, GenericCommandResult>,
        IRequestHandler<GetMotorcyclesQuery, GenericCommandResult>
    {

        private IMotorcycleRepository _motorcycleRepository;
        private IDomainNotificationContext _domainNotification;

        public MotorcycleQueryHandler(
            IMotorcycleRepository motorcycleRepository,
            IDomainNotificationContext domainNotification)
        {
            _motorcycleRepository = motorcycleRepository;
            _domainNotification = domainNotification;
        }


        public async Task<Models.Entities.Motorcycle> Handle(CheckMotorcycleByPlateQuery request, CancellationToken cancellationToken)
        {
            var _motorcycle = await _motorcycleRepository.GetMotorcycleByPlate(request.Plate);

            if (_motorcycle != null && _motorcycle.Id > 0)
            {
                _domainNotification.NotifyError($"A placa {request.Plate} já está registrada.");
                return null;
            }

            return _motorcycle;
        }

        public async Task<GenericCommandResult> Handle(SearchMotorcyclesQuery request, CancellationToken cancellationToken)
        {
            return new GenericCommandResult(true, "Buscar realizada com sucesso.", await _motorcycleRepository.SearchMotorcycles(request.Identifier, request.Plate));
        }

        public async Task<GenericCommandResult> Handle(GetMotorcyclesQuery request, CancellationToken cancellationToken)
        {
            return new GenericCommandResult(true, "Busca realizada com sucesso.", await _motorcycleRepository.GetAsync());
        }
    }
}


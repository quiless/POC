using System;
using MediatR;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Models.Entities;
using POC.Domain.Queries.Motorcycle;
using POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle;

namespace POC.Domain.QueriesHandlers.Motorcycle
{
	public class MotorcycleModelQueryHandler:
        IRequestHandler<GetMotorcycleModelByIdQuery, MotorcycleModel>,
        IRequestHandler<GetMotorcycleModelByNameQuery, MotorcycleModel>,
        IRequestHandler<GetMotorcycleModelsQuery, GenericCommandResult>
    {


        private IMotorcycleModelRepository _motorcycleModelRepository;
        private IDomainNotificationContext _notifications;


        public MotorcycleModelQueryHandler(
            IMotorcycleModelRepository motorcycleModelRepository,
            IDomainNotificationContext notifications)
        {
            _motorcycleModelRepository = motorcycleModelRepository;
            _notifications = notifications;
        }

        public async Task<MotorcycleModel> Handle(GetMotorcycleModelByIdQuery request, CancellationToken cancellationToken)
        {
            var _motorcycleModel = await this._motorcycleModelRepository.GetMotorcycleModelById(request.MotorcycleModelId);

            if (_motorcycleModel == null || _motorcycleModel.Id == 0)
            {
                _notifications.NotifyError("Marca não encontrada.");
                return null;
            }


            return _motorcycleModel;
        }

        public async Task<MotorcycleModel> Handle(GetMotorcycleModelByNameQuery request, CancellationToken cancellationToken)
        {
            var _motorcycleModel = await this._motorcycleModelRepository.GetMotorcycleModelByName(request.MotorcycleModelName);

            if (_motorcycleModel == null || _motorcycleModel.Id == 0)
            {
                _notifications.NotifyError("Marca não encontrada.");
                return null;
            }


            return _motorcycleModel;
        }

        public async Task<GenericCommandResult> Handle(GetMotorcycleModelsQuery request, CancellationToken cancellationToken)
        {
            return new GenericCommandResult(true, "Busca realizada com sucesso.", await this._motorcycleModelRepository.GetAsync());
        }
    }
}


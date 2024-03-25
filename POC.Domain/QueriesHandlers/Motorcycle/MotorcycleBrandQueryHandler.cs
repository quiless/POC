using System;
using MediatR;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Models.Entities;
using POC.Domain.Queries.Motorcycle;
using POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle;

namespace POC.Domain.QueriesHandlers.Motorcycle
{
	public class MotorcycleBrandQueryHandler:
		IRequestHandler<GetMotorcycleBrandByIdQuery, MotorcycleBrand>,
        IRequestHandler<GetMotorcycleBrandByNameQuery, MotorcycleBrand>,
        IRequestHandler<GetMotorcycleBrandsQuery, GenericCommandResult>

    {

        private IMotorcycleBrandRepository _motorcycleBrandRepository;
        private IDomainNotificationContext _notifications;


        public MotorcycleBrandQueryHandler(
            IMotorcycleBrandRepository motorcycleBrandRepository,
            IDomainNotificationContext notifications)
		{
            _motorcycleBrandRepository = motorcycleBrandRepository;
            _notifications = notifications;
		}

        public async Task<MotorcycleBrand> Handle(GetMotorcycleBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var _motorcycleBrand = await this._motorcycleBrandRepository.GetMotorcycleBrandById(request.MotorcycleBrandId);

            if (_motorcycleBrand == null || _motorcycleBrand.Id == 0)
            {
                _notifications.NotifyError("Marca não encontrada.");
                return null;
            }


            return _motorcycleBrand;
        }

        public async Task<MotorcycleBrand> Handle(GetMotorcycleBrandByNameQuery request, CancellationToken cancellationToken)
        {
            var _motorcycleBrand = await this._motorcycleBrandRepository.GetMotorcycleBrandByName(request.MotorcycleBrandName);

            if (_motorcycleBrand == null || _motorcycleBrand.Id == 0)
            {
                _notifications.NotifyError("Marca não encontrada.");
                return null;
            }


            return _motorcycleBrand;
        }

        public async Task<GenericCommandResult> Handle(GetMotorcycleBrandsQuery request, CancellationToken cancellationToken)
        {
            return new GenericCommandResult(true, "Busca realizada com sucesso.", await this._motorcycleBrandRepository.GetAsync());
        }
    }
}


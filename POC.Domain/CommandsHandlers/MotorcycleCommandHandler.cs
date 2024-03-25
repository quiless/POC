using System;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Interfaces;
using POC.Artifacts.Domain.Models;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.SQL.Transactions.Interfaces;
using POC.Domain.Commands;
using POC.Domain.Commands.Motorcycle;
using POC.Domain.Models.Entities;
using POC.Domain.Queries.Motorcycle;
using POC.Infrastructure.SQLRepositories.Interfaces.Motorcycle;

namespace POC.Domain.CommandsHandlers
{
    public class MotorcycleCommandHandler :
        IRequestHandler<RegisterMotorcycleCommand, GenericCommandResult>,
        IRequestHandler<RefreshMotorcycleCommand, GenericCommandResult>,
        IRequestHandler<RemoveMotorcycleCommand, GenericCommandResult>

    {

        private IDomainNotificationContext _notifications;
        private IMediator _mediator;
        private IUnitOfWork _transaction;
        private IMotorcycleRepository _motorcycleRepository;

        public MotorcycleCommandHandler(
            IDomainNotificationContext notifications,
            IUnitOfWork transction,
            IMotorcycleRepository motorcycleRepository,
            IMediator mediator)
        {
            _notifications = notifications;
            _mediator = mediator;
            _transaction = transction;
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<GenericCommandResult> Handle(RegisterMotorcycleCommand request, CancellationToken cancellationToken)
        {

            //Validar modelo informado
            await _mediator.Send(new GetMotorcycleModelByIdQuery() { MotorcycleModelId = request.ModelId });

            //Validar se a placa já está registrada
            await _mediator.Send(new CheckMotorcycleByPlateQuery() { Plate = request.Plate });

            //Validar marca, caso seja informada, visto que não é obrigatória
            if (request.BrandId != null)
            {
                await _mediator.Send(new GetMotorcycleBrandByIdQuery() { MotorcycleBrandId = request.BrandId.Value });

            }

            if (_notifications.HasErrorNotifications)
                return new GenericCommandResult(false);


            Motorcycle _newMotorcyle = new Motorcycle(request.Year, request.ModelId, request.Plate, request.Identifier, request.BrandId);

            await _motorcycleRepository.InsertAsync(_newMotorcyle);

            return new GenericCommandResult(true, "Moto registrada com sucesso.", _newMotorcyle);

        }

        public async Task<GenericCommandResult> Handle(RefreshMotorcycleCommand request, CancellationToken cancellationToken)
        {
            //Validar se a moto existe
            Motorcycle _motorcycle = _motorcycleRepository.GetById(request.MotorcycleId);

            if (_motorcycle == null || _motorcycle.Id <= 0)
            {
                _notifications.NotifyError("A moto informada não está registrada no sistema.");
                return new GenericCommandResult(false);
            }

            //Validar se a placa é igual a placa anterior
            if (_motorcycle.Plate == request.NewPlateNumber)
            {
                _notifications.NotifyError($"A nova placa informada da moto {_motorcycle.Identifier} é igual a placa atual. A numeração da nova placa deverá ser diferente da numeração atual.");
                return new GenericCommandResult(false);
            }

            //Validar se a nova placa já está registrada
            await _mediator.Send(new CheckMotorcycleByPlateQuery() { Plate = request.NewPlateNumber });

            if (_notifications.HasErrorNotifications)
                return new GenericCommandResult(false);

            _motorcycle.Plate = request.NewPlateNumber;

            await _motorcycleRepository.UpdateMotorcyclePlate(_motorcycle);

            return new GenericCommandResult(true, "Placa atualizada com sucesso.", _motorcycle.Plate);



        }

        public async Task<GenericCommandResult> Handle(RemoveMotorcycleCommand request, CancellationToken cancellationToken)
        {
            //Validar se a moto existe
            Motorcycle _motorcycle = _motorcycleRepository.GetByUniqueId(request.MotorcycleUniqueId);

            if (_motorcycle == null || _motorcycle.Id <= 0)
            {
                _notifications.NotifyError("A moto informada não está registrada no sistema.");
                return new GenericCommandResult(false);
            }

            //A propriedade IsAvaible será manipulada nos eventos de locação e devolução de motos.
            //IsAvailable = false: A moto está disponível para locação
            //IsAvailable = true: A moto está alocada.
            //A propriedade foi criada para facilitar a consulta de disponibilidade da moto. Caso não tivesse a propriedade, teria que consultar a tabela de locação, exigindo maior utilização da base de dados.
            if (!_motorcycle.IsAvailable)
            {
                _notifications.NotifyError("A moto informada está alocada no momento. Não é possível realizar a remoção.");
                return new GenericCommandResult(false);
            }

            _motorcycleRepository.DeleteByUniqueId(request.MotorcycleUniqueId);
            _motorcycle.IsDeleted = true;

            return new GenericCommandResult(true, "Moto removida com sucesso.", _motorcycle);

        }
    }
}


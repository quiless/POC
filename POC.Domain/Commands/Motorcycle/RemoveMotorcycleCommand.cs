using System;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;

namespace POC.Domain.Commands.Motorcycle
{
	public class RemoveMotorcycleCommand: IRequest<GenericCommandResult>
	{
		public Guid MotorcycleUniqueId { get; set; }

		public RemoveMotorcycleCommand(Guid uuid) =>
			this.MotorcycleUniqueId = uuid;
	}

    public class RemoveMotorcycleCommandValidator : AbstractValidator<RemoveMotorcycleCommand>
    {
        public RemoveMotorcycleCommandValidator()
        {
            RuleFor(x => x.MotorcycleUniqueId)
                .Must(x => x != Guid.Empty)
                .WithMessage("Selecione a moto que deseja remover.");

        }

    }
}


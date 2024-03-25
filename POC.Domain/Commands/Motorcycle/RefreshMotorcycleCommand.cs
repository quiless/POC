using System;
using System.Text.RegularExpressions;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.Helpers;

namespace POC.Domain.Commands.Motorcycle
{
	public class RefreshMotorcycleCommand: IRequest<GenericCommandResult>
	{
		public string NewPlateNumber { get; set; }
		public int MotorcycleId { get; set; }
	}



    public class RefreshMotorcycleCommandValidator : AbstractValidator<RefreshMotorcycleCommand>
    {
        public RefreshMotorcycleCommandValidator()
        {
            RuleFor(x => x.MotorcycleId)
                .Must(x => x > 0)
                .WithMessage("Selecione a moto que deseja alterar a placa.");
          
            RuleFor(x => x.NewPlateNumber)
                   .Length(7)
                   .WithMessage("Nova placa da moto inválida. A placa deve tamanho de 7 caracteres.")
                   .Must(x => x.IsMotorcyclePlateNumber())
                   .WithMessage("Nova placa da moto inválida. A placa deve ter o seguinte formato: AAA0A00.");
        }

    }
}


using System;
using System.Text.RegularExpressions;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Artifacts.Helpers;

namespace POC.Domain.Commands.Motorcycle
{
	public class RegisterMotorcycleCommand: IRequest<GenericCommandResult>
	{
		public int Year { get; set; }
        public int ModelId { get; set; }
        public string Plate { get; set; }
        public int? BrandId { get; set; } = null;
        public string Description { get; set; } = String.Empty;
        public string Identifier { get; set; } = String.Empty;
    }


    public class RegisterMotorcycleCommandValidator : AbstractValidator<RegisterMotorcycleCommand>
    {
        public RegisterMotorcycleCommandValidator()
        {
            RuleFor(x => x.ModelId)
                    .Must(x => x > 0)
                    .WithMessage("Necessário selecionar o modelo da moto.");

            RuleFor(x => x.Identifier)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Necessário preencher o identificador da moto.")
                    .MinimumLength(0)
                    .MaximumLength(255)
                    .WithMessage("Tamanho do identificador da moto inválido. O tamanho do identificador da moto deve ter entre 1 e 255 caracteres.");


            RuleFor(x => x.Year)
                    .Must(x => x > 2000)
                    .WithMessage("Serão aceitas motos fabricadas após o ano 2000.");

            RuleFor(x => x.Plate)
                   .Length(7)
                   .WithMessage("Placa da moto inválida. A placa deve tamanho de 7 caracteres.")
                   .Must(v => v.IsMotorcyclePlateNumber())
                   .WithMessage("Placa da moto inválida. A placa deve ter o seguinte formato: AAA0A00.");
        }

           

    }
}


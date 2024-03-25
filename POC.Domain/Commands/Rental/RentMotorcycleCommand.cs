using System;
using System.Security.Cryptography.Xml;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Commands.Motorcycle;
using POC.Domain.Models.Enumerations;

namespace POC.Domain.Commands.Rental
{
	public class RentMotorcycleCommand: IRequest<GenericCommandResult>
	{
		public int MotorcycleRentalPlanId { get; set; }
    }

    public class RentMotorcycleCommandValidator : AbstractValidator<RentMotorcycleCommand>
    {
        public RentMotorcycleCommandValidator()
        {
            RuleFor(x => x.MotorcycleRentalPlanId)
                     .Must(z =>
                     {
                         try
                         {
                             MotorcycleRentalPlanEnum _plan = MotorcycleRentalPlanEnum.From(z);
                             return _plan != MotorcycleRentalPlanEnum.Unknown;
                         }
                         catch
                         {
                             return false;
                         }


                     })
                     .WithMessage($"Plano informado não disponível. Planos disponíveis: {String.Join(", ", MotorcycleRentalPlanEnum.List().Select(v => v.Description))}.");
        }

    }
}




using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Commands.Rental;
using POC.Domain.Models.Enumerations;

namespace POC.Domain.Queries.Rental
{
	public class CheckRentalPriceQuery: IRequest<GenericCommandResult>
	{
        [Required]
		public DateTime ReturnDate { get; set; }
	}

    public class CheckRentalPriceQueryValidator : AbstractValidator<CheckRentalPriceQuery>
    {
        public CheckRentalPriceQueryValidator()
        {
            RuleFor(x => x.ReturnDate)
                 .Must(x => x > DateTime.Now)
                 .WithMessage("A data de devolução informada não é válida. A data de devolução deve ser maior ou igual a data e horário atual.");
        }

    }
}


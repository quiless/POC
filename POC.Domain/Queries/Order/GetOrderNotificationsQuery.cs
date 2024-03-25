using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Models.Enumerations;

namespace POC.Domain.Queries.Order
{
	public class GetOrderNotificationsQuery: IRequest<GenericCommandResult>
	{
        [Required]
		public int OrderId { get; set; }

        public class GetOrderNotificationsQueryValidator : AbstractValidator<GetOrderNotificationsQuery>
        {
            public GetOrderNotificationsQueryValidator()
            {
                RuleFor(x => x.OrderId)
                .Must(x => x > 0)
                .WithMessage("Pedido inválido.");
            }
        }
    }
}


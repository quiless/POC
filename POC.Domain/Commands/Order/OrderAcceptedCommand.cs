using System;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;

namespace POC.Domain.Commands.Order
{
	public class OrderAcceptedCommand: IRequest<GenericCommandResult>
	{
		public int OrderId { get; set; }
    }


    public class OrderAcceptedCommandValidator : AbstractValidator<OrderAcceptedCommand>
    {
        public OrderAcceptedCommandValidator()
        {
            RuleFor(x => x.OrderId)
                    .Must(x => x > 0)
                    .WithMessage("Pedido inválido.");
        }
    }
}


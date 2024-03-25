using System;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;

namespace POC.Domain.Commands.Order
{
	public class OrderFinishedCommand: IRequest<GenericCommandResult>
	{
		public int OrderId { get; set; }

        public class OrderFinishedCommandValidator : AbstractValidator<OrderFinishedCommand>
        {
            public OrderFinishedCommandValidator()
            {
                RuleFor(x => x.OrderId)
                        .Must(x => x > 0)
                        .WithMessage("Pedido inválido.");

            }
        }
    }
}


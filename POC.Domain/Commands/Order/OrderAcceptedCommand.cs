using System;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;

namespace POC.Domain.Commands.Order
{
	public class OrderAcceptedCommand: IRequest<GenericCommandResult>
	{
		public int OrderId { get; set; }
        public int NotificationId { get; set; }
    }


    public class OrderAcceptedCommandValidator : AbstractValidator<OrderAcceptedCommand>
    {
        public OrderAcceptedCommandValidator()
        {
            RuleFor(x => x.OrderId)
                    .Must(x => x > 0)
                    .WithMessage("Pedido inválido.");

            RuleFor(x => x.NotificationId)
                  .Must(x => x > 0)
                  .WithMessage("Não foi possível processar sua notificação. Estamos trabalhando para corrigir o erro.");
        }
    }
}


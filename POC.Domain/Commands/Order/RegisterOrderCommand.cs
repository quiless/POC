using System;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Commands.Deliveryman;
using POC.Domain.Models.Enumerations;

namespace POC.Domain.Commands.Order
{
    public class RegisterOrderCommand : IRequest<GenericCommandResult>
    {
        //Criei a propriedade de OrderDate pensando em um pedido agendado, e não apenas um pedido no momento atual. É um parâmetro não obrigatório. Caso não seja enviado, entende-se que o pedido é para o momento atual.
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal Price { get; set; }
        public string Identifier { get; set; }

    }

    public class RegisterOrderCommandValidator : AbstractValidator<RegisterOrderCommand>
    {
        public RegisterOrderCommandValidator()
        {
            RuleFor(x => x.Identifier)
                    .NotNull()
                    .WithMessage("Necessário preencher seu número identificador. O número identificador é apenas uma chave que você poderá definir.")
                    .MaximumLength(255)
                    .WithMessage("Tamanho do identificador da moto inválido. O tamanho do identificador da moto deve ter entre 1 e 100 caracteres.");

            RuleFor(x => x.OrderDate)
                    .Must(x => x > DateTime.Now)
                    .WithMessage("Não é possivel cadastrar pedidos com horário de entrega retroativo ao horário atual.");

            RuleFor(x => x.Price)
                    .Must(x => x > 0)
                    .WithMessage("Valor da corrida inválido. O valor da corrida deve ser maior que 0.");


        }
    }
}

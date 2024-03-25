using System;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Commands.Order;
using POC.Domain.Models.Enumerations;

namespace POC.Domain.Queries.Order
{
	public class SearchOrdersQuery : IRequest<GenericCommandResult>
	{
        
		//Status da ordem
		public int? OrderStatusId { get; set; }

		//Buscar ordens de um entregador específico
		public string CNPJ { get; set; } = String.Empty;

		//Buscar ordens em um range de data
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }




        public class SearchOrdersQueryValidator : AbstractValidator<SearchOrdersQuery>
        {
            public SearchOrdersQueryValidator()
            {
                RuleFor(x => x.OrderStatusId)
               .Must(z =>
               {
                   if (z == null)
                       return true;
                   try
                   {
                       OrderStatusEnum OrderStatusEnum = OrderStatusEnum.From(z.Value);
                       return OrderStatusEnum != OrderStatusEnum.Unknown;
                   }
                   catch
                   {
                       return false;
                   }


               })
               .WithMessage($"Status do pedido inválido. Status disponíveis: {String.Join(", ", OrderStatusEnum.List())}.");


            }
        }
    }
}


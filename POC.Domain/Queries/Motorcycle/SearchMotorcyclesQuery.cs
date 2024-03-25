using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Commands;

namespace POC.Domain.Queries.Motorcycle
{
	public class SearchMotorcyclesQuery: IRequest<GenericCommandResult>
	{
        [Required]
		public string Plate { get; set; }
		public string Identifier { get; set; }
	}

    public class SearchMotorcyclesQueryValidator : AbstractValidator<SearchMotorcyclesQuery>
    {
        public SearchMotorcyclesQueryValidator()
        {
            //Será aplicado %LIKE% na busca, não será obrigatório informar o número da placa completo.
            RuleFor(x => x.Plate)
                   .NotEmpty()
                   .WithMessage("Digite o número da placa para realizar a pesquisa.");
        }

    }
}


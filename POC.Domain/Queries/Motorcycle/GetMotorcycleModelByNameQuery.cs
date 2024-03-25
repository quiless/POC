using System;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Models.Entities;

namespace POC.Domain.Queries.Motorcycle
{
	public class GetMotorcycleModelByNameQuery : IRequest<MotorcycleModel>
	{
		public string MotorcycleModelName { get; set; }
	}
}


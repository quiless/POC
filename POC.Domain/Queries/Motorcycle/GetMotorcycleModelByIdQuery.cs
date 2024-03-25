using System;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Models.Entities;

namespace POC.Domain.Queries.Motorcycle
{
	public class GetMotorcycleModelByIdQuery: IRequest<MotorcycleModel>
    {
		public int MotorcycleModelId { get; set; }
	}
}


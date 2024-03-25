using System;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Models.Entities;

namespace POC.Domain.Queries.Motorcycle
{
	public class GetMotorcycleBrandByIdQuery: IRequest<MotorcycleBrand>
	{
		public int MotorcycleBrandId { get; set; }
	}
}


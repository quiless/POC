using System;
using MediatR;
using POC.Artifacts.Domain.Responses;
using POC.Domain.Models.Entities;

namespace POC.Domain.Queries.Motorcycle
{
	public class GetMotorcycleBrandByNameQuery : IRequest<MotorcycleBrand>
	{
		public string MotorcycleBrandName {get;set;}
	}
}


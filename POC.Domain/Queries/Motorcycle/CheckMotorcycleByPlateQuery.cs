using System;
using MediatR;

namespace POC.Domain.Queries.Motorcycle
{
	public class CheckMotorcycleByPlateQuery: IRequest<Domain.Models.Entities.Motorcycle>
	{
		public string Plate { get; set; }
	}
}


using System;
using MediatR;
using POC.Domain.Models.Aggregators;
using POC.Domain.Models.Entities;

namespace POC.Domain.Queries.Deliverymans
{
	public class GetDeliverymansAvailableQuery: IRequest<IEnumerable<DeliverymanAvailable>>
	{
		public DateTime OrderDate { get; set; }

		public GetDeliverymansAvailableQuery(DateTime orderDate) =>
			this.OrderDate = orderDate;
    }
}


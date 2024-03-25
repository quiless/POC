using System;
using MediatR;
using POC.Artifacts.Domain.Responses;

namespace POC.Domain.Queries.Deliverymans
{
	public class GeDeliverymanDriverLicenseFileQuery: IRequest<GenericCommandResult>
	{
		public int DeliverymanId { get; set; }
	}
}


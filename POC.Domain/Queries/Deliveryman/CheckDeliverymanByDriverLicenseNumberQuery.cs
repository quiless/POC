using System;
using MediatR;

namespace POC.Domain.Queries.Deliverymans
{
	public class CheckDeliverymanByDriverLicenseNumberQuery: INotification
	{
		public CheckDeliverymanByDriverLicenseNumberQuery(string driverLicenseNumber) =>
			this.DriverLicenseNumber = driverLicenseNumber;

		public string DriverLicenseNumber { get; set; }
	}
}


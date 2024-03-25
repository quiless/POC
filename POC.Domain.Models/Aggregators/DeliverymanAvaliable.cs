using System;
namespace POC.Domain.Models.Aggregators
{
	public class DeliverymanAvailable
	{
		public int DeliverymanId { get; set; }
		public Guid DeliverymanUniqueId { get; set; }
		public int MotorcycleRentalId { get; set; }
    }
}


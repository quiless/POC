using System;
using System.Diagnostics;
using POC.Domain.Models.Enumerations;

namespace POC.Domain.Models.Aggregators
{
	public class OrdersRegistered
	{
		public int Id { get; set; }
		public DateTime CreateDate { get; set; }
		public decimal Price { get; set; }
		public int OrderStatusId { get; set; }
		public OrderStatusEnum OrderStatus
		{
			get
			{
				return OrderStatusEnum.From(OrderStatusId);
			}
		}
		public DateTime OrderDate { get; set; }
		public DateTime? AcceptedDate { get; set; }
		public DateTime? DeliveredDate { get; set; }
		public string DeliverymanName { get; set; } = String.Empty;
        public string DeliverymanCNPJ { get; set; } = String.Empty;
        public string MotorcycleYear { get; set; } = String.Empty;
        public string MotorcyclePlate { get; set; } = String.Empty;
        public string MotorCycleBrandName { get; set; } = String.Empty;
        public string MotorCycleModelName { get; set; } = String.Empty;

    }
}


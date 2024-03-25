using System;
using POC.Artifacts.Domain.Models;
using POC.Artifacts.SQL.Repositories.Builders;
using POC.Domain.Models.Enumerations;

namespace POC.Domain.Models.Entities
{
	[UseAutoMapBuilder("order", false)]
	public class Order : Entity<Int32>
	{
		public DateTime CreateDate { get; set; }
		public decimal Price { get; set; }
		public int OrderStatusId { get; set; } 
		public DateTime? AcceptedDate { get; set; }
		public DateTime? DeliveredDate { get; set; }
		public DateTime OrderDate { get; set; }
		public int? MotorcycleRentalId { get; set; }
		public int CreatedByPersonId { get; set; }

        public Order () { }

		public Order(DateTime orderDate, decimal price, int requesterId)
		{
			this.OrderDate = orderDate;
			this.OrderStatusId = OrderStatusEnum.Available.Id; 
			this.CreateDate = DateTime.Now;
			this.Price = price;
			this.CreatedByPersonId = requesterId;
        }
    }
}



using System;
using POC.Artifacts.Domain.Models;
using POC.Artifacts.Helpers;
using POC.Artifacts.SQL.Repositories.Builders;

namespace POC.Domain.Models.Entities
{
	[UseAutoMapBuilder("motorcyclerental", false)]
	public class MotorcycleRental : Entity<Int32>
	{
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public bool WasFinished { get; set; }
		public int MotorcycleId { get; set; }
		public int MotorcycleRentalPlanId { get; set; }
		public DateTime ExpectedEndDate { get; set; }
		public int DeliverymanId { get; set; }
		public decimal PaidValue { get; set; }
		public bool Fine { get;set; }
		public int TotalOrders { get; set; }
		public decimal TotalValueGenerated { get; set; }
		public DateTime CreatedDate { get; set; }
		public Guid UniqueId { get; set; }
		public bool HasOpenOrder { get; set; }



        public MotorcycleRental() { }

		public MotorcycleRental(int totalDays, int deliverymanId, int motorcycleId, int motorcycleRentalPlanId)
		{
			this.StartDate = DateTime.Now.AddDays(1).AbsoluteStart();
			this.ExpectedEndDate = this.StartDate.AddDays(totalDays).AbsoluteEnd();
			this.DeliverymanId = deliverymanId;
			this.MotorcycleId = motorcycleId;
			this.MotorcycleRentalPlanId = motorcycleRentalPlanId;
			this.CreatedDate = DateTime.Now;
			this.UniqueId = Guid.NewGuid();
			this.HasOpenOrder = false;
		}
    }
}


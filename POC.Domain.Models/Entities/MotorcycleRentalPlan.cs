using System;
namespace POC.Domain.Models.Entities
{
	public class MotorcycleRentalPlan
	{
		public int Id { get; set; }
        public int TotalDays { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal AdditionalDailyPrice { get; set; }
        public decimal PercentageDailyNotEffective { get; set; }
        public string Description { get; set; } = String.Empty;
        public bool IsDeleted { get; set; }

    }
}


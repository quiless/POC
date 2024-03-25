using System;
namespace POC.Domain.Models.Aggregators
{
	public class MotorcycleRented
	{
        public int Id { get; set; }
        public string Plate { get; set; } = String.Empty;
        public int Year { get; set; }
        public string BrandName { get; set; } = String.Empty;
        public string ModelName { get; set; } = String.Empty;
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
    }
}


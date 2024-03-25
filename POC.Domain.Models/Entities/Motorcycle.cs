using System;
using POC.Artifacts.Domain.Models;
using POC.Artifacts.SQL.Repositories.Builders;

namespace POC.Domain.Models.Entities
{
    [UseAutoMapBuilder]
    public class Motorcycle: Entity<Int32>
	{
		public Guid UniqueId { get; set; }
		public int Year { get; set; }
		public int ModelId { get; set; }
		public string Plate { get; set; } = String.Empty;
		public bool IsAvailable;
		public int? BrandId { get; set; }
		public string Identifier { get; set; } = String.Empty;

		public Motorcycle() { }

		public Motorcycle(int year, int modelId, string plate, string identifier, int? brandId = null)
		{
			this.UniqueId = Guid.NewGuid();
			this.Year = year;
			this.ModelId = modelId;
			this.Plate = plate;
			this.IsAvailable = true;
			this.Identifier = identifier;
			this.BrandId = brandId;
		}
	}
}


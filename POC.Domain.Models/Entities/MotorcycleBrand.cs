using System;
using POC.Artifacts.Domain.Models;

namespace POC.Domain.Models.Entities
{
	public class MotorcycleBrand: Entity<Int32>
	{
        public string Name { get; set; } = String.Empty;
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}


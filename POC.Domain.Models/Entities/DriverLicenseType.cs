using System;
using POC.Artifacts.Domain.Models;
using POC.Artifacts.SQL.Repositories.Builders;

namespace POC.Domain.Models.Entities
{
    [UseAutoMapBuilder]
    public class DriverLicenseType: Entity<Int32>
	{
        public string Description { get; set; } = String.Empty;
        
    }
}


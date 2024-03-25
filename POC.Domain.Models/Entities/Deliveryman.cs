using System;
using POC.Artifacts.Domain.Models;
using POC.Artifacts.Helpers;
using POC.Artifacts.SQL.Repositories.Builders;

namespace POC.Domain.Models.Entities
{
	[UseAutoMapBuilder("deliveryman", false)]
	public class Deliveryman: Entity<Int32>
	{
		public string Name { get; set; } = String.Empty;
		public string CNPJ { get; set; } = String.Empty;
		public DateTime BirthDate { get; set; }
		public string DriverLicenseNumber { get; set; } = String.Empty;
		public int DriverLicenseTypeId { get; set; }
		public string DriverLicenseFilename { get; set; } = String.Empty;
		public int UserId { get; set; }
		public bool HasOpenRent { get; set; }
		public Guid UniqueId { get; set; }
		public string Identifier { get; set; } = String.Empty;

		public Deliveryman() { }

		public Deliveryman(
			string name,
			string cnpj,
			DateTime birthDate,
			string driverLicenseNumber,
			int driverLicenseTypeId,
			string driverLicenseFilename,
			int userId,
			string identifier)
		{
			this.Name = name;
			this.CNPJ = cnpj;
			this.BirthDate = birthDate;
			this.DriverLicenseNumber = driverLicenseNumber.OnlyNumbers();
			this.DriverLicenseTypeId = driverLicenseTypeId;
			this.DriverLicenseFilename = driverLicenseFilename;
			this.UserId = userId;
			this.UniqueId = Guid.NewGuid();
			this.HasOpenRent = false;
			this.Identifier = identifier;
		}

	}
}


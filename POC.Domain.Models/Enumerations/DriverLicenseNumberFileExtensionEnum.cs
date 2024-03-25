using System;
using POC.Artifacts.Application;

namespace POC.Domain.Models.Enumerations
{
	
    public class DriverLicenseNumberFileExtensionEnum : Enumeration
    {
        public static DriverLicenseNumberFileExtensionEnum Unknown = new DriverLicenseNumberFileExtensionEnum(0, nameof(Unknown).ToUpperInvariant(), "Desconhecido");
        public static DriverLicenseNumberFileExtensionEnum PNG = new DriverLicenseNumberFileExtensionEnum(1, nameof(PNG).ToUpperInvariant(), "PNG");
        public static DriverLicenseNumberFileExtensionEnum BMP = new DriverLicenseNumberFileExtensionEnum(2, nameof(BMP).ToUpperInvariant(), "BMP");


        public DriverLicenseNumberFileExtensionEnum(int id, string name, string description)
            : base(id, name, description) { }

        public static IEnumerable<DriverLicenseNumberFileExtensionEnum> List() => new[]
        {
            PNG, BMP
        };

        public static DriverLicenseNumberFileExtensionEnum FromName(string name)
        {
            var DriverLicenseNumberFileExtensionEnum = List()
                .SingleOrDefault(s => String.Equals(s.Name, name.Replace(".",""), StringComparison.CurrentCultureIgnoreCase));

            if (DriverLicenseNumberFileExtensionEnum == null)
            {
                return DriverLicenseNumberFileExtensionEnum.Unknown;
            }

            return DriverLicenseNumberFileExtensionEnum;
        }

        public static DriverLicenseNumberFileExtensionEnum From(int id)
        {
            var DriverLicenseNumberFileExtensionEnum = List().SingleOrDefault(s => s.Id == id);

            if (DriverLicenseNumberFileExtensionEnum == null)
            {
                return DriverLicenseNumberFileExtensionEnum.Unknown;
            }

            return DriverLicenseNumberFileExtensionEnum;
        }
    }
}


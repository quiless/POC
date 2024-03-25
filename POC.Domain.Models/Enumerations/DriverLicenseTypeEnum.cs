using System;
using POC.Artifacts.Application;

namespace POC.Domain.Models.Enumerations
{
    public class DriverLicenseTypeEnum : Enumeration
    {
        public static DriverLicenseTypeEnum Unknown = new DriverLicenseTypeEnum(0, nameof(Unknown).ToUpperInvariant(), "Desconhecida");
        public static DriverLicenseTypeEnum A = new DriverLicenseTypeEnum(1, nameof(A).ToUpperInvariant(), "A");
        public static DriverLicenseTypeEnum B = new DriverLicenseTypeEnum(2, nameof(B).ToUpperInvariant(), "B");
        public static DriverLicenseTypeEnum AB = new DriverLicenseTypeEnum(3, nameof(AB).ToUpperInvariant(), "AB");


        public DriverLicenseTypeEnum(int id, string name, string description)
            : base(id, name, description) { }

        public static IEnumerable<DriverLicenseTypeEnum> List() => new[]
        {
            A, B, AB
        };

        public static DriverLicenseTypeEnum FromName(string name)
        {
            var driverLicenseTypeEnum = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (driverLicenseTypeEnum == null)
            {
                return DriverLicenseTypeEnum.Unknown;
            }

            return driverLicenseTypeEnum;
        }

        public static DriverLicenseTypeEnum From(int id)
        {
            var driverLicenseTypeEnum = List().SingleOrDefault(s => s.Id == id);

            if (driverLicenseTypeEnum == null)
            {
                return DriverLicenseTypeEnum.Unknown;
            }

            return driverLicenseTypeEnum;
        }
    }
}


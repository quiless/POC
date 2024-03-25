using System;
using POC.Artifacts.Application;

namespace POC.Domain.Models.Enumerations
{
	public class MotorcycleRentalPlanEnum: Enumeration
	{
        public static MotorcycleRentalPlanEnum Unknown = new MotorcycleRentalPlanEnum(0, nameof(Unknown).ToUpperInvariant(), "Desconhecido");
        public static MotorcycleRentalPlanEnum Plan7Days = new MotorcycleRentalPlanEnum(1, nameof(Plan7Days).ToUpperInvariant(), "Plano 7 dias");
        public static MotorcycleRentalPlanEnum Plan15Days = new MotorcycleRentalPlanEnum(2, nameof(Plan15Days).ToUpperInvariant(), "Plano 15 dias");
        public static MotorcycleRentalPlanEnum Plan30Days = new MotorcycleRentalPlanEnum(3, nameof(Plan30Days).ToUpperInvariant(), "Plano 30 dias");


        public MotorcycleRentalPlanEnum(int id, string name, string description)
            : base(id, name, description) { }

        public static IEnumerable<MotorcycleRentalPlanEnum> List() => new[]
        {
            Unknown, Plan7Days, Plan15Days, Plan30Days
        };

        public static MotorcycleRentalPlanEnum FromName(string name)
        {
            var MotorcycleRentalPlanEnum = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (MotorcycleRentalPlanEnum == null)
            {
                return MotorcycleRentalPlanEnum.Unknown;
            }

            return MotorcycleRentalPlanEnum;
        }

        public static MotorcycleRentalPlanEnum From(int id)
        {
            var MotorcycleRentalPlanEnum = List().SingleOrDefault(s => s.Id == id);

            if (MotorcycleRentalPlanEnum == null)
            {
                return MotorcycleRentalPlanEnum.Unknown;
            }

            return MotorcycleRentalPlanEnum;
        }
    }
}


using System;
using POC.Artifacts.Application;

namespace POC.Domain.Models.Enumerations
{
	
    public class RentFinishTypeEnum : Enumeration
    {
        public static RentFinishTypeEnum InDeadline = new RentFinishTypeEnum(1, nameof(InDeadline).ToUpperInvariant(), "No prazo");
        public static RentFinishTypeEnum Advance = new RentFinishTypeEnum(2, nameof(Advance).ToUpperInvariant(), "Antecipado");
        public static RentFinishTypeEnum Late = new RentFinishTypeEnum(3, nameof(Late).ToUpperInvariant(), "Atrasado");


        public RentFinishTypeEnum(int id, string name, string description)
            : base(id, name, description) { }

        public static IEnumerable<RentFinishTypeEnum> List() => new[]
        {
            InDeadline, Advance, Late
        };

        public static RentFinishTypeEnum FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new System.Exception($"Valores possíveis para RentFinishTypeEnum: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static RentFinishTypeEnum From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new System.Exception($"Valores possíveis para RentFinishTypeEnum: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}


using System;
using POC.Artifacts.Application;

namespace POC.Domain.Models.Enumerations
{
	public class OrderStatusEnum: Enumeration
    {
        public static OrderStatusEnum Unknown = new OrderStatusEnum(0, nameof(Unknown).ToUpperInvariant(), "Desconhecido");
        public static OrderStatusEnum Available = new OrderStatusEnum(1, nameof(Available).ToUpperInvariant(), "Disponível");
        public static OrderStatusEnum Accepted = new OrderStatusEnum(2, nameof(Accepted).ToUpperInvariant(), "Aceito");
        public static OrderStatusEnum Delivered = new OrderStatusEnum(3, nameof(Delivered).ToUpperInvariant(), "Entregue");


        public OrderStatusEnum(int id, string name, string description)
            : base(id, name, description) { }

        public static IEnumerable<OrderStatusEnum> List() => new[]
        {
            Unknown,
            Available,
            Accepted,
            Delivered
        };

        public static OrderStatusEnum FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new System.Exception($"Valores possíveis para OrderStatusEnum: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static OrderStatusEnum From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new System.Exception($"Valores possíveis para OrderStatusEnum: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}


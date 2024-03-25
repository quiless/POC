using System;
using MediatR;

namespace POC.Domain.Commands.Deliveryman
{
    public class SetDeliverymanOpenRentCommand : INotification
    {
        public int DeliverymanId { get; set; }

        public SetDeliverymanOpenRentCommand (int deliverymanId) =>
            this.DeliverymanId = deliverymanId;
    }
}


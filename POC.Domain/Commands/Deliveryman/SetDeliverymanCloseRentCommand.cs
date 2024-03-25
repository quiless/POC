using System;
using MediatR;

namespace POC.Domain.Commands.Deliveryman
{
	public class SetDeliverymanCloseRentCommand: INotification
	{
		public int DeliverymanId { get; set; }

		public SetDeliverymanCloseRentCommand(int deliverymanId) =>
			this.DeliverymanId = deliverymanId;
    }
}


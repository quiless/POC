using System;
using MediatR;

namespace POC.Domain.Commands.Order
{
	public class NotifyDelivermansCommand: INotification
	{
		public int OrderId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal Price { get; set; }

		public NotifyDelivermansCommand(
			int orderId,
			decimal price,
			DateTime orderDate)
		{
			this.OrderDate = orderDate;
			this.OrderId = orderId;
			this.Price = price;
		}

	}
}


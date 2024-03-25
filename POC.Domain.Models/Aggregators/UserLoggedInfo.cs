using System;
namespace POC.Domain.Models.Aggregators
{
	public class UserLoggedInfo
	{
		public string Username { get; set; } = String.Empty;
		public int Id { get; set; }
		public Guid UniqueId { get; set; }
		public int? DeliverymanId { get; set; }
		public bool IsAdmin { get; set; }
		public bool IsDeliveryman
		{
			get
			{
				try
				{
					return this.DeliverymanId > 0;
				}
				catch 
				{
					return false;
				}
			}
		}
	}
}


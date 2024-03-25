using System;
using POC.Domain.Models.Enumerations;

namespace POC.Domain.Models.Aggregators
{
	public class NotificationSent
	{
        public int NotificationId { get; set; }
        public DateTime CreateDate { get; set; }
        public string DeliverymanName { get; set; } = String.Empty;
        public string DeliverymanCNPJ { get; set; } = String.Empty;
    }
}


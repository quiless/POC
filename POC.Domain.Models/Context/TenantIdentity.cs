using System;
namespace POC.Domain.Models.Context
{
	public class TenantIdentity
	{
		public bool IsAdmin { get; set; }
        public bool IsDeliveryman { get; set; }
        public Guid UserUniqueId { get; set; }
		public int UserId { get; set; }
	}
}


using System;
namespace POC.Domain.Models
{
	public struct Environments
	{
		public static string DomainModelProjects {  get { return "POC.Domain.Models";  } }

        public struct CustomClaimIdentity
        {
            public const string UserUniqueId = "UUID";
            public const string IsAdmin = "IAD";
            public const string IsDeliveryman = "IDL";
            public const string UserId = "UID";
        }

        public struct Pubnub
        {
            public struct Channels
            {
                public const string DELIVERYMAN_CHANNEL = "deliveryman_channel_";
                //Defini como cliente o responsável por criar o pedido
                public const string CUSTOMER_CHANNEL = "customer_channel_";
            }
        }


    }

	
}


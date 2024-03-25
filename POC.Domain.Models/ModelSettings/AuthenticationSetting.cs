using System;
namespace POC.Domain.Models.ModelSettings
{
	
    public class AuthenticationSetting
    {
        public string Scope { get; set; } = String.Empty;
        public string ClientId { get; set; } = String.Empty;
        public string ClientSecret { get; set; } = String.Empty;
        public string ClientName { get; set; } = String.Empty;
        public string Authority { get; set; } = String.Empty;

    }
}


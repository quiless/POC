using System;
namespace POC.Artifacts.AzureStorage.Models
{
	public class BlobSharedAccessSignature
	{
        public string Uri { get; set; } = String.Empty;
        public string ContainerName { get; set; } = String.Empty;
        public string Signature { get; set; } = String.Empty;
        public string Token { get; set; } = String.Empty;
        public DateTime ExpirationDate { get; set; }
    }
}


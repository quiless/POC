using System;
namespace POC.Artifacts.AzureStorage.Settings
{
	public class AzureStorageSettings
    {
        public string AccountName { get; set; } = String.Empty;
        public string AccessKey { get; set; } = String.Empty;
        public string Container { get; set; } = String.Empty;
        public string Endpoint { get; set; } = String.Empty;
    }
}


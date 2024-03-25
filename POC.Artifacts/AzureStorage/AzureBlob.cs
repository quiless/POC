using System;
using POC.Artifacts.AzureStorage.Inferfaces;
using POC.Artifacts.AzureStorage.Models;
using System.Collections.Concurrent;
using POC.Artifacts.AzureStorage.Settings;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;

namespace POC.Artifacts.AzureStorage
{
    public class AzureBlob : IBlob
    {
        private readonly ConcurrentDictionary<string, BlobContinuationToken> _azureContinuationTokens =
            new ConcurrentDictionary<string, BlobContinuationToken>();

        #pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
        public AzureStorageSettings AzureSettings { get; private set; }
        public StorageCredentials AzureCredentials { get; private set; } 
        public CloudStorageAccount AzureAccount { get; private set; } 
        public CloudBlobClient AzureBlobClient { get; private set; } 
        public CloudBlobContainer AzureContainer { get; private set; }
        #pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.


        public void Initialize(AzureStorageSettings settings, bool createContainerIfNotExists = false)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            AzureSettings = settings;
            AzureCredentials = new StorageCredentials(AzureSettings.AccountName, AzureSettings.AccessKey);
            AzureAccount = new CloudStorageAccount(AzureCredentials, true);
            AzureBlobClient = AzureAccount.CreateCloudBlobClient();
            AzureContainer = AzureBlobClient.GetContainerReference(AzureSettings.Container);
            AzureContainer.CreateIfNotExists();
        }

      
        public async Task<bool> Delete(string key)
        {
            try
            {
                CloudBlockBlob blockBlob = AzureContainer.GetBlockBlobReference(key);
                OperationContext ctx = new OperationContext();
                await blockBlob.DeleteAsync(DeleteSnapshotsOption.None, null, null, ctx);

                return true;
            }
            catch
            {
                return false;
            }
        }

       
        public string GenerateUrl(string key) =>
            $"https://{AzureSettings.AccountName}.blob.core.windows.net/{AzureSettings.Container}/{key}";

        public string GenerateTempLinkToDownload(string key)
        {
            // create the stored policy we will use, with the relevant permissions and expiry time
            var storedPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(60)),
                Permissions = SharedAccessBlobPermissions.Read
            };

            var blob = AzureContainer.GetBlobReference(key);

            var blobSignature = blob.GetSharedAccessSignature(storedPolicy);
            return $"{AzureContainer.Uri.AbsoluteUri}/{key}{blobSignature}";
        }

        public string GetBlobSharedAccessSignatureUrl(string key, TimeSpan timeToExpire, string permission = "r")
        {
            var sas = GetSharedAccessSignature(timeToExpire, permission);
            return $"{sas.Uri}/{key}{sas.Signature}";
        }

        public BlobSharedAccessSignature GetSharedAccessSignature(TimeSpan timeToExpire, string permission = "rw")
        {
            var storedPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(timeToExpire.TotalMinutes),
                Permissions = SharedAccessBlobPolicy.PermissionsFromString(permission)
            };

            var containerSignature = AzureContainer.GetSharedAccessSignature(storedPolicy);

            return new BlobSharedAccessSignature()
            {
                Uri = AzureContainer.Uri.AbsoluteUri.Replace(AzureContainer.Uri.AbsolutePath, ""),
                ContainerName = AzureContainer.Name,
                Signature = containerSignature,
                ExpirationDate = DateTime.Now.Add(timeToExpire)
            };
        }

      
        public async Task UploadFromFile(string key, string path)
        {
            FileInfo file = new FileInfo(path);
            await UploadFromFile(key, file);
            System.IO.File.Delete(path);
        }

        public async Task UploadFromFile(string key, FileInfo file)
        {
            CloudBlockBlob blockBlob = AzureContainer.GetBlockBlobReference(key);
            await blockBlob.UploadFromFileAsync(file.FullName);
        }
    }
}


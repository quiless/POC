﻿using System;
using POC.Artifacts.AzureStorage;
using POC.Artifacts.AzureStorage.Inferfaces;
using POC.Artifacts.AzureStorage.Settings;

namespace POC.API.Configurations
{
    ///<Summary>
    /// Setup do Azure Storage
    ///</Summary>
	public static class BlobStorageConfiguration
	{
        ///<Summary>
        /// Adiciona o serviço Azure Storage com as configurações settadas no arquivo appsettings.*.json
        ///</Summary>
        public static void AddAzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            AzureStorageSettings? azureSettings = configuration.GetSection("AzureStorage").Get<AzureStorageSettings>();

            if (azureSettings is null)
                throw new ArgumentException($"Configurações do Azure Storage não encontradas.");


            services.AddScoped<IBlob, AzureBlob>((serviceProvider) => {
                var blob = new AzureBlob();
                blob.Initialize(azureSettings, true);
                return blob;
            });
        }
    }
}


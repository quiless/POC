using System;
using POC.Artifacts.AzureStorage.Models;
using POC.Artifacts.AzureStorage.Settings;

namespace POC.Artifacts.AzureStorage.Inferfaces
{
	public interface IBlob
	{
        /// <summary>
        /// Inicializa o container.
        /// </summary>
        /// <param name="settings">Configurações do container.</param>
        /// <param name="createContainerIfNotExists">Flag que indica criação do container caso o mesmo não exista.</param>
        void Initialize(AzureStorageSettings settings, bool createContainerIfNotExists = false);

      
        /// <summary>
        /// Apaga um item no conteinar pela chave(nome do item).
        /// </summary>
        /// <param name="key">Chave do item.</param>
        /// <returns>True se houver sucesso.</returns>
        Task<bool> Delete(string key);

       
        /// <summary>
        /// Gera uma URL para o item no container com prazo de expiração.
        /// </summary>
        /// <param name="key">Chave do item.</param>
        /// <param name="timeToExpire">Tempo de vida da URL.</param>
        /// <returns>URL apontando para o item.</returns>
        string GenerateTempLinkToDownload(String key);

        /// <summary>
        /// Gera uma URL para o item no container assindada pelo BLOB.
        /// </summary>
        /// <param name="key">Chave do item.</param>
        /// <param name="timeToExpire">Tempo de vida da URL.</param>
        /// <param name="permission">Tipo de permissão.</param>
        /// <returns>URL apontando para o item.</returns>
        string GetBlobSharedAccessSignatureUrl(String key, TimeSpan timeToExpire, String permission = "r");

        /// <summary>
        /// Gera assinatura do BLOB.
        /// </summary>
        /// <param name="timeToExpire">Tempo de vida.</param>
        /// <param name="permission">Tipo de permissão</param>
        /// <returns>Instância de <see cref="BlobSharedAccessSignature"/>.</returns>
        BlobSharedAccessSignature GetSharedAccessSignature(TimeSpan timeToExpire, String permission = "rw");

        /// <summary>
        /// Write a BLOB using a byte array.
        /// </summary>
        /// <param name="key">Key of the BLOB.</param>
        /// <param name="contentType">Content-type of the object.</param>
        /// <param name="data">BLOB data.</param> 
        Task UploadFromFile(string key, string path);

    }
}


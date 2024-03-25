using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;

namespace POC.Artifacts.AzureStorage
{
	public class Helper
	{
        public static byte[] Base64ToBytes(string data)
        {
            try
            {
                return Convert.FromBase64String(data);
            }
            catch
            {
                throw new Exception("Erro ao converter arquivo base64 para array de bytes.");
            }
        }

       
        /// <summary>
        /// Converte <see cref="byte[]"/> para string Base64.
        /// </summary>
        /// <param name="data">Instância de <see cref="byte[]"/>.</param>
        /// <returns>String Base64.</returns>
        public static string BytesToBase64(byte[] data)
        {
            try
            {
                return Convert.ToBase64String(data);
            }
            catch
            {
                throw new Exception("Erro ao converter arquivo array de bytes para base64.");
            }

           
        }

        
        /// <summary>
        /// Converte <see cref="Stream"/> para <see cref="byte[]"/>.
        /// </summary>
        /// <param name="input">Stream a ser convertido.</param>
        /// <returns>Array de bytes.</returns>
        /// <exception cref="ArgumentNullException">Exceção lançada em caso do Stream ser null.</exception>
        /// <exception cref="InvalidOperationException">Exceção lançada em caso de não poder ler o Stream.</exception>
        public static byte[] StreamToBytes(Stream input)
        {
            if (!input.CanRead)
                throw new InvalidOperationException("Falha ao tentar converter o arquivo.");

            try
            {
                byte[] buffer = new byte[16 * 1024];
                using MemoryStream ms = new MemoryStream();
                int read;

                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                ms.Close();

                return ms.ToArray();
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Falha ao tentar converter o arquivo de stream para array de bytes.");
            }
          
        }
    }
}


using System;
namespace POC.Artifacts.AzureStorage.Models
{
    public class BlobData
    {
        public BlobData() { }

        internal BlobData(long contentLength, Stream data)
        {
            ContentLength = contentLength;
            Data = data;
        }

       
        public long ContentLength { get; set; }

        public Stream? Data { get; set; }
    }
}


using System;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using StockPicker.Sources;
using System.Threading.Tasks;

namespace StockPicker.Destinations
{
    public class AlphaVantageStorage
    {
        CloudStorageAccount storageAccount;
        CloudBlobClient blobClient;
        CloudBlobContainer blobContainer;

        public AlphaVantageStorage(string connectionString, string containerName)
        {
            CloudStorageAccount.TryParse(connectionString, out storageAccount);
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(containerName);
        }

        public async Task TimeSeriesDailyAdjustedLoad(TimeSeriesDailyAdjustedResponse timeSeriesDaily)
        {
            // get a reference to the blob
            string blobName = timeSeriesDaily.Ticker;
            CloudBlockBlob cloudBlob = blobContainer.GetBlockBlobReference(blobName);

            // generate the blob
            CloudBlobStream stream = await cloudBlob.OpenWriteAsync();
            
            // serialize the quotes
            byte[] vs = timeSeriesDaily.Serialize();

            // write the Quotes into a blob
            await stream.WriteAsync(vs, 0, vs.Length);

            // commit the changes to azure storage
            await stream.CommitAsync();

        }
    }
}

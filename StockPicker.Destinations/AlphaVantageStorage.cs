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

        AlphaVantageStorage(string connectionString, string containerName)
        {
            CloudStorageAccount.TryParse(connectionString, out storageAccount);
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(containerName);
        }

        public async Task TimeSeriesDailyAdjustedLoad(TimeSeriesDailyAdjustedResponse timeSeriesDaily)
        {
            // write the Quotes into a blob
            foreach (Quote q in timeSeriesDaily.Quotes)
            {
                // serialize the quote
                byte[] vs = q.Serialize();

                // generate the blob
                string blobName = $"{q.Symbol}_{q.QuoteDate.ToString()}";
                CloudBlockBlob cloudBlob = blobContainer.GetBlockBlobReference(blobName);
                CloudBlobStream stream = await cloudBlob.OpenWriteAsync();
                await stream.WriteAsync(vs, 0, vs.Length);
                await stream.FlushAsync();
            }
        }
    }
}

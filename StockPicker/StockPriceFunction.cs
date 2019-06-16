using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using StockPicker.Sources;
using StockPicker.Destinations;
using System.Threading.Tasks;
using System.Configuration;

namespace StockPicker.Function
{
    public static class StockPriceFunction
    {
        [FunctionName("DailyPricesExport")]
        public static async void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            log.LogInformation("BEGIN GetTimeSeriesDailyAdjusted");

            string apiKey = System.Environment.GetEnvironmentVariable("AlphaVantageAPIKey", EnvironmentVariableTarget.Process);

            AlphaVantageClient client = new AlphaVantageClient(apiKey, "compact");
            var data = await client.GetTimeSeriesDailyAdjusted("MSFT");

            string connectionString = System.Environment.GetEnvironmentVariable("AzureStorageConnectionString", EnvironmentVariableTarget.Process);
            string containerName = System.Environment.GetEnvironmentVariable("AzureStorageContainer", EnvironmentVariableTarget.Process);

            AlphaVantageStorage alphaVantageStorage = new AlphaVantageStorage(connectionString, containerName);
            await alphaVantageStorage.TimeSeriesDailyAdjustedLoad(data);

            log.LogInformation("END GetTimeSeriesDailyAdjusted");
        }
    }
}

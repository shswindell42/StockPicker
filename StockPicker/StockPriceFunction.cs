using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using StockPicker.Sources;
using System.Threading.Tasks;

namespace StockPicker.Function
{
    public static class StockPriceFunction
    {
        [FunctionName("DailyPricesExport")]
        public static async void Run([TimerTrigger("*/30 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            log.LogInformation("BEGIN GetTimeSeriesDailyAdjusted");

            AlphaVantageClient client = new AlphaVantageClient("GIRGUKTU3NU0IJF3", "compact");
            await client.GetTimeSeriesDailyAdjusted("MSFT");
        }
    }
}

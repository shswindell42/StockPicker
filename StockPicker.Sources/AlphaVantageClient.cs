using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StockPicker.Sources
{
    public class AlphaVantageClient
    {
        private HttpClient _client = new HttpClient();
        private const string _baseUrl = "https://www.alphavantage.co/query?";
        private string _apiKey;
        private string _outputSize;
        
        public AlphaVantageClient(string apiKey, string outputSize)
        {
            _apiKey = apiKey;
            _outputSize = outputSize;
        }
        /// <summary>
        /// Fetches daily time series from the Alpha Vantage API
        /// </summary>
        /// <param name="ticker">Stock symbol to fetch price data for</param>
        /// <returns>json string containing pricing data</returns>
        public async Task<TimeSeriesDailyAdjustedResponse> GetTimeSeriesDailyAdjusted(string ticker)
        {
            // build the URL to fetch data
            NameValueCollection param = new NameValueCollection();
            param.Add("function", "TIME_SERIES_DAILY_ADJUSTED");
            param.Add("symbol", ticker);
            param.Add("outputSize", _outputSize);
            param.Add("apikey", _apiKey);

            string[] array = (from key in param.AllKeys
                         from value in param.GetValues(key)
                         select $"{key}={HttpUtility.UrlEncode(value)}").ToArray();

            string queryString = string.Join("&", array);
            string uri = string.Concat(_baseUrl, queryString);

            // call the api
            HttpResponseMessage response = await _client.GetAsync(uri);

            // parse the response
            string statusCode = response.StatusCode.ToString();
            string json = await response.Content.ReadAsStringAsync();
            TimeSeriesDailyAdjustedResponse r = AlphaVantageReader.ReadTimeSeriesDailyAdjusted(statusCode, json);

            return r;
        }

    }
}

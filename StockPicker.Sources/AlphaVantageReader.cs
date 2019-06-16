using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace StockPicker.Sources
{
    static class AlphaVantageReader
    {
        public static TimeSeriesDailyAdjustedResponse ReadTimeSeriesDailyAdjusted(string statusCode, string json)
        {
            TimeSeriesDailyAdjustedResponse t = new TimeSeriesDailyAdjustedResponse();
            JObject response = JObject.Parse(json);

            // fill out the time series object
            t.ResponseCode = statusCode;
            
            // set the error message if needed
            if (!statusCode.Equals("OK"))
            {
                t.ErrorMessage = response["Information"].ToString();
            }
            else
            {
                // parse the metadata
                t.Metadata = response["Meta Data"].ToString();
                string symbol = response["Meta Data"]["2. Symbol"].ToString();
                t.Ticker = symbol;
                // parse the quotes
                foreach(var q in response["Time Series (Daily)"].Values())
                {
                    Quote quote = new Quote();
                    quote.QuoteDate = Convert.ToDateTime(((JProperty)q.Parent).Name);
                    quote.Symbol = symbol;
                    quote.Open = q["1. open"].Value<decimal>();
                    quote.High = q["2. high"].Value<decimal>();
                    quote.Low = q["3. low"].Value<decimal>();
                    quote.Close = q["4. close"].Value<decimal>();
                    quote.AdjustedClose = q["5. adjusted close"].Value<decimal>();
                    quote.Volume = q["6. volume"].Value<int>();
                    quote.Dividend = q["7. dividend amount"].Value<decimal>();
                    quote.SplitCoefficient = q["8. split coefficient"].Value<decimal>();

                    // add quote to the list of quotes
                    t.Quotes.Add(quote);
                }
            }
            
            return t;
        }
    }
}

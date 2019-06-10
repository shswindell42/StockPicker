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
                
                // parse the quotes
                foreach(var q in response["Time Series (Daily)"].Values())
                {
                    Quote quote = new Quote();
                    quote.QuoteDate = Convert.ToDateTime(((JProperty)q.Parent).Name);
                    quote.Symbol = symbol;
                    quote.Open = (decimal)q["1. open"];
                    quote.High = (decimal)q["2. high"];
                    quote.Low = (decimal)q["3. low"];
                    quote.Close = (decimal)q["4. close"];
                    quote.AdjustedClose = (decimal)q["5. adjusted close"];
                    quote.Volume = (int)q["6. volume"];
                    quote.Dividend = (decimal)q["7. dividend amount"];
                    quote.SplitCoefficient = (decimal)q["8. split coefficient"];

                    // add quote to the list of quotes
                    t.Quotes.Add(quote);
                }
            }
            
            return t;
        }
    }
}

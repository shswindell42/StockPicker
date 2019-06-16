using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockPicker.Sources
{
    public class TimeSeriesDailyAdjustedResponse
    {
        public string ResponseCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Metadata { get; set; }
        public string Ticker { get; set; }
        public List<Quote> Quotes { get; set; } = new List<Quote>();    

        public byte[] Serialize()
        {
            string json = JsonConvert.SerializeObject(Quotes);
            return Encoding.ASCII.GetBytes(json);
        }
    }

}

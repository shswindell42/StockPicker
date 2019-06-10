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
        public List<Quote> Quotes { get; set; }       
    }

}

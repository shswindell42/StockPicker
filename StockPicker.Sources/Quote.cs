using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace StockPicker.Sources
{
    public class Quote
    {
        public DateTime QuoteDate { get; set; }
        public string Symbol { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal AdjustedClose { get; set; }
        public int Volume { get; set; }
        public decimal Dividend { get; set; }
        public decimal SplitCoefficient { get; set; }

        public byte[] Serialize()
        {
            string json = JsonConvert.SerializeObject(this);
            return Encoding.ASCII.GetBytes(json);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFTest.Models
{
    public class TickerInfo
    {
        public string? TickerSymbol { get; set; }
        public string? Beta { get; set; }
        public string? Price { get; set; }
        public string? YahooAnalystMeanPrice { get; set; }
       // public string? YahooAnalystMedianPrice { get; set; }
        public string? YahooAnalystMinPrice { get; set; }
        public string? YahooAnalystMaxPrice { get; set; }
      //  public string? YahooRating{ get; set; }
        public string? YahooRecommendationMean { get; set; }
    }
}

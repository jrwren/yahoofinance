using System;
using System.Collections.Generic;

namespace MaasOne.YahooFinance.Data
{
    public class ETFSearchItem
    {
          public string Name { get; set; }
        public string ID { get; set; }
        public Dictionary<ETFSearchProperty, object> Values { get; private set; }
        public ETFSearchItem() { this.Values = new Dictionary<ETFSearchProperty, object>(); }
   
    }
}

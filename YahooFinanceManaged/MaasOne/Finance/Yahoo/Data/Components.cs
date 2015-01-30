using System;
using System.Collections.Generic;

namespace MaasOne.Finance.Yahoo.Data
{

    public class ComponentsItem : IQuotePrice
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double LastTradePriceOnly { get; set; }
        public double ChangeInPercent { get; set; }
        public int? Volume { get; set; }

        public ComponentsItem()
        {
        }
    }

}

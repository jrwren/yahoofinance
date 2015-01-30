using System;
using System.Collections.Generic;

namespace MaasOne.Finance.Yahoo.Data
{

    public class IndustryInfoData
    {
        public string Name { get; set; }
        public double? ChangeInPercent { get; set; }
        public double? MarketCapInMillion { get; set; }
        public double? PriceToEarnings { get; set; }
        public double? ReturnOnEquityInPercent { get; set; }
        public double? DividendYieldInPercent { get; set; }
        public double? DeptToEquity { get; set; }
        public double? PriceBook { get; set; }
        public double? NetProfitMargin { get; set; }
        public double? PriceToFreeCashFlow { get; set; }

        internal IndustryInfoData() { }
    }

    public class IndustryInfoDataCollection
    {
        public IndustryInfoData[] Items { get; internal set; }

        internal IndustryInfoDataCollection() { }
    }


}

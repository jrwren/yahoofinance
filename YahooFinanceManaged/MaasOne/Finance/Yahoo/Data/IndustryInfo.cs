// **************************************************************************************************
// **  
// **  Yahoo! Finance Managed
// **  Written by Marius Häusler 2015
// **  It would be pleasant, if you could contact me when you are using this code.
// **  Contact: maasone@live.com
// **  Project Home: https://yahoofinance.codeplex.com/
// **  
// **************************************************************************************************
// **  
// **  Copyright @ Marius Häusler
// **  
// **  Licensed under GNU Lesser General Public License (LGPL) (Version 2.1, February 1999).
// **  
// **  License Text: https://yahoofinance.codeplex.com/license
// **  
// **  
// **************************************************************************************************
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

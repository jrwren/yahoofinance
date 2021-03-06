#region "License"
// *********************************************************************************************
// **                                                                                         **
// **  Yahoo! Finance Managed                                                                 **
// **                                                                                         **
// **  Copyright (c) Marius Häusler 2009-2015                                                 **
// **                                                                                         **
// **  Licensed under GNU Lesser General Public License (LGPL) (Version 2.1, February 1999).  **
// **                                                                                         **
// **  License: https://www.gnu.org/licenses/old-licenses/lgpl-2.1.txt                        **
// **                                                                                         **
// **  Project: https://yahoofinance.codeplex.com/                                            **
// **                                                                                         **
// **  Contact: maasone@live.com                                                              **
// **                                                                                         **
// *********************************************************************************************
#endregion
using System;
using System.Collections.Generic;

namespace MaasOne.YahooFinance.Data
{

    public class IndustryInfo
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


        internal IndustryInfo() { }
    }

    public class IndustryInfoCollection
    {
        public IndustryInfo[] Items { get; internal set; }


        internal IndustryInfoCollection() { }
    }



    public class IndustryCompany
    {
        public string Name { get; set; }
        public int IndustryComponentID { get; set; }
        public string ID { get; set; }
    }

}

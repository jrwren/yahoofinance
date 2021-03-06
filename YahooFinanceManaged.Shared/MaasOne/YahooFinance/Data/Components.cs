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
    public class IndexComponentsItem : MembershipItem
    {
        public int? Volume { get; set; }
       

        public IndexComponentsItem() { }
    }


    public class MembershipItem : IQuotePrice
    {
        public double? ChangeInPercent { get; set; }

        public string ID { get; set; }
       
        public double? LastTradePriceOnly { get; set; }
       
        public string Name { get; set; }
       

        public MembershipItem() { }
    }
}

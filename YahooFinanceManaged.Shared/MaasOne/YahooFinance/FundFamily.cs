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

namespace MaasOne.YahooFinance
{
    public class FundFamily
    {
        public FundFamily(string id, string name) { this.ID = id; this.Name = name; }


        internal FundFamily(System.Resources.WorldMarketFamily family)
        {
            this.ID = family.ID;
            this.Name = family.Name;
        }



        public string ID { get; private set; }

        public string Name { get; private set; }



        public override bool Equals(object obj)
        {
            return obj != null && obj is FundFamily && this.ID.Equals(((FundFamily)obj).ID) && this.Name.Equals(((FundFamily)obj).Name);
        }
    }
}

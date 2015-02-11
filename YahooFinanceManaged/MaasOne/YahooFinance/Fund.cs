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

namespace MaasOne.YahooFinance
{

    public class FundCategory
    {

        public string ID { get; private set; }
        public string Name { get; private set; }

        public FundCategory(string curID, string curDesc)
        {
            this.ID = curID.ToUpper();
            this.Name = curDesc;
        }
        internal FundCategory(System.Resources.WorldMarketCategory orig)
        {
            this.ID = orig.ID;
            this.Name = orig.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is FundCategory)
            {
                return this.ID.Equals(((FundCategory)obj).ID) && this.Name.Equals(((FundCategory)obj).Name);
            }
            return false;
        }

        public override string ToString()
        {
            if (this.Name != string.Empty)
            {
                return this.Name;
            }
            else
            {
                return this.ID;
            }
        }

    }

    public class FundFamily
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public FundFamily(string id, string name) { this.ID = id; this.Name = name; }
        internal FundFamily(System.Resources.WorldMarketFamily family)
        {
            this.ID = family.ID;
            this.Name = family.Name;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is FundFamily && this.ID.Equals(((FundFamily)obj).ID) && this.Name.Equals(((FundFamily)obj).Name);
        }

    }

}

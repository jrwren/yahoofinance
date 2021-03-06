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
    public class Industry
    {
        public Industry(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }


        internal Industry(System.Resources.WorldMarketIndustry orig, WorldMarket wm) : this(orig.ID, orig.Name) { }



        public int ID { get; private set; }

        public string Name { get; private set; }



        public override bool Equals(object obj)
        {
            return obj != null && obj is Industry && ((Industry)obj).ID.Equals(this.ID) && ((Industry)obj).Name.Equals(this.Name);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

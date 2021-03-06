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
using System.Text;


namespace MaasOne.YahooFinance
{
    /// <summary>
    /// Provides data of a Yahoo! listed currency.
    /// </summary>
    /// <remarks></remarks>
    public class CurrencyInfo
    {

        /// <summary>
        /// The currency ID.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID { get; private set; }
        /// <summary>
        /// The currency name/description.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name { get; private set; }

        /// <summary>
        /// Defaul constructor
        /// </summary>
        /// <remarks></remarks>
        public CurrencyInfo(string curID, string curDesc)
        {
            if (string.IsNullOrEmpty(curID)) throw new ArgumentException("ID is NULL.", "curID");
            this.ID = curID.ToUpper();
            this.Name = curDesc;
        }
        internal CurrencyInfo(System.Resources.WorldMarketCurrency orig)
        {
            this.ID = orig.ID;
            this.Name = orig.Name;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is CurrencyInfo && this.ID.Equals(((CurrencyInfo)obj).ID) && this.Name.Equals(((CurrencyInfo)obj).Name);
        }

        /// <summary>
        /// Returns Description and ID.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            if (this.Name != string.Empty)
            {
                return this.Name + " (" + this.ID.ToString() + ")";
            }
            else
            {
                return this.ID.ToString();
            }
        }

    }
}

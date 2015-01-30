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
using System.Text;


namespace MaasOne.Finance.Yahoo
{
    /// <summary>
    /// Provides the description of a currency of YahooManaged.Finance.Currency.
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
            this.ID = curID.ToUpper();
            this.Name = curDesc;
        }
        internal CurrencyInfo(MaasOne.Resources.WorldMarketCurrency orig)
        {
            this.ID = orig.ID;
            this.Name = orig.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is CurrencyInfo)
            {
                return this.ID.Equals(((CurrencyInfo)obj).ID) && this.Name.Equals(((CurrencyInfo)obj).Name);
            }
            return false;
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

// **************************************************************************************************
// **  
// **  Yahoo! Finance Managed
// **  
// **  Copyright @ Marius Häusler 2015
// **  
// **  Licensed under GNU Lesser General Public License (LGPL) (Version 2.1, February 1999).
// **  
// **  License: https://www.gnu.org/licenses/old-licenses/lgpl-2.1.txt
// **  
// **  Project: https://yahoofinance.codeplex.com/
// **  
// **  Contact: maasone@live.com
// **  
// **************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace MaasOne.YahooFinance
{
    /// <summary>
    /// Stores informations about a country.
    /// </summary>
    /// <remarks></remarks>
    public class CountryInfo
    {
        /// <summary>
        /// The country ID.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID { get; private set; }

        /// <summary>
        /// The name of the country
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name { get; private set; }

        /// <summary>
        /// The currency of this country
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CurrencyInfo Currency { get; private set; }

        public CountryInfo(string id, string name, CurrencyInfo cur)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID is NULL.", "id");
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name is NULL.", "name");
            if (cur == null) throw new ArgumentException("Currency is NULL.", "cur");
            this.ID = id;
            this.Name = name;
            this.Currency = cur;
        }
        internal CountryInfo(MaasOne.Resources.WorldMarketCountry orig, WorldMarket wm) : this(orig.ID, orig.Name, wm.GetCurrencyInfoFromID(orig.Currency)) { }

        public override bool Equals(object obj)
        {
            return obj != null && obj is CountryInfo && this.ID.Equals(((CountryInfo)obj).ID) && this.Name.Equals(((CountryInfo)obj).Name) && this.Currency.Equals(((CountryInfo)obj).Currency);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

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
        /// The currency of this country
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CurrencyInfo Currency { get; private set; }
        /// <summary>
        /// The name of the country
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name { get; private set; }

        public CountryInfo(string id, string name, CurrencyInfo cur)
        {
            this.ID = id;
            this.Name = name;
            this.Currency = cur;
        }
        internal CountryInfo(MaasOne.Resources.WorldMarketCountry orig, WorldMarket wm)
        {
            this.ID = orig.ID;
            this.Name = orig.Name;
            this.Currency = wm.GetCurrencyFromID(orig.Currency);
            if (this.Currency == null) throw new ArgumentException("orig.Currency", "The passed currency ID could not be found.");
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is CountryInfo)
            {
                return this.ID.Equals(((CountryInfo)obj).ID) && this.Name.Equals(((CountryInfo)obj).Name) && this.Currency.Equals(((CountryInfo)obj).Currency);
            }
            return false;
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}

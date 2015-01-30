// ******************************************************************************
// ** 
// **  Yahoo! Managed
// **  Written by Marius Häusler 2012
// **  It would be pleasant, if you contact me when you are using this code.
// **  Contact: YahooFinanceManaged@gmail.com
// **  Project Home: http://code.google.com/p/yahoo-finance-managed/
// **  
// ******************************************************************************
// **  
// **  Copyright 2012 Marius Häusler
// **  
// **  Licensed under the Apache License, Version 2.0 (the "License");
// **  you may not use this file except in compliance with the License.
// **  You may obtain a copy of the License at
// **  
// **    http://www.apache.org/licenses/LICENSE-2.0
// **  
// **  Unless required by applicable law or agreed to in writing, software
// **  distributed under the License is distributed on an "AS IS" BASIS,
// **  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// **  See the License for the specific language governing permissions and
// **  limitations under the License.
// ** 
// ******************************************************************************
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

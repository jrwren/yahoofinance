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

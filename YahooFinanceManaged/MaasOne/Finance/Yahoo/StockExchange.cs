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
using System.ComponentModel;


namespace MaasOne.Finance.Yahoo
{
    /// <summary>
    /// Class for managing stock exchange information. 
    /// </summary>
    /// <remarks></remarks>
    public class StockExchange
    {

        /// <summary>
        /// The ID of the exchange
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>If the ID is in WorldMarket.DefaultStockExchanges, properties will be setted automatically</remarks>
        public string ID { get; private set; }
        /// <summary>
        /// The ending string for stock IDs
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>If the suffix is in DefaultStockExchanges, properties will get automatically</remarks>
        public string Suffix { get; private set; }
        /// <summary>
        /// The name of the exchange
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name { get; private set; }
        public CountryInfo Country { get; private set; }
        public TradingTimeInfo TradingTime { get; private set; }
        /// <summary>
        ///The indices of the StockExchange
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>

        public List<YIndexID> Indices { get; private set; }

        internal List<string> Tags { get; private set; }

        private StockExchange()
        {
            this.Indices = new List<YIndexID>();
            this.Tags = new List<string>();
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="suffix"></param>
        /// <remarks></remarks>
        public StockExchange(string id, string suffix, string name, CountryInfo country, TradingTimeInfo tradeTime)
            : this()
        {
            if (id.IsNullOrWhiteSpace()) throw new ArgumentNullException("id", "The ID is empty.");
            if (country == null) throw new ArgumentNullException("country", "The country is null.");
            if (tradeTime == null) throw new ArgumentNullException("tradeTime", "The trade time is null.");

            this.ID = id;
            this.Name = name;
            this.Suffix = suffix;
            this.Country = country;
            this.TradingTime = tradeTime;
        }
        internal StockExchange(MaasOne.Resources.WorldMarketStockExchange orig, WorldMarket wm)
            : this()
        {
            this.ID = orig.ID;
            this.Name = orig.Name;
            this.Country = wm.GetCountryFromID(orig.Country);
            if (this.Country == null) throw new ArgumentException("orig.Country", "The passed country ID could not be found.");
            this.Suffix = orig.Suffix;
            this.TradingTime = new TradingTimeInfo(orig.TradingTime);
        }


        /// <summary>
        /// Returns the name of the stock exchange
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.Name;
        }

    }


}

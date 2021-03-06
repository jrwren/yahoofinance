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
using System.ComponentModel;


namespace MaasOne.YahooFinance
{
    /// <summary>
    /// Class for managing stock exchange information. 
    /// </summary>
    /// <remarks></remarks>
    public class StockExchange
    {
        /// <summary>
        /// Gets the ID of the stock exchange.
        /// </summary>
        /// <remarks>If the ID is in WorldMarket.DefaultStockExchanges, properties will be setted automatically</remarks>
        public string ID { get; private set; }

        /// <summary>
        /// Gets the ending string for stock IDs.
        /// </summary>
        /// <remarks>If the suffix is in DefaultStockExchanges, properties will get automatically</remarks>
        public string Suffix { get; private set; }

        /// <summary>
        /// Gets the name of the stock exchange.
        /// </summary>
        public string Name { get; private set; }

        public CountryInfo Country { get; private set; }

        public TradingTimeInfo TradingTime { get; private set; }

        /// <summary>
        ///Gets the indices of the stock exchange.
        /// </summary>
        public List<YID> Indices { get; private set; }

        internal List<string> Tags { get; private set; }

        private StockExchange()
        {
            this.Indices = new List<YID>();
            this.Tags = new List<string>();
        }


        public StockExchange(string id, string suffix, string name, CountryInfo country, TradingTimeInfo tradeTime)
            : this()
        {
            if (id.IsNullOrWhiteSpace()) throw new ArgumentNullException("id", "The ID is empty.");
            if (country == null) throw new ArgumentNullException("country", "The country is NULL.");
            if (tradeTime == null) throw new ArgumentNullException("tradeTime", "The trade time is NULL.");

            this.ID = id;
            this.Name = name;
            this.Suffix = suffix;
            this.Country = country;
            this.TradingTime = tradeTime;
        }

        internal StockExchange(System.Resources.WorldMarketStockExchange orig, WorldMarket wm)
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
        /// Returns the name of the stock exchange.
        /// </summary>
        public override string ToString()
        {
            return this.Name;
        }

    }
}

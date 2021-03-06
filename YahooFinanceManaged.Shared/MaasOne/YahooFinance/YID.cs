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
using MaasOne.YahooFinance;


namespace MaasOne.YahooFinance
{
    /// <summary>
    /// Stores informations of a financial product. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class YID : IID
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string mID = string.Empty;
        private string mName = string.Empty;
        private string mIndustry = string.Empty;
        private StockExchange mStockExchange = null;
        private ISIN mISIN = null;

        private FinanceType mType = FinanceType.Any;
        /// <summary>
        /// The full ID with suffix 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID
        {
            get { return mID; }
        }
        /// <summary>
        /// The ID without suffix
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string BaseID
        {
            get
            {
                int index = mID.LastIndexOf('.');
                if (index == -1)
                {
                    return mID;
                }
                else
                {
                    return mID.Substring(0, index);
                }
            }
        }
        /// <summary>
        /// The suffix of the ID
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Suffix
        {
            get
            {
                int index = mID.LastIndexOf('.');
                if (index == -1)
                {
                    return string.Empty;
                }
                else
                {
                    return mID.Substring(index);
                }
            }
        }
        /// <summary>
        /// The name of the security
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        /// <summary>
        /// The name of the industry
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Industry
        {
            get { return mIndustry; }
            set { mIndustry = value; }
        }
        /// <summary>
        /// Informations about the stock exchange where the stock is traded
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public StockExchange StockExchange
        {
            get { return mStockExchange; }
            set { mStockExchange = value; }
        }
        /// <summary>
        /// The International Securities Identification Number
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ISIN ISIN
        {
            get { return mISIN; }
            set { mISIN = value; }
        }
        /// <summary>
        /// The type of the security
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FinanceType Type
        {
            get { return mType; }
            set { mType = value; }
        }


        /// <summary>
        /// Creates a new instance by an ID
        /// </summary>
        /// <param name="id"></param>
        /// <remarks></remarks>
        public YID(string id)
        {
            if (id.IsNullOrWhiteSpace()) throw new ArgumentNullException("id", "The ID is NULL.");
            mID = id.Trim().ToUpper();
            if (this.ID.StartsWith("^")) this.Type = FinanceType.Index;
        }
        internal YID(System.Resources.WorldMarketIndex orig, WorldMarket wm)
            : this(orig.ID)
        {
            this.Name = orig.Name;
            this.Type = FinanceType.Index;
            this.StockExchange = wm.GetStockExchangeFromID(orig.StockExchange);
            if (this.StockExchange == null) throw new ArgumentException("The stock exchange ID could not be found.", "StockExchangeID");
            this.StockExchange.Indices.Add(this);
        }


        /*		/// <summary>
                /// Creates a new instance from an IDSearchResult
                /// </summary>
                /// <param name="searchResult"></param>
                /// <remarks></remarks>
                public YID(IDSearchData searchResult)
                {
                    if (searchResult != null)
                    {
                        this.SetID(searchResult.ID, false);
                        this.Name = searchResult.Name;
                        this.Industry = searchResult.Industry;
                        this.Type = searchResult.Type;

                        string exc = searchResult.Exchange.Replace("N/A", "").Trim();
                        if (exc != string.Empty)
                        {
                            StockExchange se = WorldMarket.GetStockExchangeByID(exc);
                            if (se == null)
                                se = WorldMarket.GetStockExchangeByName(exc);
                            if (se != null)
                            {
                                this.StockExchange = se;
						
                            }
                            else
                            {
                                se = WorldMarket.GetStockExchangeBySuffix(this.ID);
                                if (se != null)
                                {
                                    this.StockExchange = se;
							
                                }
                                else
                                {
                                    CountryInfo cnt = WorldMarket.GetDefaultCountry(Country.US);
                                    TradingTimeInfo tti = new TradingTimeInfo(0, new DayOfWeek[] {
                                    DayOfWeek.Monday,
                                    DayOfWeek.Tuesday,
                                    DayOfWeek.Wednesday,
                                    DayOfWeek.Thursday,
                                    DayOfWeek.Friday
                                }, null, new DateTime(), new TimeSpan(23, 59, 59), -5);
                                    this.StockExchange = new StockExchange(searchResult.Exchange, this.Suffix, searchResult.Exchange, cnt, tti);
                                }
                            }
                        }

                        if (searchResult.ISIN != null && searchResult.ISIN.Value.Replace("N/A", "").Trim() != string.Empty)
                        {
                            try
                            {
                                this.ISIN = new ISIN(searchResult.ISIN.Value);
                            }
                            catch (ArgumentException ex)
                            {
                                this.ISIN = null;
                            }
                        }

                    }
                    else
                    {
                        throw new ArgumentException("The passed result is null", "searchResult");
                    }
                }
        */
     
        /// <summary>
        /// Returns the URL of the Yahoo! RSS news feed.
        /// </summary>
        /// <param name="culture">The culture of the feed.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string RssNewsURL(Culture culture = null)
        {
            Culture cult = culture;
            if (cult == null)
                cult = Cultures.UnitedStates_English;
            return "http://feeds.finance.yahoo.com/rss/2.0/headline?s=" + YFHelper.CleanYqlParam(mID) + YFHelper.CultureToParameters(cult);
        }
        /// <summary>
        /// Returns the URL of the Yahoo! RSS Blog feed.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string RssFinancialBlogsURL()
        {
            return "http://finance.yahoo.com/rss/blog?s=" + YFHelper.CleanYqlParam(mID);
        }

        /// <summary>
        /// Returns the full ID of the stock.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return mID;
        }

    }
}

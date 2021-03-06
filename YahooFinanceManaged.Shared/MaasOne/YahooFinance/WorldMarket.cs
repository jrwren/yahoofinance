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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MaasOne.YahooFinance
{
    /// <summary>
    /// Stores default world market informations
    /// </summary>
    /// <remarks></remarks>
    public class WorldMarket
    {

        private List<Sector> mSectors = new List<Sector>();
        private List<Industry> mIndustries = new List<Industry>();
        private List<CurrencyInfo> mCurrencies = new List<CurrencyInfo>();
        private List<CountryInfo> mCountries = new List<CountryInfo>();
        private List<StockExchange> mStockExchanges = new List<StockExchange>();
        private List<YID> mIndices = new List<YID>();
        private List<FundCategory> mFundCategories = new List<FundCategory>();
        private List<FundFamily> mFundFamilies = new List<FundFamily>();


        private WorldMarket() { }



        public static WorldMarket LoadInternalSource() { return LoadTextSource(Properties.Resources.market); }

        /// <summary>
        /// Loads the WorldMarket data from text source.
        /// </summary>
        /// <param name="xmlText">The UTF-8 text source.</param>
        public static WorldMarket LoadTextSource(string xmlText)
        {
            try
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings();
#if !(NETFX_CORE || SILVERLIGHT)
                XmlSchema sch = XmlSchema.Read(new System.IO.StringReader(Properties.Resources.market_schema), null);
                readerSettings.Schemas.Add(sch);
                readerSettings.ValidationType = ValidationType.Schema;
#endif
                XmlReader market = XmlReader.Create(new System.IO.StringReader(xmlText), readerSettings);
                XmlSerializer ser = new XmlSerializer(typeof(System.Resources.WorldMarket));

                System.Resources.WorldMarket wm = (System.Resources.WorldMarket)ser.Deserialize(market);

                WorldMarket result = new WorldMarket();

                foreach (var s in wm.Sectors) { result.mSectors.Add(new Sector(s)); }
                foreach (var s in wm.Industries) { result.mIndustries.Add(new Industry(s, result)); }
                foreach (var s in wm.Currencies) { result.mCurrencies.Add(new CurrencyInfo(s)); }
                foreach (var s in wm.Countries) { result.mCountries.Add(new CountryInfo(s, result)); }
                foreach (var s in wm.StockExchanges) { result.mStockExchanges.Add(new StockExchange(s, result)); }
                foreach (var s in wm.Indices) { result.mIndices.Add(new YID(s, result)); }
                foreach (var s in wm.FundCategories) { result.mFundCategories.Add(new FundCategory(s)); }
                foreach (var s in wm.FundFamilies) { result.mFundFamilies.Add(new FundFamily(s)); }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("The XML text is not valid. See InnerException for more details.", ex);
            }
        }



        public FundCategory[] GetAllFundCategories() { return mFundCategories.ToArray(); }
        public FundFamily[] GetAllFundFamilies() { return mFundFamilies.ToArray(); }

        public Sector[] GetAllSectors() { return mSectors.ToArray(); }
        public Sector GetSectorFromID(int id)
        {
            foreach (Sector s in mSectors) { if (s.ID.Equals(id)) return s; }
            return null;
        }
        public Sector GetSectorFromIndustry(int industryID)
        {
            return this.GetSectorFromID((int)Math.Floor(industryID / 100.0));
        }

        public Industry[] GetAllIndustries() { return mIndustries.ToArray(); }
        public Industry GetIndustryFromID(int id)
        {
            foreach (Industry s in mIndustries) { if (s.ID.Equals(id)) return s; }
            return null;
        }

        public YCurrencyID GetYCurrencyIDFromString(string id)
        {
            string idStr = id.ToUpper();
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\A[A-Z][A-Z][A-Z][A-Z][A-Z][A-Z]=X\z");
            if (idStr.Length == 8 && regex.Match(idStr).Success)
            {
                CurrencyInfo b = null;
                CurrencyInfo dep = null;
                string baseStr = idStr.Substring(0, 3);
                string depStr = idStr.Substring(3, 3);
                foreach (CurrencyInfo cur in mCurrencies)
                {
                    if (baseStr == cur.ID) { b = new CurrencyInfo(cur.ID, cur.Name); }
                    else if (depStr == cur.ID) { dep = new CurrencyInfo(cur.ID, cur.Name); }

                    if (b != null && dep != null) { return new YCurrencyID(b, dep); }
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        public CurrencyInfo GetCurrencyInfoFromID(string id)
        {
            foreach (CurrencyInfo cur in mCurrencies) { if (cur.ID.Equals(id)) { return cur; } }
            return null;
        }

        /// <summary>
        /// A list of all available indices
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public YID[] GetAllIndices()
        {
            return mIndices.ToArray();
        }

        public CountryInfo GetCountryFromID(string cnt)
        {
            foreach (CountryInfo defaultCnt in mCountries)
            {
                if (defaultCnt.ID == cnt)
                {
                    return defaultCnt;
                }
            }
            return null;
        }

        public StockExchange[] GetAllStockExchanges() { return mStockExchanges.ToArray(); }
        /// <summary>
        ///  Tries to return a StockExchange by StockExchange ID string.
        /// </summary>
        /// <param name="id">The non-case sensitive ID of the stock exchange. E.g. "DJI" or "gEr"</param>
        /// <returns>The confirmed stock exchange or null.</returns>
        /// <remarks></remarks>
        public StockExchange GetStockExchangeFromID(string id)
        {
            if (mStockExchanges != null & id != string.Empty)
            {
                string n = id.ToUpper();
                foreach (StockExchange se in mStockExchanges)
                {
                    if (se.ID == id)
                    {
                        return se;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Tries to return a StockExchange by an YID String.
        /// </summary>
        /// <param name="suffix">The non-case sensitive ID or Suffix of ID. E.g. "BAS.DE" or ".DE"</param>
        /// <returns>The confirmed stock exchange or null.</returns>
        /// <remarks></remarks>
        public StockExchange GetStockExchangeFromSuffix(string suffix)
        {
            if (mStockExchanges != null & suffix != string.Empty)
            {
                int index = suffix.LastIndexOf('.');
                if (index >= 0 & suffix.Length >= index)
                {
                    string suffStr = suffix.Substring(index, suffix.Length - index).ToUpper();
                    foreach (StockExchange se in mStockExchanges)
                    {
                        if (se.Suffix == suffStr)
                            return se;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Tries to return a StockExchange by the StockExchange's name
        /// </summary>
        /// <param name="name">A non-case sensitive name or part of name of a stock exchange. The first name that contains the string will be returned.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public StockExchange GetStockExchangeFromName(string name)
        {
            if (mStockExchanges != null & name != string.Empty)
            {
                string n = name.ToLower();
                foreach (StockExchange se in mStockExchanges)
                {
                    if (se.Name.ToLower().IndexOf(n) > -1)
                    {
                        return se;
                    }
                    else
                    {
                        foreach (string tag in se.Tags)
                        {
                            if (n == tag.ToLower())
                                return se;
                        }
                    }
                }
            }
            return null;
        }

    }
}

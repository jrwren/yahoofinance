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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MaasOne.Finance.Yahoo
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
        private List<YIndexID> mIndices = new List<YIndexID>();
        private List<Screener.FundCategory> mFundCategories = new List<Screener.FundCategory>();


        public WorldMarket() { this.LoadInternalSource(); }


        public void LoadInternalSource() { this.LoadTextSource(Properties.Resources.market); }
#if !(PORTABLE40 || PORTABLE45)
        public void LoadFileSource(string path)
        {
            if (System.IO.File.Exists(path) == false) throw new System.IO.FileNotFoundException("Could not find file.", path);
            this.LoadTextSource(System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8));
        }
#endif
#if !(PORTABLE40 || PORTABLE45)
        public void LoadWebSource(Uri source)
        {
            if (source == null) throw new ArgumentNullException("source", "Source is null.");

            var txt = string.Empty;
            if (source.Scheme == "http" || source.Scheme == "https")
            {
                var resp = new System.Net.DownloadClient<string>().Download(new System.Net.StringQuery(source));
                if (resp.Connection.State == System.Net.ConnectionState.Success)
                {
                    txt = resp.Result;
                }
                else
                {
                    throw resp.Connection.Exception;
                }
            }
            this.LoadTextSource(txt);
        }
#endif
#if (PORTABLE40 || PORTABLE45)
        internal void LoadTextSource(string xmlText)
#else
        public void LoadTextSource(string xmlText)
#endif
        {
            try
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings();
#if !(PORTABLE40 || PORTABLE45)
                XmlSchema sch = XmlSchema.Read(new System.IO.StringReader(Properties.Resources.market_schema), null);
                readerSettings.Schemas.Add(sch);
                readerSettings.ValidationType = ValidationType.Schema;
#endif
                XmlReader market = XmlReader.Create(new System.IO.StringReader(xmlText), readerSettings);
                XmlSerializer ser = new XmlSerializer(typeof(MaasOne.Resources.WorldMarket));

                MaasOne.Resources.WorldMarket wm = (MaasOne.Resources.WorldMarket)ser.Deserialize(market);

                mSectors.Clear();
                mIndustries.Clear();
                mCurrencies.Clear();
                mCountries.Clear();
                mStockExchanges.Clear();
                mIndices.Clear();
                mFundCategories.Clear();

                foreach (var s in wm.Sectors) { mSectors.Add(new Sector(s)); }
                foreach (var s in wm.Industries) { mIndustries.Add(new Industry(s, this)); }
                foreach (var s in wm.Currencies) { mCurrencies.Add(new CurrencyInfo(s)); }
                foreach (var s in wm.Countries) { mCountries.Add(new CountryInfo(s, this)); }
                foreach (var s in wm.StockExchanges) { mStockExchanges.Add(new StockExchange(s, this)); }
                foreach (var s in wm.Indices) { mIndices.Add(new YIndexID(s, this)); }
                foreach (var s in wm.FundCategories) { mFundCategories.Add(new Screener.FundCategory(s)); }
            }
            catch (Exception ex)
            {
                throw new Exception("The XML text is not valid. See InnerException for more details.", ex);
            }
        }

        public Screener.FundCategory[] GetAllFundCategories() { return mFundCategories.ToArray(); }

        public Sector[] GetAllSectors() { return mSectors.ToArray(); }
        public Sector GetSectorFromID(int id)
        {
            foreach (Sector s in mSectors) { if (s.ID.Equals(id)) return s; }
            return null;
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
                    if (baseStr == cur.ID)
                    {
                        b = new CurrencyInfo(cur.ID, cur.Name);
                    }
                    else if (depStr == cur.ID)
                    {
                        dep = new CurrencyInfo(cur.ID, cur.Name);
                    }
                    if (b != null & dep != null)
                    {
                        return new YCurrencyID(b, dep);
                    }
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        public CurrencyInfo GetCurrencyFromID(string id)
        {
            foreach (CurrencyInfo cur in mCurrencies)
            {
                if (cur.ID == id)
                {
                    return cur;
                }
            }
            return null;
        }

        /// <summary>
        /// A list of all available indices
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public YIndexID[] GetAllIndices()
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

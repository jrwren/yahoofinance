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
using MaasOne.Finance.Yahoo.Web;
using MaasOne.Finance.Yahoo.Screener;
using MaasOne.Finance.Yahoo.Data;
using Newtonsoft.Json.Linq;


namespace MaasOne.Finance.Yahoo
{

    internal static class YFHelper
    {

        public static string CultureToParameters(Culture cult)
        {
            if (cult == null)
                throw new ArgumentNullException("cult", "Culture must have a value");
            return string.Format("&region={0}&lang={1}-{2}", cult.Country.ToString(), cult.Language.ToString(), cult.Country.ToString());
        }

        public static string ServerString(YahooServer server)
        {
            switch (server)
            {
                case YahooServer.Argentina:
                    return "ar.";
                case YahooServer.Australia:
                    return "au.";
                case YahooServer.Brazil:
                    return "br.";
                case YahooServer.Canada:
                    return "ca.";
                case YahooServer.France:
                    return "fr.";
                case YahooServer.Germany:
                    return "de.";
                case YahooServer.HongKong:
                    return "hk.";
                case YahooServer.India:
                    return "in.";
                case YahooServer.Italy:
                    return "it.";
                case YahooServer.Korea:
                    return "kr.";
                case YahooServer.Mexico:
                    return "mx.";
                case YahooServer.NewZealand:
                    return "nz.";
                case YahooServer.Singapore:
                    return "sg.";
                case YahooServer.Spain:
                    return "es.";
                case YahooServer.UK:
                    return "uk.";
                default:
                    return string.Empty;
            }
        }


        public static IEnumerable<string> IIDsToStrings(IEnumerable<IID> idList)
        {
            List<string> lst = new List<string>();
            if (idList != null)
            {
                foreach (IID id in idList)
                {
                    if (id != null && id.ID != string.Empty)
                        lst.Add(id.ID);
                }
            }
            return lst;
        }

        public static Sector[] SectorEnumToArray(IEnumerable<Sector> values)
        {
            List<Sector> lst = new List<Sector>();
            if (values != null)
            {
                lst.AddRange(values);
            }
            return lst.ToArray();
        }


        public static string[] CleanIDfromAT(IEnumerable<string> enm)
        {
            if (enm != null)
            {
                List<string> lst = new List<string>();
                foreach (string id in enm)
                {
                    lst.Add(CleanIndexID(id));
                }
                return lst.ToArray();
            }
            else
            {
                return null;
            }
        }
        public static string CleanIndexID(string id)
        {
            return id.Replace("@", "");
        }

        public static double GetStringMillionFactor(string s)
        {
            if (s.EndsWith("T") || s.EndsWith("K"))
            {
                return 1.0 / 1000;
            }
            else if (s.EndsWith("M"))
            {
                return 1;
            }
            else if (s.EndsWith("B"))
            {
                return 1000;
            }
            else
            {
                return 0;
            }
        }
        public static double? GetMillionValue(string s)
        {
            double? v = MyHelper.ParseDouble(s.TrimEnd().Substring(0, s.Length - 1));
            if (v.HasValue) v = v.Value * GetStringMillionFactor(s);
            return v;
        }

        public static string MarketQuotesRankingTypeString(IndustryQuoteProperty rankedBy)
        {
            switch (rankedBy)
            {
                case IndustryQuoteProperty.Name:
                    return "coname";
                case IndustryQuoteProperty.DividendYieldPercent:
                    return "yie";
                case IndustryQuoteProperty.LongTermDeptToEquity:
                    return "qto";
                case IndustryQuoteProperty.MarketCapitalizationInMillion:
                    return "mkt";
                case IndustryQuoteProperty.NetProfitMarginPercent:
                    return "qpm";
                case IndustryQuoteProperty.OneDayPriceChangePercent:
                    return "pr1";
                case IndustryQuoteProperty.PriceEarningsRatio:
                    return "pee";
                case IndustryQuoteProperty.PriceToBookValue:
                    return "pri";
                case IndustryQuoteProperty.PriceToFreeCashFlow:
                    return "prf";
                case IndustryQuoteProperty.ReturnOnEquityPercent:
                    return "ttm";
                default:
                    return string.Empty;
            }
        }
        public static string MarketQuotesRankingDirectionString(System.ComponentModel.ListSortDirection dir)
        {
            if (dir == System.ComponentModel.ListSortDirection.Ascending)
            {
                return "u";
            }
            else
            {
                return "d";
            }
        }

        public static char ServerToDelimiter(YahooServer server)
        {
            if (server == YahooServer.Australia | server == YahooServer.Canada | server == YahooServer.HongKong | server == YahooServer.India | server == YahooServer.Korea | server == YahooServer.Mexico | server == YahooServer.Singapore | server == YahooServer.UK | server == YahooServer.USA)
            {
                return ',';
            }
            else
            {
                return ';';
            }
        }
        public static System.Globalization.CultureInfo ServerToCulture(YahooServer server)
        {
            switch (server)
            {
                case YahooServer.Argentina: return DefaultData.Cultures.Argentina;
                case YahooServer.Australia: return DefaultData.Cultures.Australia;
                case YahooServer.Brazil: return DefaultData.Cultures.Brazil;
                case YahooServer.Canada: return DefaultData.Cultures.Canada_English;
                case YahooServer.France: return DefaultData.Cultures.France;
                case YahooServer.Germany: return DefaultData.Cultures.Germany;
                case YahooServer.HongKong: return DefaultData.Cultures.HongKong;
                case YahooServer.India: return DefaultData.Cultures.India;
                case YahooServer.Italy: return DefaultData.Cultures.Italy;
                case YahooServer.Korea: return DefaultData.Cultures.Korea;
                case YahooServer.Mexico: return DefaultData.Cultures.Mexico;
                case YahooServer.NewZealand: return DefaultData.Cultures.NewZealand;
                case YahooServer.Singapore: return DefaultData.Cultures.Singapore;
                case YahooServer.Spain: return DefaultData.Cultures.Spain;
                case YahooServer.UK: return DefaultData.Cultures.UnitedKingdom;
                default: return MyHelper.ConverterCulture;
            }
        }

        public static string YqlUrl(string fields, string table, string whereParam, bool json, bool diag, IResultIndexSettings opt)
        {
            return YqlUrl(YqlStatement(fields, table, whereParam, opt), json, diag);
        }
        public static string YqlHtmlUrl(string whereParam, bool json, bool diag)
        {
            return YqlUrl(YqlHtmlStatement(whereParam), json, diag);
        }
        public static string YqlUrl(string statement, bool json, bool diag)
        {
            string format = "json";
            if (json == false)
                format = "xml";
            return "http://query.yahooapis.com/v1/public/yql?q=" + Uri.EscapeDataString(statement) + "&format=" + format + "&diagnostics=" + diag.ToString() + "&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
        }
        public static string YqlHtmlStatement(string whereParam) { return YqlStatement("*", "html", whereParam, null); }
        public static string YqlStatement(string fields, string table, string whereParam, IResultIndexSettings opt)
        {
            System.Text.StringBuilder stmt = new System.Text.StringBuilder();
            stmt.Append("select ");
            stmt.Append(fields);
            stmt.Append(" from ");
            stmt.Append(table);
            if (opt != null && opt.Count > 0)
            {
                stmt.Append("(");
                stmt.Append(opt.Index.ToString());
                stmt.Append(",");
                stmt.Append(opt.Count.ToString());
                stmt.Append(")");
            }
            if (whereParam.Trim() != string.Empty)
            {
                stmt.Append(" where ");
                stmt.Append(whereParam);
            }
            return stmt.ToString();
        }
        public static string CleanYqlParam(string id)
        {
            return id.Replace("\"", "").Replace("'", "").Trim();
        }





        private static Dictionary<ScreenerProperty, string> Tags { get; set; }
        public static string GetPropertyTag(ScreenerProperty prp) { return Tags[prp]; }
        public static bool ContainsPropertyTag(ScreenerProperty prp) { return Tags.ContainsKey(prp); }

        public static QuotesShortInfo HtmlConvertShortInfo(JToken shortInfoObject)
        {
            if (shortInfoObject != null)
            {
                try
                {
                    var sio = (JObject)shortInfoObject;
                    var si = new QuotesShortInfo();
                    si = new QuotesShortInfo();

                    var titleDiv = (JObject)shortInfoObject["div"][0];
                    var dataDiv = (JObject)shortInfoObject["div"][1];

                    var title = MyHelper.HtmlFirstContent(sio.FindFirst("h2"));
                    var id = title.Substring(title.IndexOf('(') + 1).Replace(")", "").Trim();
                    title = title.Substring(0, title.IndexOf('(')).Trim();
                    si.ID = id;
                    si.Title = title;

                    var actualDataDiv = dataDiv["div"][0];
                    var preAfterDataDiv = dataDiv["div"][1];

                    var lastTradeSpan = (JObject)actualDataDiv["span"][0];
                    var changeSpan = (JObject)actualDataDiv["span"][1];

                    string idID = id.ToLower();
                    si.LastTradePriceOnly = double.Parse(MyHelper.HtmlFirstContent(lastTradeSpan), MyHelper.ConverterCulture);

                    var changeObj = (JObject)changeSpan["span"][0];
                    var mult = ((JValue)changeObj["img"]["alt"]).ToString() == "Up" ? 1 : -1;
                    si.Change = mult * double.Parse(MyHelper.HtmlFirstContent(changeObj), MyHelper.ConverterCulture);

                    var changeInPercObj = (JObject)changeSpan["span"][1];
                    if (changeInPercObj["span"] != null) changeInPercObj = (JObject)changeInPercObj["span"];
                    si.ChangeInPercent = mult * double.Parse(MyHelper.HtmlFirstContent(changeInPercObj).Replace("(", "").Replace(")", "").Replace("%", ""), MyHelper.ConverterCulture);

                    return si;
                }
                catch (Exception ex)
                {

                    var x = "";

                }
            }
            return null;

        }

        public static LimitedPagination HtmlConvertPagination(string pagingText, System.Globalization.CultureInfo conv = null)
        {
            if (conv == null) conv = MyHelper.ConverterCulture;
            if (pagingText.IsNullOrWhiteSpace() == false)
            {
                int from = -1, to = -1, max = -1, temp;
                var parts = pagingText.Replace("&nbsp;", "").Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    if (int.TryParse(p, System.Globalization.NumberStyles.Any, conv, out temp))
                    {
                        if (from == -1) { from = temp - 1; }
                        else if (to == -1) { to = temp - 1; }
                        else if (max == -1) { max = temp; }
                    }
                }
                return new LimitedPagination(from, to - from + 1, max);
            }
            return null;
        }



        static YFHelper()
        {
            FillPropertyTags();
        }

        private static void FillPropertyTags()
        {
            Tags = new Dictionary<ScreenerProperty, string>();
            Tags.Add(ScreenerProperty.Symbol, "tk");
            Tags.Add(ScreenerProperty.Name, "nm");
            Tags.Add(ScreenerProperty.Industry, "sc");
            Tags.Add(ScreenerProperty.IndexMembership, "im");
            Tags.Add(ScreenerProperty.RetailPrice, "pr");
            Tags.Add(ScreenerProperty.AverageVolume, "av");
            Tags.Add(ScreenerProperty.MarketCapitalization, "mc");
            Tags.Add(ScreenerProperty.DividendYieldRatio, "dvy");
            Tags.Add(ScreenerProperty.ReturnPercent, "roy");
            Tags.Add(ScreenerProperty.Beta, "beta");
            Tags.Add(ScreenerProperty.Sales, "re");
            Tags.Add(ScreenerProperty.ProfitMargin, "pm");
            Tags.Add(ScreenerProperty.PriceEarningsRatio, "pe");
            Tags.Add(ScreenerProperty.PriceBookRatio, "pb");
            Tags.Add(ScreenerProperty.PriceSalesRatio, "ps");
            Tags.Add(ScreenerProperty.PEG, "peg");
            Tags.Add(ScreenerProperty.FiveYearGrowth, "grfy");
            Tags.Add(ScreenerProperty.Growth, "gr");
            Tags.Add(ScreenerProperty.AverageRec, "ar");

            Tags.Add(ScreenerProperty.Category, "cc");
            Tags.Add(ScreenerProperty.RankPercent, "proy");
            Tags.Add(ScreenerProperty.ManagerTenureYears, "mgrt");
            Tags.Add(ScreenerProperty.MorningstarRating, "rt");
            Tags.Add(ScreenerProperty.ReturnRating, "retr");
            Tags.Add(ScreenerProperty.RiskRating, "risr");
            Tags.Add(ScreenerProperty.NetAssets, "na");
            Tags.Add(ScreenerProperty.TurnoverPercent, "to");
            Tags.Add(ScreenerProperty.HoldingMedianMarketCap, "mmc");
            Tags.Add(ScreenerProperty.MinInvest, "mii");
            Tags.Add(ScreenerProperty.FrontLoadPercent, "mfl");
            Tags.Add(ScreenerProperty.ExpenseRatioPercent, "er");
            Tags.Add(ScreenerProperty.ReturnYearToDatePercent, "trytd");
            Tags.Add(ScreenerProperty.OneYearReturn, "troy");
            Tags.Add(ScreenerProperty.Annualized3YearReturnPercent, "trty");
            Tags.Add(ScreenerProperty.Annualized5YearreturnPercent, "trfy");

            Tags.Add(ScreenerProperty.Type, "t");
            Tags.Add(ScreenerProperty.Issue, "i");
            Tags.Add(ScreenerProperty.Price, "p");
            Tags.Add(ScreenerProperty.CouponPercent, "c");
            Tags.Add(ScreenerProperty.Maturity, "m");
            Tags.Add(ScreenerProperty.YTMPercent, "y");
            Tags.Add(ScreenerProperty.CurrentYieldPercent, "Y");
            Tags.Add(ScreenerProperty.FitchRating, "r");
            Tags.Add(ScreenerProperty.Callable, "l");

        }

    }
}



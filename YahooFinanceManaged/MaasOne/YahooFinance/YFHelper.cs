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
using MaasOne.YahooFinance.Web;
using MaasOne.YahooFinance.Data;
using Newtonsoft.Json.Linq;


namespace MaasOne.YahooFinance
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


        public static string[] IIDsToStrings(IEnumerable<IID> idList)
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
            return lst.ToArray();
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
            double? v = s.TrimEnd().Substring(0, s.Length - 1).ParseDouble();
            if (v.HasValue) v = v.Value * GetStringMillionFactor(s);
            return v;
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



        private static Dictionary<ETFSearchProperty, string> ETFTags = new Dictionary<ETFSearchProperty, string>();
        public static string GetETFPropertyTag(ETFSearchProperty prp) { return ETFTags[prp]; }
        public static ETFSearchProperty? GetETFProperty(string prp)
        {
            foreach (var kv in ETFTags) { if (kv.Value.Equals(prp)) return kv.Key; }
            return null;
        }


        private static Dictionary<ScreenerProperty, string> SreenerTags { get; set; }
        public static string GetScreenerPropertyTag(ScreenerProperty prp) { return SreenerTags[prp]; }
        public static bool ContainsScreenerPropertyTag(ScreenerProperty prp) { return SreenerTags.ContainsKey(prp); }

        public static QuotesBaseData HtmlConvertShortInfo(JToken shortInfoObject)
        {
            if (shortInfoObject != null)
            {
                try
                {
                    var sio = (JObject)shortInfoObject;
                    var si = new QuotesBaseData();
                    si = new QuotesBaseData();

                    var titleDiv = (JObject)shortInfoObject["div"][0];
                    var dataDiv = (JObject)shortInfoObject["div"][1];

                    var title = sio.FindFirst("h2").HtmlFirstContent();
                    var id = title.Substring(title.IndexOf('(') + 1).Replace(")", "").Trim();
                    title = title.Substring(0, title.IndexOf('(')).Trim();
                    si.ID = id;
                    si.Name = title;

                    var actualDataDiv = dataDiv["div"][0];
                    var preAfterDataDiv = dataDiv["div"][1];

                    var lastTradeSpan = (JObject)actualDataDiv["span"][0];
                    var changeSpan = (JObject)actualDataDiv["span"][1];

                    string idID = id.ToLower();
                    si.LastTradePriceOnly = double.Parse(lastTradeSpan.HtmlFirstContent(), MyHelper.ConverterCulture);

                    var changeObj = (JObject)changeSpan["span"][0];
                    var mult = ((JValue)changeObj["img"]["alt"]).ToString() == "Up" ? 1 : -1;
                    si.Change = mult * double.Parse(changeObj.HtmlFirstContent(), MyHelper.ConverterCulture);

                    var changeInPercObj = (JObject)changeSpan["span"][1];
                    if (changeInPercObj["span"] != null) changeInPercObj = (JObject)changeInPercObj["span"];
                    si.ChangeInPercent = mult * double.Parse(changeInPercObj.HtmlFirstContent().Replace("(", "").Replace(")", "").Replace("%", ""), MyHelper.ConverterCulture);

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
                if (from > -1 && to > -1 && max > -1) return new LimitedPagination(from, to - from + 1, max);
            }
            return null;
        }



        static YFHelper()
        {
            FillPropertyTags();
        }

        private static void FillPropertyTags()
        {
            SreenerTags = new Dictionary<ScreenerProperty, string>();
            SreenerTags.Add(ScreenerProperty.Symbol, "tk");
            SreenerTags.Add(ScreenerProperty.Name, "nm");
            SreenerTags.Add(ScreenerProperty.Industry, "sc");
            SreenerTags.Add(ScreenerProperty.IndexMembership, "im");
            SreenerTags.Add(ScreenerProperty.RetailPrice, "pr");
            SreenerTags.Add(ScreenerProperty.AverageVolume, "av");
            SreenerTags.Add(ScreenerProperty.MarketCapitalization, "mc");
            SreenerTags.Add(ScreenerProperty.DividendYieldRatio, "dvy");
            SreenerTags.Add(ScreenerProperty.ReturnPercent, "roy");
            SreenerTags.Add(ScreenerProperty.Beta, "beta");
            SreenerTags.Add(ScreenerProperty.Sales, "re");
            SreenerTags.Add(ScreenerProperty.ProfitMargin, "pm");
            SreenerTags.Add(ScreenerProperty.PriceEarningsRatio, "pe");
            SreenerTags.Add(ScreenerProperty.PriceBookRatio, "pb");
            SreenerTags.Add(ScreenerProperty.PriceSalesRatio, "ps");
            SreenerTags.Add(ScreenerProperty.PEG, "peg");
            SreenerTags.Add(ScreenerProperty.FiveYearGrowth, "grfy");
            SreenerTags.Add(ScreenerProperty.Growth, "gr");
            SreenerTags.Add(ScreenerProperty.AnalystRecommend, "ar");

            SreenerTags.Add(ScreenerProperty.Category, "cc");
            SreenerTags.Add(ScreenerProperty.RankPercent, "proy");
            SreenerTags.Add(ScreenerProperty.ManagerTenureYears, "mgrt");
            SreenerTags.Add(ScreenerProperty.MorningstarRating, "rt");
            SreenerTags.Add(ScreenerProperty.ReturnRating, "retr");
            SreenerTags.Add(ScreenerProperty.RiskRating, "risr");
            SreenerTags.Add(ScreenerProperty.NetAssets, "na");
            SreenerTags.Add(ScreenerProperty.TurnoverPercent, "to");
            SreenerTags.Add(ScreenerProperty.HoldingMedianMarketCap, "mmc");
            SreenerTags.Add(ScreenerProperty.MinInvest, "mii");
            SreenerTags.Add(ScreenerProperty.FrontLoadPercent, "mfl");
            SreenerTags.Add(ScreenerProperty.ExpenseRatioPercent, "er");
            SreenerTags.Add(ScreenerProperty.ReturnYearToDatePercent, "trytd");
            SreenerTags.Add(ScreenerProperty.OneYearReturn, "troy");
            SreenerTags.Add(ScreenerProperty.Annualized3YearReturnPercent, "trty");
            SreenerTags.Add(ScreenerProperty.Annualized5YearreturnPercent, "trfy");

            SreenerTags.Add(ScreenerProperty.Type, "t");
            SreenerTags.Add(ScreenerProperty.Issue, "i");
            SreenerTags.Add(ScreenerProperty.Price, "p");
            SreenerTags.Add(ScreenerProperty.CouponPercent, "c");
            SreenerTags.Add(ScreenerProperty.Maturity, "m");
            SreenerTags.Add(ScreenerProperty.YTMPercent, "y");
            SreenerTags.Add(ScreenerProperty.CurrentYieldPercent, "Y");
            SreenerTags.Add(ScreenerProperty.FitchRating, "r");
            SreenerTags.Add(ScreenerProperty.Callable, "l");

            ETFTags.Add(ETFSearchProperty.Name, "fname");
            ETFTags.Add(ETFSearchProperty.ID, "tkr");
            ETFTags.Add(ETFSearchProperty.Category, "cat");
            ETFTags.Add(ETFSearchProperty.Family, "ffly");
            ETFTags.Add(ETFSearchProperty.ReturnIntraday, "imkt");
            ETFTags.Add(ETFSearchProperty.Return3Month, "mkt3m");
            ETFTags.Add(ETFSearchProperty.ReturnYTD, "mktytd");
            ETFTags.Add(ETFSearchProperty.Return1Y, "mkt1y");
            ETFTags.Add(ETFSearchProperty.Return3Y, "mkt3y");
            ETFTags.Add(ETFSearchProperty.Return3MNAV, "nav3m");
            ETFTags.Add(ETFSearchProperty.ReturnYTDNAV, "navytd");
            ETFTags.Add(ETFSearchProperty.Return1YNAV, "nav1y");
            ETFTags.Add(ETFSearchProperty.Return5YNAV, "nav5y");
            ETFTags.Add(ETFSearchProperty.VolumeIntraday, "volint");
            ETFTags.Add(ETFSearchProperty.Volume3MAvg, "vol3m");
            ETFTags.Add(ETFSearchProperty.LastTrade, "ltrde");
            ETFTags.Add(ETFSearchProperty.YearHigh, "high52");
            ETFTags.Add(ETFSearchProperty.YearLow, "low52");
            ETFTags.Add(ETFSearchProperty.AvgMarketCap, "avgcap");
            ETFTags.Add(ETFSearchProperty.PortfolioPE, "ppe");
            ETFTags.Add(ETFSearchProperty.PortfolioPS, "pps");
            ETFTags.Add(ETFSearchProperty.PortfolioPCash, "pcash");
            ETFTags.Add(ETFSearchProperty.PortfolioPB, "pbook");
            ETFTags.Add(ETFSearchProperty.EarningsGrowth, "egrate");
            ETFTags.Add(ETFSearchProperty.Beta3Y, "riskb");
            ETFTags.Add(ETFSearchProperty.Alpha3Y, "riska");
            ETFTags.Add(ETFSearchProperty.RSqrd3Y, "risksq");
            ETFTags.Add(ETFSearchProperty.NetAssets, "nasset");
            ETFTags.Add(ETFSearchProperty.ExpenseRatio, "eratio");
            ETFTags.Add(ETFSearchProperty.AnnualTurnover, "tratio");
            ETFTags.Add(ETFSearchProperty.InceptDate, "idate");
        }

    }
}



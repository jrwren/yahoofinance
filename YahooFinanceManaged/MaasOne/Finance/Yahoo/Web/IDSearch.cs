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
using System.Net;
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.Finance.Yahoo.Data;



namespace MaasOne.Finance.Yahoo.Web
{



    /// <summary>
    /// Stores settings for Yahoo ID search
    /// </summary>
    /// <remarks></remarks>
    public class IDSearchQuery : Query<IDSearchResult>, ILookupQuery
    {

        /// <summary>
        /// 
        /// </summary>
        public string LookupText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public YahooServer Server { get; set; }
        /// <summary>
        /// The search will be limited to a special type or all
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public SecurityType Type { get; set; }
        /// <summary>
        /// The search will be limited to a special market or all
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IDSearchMarket Markets { get; set; }
        /// <summary>
        /// The ranking property of the result list
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Will be ignored if "GermanServer" is True</remarks>
        public IDSearchRankProperty RankedBy { get; set; }
        /// <summary>
        /// The ranking direction of the result list
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Will be ignored if "GermanServer" is True</remarks>
        public System.ComponentModel.ListSortDirection RankingDirection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IDSearchPagination Pagination { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public IDSearchQuery()
        {
            this.Pagination = new IDSearchPagination();
            this.Type = SecurityType.Any;
            this.Markets = IDSearchMarket.AllMarkets;
            this.RankedBy = IDSearchRankProperty.NoRanking;
            this.Server = YahooServer.USA;
        }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.LookupText.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("Query", "Query is empty."));
            }
            if (this.Pagination == null)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("Pagination", "Pagination is null."));
            }
        }
        protected override Uri CreateUrl()
        {
            System.Text.StringBuilder url = new System.Text.StringBuilder();
            url.Append("http://");
            url.Append(YFHelper.ServerString(this.Server));
            url.Append("finance.yahoo.com/lookup/");
            switch (this.Type)
            {
                case SecurityType.ETF: url.Append("etfs"); break;
                case SecurityType.Fund: url.Append("funds"); break;
                case SecurityType.Future: url.Append("futures"); break;
                case SecurityType.Index: url.Append("indices"); break;
                case SecurityType.Stock: url.Append("stocks"); break;
                case SecurityType.Warrant: url.Append("warrants"); break;
                case SecurityType.Currency: url.Append("currency"); break;
                default: url.Append("all"); break;
            }
            url.Append("?s=");
            url.Append(Uri.EscapeDataString(this.LookupText));
            url.Append("&t=");
            url.Append(this.GetSecurityTypeTag(this.Type));
            url.Append("&m=");
            switch (this.Markets)
            {
                case IDSearchMarket.France: url.Append("FR"); break;
                case IDSearchMarket.Germany: url.Append("DR"); break;
                case IDSearchMarket.Spain: url.Append("ES"); break;
                case IDSearchMarket.UK: url.Append("GB"); break;
                case IDSearchMarket.UsAndCanada: url.Append("US"); break;
                default: url.Append("ALL"); break;
            }
            url.Append("&r=");
            if (this.RankedBy != IDSearchRankProperty.NoRanking)
            {
                int rankNumber = Convert.ToInt32(this.RankedBy);
                if (this.RankingDirection == ListSortDirection.Descending) rankNumber += 1;
                url.Append(rankNumber.ToString());
            }
            url.Append("&b=");
            url.Append(this.Pagination.Start);

            return new Uri(url.ToString(), UriKind.RelativeOrAbsolute);
        }
        protected override IDSearchResult ConvertResult(System.IO.Stream stream)
        {
            var conv = YFHelper.ServerToCulture(this.Server);

            IDSearchResult result = new IDSearchResult();

            result.ResultsCount = new Dictionary<SecurityType, int>();
            List<IDSearchData> lst = new List<IDSearchData>();

            string htmlText = MyHelper.StreamToString(stream);
            JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);

            JObject tableObject = (JObject)htmlDoc.FindFirst("div", "id", "yfi_sym_lookup");

            JArray navObject = (JArray)tableObject["ul"]["li"];
            JObject resultObject = (JObject)tableObject.FindFirst("div", "id", "yfi_sym_results");
            JObject pagingObject = (JObject)tableObject.FindFirst("div", "id", "pagination");

            if (navObject == null || resultObject == null || pagingObject == null) throw new Exception("The received data seems to be incorrect.");

            #region "Navigation"

            if (navObject != null)
            {
                foreach (var li in navObject)
                {
                    var link = li["a"]["href"].ToString();
                    char c = link.Substring(link.LastIndexOf("t=") + 2)[0];

                    SecurityType type = SecurityType.Any;
                    foreach (SecurityType st in Enum.GetValues(typeof(SecurityType)))
                    {
                        if (this.GetSecurityTypeTag(st).Equals(c))
                        {
                            type = st;
                            break;
                        }
                    }
                    var s = li["a"]["em"].ToString().Trim();
                    s = s.Substring(s.LastIndexOf("(") + 1).Replace(")", "").Trim();

                    result.ResultsCount.Add(type, Convert.ToInt32(s));
                }
            }

            #endregion

            #region "Results"

            if (resultObject != null)
            {

                List<string> tableColumnNames = new List<string>();

                var headers = (JArray)resultObject["table"]["thead"]["tr"]["th"];
                tableColumnNames.Add("symbol");
                tableColumnNames.Add("name");
                bool hasISIN = headers[2]["content"].ToString().ToLower().Contains("isin");
                if (hasISIN) { tableColumnNames.Add("isin"); }
                else { tableColumnNames.Add("lasttrade"); }
                int l = headers.EnumToArray().Length;
                for (int i = 3; i < l; i++)
                {
                    if (hasISIN)
                    {
                        switch (i)
                        {
                            case 3: tableColumnNames.Add("lasttrade"); break;
                            case 4: tableColumnNames.Add("type"); break;
                            case 5: tableColumnNames.Add("exchange"); break;
                        }
                    }
                    else
                    {
                        string name = MyHelper.HtmlFirstContent(headers[i]).ToLower();
                        if (name.Contains("type")) { tableColumnNames.Add("type"); }
                        else if (name.Contains("industry")) { tableColumnNames.Add("industry"); }
                        else if (name.Contains("exchange")) { tableColumnNames.Add("exchange"); }
                    }
                }

                var rows = resultObject["table"]["tbody"]["tr"];
                foreach (var r in rows)
                {
                    var isd = new IDSearchData();


                    var parts = r["td"];
                    var cnt = 0;
                    for (int i = 0; i < tableColumnNames.Count; i++)
                    {
                        switch (tableColumnNames[i])
                        {
                            case "symbol": isd.ID = MyHelper.HtmlFirstContent(parts[i]); break;
                            case "name": isd.Name = MyHelper.HtmlFirstContent(parts[i]); break;
                            case "isin":
                                var isin = MyHelper.HtmlFirstContent(parts[i]);
                                if (string.IsNullOrEmpty(isin) == false)
                                {
                                    try { isd.ISIN = new ISIN(isin.Trim()); }
                                    catch { }
                                }
                                break;
                            case "lasttrade":
                                isd.LastTradePriceOnly = double.Parse(MyHelper.HtmlFirstContent(parts[i]), System.Globalization.NumberStyles.Any, conv);
                                break;
                            case "type": isd.SetType(MyHelper.HtmlFirstContent(parts[i])); break;
                            case "industry": isd.Industry = MyHelper.HtmlFirstContent(parts[i]); break;
                            case "exchange": isd.Exchange = MyHelper.HtmlFirstContent(parts[i]); break;
                        }
                        cnt++;
                    }

                    lst.Add(isd);
                }

                result.Items = lst.ToArray();
            }

            #endregion

            if (pagingObject != null)  result.Pagination = YFHelper.HtmlConvertPagination(MyHelper.HtmlFirstContent(pagingObject));

            return result;
        }
        public override Query<IDSearchResult> Clone()
        {
            return new IDSearchQuery()
            {
                LookupText = this.LookupText,
                Pagination = this.Pagination.Clone(),
                Server = this.Server,
                Type = this.Type,
                Markets = this.Markets,
                RankedBy = this.RankedBy,
                RankingDirection = this.RankingDirection
            };
        }

        private char GetSecurityTypeTag(SecurityType t)
        {
            return (t == SecurityType.Fund ? 'M' : char.ToUpper(t.ToString()[0]));
        }


    }

    public class IDSearchPagination : StartIndexPagination
    {
        public IDSearchPagination() { this.Count = 20; }
        public new IDSearchPagination Clone()
        {
            return new IDSearchPagination() { Start = this.Start };
        }
    }

    /// <summary>
    /// Stores the result data
    /// </summary>
    public class IDSearchResult
    {
        /// <summary>
        /// Gets the received ID search results.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IDSearchData[] Items { get; internal set; }
        public LimitedPagination Pagination { get; internal set; }
        public Dictionary<SecurityType, int> ResultsCount { get; internal set; }

        internal IDSearchResult() { }
    }


}

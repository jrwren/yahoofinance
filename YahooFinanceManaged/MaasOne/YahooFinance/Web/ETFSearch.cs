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
using MaasOne.Net;
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.YahooFinance.Data;



namespace MaasOne.YahooFinance.Web
{

    public enum ETFSearchView
    {
        ReturnMarket = 1,
        ReturnNAV = 2,
        TradingVolume = 3,
        Holdings = 4,
        Risk = 5,
        Operations = 6
    }


    /// <summary>
    /// Stores settings for Exchange Traded Funds search
    /// </summary>
    /// <remarks></remarks>
    public class ETFSearchQuery : Query<ETFSearchResult>, IYqlQuery
    {


        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public FundCategory Category { get; set; }
        public FundFamily Family { get; set; }

        public ETFSearchView View { get; set; }
        public ETFSearchProperty? SortProperty { get; set; }
        public ListSortDirection SortDirection { get; set; }
        public ETFSearchPagination Pagination { get; set; }

        public ETFSearchQuery()
        {
            this.View = ETFSearchView.ReturnMarket;
            this.Pagination = new ETFSearchPagination();
            this.SortDirection = ListSortDirection.Ascending;
        }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.Pagination == null)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("Pagination", "Pagination is NULL."));
            }
        }
        protected override Uri CreateUrl()
        {
            string url = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf";
            url += string.Format("&tab=tab{0}", (int)this.View);
            if (this.SortProperty.HasValue)
            {
                url += string.Format("&scol={}&stype=", YFHelper.GetETFPropertyTag(this.SortProperty.Value), this.SortDirection == ListSortDirection.Ascending ? "asc" : "desc");
            }
            url += string.Format("&rcnt={0}&page={1}", this.Pagination.Count, this.Pagination.Page + 1);
            if (this.Category != null) url += string.Format("&cat={0}", this.Category.ID);
            if (this.Family != null) url += string.Format("&ff={0}", this.Family.ID);
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       "url='" + url + "' and (xpath='//div[@class=\"yfi-table-container\"]')",
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        protected override ETFSearchResult ConvertResult(System.IO.Stream stream)
        {
            ETFSearchResult result = new ETFSearchResult();

            JObject componentsObject = null;

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);

                componentsObject = (JObject)((JObject)htmlDoc).FindFirst("div", "class", "yfi-table-container")["table"];

            }
            else
            {
                JObject resultsObj = null;
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                result.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    resultsObj = (JObject)yqlDoc.Query.Results;
                }

                if (resultsObj == null) throw new ParseException("The [result] object could not be load.");

                componentsObject = (JObject)resultsObj["div"];
            }

            var tableObject = (JObject)componentsObject["table"];

            var lstHeaders = new List<ETFSearchProperty?>();
            ETFSearchProperty? sortPrp = null;
            ListSortDirection? sortDir = null;
            var lst = new List<ETFSearchItem>();

            if (tableObject["thead"] != null)
            {
                var headerObj = (JObject)tableObject["thead"]["tr"];
                foreach (var td in headerObj["th"])
                {
                    ETFSearchProperty? prp = YFHelper.GetETFProperty(td["class"].ToString().Split(' ')[0]);
                    lstHeaders.Add(prp);
                    if (prp.HasValue)
                    {
                        if (td["class"].ToString().EndsWith("selected"))
                        {
                            var sort = td["class"]["a"][1]["class"].ToString();
                            if (sort.Contains("sort"))
                            {
                                sortPrp = prp;
                                sortDir = sort.Contains("desc") ? ListSortDirection.Descending : ListSortDirection.Ascending;
                            }
                        }
                    }
                }
            }
            

            if (lstHeaders.Count > 0)
            {
                var bodyObj = (JObject)tableObject["tbody"];

                foreach (JObject tr in bodyObj["tr"])
                {
                    var tds = (JArray)tr["td"];
                    if (tds.Count == lstHeaders.Count)
                    {
                        var item = new ETFSearchItem();
                        for (int n = 0; n < lstHeaders.Count; n++)
                        {
                            if (lstHeaders[n].HasValue)
                            {
                                var td = tds[n];

                                var value = td.HtmlFirstContent().ToObject();

                                var prp = lstHeaders[n].Value;
                                switch (prp)
                                {
                                    case ETFSearchProperty.ID:
                                        var lnkS = td.HtmlFirstLink();
                                        value = MyHelper.GetRestTagValue(lnkS, "s");
                                        break;
                                    case ETFSearchProperty.Category:
                                        var lnkCat = td.HtmlFirstLink();
                                        value = MyHelper.GetRestTagValue(lnkCat, "cat");
                                        break;
                                    case ETFSearchProperty.Family:
                                        var lnkFam = td.HtmlFirstLink();
                                        value = new FundFamily(MyHelper.GetRestTagValue(lnkFam, "ff"), value.ToString());
                                        break;
                                }

                                item.Values.Add(lstHeaders[n].Value, value);

                            }
                        }
                        lst.Add(item);
                    }
                }
            }


            try
            {
                var paginationObject = (JObject)componentsObject["div"];
                if (paginationObject != null)
                {
                    var cntStr = paginationObject.FindFirst("span", "class", "table-rcount").HtmlFirstContent();

                    var pageStr = ((JObject)paginationObject["div"][1]["div"][1]["ol"]).FindFirst("strong").HtmlFirstContent();

                    var itemsStr = ((JObject)paginationObject["div"][1]["div"][2]).HtmlFirstContent();

                    if (cntStr != null && pageStr != null && itemsStr != null)
                    {
                        int cnt = cntStr.ParseInt().Value;
                        int page = pageStr.ParseInt().Value;
                        int items = itemsStr.Substring(0, itemsStr.LastIndexOf(")")).Substring(itemsStr.LastIndexOf("(") + 1).ParseInt().Value;

                        result.Pagination = new LimitedPagination((page - 1) * cnt, cnt, items - 1);
                    }
                }
            }
            catch (Exception ex) { }


            result.Items = lst.ToArray();


            return result;
        }
        public override Query<ETFSearchResult> Clone()
        {
            return new ETFSearchQuery()
            {
                Category = this.Category,
                Family = this.Family,
                View = this.View,
                Pagination = this.Pagination.Clone(),
                SortProperty = this.SortProperty,
                SortDirection = this.SortDirection,
                GetDiagnostics = this.GetDiagnostics,
                UseDirectSource = this.UseDirectSource
            };
        }



    }





    public class ETFSearchPagination : Pagination
    {
        public override int Start
        {
            get
            {
                return this.Page * this.Count;
            }
            protected set
            {
                throw new NotSupportedException();
            }
        }
        public int Page { get; set; }

        public ETFSearchPagination() { this.Count = 20; }

        public void SetCount20() { this.Count = 20; }
        public void SetCount50() { this.Count = 50; }
        public void SetCount100() { this.Count = 100; }

        public new ETFSearchPagination Clone()
        {
            return new ETFSearchPagination() { Start = this.Start, Count = this.Count };
        }
    }

    /// <summary>
    /// Stores the result data
    /// </summary>
    public class ETFSearchResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public LimitedPagination Pagination { get; internal set; }
        public ETFSearchItem[] Items { get; internal set; }
        internal ETFSearchResult() { }
    }


}

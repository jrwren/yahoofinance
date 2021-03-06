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
using MaasOne.Net;
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.YahooFinance.Data;



namespace MaasOne.YahooFinance.Web
{


    /// <summary>
    /// Stores settings for Exchange Traded Funds search
    /// </summary>
    /// <remarks></remarks>
    public class ETFSearchQuery : YqlQuery<ETFSearchResult>
    {
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


        public override QueryBase Clone()
        {
            return new ETFSearchQuery()
            {
                Category = this.Category,
                Family = this.Family,
                View = this.View,
                Pagination = this.Pagination.Clone(),
                SortProperty = this.SortProperty,
                SortDirection = this.SortDirection
            };
        }


        protected override string CreateUrl()
        {
            string url = "http://finance.yahoo.com/etf/lists/?mod_id=mediaquotesetf";
            url += string.Format("&tab=tab{0}", (int)this.View);
            if (this.SortProperty.HasValue)
            {
                url += string.Format("&scol={0}&stype={1}", YFHelper.GetETFPropertyTag(this.SortProperty.Value), this.SortDirection == ListSortDirection.Ascending ? "asc" : "desc");
            }
            url += string.Format("&rcnt={0}&page={1}", this.Pagination.Count, this.Pagination.Page + 1);
            if (this.Category != null) url += string.Format("&cat={0}", this.Category.ID);
            if (this.Family != null) url += string.Format("&ff={0}", this.Family.ID);

            return url;
        }

        protected override void Validate(ValidationResult result)
        {
            if (this.Pagination == null)
            {
                result.Success = false;
                result.Info.Add("Pagination", "Pagination is NULL.");
            }
        }


        internal override ETFSearchResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            ETFSearchResult result = new ETFSearchResult();

            JObject componentsObject = (JObject)yqlToken;

            JObject tableObject = (JObject)componentsObject["table"];

            List<ETFSearchProperty?> lstHeaders = new List<ETFSearchProperty?>();
            ETFSearchProperty? sortPrp = null;
            ListSortDirection? sortDir = null;
            List<ETFSearchItem> lst = new List<ETFSearchItem>();

            if (tableObject["thead"] != null)
            {
                JObject headerObj = (JObject)tableObject["thead"]["tr"];
                foreach (JToken td in headerObj["th"])
                {
                    ETFSearchProperty? prp = YFHelper.GetETFProperty(td["class"].ToString().Split(' ')[0]);
                    lstHeaders.Add(prp);
                    if (prp.HasValue)
                    {
                        if (td["class"].ToString().EndsWith("selected"))
                        {
                            string sort = td["class"]["a"][1]["class"].ToString();
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
                JObject bodyObj = (JObject)tableObject["tbody"];

                foreach (JObject tr in bodyObj["tr"])
                {
                    JArray tds = (JArray)tr["td"];
                    if (tds.Count == lstHeaders.Count)
                    {
                        ETFSearchItem item = new ETFSearchItem();
                        for (int n = 0; n < lstHeaders.Count; n++)
                        {
                            if (lstHeaders[n].HasValue)
                            {
                                JToken td = tds[n];

                                object value = td.HtmlFirstContent().ToObject();

                                ETFSearchProperty prp = lstHeaders[n].Value;
                                switch (prp)
                                {
                                    case ETFSearchProperty.ID:
                                        string lnkS = td.HtmlFirstLink();
                                        value = MyHelper.GetRestTagValue(lnkS, "s");
                                        break;
                                    case ETFSearchProperty.Category:
                                        string lnkCat = td.HtmlFirstLink();
                                        value = MyHelper.GetRestTagValue(lnkCat, "cat");
                                        break;
                                    case ETFSearchProperty.Family:
                                        string lnkFam = td.HtmlFirstLink();
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
                JObject paginationObject = (JObject)componentsObject["div"];
                if (paginationObject != null)
                {
                    string cntStr = paginationObject.FindFirst("span", "class", "table-rcount").HtmlFirstContent();

                    string pageStr = ((JObject)paginationObject["div"][1]["div"][1]["ol"]).FindFirst("strong").HtmlFirstContent();

                    string itemsStr = ((JObject)paginationObject["div"][1]["div"][2]).HtmlFirstContent();

                    if (cntStr != null && pageStr != null && itemsStr != null)
                    {
                        int cnt = cntStr.ParseInt().Value;
                        int page = pageStr.ParseInt().Value;
                        int items = itemsStr.Substring(0, itemsStr.LastIndexOf(")")).Substring(itemsStr.LastIndexOf("(") + 1).ParseInt().Value;

                        result.Pagination = new LimitedPagination((page - 1) * cnt, cnt, items - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                ci.IsIntegrityComplete = false;
                ci.IntegrityMessages.Add("No pagination available.");
            }


            result.Items = lst.ToArray();


            return result;
        }

        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc) { return htmlDoc.FindFirst("div", "class", "yfi-table-container"); }

        internal override string YqlXPath() { return "//div[@class=\"yfi-table-container\"]"; }

    }




    public enum ETFSearchView
    {
        ReturnMarket = 1,
        ReturnNAV = 2,
        TradingVolume = 3,
        Holdings = 4,
        Risk = 5,
        Operations = 6
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


    public class ETFSearchResult : ResultBase
    {
        public LimitedPagination Pagination { get; internal set; }
        public ETFSearchItem[] Items { get; internal set; }
        internal ETFSearchResult() { }
    }
}

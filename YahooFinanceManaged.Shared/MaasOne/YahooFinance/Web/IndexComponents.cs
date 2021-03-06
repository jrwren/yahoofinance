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
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.YahooFinance.Data;

namespace MaasOne.YahooFinance.Web
{
    public class IndexComponentsQuery : YqlQuery<IndexComponentsResult>
    {
        public char? Alpha { get; set; }

        public string ID { get; private set; }
        
        public IndexComponentsPagination Pagination { get; set; }


        public IndexComponentsQuery() { }

        public IndexComponentsQuery(string id) { this.ID = id; }


        public override QueryBase Clone()
        {
            return new IndexComponentsQuery(this.ID) { Alpha = this.Alpha, Pagination = this.Pagination.Clone() };
        }


        protected override string CreateUrl()
        {
            return string.Format("http://finance.yahoo.com/q/cp?s={0}&c={1}&alpha={2}",
                Uri.EscapeDataString(this.ID),
                this.Pagination != null ? Math.Abs(this.Pagination.Page) : 0,
                this.Alpha);
        }

        protected override void Validate(ValidationResult result)
        {
            if (this.ID.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Info.Add("ID", "ID is NULL or empty.");
            }
            if (this.Pagination == null)
            {
                result.Success = false;
                result.Info.Add("Pagination", string.Format("{0}: Pagination is NULL.", this.ID));
            }
        }


        internal override IndexComponentsResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            IndexComponentsResult result = new IndexComponentsResult();

            JObject rightcolObj = (JObject)yqlToken;

            JObject sumObject = (JObject)rightcolObj.FindFirst("table", "id", "yfncsumtab");
            JObject shortInfoObject = (JObject)rightcolObj.FindFirst("div", "id", "yfi_rt_quote_summary");

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);
            
            JObject tr = (JObject)sumObject.FindFirst("tr", "valign", "top");

            JObject td = (JObject)tr["td"][0];

            if (td["table"] is JArray)
            {
                JArray tables = (JArray)td["table"];
                if (tables.Count >= 2)
                {
                    JObject indexTable = (JObject)tables[1];
                    result.Components = this.GetItems(indexTable);
                }
            }

            JObject alphaObj = (JObject)td["div"][0];
            JObject pagingObject = (JObject)td["div"][1];
            string pageStr = (pagingObject["small"] != null ? pagingObject["small"]["p"] : pagingObject["p"]).HtmlFirstContent();
            result.Pagination = YFHelper.HtmlConvertPagination(pageStr);
            
            return result;
        }

        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc) { return htmlDoc.FindFirst("div", "id", "rightcol"); }

        internal override string YqlXPath() { return "//div[@id=\"rightcol\"]"; }


        private IndexComponentsItem[] GetItems(JObject table)
        {
            var items = new List<IndexComponentsItem>();
            if (table != null)
            {
                table = table.HtmlInnerTable();
                if (table["tr"] is JArray)
                {
                    var trs = (JArray)table["tr"];
                    for (int i = 1; i < trs.Count; i++)
                    {
                        var tds = (JArray)trs[i]["td"];
                        var item = new IndexComponentsItem();
                        for (int n = 0; n < tds.Count; n++)
                        {
                            try
                            {
                                switch (n)
                                {
                                    case 0: item.ID = tds[n].HtmlFirstContent(); break;
                                    case 1: item.Name = tds[n].HtmlFirstContent(); break;
                                    case 2: item.LastTradePriceOnly = tds[n].HtmlFirstContent().ParseDouble(); break;
                                    case 3:
                                        item.ChangeInPercent = tds[n].HtmlFirstContent().Replace("%", "").ParseDouble();
                                        item.ChangeInPercent *= (tds[n]["span"]["img"]["class"].HtmlFirstContent() == "neg_arrow") ? -1 : 1;
                                        break;
                                    case 4: item.Volume = tds[n].HtmlFirstContent().ParseInt(); break;
                                }
                            }
                            catch { }
                        }
                        items.Add(item);
                    }
                }
            }
            return items.ToArray();
        }
    }


    public class IndexComponentsPagination : Pagination
    {
        public int Page { get; set; }

        public override int Start
        {
            get { return this.Page * this.Count; }
            protected set { throw new NotSupportedException(); }
        }


        public IndexComponentsPagination() { this.Count = 50; }


        public new IndexComponentsPagination Clone()
        {
            return new IndexComponentsPagination() { Page = this.Page };
        }
    }


    public class IndexComponentsResult : ResultBase
    {
        public IndexComponentsItem[] Components { get; internal set; }

        public LimitedPagination Pagination { get; internal set; }

        public QuotesBase ShortInfo { get; internal set; }


        internal IndexComponentsResult() { }
    }
}

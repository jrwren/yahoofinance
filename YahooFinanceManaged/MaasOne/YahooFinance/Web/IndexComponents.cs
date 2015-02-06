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
    public class IndexComponentsQuery : Query<IndexComponentsResult>, IYqlQuery
    {
        public bool UseDirectSource { get; set; }
    
        public bool GetDiagnostics { get; set; }

        public IndexComponentsSetting[] Settings { get; set; }

        public IndexComponentsQuery() : this(new string[0]) { }
        public IndexComponentsQuery(string id) : this(new string[] { id }) { }
        public IndexComponentsQuery(IEnumerable<string> ids)
        {
            var lst = new List<IndexComponentsSetting>();
            foreach (var id in ids) { lst.Add(new IndexComponentsSetting(id)); }
            this.Settings = lst.ToArray();
        }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.Settings == null || this.Settings.Length == 0)
            {
                result.Success = false;
                result.Info.Add("Settings", "No Settings available.");
            }
            else
            {
                if (this.UseDirectSource)
                {
                    if (this.Settings[0] == null)
                    {
                        result.Success = false;
                        result.Info.Add("Settings", "No valid Setting available.");
                    }
                }
                else
                {
                    foreach (var setting in this.Settings)
                    {
                        if (setting == null)
                        {
                            result.Success = false;
                            result.Info.Add("Settings", "No valid Settings available.");
                        }
                    }
                }
            }
        }

        protected override Uri CreateUrl()
        {
            string url = string.Empty;
            if (this.UseDirectSource)
            {
                url = this.Settings[0].Url();
            }
            else
            {
                string urlIn = string.Empty;
                foreach (var setting in this.Settings) { if (setting != null) urlIn += string.Format("'{0}', ", setting.Url()); }
                urlIn = urlIn.Substring(0, urlIn.Length - 2);
                url = YFHelper.YqlUrl("*", "html",
                                       string.Format("url in ({0}) and xpath='//div[@id=\"rightcol\"]'", urlIn),
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        
        protected override IndexComponentsResult ConvertResult(System.IO.Stream stream)
        {
            IndexComponentsResult result = new IndexComponentsResult();

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                result.Data = new IndexComponentsData[] { this.ConvertComponents((JObject)htmlDoc.FindFirst("div", "id", "rightcol")) };
            }
            else
            {
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                result.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    JToken divTok = yqlDoc.Query.Results["div"];
                    JArray divArr = null;
                    if (divTok is JArray) { divArr = (JArray)divTok; }
                    else { divArr = new JArray(divTok); }

                    List<IndexComponentsData> lstData = new List<IndexComponentsData>();
                    foreach (JObject divObj in divArr) { lstData.Add(this.ConvertComponents(divObj)); }
                    result.Diagnostics = yqlDoc.Query.Diagnostics;
                    result.Data = lstData.ToArray();
                }
            }

            return result;
        }

        public override Query<IndexComponentsResult> Clone()
        {
            var result = new IndexComponentsQuery()
            {
                GetDiagnostics = this.GetDiagnostics,
                UseDirectSource = this.UseDirectSource
            };
            result.Settings = new IndexComponentsSetting[this.Settings.Length];
            for (int i = 0; i < this.Settings.Length; i++) { result.Settings[i] = this.Settings[i].Clone(); }
            return result;
        }

        private IndexComponentsData ConvertComponents(JObject rightcolObj)
        {
            var result = new IndexComponentsData();

            JObject sumObject = (JObject)rightcolObj.FindFirst("table", "id", "yfncsumtab");
            JObject shortInfoObject = (JObject)rightcolObj.FindFirst("div", "id", "yfi_rt_quote_summary");

            if (sumObject == null) throw new ParseException("The [summary] object could not be load.");


            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);


            JObject tr = (JObject)sumObject.FindFirst("tr", "valign", "top");

            JObject td = (JObject)tr["td"][0];

            if (td["table"] is JArray)
            {
                var tables = (JArray)td["table"];
                if (tables.Count >= 2)
                {
                    JObject indexTable = (JObject)tables[1];
                    result.Components = this.GetItems(indexTable);
                }
            }

            JObject alphaObj = (JObject)td["div"][0];
            JObject pagingObject = (JObject)td["div"][1];
            var pageStr = (pagingObject["small"] != null ? pagingObject["small"]["p"] : pagingObject["p"]).HtmlFirstContent();
            result.Pagination = YFHelper.HtmlConvertPagination(pageStr);
                  

            return result;
        }

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
    
    public class IndexComponentsSetting
    {
        public string ID { get; private set; }

        public IndexComponentsPagination Pagination { get; set; }

        public char? Alpha { get; set; }

        public IndexComponentsSetting(string id)
        {
            if (id.IsNullOrWhiteSpace()) throw new ArgumentException("The ID is NULL or Empty.", "id");
            this.ID = id;
            this.Pagination = new IndexComponentsPagination();
        }

        public IndexComponentsSetting Clone()
        {
            return new IndexComponentsSetting(this.ID)
            {
                Pagination = this.Pagination.Clone(),
                Alpha = this.Alpha
            };
        }

        internal string Url()
        {
            return string.Format("http://finance.yahoo.com/q/cp?s={0}&c={1}&alpha={2}",
                Uri.EscapeDataString(this.ID),
                this.Pagination != null ? this.Pagination.Page : 0,
                this.Alpha
            );
        }
    }

    public class IndexComponentsPagination : Pagination
    {
        public override int Start
        {
            get { return this.Page * this.Count; }
            protected set { throw new NotSupportedException(); }
        }

        public int Page { get; set; }

        public IndexComponentsPagination() { this.Count = 50; }

        public new IndexComponentsPagination Clone()
        {
            return new IndexComponentsPagination() { Page = this.Page };
        }
    }
    
    public class IndexComponentsResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }

        public IndexComponentsData[] Data { get; internal set; }

        internal IndexComponentsResult() { }
    }

    public class IndexComponentsData
    {
        public QuotesBase ShortInfo { get; internal set; }

        public IndexComponentsItem[] Components { get; internal set; }

        public LimitedPagination Pagination { get; internal set; }

        internal IndexComponentsData() { }
    }
}

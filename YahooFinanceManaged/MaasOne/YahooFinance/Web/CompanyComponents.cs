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
    public class CompanyComponentsQuery : Query<CompanyComponentsResult>, IYqlQuery
    {
        public bool UseDirectSource { get; set; }

        public bool GetDiagnostics { get; set; }

        public CompanyComponentsSetting[] Settings { get; set; }

        public CompanyComponentsQuery() : this(new string[0]) { }
        public CompanyComponentsQuery(string id) : this(new string[] { id }) { }
        public CompanyComponentsQuery(IEnumerable<string> ids)
        {
            var lst = new List<CompanyComponentsSetting>();
            foreach (var id in ids) { lst.Add(new CompanyComponentsSetting(id)); }
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

        protected override CompanyComponentsResult ConvertResult(System.IO.Stream stream)
        {
            CompanyComponentsResult result = new CompanyComponentsResult();

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                result.Data = new CompanyComponentsData[] { this.ConvertComponents((JObject)htmlDoc.FindFirst("div", "id", "rightcol")) };
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

                    List<CompanyComponentsData> lstData = new List<CompanyComponentsData>();
                    foreach (JObject divObj in divArr) { lstData.Add(this.ConvertComponents(divObj)); }
                    result.Diagnostics = yqlDoc.Query.Diagnostics;
                    result.Data = lstData.ToArray();
                }
            }

            return result;
        }

        public override Query<CompanyComponentsResult> Clone()
        {
            var result = new CompanyComponentsQuery()
            {
                GetDiagnostics = this.GetDiagnostics,
                UseDirectSource = this.UseDirectSource
            };
            result.Settings = new CompanyComponentsSetting[this.Settings.Length];
            for (int i = 0; i < this.Settings.Length; i++) { result.Settings[i] = this.Settings[i].Clone(); }
            return result;
        }

        private CompanyComponentsData ConvertComponents(JObject rightcolObj)
        {
            var result = new CompanyComponentsData();

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
                    result.Indices = this.GetItems(indexTable);
                }
                if (tables.Count >= 4)
                {
                    JObject etfTable = (JObject)tables[3];
                    result.ETFs = this.GetItems(etfTable);
                }
                if (tables.Count >= 6)
                {
                    JObject fundTable = (JObject)tables[5];
                    result.Funds = this.GetItems(fundTable);
                }
            }

            if (td["div"] != null)
            {
                JArray pagingObjects = td["div"] is JArray ? (JArray)td["div"] : new JArray(td["div"]);
                foreach (var pagingObject in pagingObjects)
                {
                    var pageStr = pagingObject["small"]["content"].HtmlFirstContent();
                    if (pageStr.Contains("Indices")) { result.IndicesPagination = YFHelper.HtmlConvertPagination(pageStr); }
                    else if (pageStr.Contains("ETFs")) { result.ETFsPagination = YFHelper.HtmlConvertPagination(pageStr); }
                    else if (pageStr.Contains("Mutual Funds")) { result.FundsPagination = YFHelper.HtmlConvertPagination(pageStr); }
                }
            }
            if (result.IndicesPagination == null) result.IndicesPagination = new LimitedPagination(0, result.Indices.Length, result.Indices.Length - 1);
            if (result.ETFsPagination == null) result.ETFsPagination = new LimitedPagination(0, result.ETFs.Length, result.ETFs.Length - 1);
            if (result.FundsPagination == null) result.FundsPagination = new LimitedPagination(0, result.Funds.Length, result.Funds.Length - 1);

            return result;
        }

        private ComponentsItem[] GetItems(JObject table)
        {
            var items = new List<ComponentsItem>();
            if (table != null)
            {
                table = table.HtmlInnerTable();
                if (table["tr"] is JArray)
                {
                    var trs = (JArray)table["tr"];
                    for (int i = 1; i < trs.Count; i++)
                    {
                        var tds = (JArray)trs[i]["td"];
                        var item = new ComponentsItem();
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

    public class CompanyComponentsSetting
    {
        public string ID { get; private set; }

        public CompanyComponentsPagination IndicesPagination { get; set; }

        public CompanyComponentsPagination ETFsPagination { get; set; }

        public CompanyComponentsPagination FundsPagination { get; set; }

        public CompanyComponentsSetting(string id)
        {
            if (id.IsNullOrWhiteSpace()) throw new ArgumentException("The ID is NULL or Empty.", "id");
            this.ID = id;
            this.IndicesPagination = new CompanyComponentsPagination();
            this.ETFsPagination = new CompanyComponentsPagination();
            this.FundsPagination = new CompanyComponentsPagination();
        }

        internal string Url()
        {
            return string.Format("http://finance.yahoo.com/q/ct?s={0}&e={1}&f={2}&i={3}",
                Uri.EscapeDataString(this.ID),
                this.ETFsPagination != null ? this.ETFsPagination.Page + 1 : 1,
                this.FundsPagination != null ? this.FundsPagination.Page + 1 : 1,
                this.IndicesPagination != null ? this.IndicesPagination.Page + 1 : 1
            );
        }

        public new CompanyComponentsSetting Clone()
        {
            return new CompanyComponentsSetting(this.ID)
            {
                IndicesPagination = this.IndicesPagination.Clone(),
                ETFsPagination = this.ETFsPagination.Clone(),
                FundsPagination = this.FundsPagination.Clone()
            };
        }
    }

    public class CompanyComponentsPagination : Pagination
    {
        public override int Start
        {
            get { return this.Page * this.Count; }
            protected set { throw new NotSupportedException(); }
        }

        public int Page { get; set; }

        public CompanyComponentsPagination() { this.Count = 10; }

        public new CompanyComponentsPagination Clone()
        {
            return new CompanyComponentsPagination() { Page = this.Page };
        }
    }

    public class CompanyComponentsResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }

        public CompanyComponentsData[] Data { get; internal set; }

        internal CompanyComponentsResult() { }
    }

    public class CompanyComponentsData
    {
        public QuotesBase ShortInfo { get; internal set; }

        public ComponentsItem[] Indices { get; internal set; }

        public ComponentsItem[] ETFs { get; internal set; }

        public ComponentsItem[] Funds { get; internal set; }

        public LimitedPagination IndicesPagination { get; internal set; }

        public LimitedPagination FundsPagination { get; internal set; }

        public LimitedPagination ETFsPagination { get; internal set; }

        internal CompanyComponentsData() { }
    }
}

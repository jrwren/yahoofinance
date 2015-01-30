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
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.Finance.Yahoo.Data;


namespace MaasOne.Finance.Yahoo.Web
{
    public class ComponentsQuery : Query<ComponentsResult>
    {
        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public string ID { get; set; }
        public char? Alpha { get; set; }
        public ComponentsQuery() : this(string.Empty) { }
        public ComponentsQuery(string id) { this.ID = id; }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (string.IsNullOrEmpty(this.ID))
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("ID", "No ID available."));
            }
        }
        protected override Uri CreateUrl()
        {
            var isIndex = this.ID.StartsWith("^");
            var siteTag = isIndex ? "cp" : "ct";
            string url = string.Format("http://finance.yahoo.com/q/{0}?s={1}", siteTag, Uri.EscapeDataString(this.ID));
            if (isIndex && this.Alpha.HasValue) url += string.Format("&alpha={0}", this.Alpha.Value);
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       "url='" + url + "' and (xpath='//table[@id=\"yfncsumtab\"]' or xpath='//div[@id=\"yfi_rt_quote_summary\"]')",
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        protected override ComponentsResult ConvertResult(System.IO.Stream stream)
        {
            ComponentsResult result = new ComponentsResult();

            JObject shortInfoObject = null;
            JObject componentsObject = null;

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                componentsObject = (JObject)htmlDoc.FindFirst("table", "id", "yfncsumtab");
                shortInfoObject = (JObject)htmlDoc.FindFirst("div", "id", "yfi_rt_quote_summary");
            }
            else
            {
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                result.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    componentsObject = (JObject)yqlDoc.Query.Results["table"];
                    shortInfoObject = (JObject)yqlDoc.Query.Results["div"];
                }
            }


            if (componentsObject == null) throw new ConvertException("The [profile] object could not be load.");

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);


            JObject tr = (JObject)componentsObject.FindFirst("tr", "valign", "top");

            JObject td = (JObject)tr["td"][0];

            if (td["table"] is JArray)
            {
                var tables = (JArray)td["table"];
                if (tables.Count >= 2)
                {
                    JObject indexTable = (JObject)tables[1];
                    result.IndexItems = this.GetItems(indexTable);
                }
                if (tables.Count >= 4)
                {
                    JObject etfTable = (JObject)tables[3];
                    result.ETFItems = this.GetItems(etfTable);
                }
                if (tables.Count >= 6)
                {
                    JObject fundTable = (JObject)tables[5];
                    result.FundItems = this.GetItems(fundTable);
                }
            }

            JObject pagingObject = null;
            if (td["div"] is JArray)
            { pagingObject = (JObject)((JArray)td["div"])[((JArray)td["div"]).Count - 1]; }
            else
            { pagingObject = (JObject)td["div"]; }

            if (pagingObject != null)
            {
                var pageStr = MyHelper.HtmlFirstContent(pagingObject["small"] != null ? pagingObject["small"]["p"] : pagingObject["p"]);
                result.Pagination = YFHelper.HtmlConvertPagination(pageStr);
            }


            return result;
        }
        public override Query<ComponentsResult> Clone()
        {
            return new ComponentsQuery(this.ID) { GetDiagnostics = this.GetDiagnostics, UseDirectSource = this.UseDirectSource };
        }



        private ComponentsItem[] GetItems(JObject table)
        {
            var items = new List<ComponentsItem>();
            if (table != null)
            {
                table = MyHelper.HtmlInnerTable(table);  
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
                                    case 0: item.ID = MyHelper.HtmlFirstContent(tds[n]); break;
                                    case 1: item.Name = MyHelper.HtmlFirstContent(tds[n]); break;
                                    case 2: item.LastTradePriceOnly = MyHelper.ParseToDouble(MyHelper.HtmlFirstContent(tds[n])); break;
                                    case 3:
                                        item.ChangeInPercent = MyHelper.ParseToDouble(MyHelper.HtmlFirstContent(tds[n]).Replace("%", ""));
                                        item.ChangeInPercent *= (tds[n]["span"]["img"]["class"].ToString() == "neg_arrow") ? -1 : 1;
                                        break;
                                    case 4: item.Volume = (int)MyHelper.ParseToDouble(MyHelper.HtmlFirstContent(tds[n])); break;
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



    public class ComponentsResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public ComponentsItem[] IndexItems { get; internal set; }
        public ComponentsItem[] ETFItems { get; internal set; }
        public ComponentsItem[] FundItems { get; internal set; }
        public LimitedPagination Pagination { get; internal set; }
        public QuotesShortInfo ShortInfo { get; internal set; }
        internal ComponentsResult() { }
    }




}

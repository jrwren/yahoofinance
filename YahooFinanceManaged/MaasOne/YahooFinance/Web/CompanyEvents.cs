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
    public class CompanyEventsQuery : Query<CompanyEventsResult>, IYqlQuery
    {
        public bool UseDirectSource { get; set; }

        public bool GetDiagnostics { get; set; }

        public string[] IDs { get; set; }

        public CompanyEventsQuery() { }
        public CompanyEventsQuery(string id) : this(new string[] { id }) { }
        public CompanyEventsQuery(IEnumerable<string> ids) { this.IDs = ids.EnumToArray(); }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.IDs == null || this.IDs.Length == 0)
            {
                result.Success = false;
                result.Info.Add("IDs", "No IDs available.");
            }
            else
            {
                if (this.UseDirectSource)
                {
                    if (this.IDs[0].IsNullOrWhiteSpace())
                    {
                        result.Success = false;
                        result.Info.Add("IDs", "No valid ID available.");
                    }
                }
                else
                {
                    var validCnt = 0;
                    foreach (string id in this.IDs) { validCnt += id.IsNullOrWhiteSpace() ? 0 : 1; }
                    if (validCnt == 0)
                    {
                        result.Success = false;
                        result.Info.Add("IDs", "No valid IDs available.");
                    }
                }
            }
        }

        protected override Uri CreateUrl()
        {
            string url = string.Empty;
            if (this.UseDirectSource)
            {
                url = this.CreateUrl(this.IDs[0]);
            }
            else
            {
                string urlIn = string.Empty;
                foreach (var id in this.IDs) { if (!id.IsNullOrWhiteSpace()) urlIn += string.Format("'{0}', ", this.CreateUrl(id)); }
                urlIn = urlIn.Substring(0, urlIn.Length - 2);
                url = YFHelper.YqlUrl("*", "html",
                                       string.Format("url in ({0}) and xpath='//div[@id=\"rightcol\"]'", urlIn),
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        private string CreateUrl(string id) { return string.Format("http://finance.yahoo.com/q/ce?s={0}", Uri.EscapeDataString(id)); }

        protected override CompanyEventsResult ConvertResult(System.IO.Stream stream)
        {
            CompanyEventsResult result = new CompanyEventsResult();

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                result.Data = new CompanyEventsData[] { this.ConvertEvents((JObject)htmlDoc.FindFirst("div", "id", "rightcol")) };
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

                    List<CompanyEventsData> lstData = new List<CompanyEventsData>();
                    foreach (JObject divObj in divArr) { lstData.Add(this.ConvertEvents(divObj)); }
                    result.Diagnostics = yqlDoc.Query.Diagnostics;
                    result.Data = lstData.ToArray();
                }
            }

            return result;
        }

        public override Query<CompanyEventsResult> Clone()
        {
            return new CompanyEventsQuery()
            {
                GetDiagnostics = this.GetDiagnostics,
                UseDirectSource = this.UseDirectSource,
                IDs = (string[])this.IDs.Clone()
            };
        }

        private CompanyEventsData ConvertEvents(JObject rightcolObj)
        {
            JObject shortInfoObject = (JObject)rightcolObj.FindFirst("div", "id", "yfi_rt_quote_summary");
            JObject sumObject = (JObject)rightcolObj.FindFirst("table", "id", "yfncsumtab");

            if (sumObject == null) throw new ParseException("The [summary] object could not be load.");

            var result = new CompanyEventsData();

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);

            JObject tr = (JObject)sumObject.FindFirst("tr", "valign", "top");


            var upcomingTable = ((JObject)tr["td"][0]["table"][1]).HtmlInnerTable();
            var recentTable = ((JObject)tr["td"][0]["table"][3]).HtmlInnerTable();

            var lstUp = new List<CompanyEventsItem>();
            if (upcomingTable["tr"] is JArray)
            {
                var trs = (JArray)upcomingTable["tr"];

                int startIndex = (trs[0]["th"] != null) ? 1 : 0;
                for (int i = startIndex; i < trs.Count; i++)
                {
                    var utr = trs[i];
                    if (utr["td"] is JArray && ((JArray)utr["td"]).Count == 2)
                    {
                        var ue = new CompanyEventsItem();
                        ue.Date = utr["td"][0].HtmlFirstContent();
                        ue.Title = utr["td"][1].HtmlFirstContent();
                        lstUp.Add(ue);
                    }
                }
            }

            var lstRe = new List<CompanyRecentEventsItem>();
            if (recentTable["tr"] is JArray)
            {
                var trs = (JArray)recentTable["tr"];

                int startIndex = (trs[0]["th"] != null) ? 1 : 0;
                for (int i = startIndex; i < trs.Count; i++)
                {
                    var rtr = trs[i];
                    if (rtr["td"] is JArray && ((JArray)rtr["td"]).Count == 2)
                    {
                        var re = new CompanyRecentEventsItem();
                        re.Date = rtr["td"][0].HtmlFirstContent();
                        re.Title = rtr["td"][1].HtmlFirstContent();
                        re.Link = new Uri(rtr["td"][1].HtmlFirstLink());
                        lstRe.Add(re);
                    }
                }
            }

            result.UpcomingEvents = lstUp.ToArray();
            result.RecentEvents = lstRe.ToArray();

            return result;
        }
    }

    public class CompanyEventsResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }

        public CompanyEventsData[] Data { get; internal set; }

        public QuotesBase ShortInfo { get; internal set; }

        internal CompanyEventsResult() { }
    }

    public class CompanyEventsData
    {
        public CompanyEventsItem[] UpcomingEvents { get; internal set; }

        public CompanyRecentEventsItem[] RecentEvents { get; internal set; }

        public QuotesBase ShortInfo { get; internal set; }

        internal CompanyEventsData() { }
    }
}

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

        public string ID { get; set; }

        public CompanyEventsQuery() : this(string.Empty) { }
        public CompanyEventsQuery(string id) { this.ID = id; }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.ID.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("ID", "No ID available."));
            }
        }
        protected override Uri CreateUrl()
        {
            string url = string.Format("http://finance.yahoo.com/q/ks?s={0}", this.ID);
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       "url='" + url + "' and (xpath='//table[@id=\"yfncsumtab\"]' or xpath='//div[@id=\"yfi_rt_quote_summary\"]')",
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        protected override CompanyEventsResult ConvertResult(System.IO.Stream stream)
        {
            CompanyEventsResult result = new CompanyEventsResult();

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


            if (componentsObject == null) throw new ParseException("The [profile] object could not be load.");

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);

            JObject tr = (JObject)componentsObject.FindFirst("tr", "valign", "top");


            var upcomingTable = ((JObject)tr["td"][0]["table"][1]).HtmlInnerTable();
            var recentTable = ((JObject)tr["td"][0]["table"][3]).HtmlInnerTable();

            var lstUp = new List<CompanyEventItem>();
            if (upcomingTable["tr"] is JArray)
            {
                var trs = (JArray)upcomingTable["tr"];

                int startIndex = (trs[0]["th"] != null) ? 1 : 0;
                for (int i = startIndex; i < trs.Count; i++)
                {
                    var utr = trs[i];
                    if (utr["td"] is JArray && ((JArray)utr["td"]).Count == 2)
                    {
                        var ue = new CompanyEventItem();
                        ue.Date = utr["td"][0].HtmlFirstContent();
                        ue.Title = utr["td"][1].HtmlFirstContent();
                        lstUp.Add(ue);
                    }
                }
            }

            var lstRe = new List<CompanyRecentEventItem>();
            if (recentTable["tr"] is JArray)
            {
                var trs = (JArray)recentTable["tr"];

                int startIndex = (trs[0]["th"] != null) ? 1 : 0;
                for (int i = startIndex; i < trs.Count; i++)
                {
                    var rtr = trs[i];
                    if (rtr["td"] is JArray && ((JArray)rtr["td"]).Count == 2)
                    {
                        var re = new CompanyRecentEventItem();
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
        public override Query<CompanyEventsResult> Clone()
        {
            return new CompanyEventsQuery() { ID = this.ID, GetDiagnostics = this.GetDiagnostics, UseDirectSource = this.UseDirectSource };
        }


    }



    /// <summary>
    /// Stores the result data
    /// </summary>
    public class CompanyEventsResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public CompanyEventItem[] UpcomingEvents { get; internal set; }
        public CompanyRecentEventItem[] RecentEvents { get; internal set; }
        public QuotesBaseData ShortInfo { get; internal set; }
    }



}

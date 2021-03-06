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
    public class CompanyEventsQuery : YqlQuery<CompanyEventsResult>
    {
        public string ID { get; set; }


        public CompanyEventsQuery() { }

        public CompanyEventsQuery(string id) { this.ID = id; }


        public override QueryBase Clone() { return new CompanyEventsQuery(this.ID); }


        protected override void Validate(ValidationResult result)
        {
            if (this.ID.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Info.Add("ID", "ID is NULL or empty.");
            }
        }

        protected override string CreateUrl() { return string.Format("http://finance.yahoo.com/q/ce?s={0}", Uri.EscapeDataString(this.ID)); }


        internal override CompanyEventsResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            CompanyEventsResult result = new CompanyEventsResult();

            JObject rightcolObj = (JObject)yqlToken;

            JObject shortInfoObj = (JObject)rightcolObj.FindFirst("div", "id", "yfi_rt_quote_summary");
            JObject sumObj = (JObject)rightcolObj.FindFirst("table", "id", "yfncsumtab");

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObj);

            JObject tr = (JObject)sumObj.FindFirst("tr", "valign", "top");

            JObject upcomingTable = ((JObject)tr["td"][0]["table"][1]).HtmlInnerTable();
            JObject recentTable = ((JObject)tr["td"][0]["table"][3]).HtmlInnerTable();

            List<CompanyEventsItem> lstUp = new List<CompanyEventsItem>();
            if (upcomingTable["tr"] is JArray)
            {
                JArray trs = (JArray)upcomingTable["tr"];

                int startIndex = (trs[0]["th"] != null) ? 1 : 0;
                for (int i = startIndex; i < trs.Count; i++)
                {
                    var utr = trs[i];
                    if (utr["td"] is JArray && ((JArray)utr["td"]).Count == 2)
                    {
                        CompanyEventsItem ue = new CompanyEventsItem();
                        ue.Date = utr["td"][0].HtmlFirstContent();
                        ue.Title = utr["td"][1].HtmlFirstContent();
                        lstUp.Add(ue);
                    }
                }
            }

            var lstRe = new List<CompanyRecentEventsItem>();
            if (recentTable["tr"] is JArray)
            {
                JArray trs = (JArray)recentTable["tr"];

                int startIndex = (trs[0]["th"] != null) ? 1 : 0;
                for (int i = startIndex; i < trs.Count; i++)
                {
                    var rtr = trs[i];
                    if (rtr["td"] is JArray && ((JArray)rtr["td"]).Count == 2)
                    {
                        CompanyRecentEventsItem re = new CompanyRecentEventsItem();
                        re.Date = rtr["td"][0].HtmlFirstContent();
                        re.Title = rtr["td"][1].HtmlFirstContent();
                        string lnk = rtr["td"][1].HtmlFirstLink();
                        if (lnk.IsNullOrWhiteSpace() == false) { re.Link = new Uri(lnk); }
                        lstRe.Add(re);
                    }
                }
            }

            result.UpcomingEvents = lstUp.ToArray();
            result.RecentEvents = lstRe.ToArray();

            return result;
        }

        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc) { return htmlDoc.FindFirst("div", "id", "rightcol"); }

        internal override string YqlXPath() { return "//div[@id=\"rightcol\"]"; }
    }


    public class CompanyEventsResult : ResultBase
    {
        public CompanyEventsItem[] UpcomingEvents { get; internal set; }

        public CompanyRecentEventsItem[] RecentEvents { get; internal set; }

        public QuotesBase ShortInfo { get; internal set; }


        internal CompanyEventsResult() { }
    }
}

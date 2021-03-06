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
using Newtonsoft.Json.Linq.JsonPath;
using MaasOne.YahooFinance.Data;

namespace MaasOne.YahooFinance.Web
{
    public class QuotesOrderBookQuery : YqlQuery<QuotesOrderBookResult>
    {
        public string ID { get; set; }


        public QuotesOrderBookQuery() { }

        public QuotesOrderBookQuery(string id) { this.ID = id; }


        public override QueryBase Clone() { return new QuotesOrderBookQuery(this.ID); }


        protected override string CreateUrl() { return string.Format("http://finance.yahoo.com/q/ecn?s={0}", Uri.EscapeDataString(this.ID)); }

        protected override void Validate(ValidationResult result)
        {
            if (this.ID.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Info.Add("ID", "ID is NULL or empty.");
            }
        }


        internal override QuotesOrderBookResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            QuotesOrderBookResult result = new QuotesOrderBookResult();

            JObject contentObj = (JObject)yqlToken;

            JObject shortInfoObject = (JObject)contentObj.FindFirst("div", "id", "yfi_rt_quote_summary");
            JObject profileObject = (JObject)contentObj.FindFirst("div", "id", "yfi_orderbooks");

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);

            JToken bidTable = ((JObject)profileObject.FindFirst("table", "id", "yfi_orderbook_bid"));
            JToken askTable = ((JObject)profileObject.FindFirst("table", "id", "yfi_orderbook_ask"));

            List<QuotesOrderBookItem> lstBids = new List<QuotesOrderBookItem>();
            List<QuotesOrderBookItem> lstAsks = new List<QuotesOrderBookItem>();

            foreach (JToken bid in bidTable["tbody"]["tr"])
            {
                if (((JObject)bid["td"][0]).HtmlFirstContent() != "Price")
                {
                    lstBids.Add(new QuotesOrderBookItem()
                    {
                        Price = ((JObject)bid["td"][0]).HtmlFirstContent().ParseDouble().Value,
                        Size = ((JObject)bid["td"][1]).HtmlFirstContent().ParseInt().Value
                    });
                }
            }
            foreach (JToken ask in askTable["tbody"]["tr"])
            {
                if (((JObject)ask["td"][0]).HtmlFirstContent() != "Price")
                {
                    lstAsks.Add(new QuotesOrderBookItem()
                    {
                        Price = ((JObject)ask["td"][0]).HtmlFirstContent().ParseDouble().Value,
                        Size = ((JObject)ask["td"][1]).HtmlFirstContent().ParseInt().Value
                    });
                }
            }

            result.Bids = lstBids.ToArray();
            result.Asks = lstAsks.ToArray();

            return result;
        }

        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc) { return htmlDoc.FindFirst("div", "id", "yfi_investing_content"); }

        internal override string YqlXPath() { return "//div[@id=\"yfi_investing_content\"]"; }
    }


    public class QuotesOrderBookResult : ResultBase
    {
        public QuotesOrderBookItem[] Asks { get; internal set; }

        public QuotesOrderBookItem[] Bids { get; internal set; }

        public QuotesBase ShortInfo { get; internal set; }


        internal QuotesOrderBookResult() { }
    }
}

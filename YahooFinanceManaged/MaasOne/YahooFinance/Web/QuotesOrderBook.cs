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
using Newtonsoft.Json.Linq.JsonPath;
using MaasOne.YahooFinance.Data;
/*
#if  (NET20)
using Newtonsoft.Json.Utilities.LinqBridge;
#else
using System.Linq;
#endif
*/

namespace MaasOne.YahooFinance.Web
{

    public class QuotesOrderBookQuery : Query<QuotesOrderBookResult>, IYqlQuery
    {

        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public string ID { get; set; }

        public QuotesOrderBookQuery() : this(string.Empty) { }
        public QuotesOrderBookQuery(string id) { this.ID = id; }

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
            string url = string.Format("http://finance.yahoo.com/q/ecn?s={0}", this.ID);
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       "url='" + url + "' and (xpath='//div[@id=\"yfi_orderbooks\"]' or xpath='//div[@id=\"yfi_rt_quote_summary\"]')",
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        protected override QuotesOrderBookResult ConvertResult(System.IO.Stream stream)
        {
            QuotesOrderBookResult result = new QuotesOrderBookResult();

            JObject shortInfoObject = null;
            JObject profileObject = null;

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                profileObject = (JObject)htmlDoc.FindFirst("div", "id", "yfi_orderbooks");
                shortInfoObject = (JObject)htmlDoc.FindFirst("div", "id", "yfi_rt_quote_summary");
            }
            else
            {
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                result.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    profileObject = (JObject)yqlDoc.Query.Results["div"][0];
                    shortInfoObject = (JObject)yqlDoc.Query.Results["div"][1];
                }
            }


            if (profileObject == null) throw new ParseException("The [profile] object could not be load.");

            var bidTable = ((JObject)profileObject.FindFirst("table", "id", "yfi_orderbook_bid"));
            var askTable = ((JObject)profileObject.FindFirst("table", "id", "yfi_orderbook_ask"));

            var lstBids = new List<QuotesOrderBookItem>();
            var lstAsks = new List<QuotesOrderBookItem>();

            foreach (var bid in bidTable["tbody"]["tr"])
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
            foreach (var ask in askTable["tbody"]["tr"])
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

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);

            return result;
        }
        public override Query<QuotesOrderBookResult> Clone()
        {
            return new QuotesOrderBookQuery(this.ID) { GetDiagnostics = this.GetDiagnostics, UseDirectSource = this.UseDirectSource };
        }

    }

    /// <summary>
    /// Stores the result data
    /// </summary>
    public class QuotesOrderBookResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public QuotesOrderBookItem[] Bids { get; internal set; }
        public QuotesOrderBookItem[] Asks { get; internal set; }
        public QuotesBaseData ShortInfo { get; internal set; }
        internal QuotesOrderBookResult() { }
    }

}

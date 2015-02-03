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

    public class QuotesOptionsQuery : Query<QuotesOptionsResult>, IYqlQuery
    {

        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public string ID { get; set; }
        public DateTime Date { get; set; }

        public QuotesOptionsQuery() : this(string.Empty) { }
        public QuotesOptionsQuery(string id)
        {
            this.ID = id;
            this.Date = this.NextFriday();
        }
        public DateTime NextFriday()
        {
            for (int i = 1; i <= 7; i++)
            {
                var now = DateTime.UtcNow;
                var today = now.AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second).AddMilliseconds(-now.Millisecond);
                if (today.AddDays(i).DayOfWeek == DayOfWeek.Friday)
                { return today.AddDays(i); }
            }
            throw new Exception("This exception is not possible");
        }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (string.IsNullOrEmpty(this.ID))
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("ID", "No ID available."));
            }
            if (this.Date.Date < DateTime.Today)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("Date", "The date must be in the future."));
            }
            if (this.Date.DayOfWeek != DayOfWeek.Friday)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("Date", "The day must be a friday."));
            }
        }
        protected override Uri CreateUrl()
        {
            string url = string.Format("http://finance.yahoo.com/q/op?s={0}&date={1}", this.ID, this.Timestamp(this.Date));
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       "url='" + url + "' and (xpath='//div[@id=\"quote-table\"]' or xpath='//div[@id=\"yfi_rt_quote_summary\"]')",
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        protected override QuotesOptionsResult ConvertResult(System.IO.Stream stream)
        {
            QuotesOptionsResult result = new QuotesOptionsResult();

            JObject shortInfoObject = null;
            JObject componentsObject = null;

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                componentsObject = (JObject)htmlDoc.FindFirst("div", "id", "quote-table");
                shortInfoObject = (JObject)htmlDoc.FindFirst("div", "id", "yfi_rt_quote_summary");
            }
            else
            {
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                result.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    componentsObject = (JObject)yqlDoc.Query.Results["div"][0];
                    shortInfoObject = (JObject)yqlDoc.Query.Results["div"][1];
                }
            }


            if (componentsObject == null) throw new ParseException("The [table] object could not be load.");

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);

            JObject callsTable = (JObject)((JObject)componentsObject.FindFirst("div", "id", "optionsCallsTable")).FindFirst("table")["tbody"];
            JObject putsTable = (JObject)((JObject)componentsObject.FindFirst("div", "id", "optionsPutsTable")).FindFirst("table")["tbody"];

            result.Calls = this.ConvertTable(QuotesOptionsType.Call, callsTable);
            result.Puts = this.ConvertTable(QuotesOptionsType.Put, putsTable);


            return result;
        }
        public override Query<QuotesOptionsResult> Clone()
        {
            return new QuotesOptionsQuery(this.ID) { GetDiagnostics = this.GetDiagnostics, UseDirectSource = this.UseDirectSource };
        }

        private int Timestamp(DateTime d) { return (int)(d.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; }
        private QuotesOptionsData[] ConvertTable(QuotesOptionsType type, JObject table)
        {
            var items = new List<QuotesOptionsData>();
            if (table != null)
            {
                foreach (JObject tr in table["tr"])
                {
                    var lst = new List<object>();
                    foreach (JObject td in tr["td"])
                    {
                        lst.Add(td.HtmlFirstContent(true).ToObject());
                    }
                    items.Add(new QuotesOptionsData(type, lst.ToArray()));
                }
            }
            return items.ToArray();
        }

    }

    /// <summary>
    /// Stores the result data
    /// </summary>
    public class QuotesOptionsResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public QuotesOptionsData[] Calls { get; internal set; }
        public QuotesOptionsData[] Puts { get; internal set; }
        public QuotesBaseData ShortInfo { get; internal set; }
        internal QuotesOptionsResult() { }
    }

}

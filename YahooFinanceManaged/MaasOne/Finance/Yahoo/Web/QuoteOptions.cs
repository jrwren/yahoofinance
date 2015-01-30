// ******************************************************************************
// ** 
// **  Yahoo! Managed
// **  Written by Marius Häusler 2012
// **  It would be pleasant, if you contact me when you are using this code.
// **  Contact: YahooFinanceManaged@gmail.com
// **  Project Home: http://code.google.com/p/yahoo-finance-managed/
// **  
// ******************************************************************************
// **  
// **  Copyright 2012 Marius Häusler
// **  
// **  Licensed under the Apache License, Version 2.0 (the "License");
// **  you may not use this file except in compliance with the License.
// **  You may obtain a copy of the License at
// **  
// **    http://www.apache.org/licenses/LICENSE-2.0
// **  
// **  Unless required by applicable law or agreed to in writing, software
// **  distributed under the License is distributed on an "AS IS" BASIS,
// **  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// **  See the License for the specific language governing permissions and
// **  limitations under the License.
// ** 
// ******************************************************************************
using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.Finance.Yahoo.Data;

namespace MaasOne.Finance.Yahoo.Web
{

    public class QuoteOptionsQuery : Query<QuoteOptionsResult>, IYqlQuery
    {

        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public string ID { get; set; }
        public DateTime Date { get; set; }

        public QuoteOptionsQuery() : this(string.Empty) { }
        public QuoteOptionsQuery(string id)
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
        protected override QuoteOptionsResult ConvertResult(System.IO.Stream stream)
        {
            QuoteOptionsResult result = new QuoteOptionsResult();

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


            if (componentsObject == null) throw new ConvertException("The [table] object could not be load.");

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);

            JObject callsTable = (JObject)((JObject)componentsObject.FindFirst("div", "id", "optionsCallsTable")).FindFirst("table")["tbody"];
            JObject putsTable = (JObject)((JObject)componentsObject.FindFirst("div", "id", "optionsPutsTable")).FindFirst("table")["tbody"];

            result.Calls = this.ConvertTable(QuoteOptionType.Call, callsTable);
            result.Puts = this.ConvertTable(QuoteOptionType.Put, putsTable);


            return result;
        }
        public override Query<QuoteOptionsResult> Clone()
        {
            return new QuoteOptionsQuery(this.ID) { GetDiagnostics = this.GetDiagnostics, UseDirectSource = this.UseDirectSource };
        }

        private int Timestamp(DateTime d) { return (int)(d.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; }
        private QuoteOptionsData[] ConvertTable(QuoteOptionType type, JObject table)
        {
            var items = new List<QuoteOptionsData>();
            if (table != null)
            {
                foreach (JObject tr in table["tr"])
                {
                    var lst = new List<object>();
                    foreach (JObject td in tr["td"])
                    {
                        lst.Add(MyHelper.StringToObject(MyHelper.HtmlFirstContent(td, true), MyHelper.ConverterCulture));
                    }
                    items.Add(new QuoteOptionsData(type, lst.ToArray()));
                }
            }
            return items.ToArray();
        }

    }

    /// <summary>
    /// Stores the result data
    /// </summary>
    public class QuoteOptionsResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public QuoteOptionsData[] Calls { get; internal set; }
        public QuoteOptionsData[] Puts { get; internal set; }
        public QuotesShortInfo ShortInfo { get; internal set; }
        internal QuoteOptionsResult() { }
    }

}

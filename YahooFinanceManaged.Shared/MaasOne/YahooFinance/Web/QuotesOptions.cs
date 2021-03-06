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
    public class QuotesOptionsQuery : YqlQuery<QuotesOptionsResult>
    {
        public string ID { get; set; }

        public DateTime Date { get; set; }


        public QuotesOptionsQuery() { }

        public QuotesOptionsQuery(string id) { this.ID = id; this.Date = NextFriday(); }


        public override QueryBase Clone() { return new QuotesOptionsQuery(this.ID) { Date = this.Date }; }



        protected override string CreateUrl() { return string.Format("http://finance.yahoo.com/q/op?s={0}&date={1}", Uri.EscapeDataString(this.ID), this.Timestamp(this.Date)); }

        protected override void Validate(ValidationResult result)
        {
            if (this.Date.Date < DateTime.Today)
            {
                result.Success = false;
                result.Info.Add("Date", string.Format("{0}: The date must be in the future.", this.ID));
            }
            if (this.Date.DayOfWeek != DayOfWeek.Friday)
            {
                result.Success = false;
                result.Info.Add("Date", string.Format("{0}: The day must be a friday.", this.ID));
            }
        }


        internal override QuotesOptionsResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            QuotesOptionsResult result = new QuotesOptionsResult();

            JObject yqlObject = (JObject)yqlToken;

            JObject componentsObject = (JObject)yqlObject.FindFirst("div", "id", "quote-table");
            JObject shortInfoObject = (JObject)yqlObject.FindFirst("div", "id", "yfi_rt_quote_summary");


            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);

            JObject callsTable = (JObject)((JObject)componentsObject.FindFirst("div", "id", "optionsCallsTable")).FindFirst("table")["tbody"];
            JObject putsTable = (JObject)((JObject)componentsObject.FindFirst("div", "id", "optionsPutsTable")).FindFirst("table")["tbody"];

            result.CallChain = this.ConvertTable(QuotesOptionType.Call, callsTable);
            result.PutChain = this.ConvertTable(QuotesOptionType.Put, putsTable);

            return result;
        }

        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc) { return htmlDoc.FindFirst("div", "id", "yfi_investing_content"); }

        internal override string YqlXPath() { return "//div[@id=\"yfi_investing_content\"]"; }


        private QuotesOption[] ConvertTable(QuotesOptionType type, JObject table)
        {
            var items = new List<QuotesOption>();
            if (table != null)
            {
                foreach (JObject tr in table["tr"])
                {
                    var lst = new List<object>();
                    foreach (JObject td in tr["td"])
                    {
                        lst.Add(td.HtmlFirstContent(true).ToObject());
                    }
                    items.Add(new QuotesOption(type, lst.ToArray()));
                }
            }
            return items.ToArray();
        }

        private int Timestamp(DateTime d) { return (int)Math.Floor((d.Subtract(new DateTime(1970, 1, 1))).TotalSeconds); }


        public static DateTime NextFriday()
        {
            var now = DateTime.UtcNow;
            var today = now.AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second).AddMilliseconds(-now.Millisecond);
            for (int i = 1; i <= 7; i++)
            {
                if (today.AddDays(i).DayOfWeek == DayOfWeek.Friday)
                { return today.AddDays(i); }
            }
            throw new Exception("This exception is not possible");
        }
    }

    public class QuotesOptionsResult : ResultBase
    {
        public QuotesOption[] CallChain { get; internal set; }

        public QuotesOption[] PutChain { get; internal set; }

        public QuotesBase ShortInfo { get; internal set; }


        internal QuotesOptionsResult() { }
    }
}

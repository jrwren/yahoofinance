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
using MaasOne.YahooFinance;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.YahooFinance.Data;

namespace MaasOne.YahooFinance.Web
{

    public class HistoricalQuotesQuery : Query<HistoricalQuotesResult>
    {
        public string ID { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public HistoricalQuotesInterval Interval { get; set; }


        public HistoricalQuotesQuery() : this(null) { }
        public HistoricalQuotesQuery(string id) : this(id, new DateTime(2013, 1, 1), DateTime.Today, HistoricalQuotesInterval.Monthly) { }
        public HistoricalQuotesQuery(string id, System.DateTime fromDate, System.DateTime toDate, HistoricalQuotesInterval interval)
        {
            this.ID = id;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.Interval = interval;
        }


        public override QueryBase Clone()
        {
            return new HistoricalQuotesQuery(this.ID, this.FromDate, this.ToDate, this.Interval);
        }


        protected override HistoricalQuotesResult ConvertResult(System.IO.Stream stream, ConvertInfo ci)
        {
            HistoricalQuotesResult result = new HistoricalQuotesResult();
            string csvText = MyHelper.StreamToString(stream);
            result.Chain = this.ConvertCSV(csvText, this.ID);
            return result;
        }

        protected override string CreateUrl()
        {
            string url = string.Format("http://ichart.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g={7}&ignore=.csv",
                Uri.EscapeDataString(this.ID),
                this.FromDate.Month - 1,
                this.FromDate.Day,
                this.FromDate.Year,
                this.ToDate.Month - 1,
                this.ToDate.Day,
                this.ToDate.Year,
                this.GetHistoricalQuotesInterval(this.Interval)
                );
            return url;
        }

        protected override void Validate(ValidationResult result)
        {
            if (this.FromDate > this.ToDate)
            {
                result.Success = false;
                result.Info.Add("FromDate", "FromDate must be earlier than ToDate.");
            }
            if (this.ID.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Info.Add("ID", "ID is NULL or empty.");
            }
        }


        private char GetHistoricalQuotesInterval(HistoricalQuotesInterval item)
        {
            switch (item)
            {
                case HistoricalQuotesInterval.Daily: return 'd';
                case HistoricalQuotesInterval.Weekly: return 'w';
                default: return 'm';
            }
        }

        private HistoricalQuotesCollection[] ConvertJson(JObject results)
        {
            throw new NotImplementedException();

            List<HistoricalQuotesCollection> quotes = new List<HistoricalQuotesCollection>();
            HistoricalQuotesCollection chain = null;

            var rows = results["row"];

            JArray rowArr = null;
            if (rows is JArray) { rowArr = (JArray)rows; }
            else { rowArr = new JArray((JObject)rows); }

            foreach (JObject row in rowArr)
            {
                if (row["c1"].Value<string>() == "Date")
                {
                    if (chain != null) quotes.Add(chain);
                    //chain = new HistoricalQuotesCollection(this.IDs[quotes.Count]);
                }
                else
                {
                    HistoricalQuotes hqd = new HistoricalQuotes("");//ID

                    DateTime t1;
                    double t2, t3, t4, t5, t7;
                    long t6;
                    if (System.DateTime.TryParse(row["c1"].Value<string>(), MyHelper.ConverterCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out t1)) hqd.TradingDate = t1;
                    if (double.TryParse(row["c2"].Value<string>(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t2)) hqd.Open = t2;
                    if (double.TryParse(row["c3"].Value<string>(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t3)) hqd.High = t3;
                    if (double.TryParse(row["c4"].Value<string>(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t4)) hqd.Low = t4;
                    if (double.TryParse(row["c5"].Value<string>(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t5)) hqd.Close = t5;
                    if (long.TryParse(row["c6"].Value<string>(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t6)) hqd.Volume = t6;
                    if (double.TryParse(row["c7"].Value<string>(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t7)) hqd.CloseAdjusted = t7;

                    chain.Items.Add(hqd);
                }
            }

            if (chain != null) quotes.Add(chain);


            return quotes.ToArray();
        }

        private HistoricalQuotesCollection ConvertCSV(string csvText, string id)
        {
            HistoricalQuotesCollection chain = new HistoricalQuotesCollection(id);
            if (string.IsNullOrEmpty(csvText) == false)
            {
                string[][] table = MyHelper.CsvTextToStringTable(csvText, ',');
                if (table.Length > 0)
                {
                    for (int i = 0; i < table.Length; i++)
                    {
                        if (table[i].Length == 7)
                        {
                            HistoricalQuotes qd = new HistoricalQuotes(chain.ID);
                            DateTime t1;
                            double t2, t3, t4, t5, t7;
                            long t6;
                            if (System.DateTime.TryParse(table[i][0], MyHelper.ConverterCulture, System.Globalization.DateTimeStyles.None, out t1) &&
                                double.TryParse(table[i][1], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t2) &&
                                double.TryParse(table[i][2], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t3) &&
                                double.TryParse(table[i][3], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t4) &&
                                double.TryParse(table[i][4], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t5) &&
                                long.TryParse(table[i][5], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t6) &&
                                double.TryParse(table[i][6], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t7))
                            {
                                qd.TradingDate = t1;
                                qd.Open = t2;
                                qd.High = t3;
                                qd.Low = t4;
                                qd.Close = t5;
                                qd.Volume = t6;
                                qd.CloseAdjusted = t7;

                                chain.Items.Add(qd);
                            }
                        }
                    }
                }
            }
            return chain;
        }

        private void CheckDates(System.DateTime fromDate, System.DateTime toDate)
        {
            if (fromDate > toDate) throw new ArgumentException("The start date is later than the end date.", "fromDate/toDate");
        }
    }

    public class HistoricalQuotesResult : ResultBase
    {
        public HistoricalQuotesCollection Chain { get; internal set; }

        internal HistoricalQuotesResult() { }
    }
}

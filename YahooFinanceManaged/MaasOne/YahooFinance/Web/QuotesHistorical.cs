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
using MaasOne.YahooFinance;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.YahooFinance.Data;

namespace MaasOne.YahooFinance.Web
{

    public class QuotesHistoricalQuery : Query<QuotesHistoricalResult>, IYqlQuery
    {

        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public string[] IDs { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public QuotesHistoricalInterval Interval { get; set; }

        public QuotesHistoricalQuery() : this(string.Empty, new DateTime(2013, 1, 1), DateTime.Today, QuotesHistoricalInterval.Monthly) { }
        public QuotesHistoricalQuery(IID managedID, System.DateTime fromDate, System.DateTime toDate, QuotesHistoricalInterval interval) : this(managedID.ID, fromDate, toDate, interval) { }
        public QuotesHistoricalQuery(string id, System.DateTime fromDate, System.DateTime toDate, QuotesHistoricalInterval interval) : this(string.IsNullOrEmpty(id) ? new string[] { } : new string[] { id }, fromDate, toDate, interval) { }
        public QuotesHistoricalQuery(IEnumerable<IID> managedIDs, System.DateTime fromDate, System.DateTime toDate, QuotesHistoricalInterval interval) : this(YFHelper.IIDsToStrings(managedIDs), fromDate, toDate, interval) { }
        public QuotesHistoricalQuery(IEnumerable<string> ids, System.DateTime fromDate, System.DateTime toDate, QuotesHistoricalInterval interval)
        {
            this.IDs = ids.EnumToArray();
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.Interval = interval;
            this.UseDirectSource = true;
        }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.FromDate > this.ToDate)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("FromDate", "FromDate must be smaller than ToDate."));
            }
            if (this.IDs == null)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("IDs", "No IDs available."));
            }
            else
            {
                if (this.IDs.Length == 0)
                {
                    result.Success = false;
                    result.Info.Add(new KeyValuePair<string, string>("IDs", "No IDs available."));
                }
                else
                {
                    var nonCnt = 0;
                    foreach (var id in this.IDs)
                    {
                        if (string.IsNullOrEmpty(id.Trim())) nonCnt++;
                    }
                    if (nonCnt == this.IDs.Length)
                    {
                        result.Success = false;
                        result.Info.Add(new KeyValuePair<string, string>("IDs", "No valid ID available."));
                    }
                }
            }
        }
        protected override Uri CreateUrl() { return this.GetUrl(this.IDs, this.UseDirectSource); }
        protected override QuotesHistoricalResult ConvertResult(System.IO.Stream stream)
        {
            QuotesHistoricalResult result = new QuotesHistoricalResult();
            if (this.UseDirectSource == true)
            {
                string csvText = MyHelper.StreamToString(stream);
                result.Collections = this.ConvertCSV(csvText, this.IDs[0]);
            }
            else
            {
                    YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                    if (yqlDoc == null) throw new MaasOne.Net.ParseException("Cannot read YQL response data.");
                    result.Diagnostics = yqlDoc.Query.Diagnostics;
                    if (yqlDoc.Query.Results != null)
                    {
                        result.Collections = this.ConvertJson((JObject)yqlDoc.Query.Results);
                    }
            }
            return result;
        }
        public override Query<QuotesHistoricalResult> Clone()
        {
            return new QuotesHistoricalQuery((string[])this.IDs.Clone(), this.FromDate, this.ToDate, this.Interval) { GetDiagnostics = this.GetDiagnostics, UseDirectSource = this.UseDirectSource };
        }

        private char GetHistQuotesInterval(QuotesHistoricalInterval item)
        {
            switch (item)
            {
                case QuotesHistoricalInterval.Daily:
                    return 'd';
                case QuotesHistoricalInterval.Weekly:
                    return 'w';
                default:
                    return 'm';
            }
        }
        private Uri GetUrl(IEnumerable<string> ids, bool useDirectSource)
        {
            string[] idArr = ids.EnumToArray();

            if (useDirectSource == true)
            {
                System.Text.StringBuilder url = new System.Text.StringBuilder();
                url.Append("http://ichart.yahoo.com/table.csv?s=");
                url.Append(Uri.EscapeDataString(YFHelper.CleanYqlParam(idArr[0]).ToUpper()));
                url.Append("&a=");
                url.Append(this.FromDate.Month - 1);
                url.Append("&b=");
                url.Append(this.FromDate.Day);
                url.Append("&c=");
                url.Append(this.FromDate.Year);
                url.Append("&d=");
                url.Append(this.ToDate.Month - 1);
                url.Append("&e=");
                url.Append(this.ToDate.Day);
                url.Append("&f=");
                url.Append(this.ToDate.Year);
                url.Append("&g=");
                url.Append(this.GetHistQuotesInterval(this.Interval));
                url.Append("&ignore=.csv");
                return new Uri(url.ToString(), UriKind.RelativeOrAbsolute);
            }
            else
            {
                System.Text.StringBuilder url = new System.Text.StringBuilder();
                url.Append("url in (");
                for (int i = 0; i <= idArr.Length - 1; i++)
                {
                    url.Append('\'');
                    url.Append(this.GetUrl(new string[] { YFHelper.CleanYqlParam(idArr[i].ToUpper()) }, true));
                    url.Append('\'');
                    if (i < idArr.Length - 1)
                        url.Append(',');
                }
                url.Append(") and columns='c1,c2,c3,c4,c5,c6,c7'");
                return new Uri(YFHelper.YqlUrl(YFHelper.YqlStatement("*", "csv", url.ToString(), null), true, this.GetDiagnostics), UriKind.RelativeOrAbsolute);
            }

        }
        private QuotesHistoricalDataCollection[] ConvertJson(JObject results)
        {
            List<QuotesHistoricalDataCollection> quotes = new List<QuotesHistoricalDataCollection>();
            QuotesHistoricalDataCollection chain = null;

            var rows = results["row"];

            JArray rowArr = null;
            if (rows is JArray) { rowArr = (JArray)rows; }
            else { rowArr = new JArray((JObject)rows); }

            foreach (JObject row in rowArr)
            {
                if (row["c1"].Value<string>() == "Date")
                {
                    if (chain != null) quotes.Add(chain);
                    chain = new QuotesHistoricalDataCollection(this.IDs[quotes.Count]);
                }
                else
                {
                    QuotesHistoricalData hqd = new QuotesHistoricalData();

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
        private QuotesHistoricalDataCollection[] ConvertCSV(string csvText, string id)
        {
            QuotesHistoricalDataCollection chain = new QuotesHistoricalDataCollection();
            if (string.IsNullOrEmpty(csvText) == false)
            {
                string[][] table = MyHelper.CsvTextToStringTable(csvText, ',');
                if (table.Length > 0)
                {
                    for (int i = 0; i < table.Length; i++)
                    {
                        if (table[i].Length == 7)
                        {
                            QuotesHistoricalData qd = new QuotesHistoricalData();
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
            return new QuotesHistoricalDataCollection[] { chain };
        }
        private void CheckDates(System.DateTime fromDate, System.DateTime toDate)
        {
            if (fromDate > toDate) throw new ArgumentException("The start date is later than the end date.", "fromDate/toDate");
        }

    }


    /// <summary>
    /// Stores the result data
    /// </summary>
    public class QuotesHistoricalResult : IYqlResult
    {

        public YqlDiagnostics Diagnostics { get; internal set; }
        public QuotesHistoricalDataCollection[] Collections { get; internal set; }

        internal QuotesHistoricalResult() { }

    }

}

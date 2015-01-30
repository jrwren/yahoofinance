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
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.Finance.Yahoo.Data;

namespace MaasOne.Finance.Yahoo.Web
{

    public class IndustryInfoQuery : Query<IndustryInfoResult>
    {

        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public int[] IDs { get; set; }
        public IndustryQuoteProperty RankedBy { get; set; }
        public System.ComponentModel.ListSortDirection RankDirection { get; set; }

        public IndustryInfoQuery()
        {
            this.IDs = new int[] { };
            this.UseDirectSource = true;
            this.RankedBy = IndustryQuoteProperty.Name;
            this.RankDirection = System.ComponentModel.ListSortDirection.Ascending;
        }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.IDs != null && this.IDs.Length > 0)
            {
                if (this.IDs == null)
                {
                    result.Success = false;
                    result.Info.Add(new KeyValuePair<string, string>("IDs", "The IDs property is null."));
                }
                else
                {
                    foreach (int id in this.IDs)
                    {
                        if (id < 1 || id > 999)
                        {
                            result.Success = false;
                            result.Info.Add(new KeyValuePair<string, string>("IDs", string.Format("The ID [{0}] is not valid. Only IDs in a range from 1 to 999 are valid.", id)));
                        }
                    }
                }
            }
        }
        protected override Uri CreateUrl()
        {
            string url = string.Empty;
            if (this.UseDirectSource == true)
            { url = this.CreateUrl(this.IDs.Length > 0 ? (Nullable<int>)this.IDs[0] : null); }
            else
            {
                string where = "url in (";
                if (this.IDs.Length > 0)
                {
                    foreach (int id in this.IDs)
                    {
                        where += string.Format("'{0}',", this.CreateUrl(id));
                    }
                    where = where.Substring(0, where.Length - 1);
                }
                else
                { where += string.Format("'{0}'", this.CreateUrl(null)); }
                where += ")";
                url = YFHelper.YqlUrl("*", "csv", where, true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        private string CreateUrl(Nullable<int> id)
        {
            var rankType = this.MarketQuotesRankingTypeString(this.RankedBy);
            var rankDir = this.MarketQuotesRankingDirectionString(this.RankDirection);
            var sid = string.Empty;
            if (id.HasValue) { sid = "s_"; }
            else { sid = id.ToString(); }
            return string.Format("http://biz.yahoo.com/p/csv/{0}{1}{2}.csv", sid, rankType, rankDir);
        }
        protected override IndustryInfoResult ConvertResult(System.IO.Stream stream)
        {
            IndustryInfoResult result = new IndustryInfoResult();

            if (this.UseDirectSource == true)
            {
                string csvText = MyHelper.StreamToString(stream);
                result.Collections = this.ConvertCsv(csvText);
            }
            else
            {
                JObject resultsObj = null;
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                result.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    resultsObj = (JObject)yqlDoc.Query.Results;
                }

                if (resultsObj == null) throw new ConvertException("The [result] object could not be load.");

                result.Collections = this.ConvertJson(resultsObj);
            }
            return result;
        }
        public override Query<IndustryInfoResult> Clone()
        {
            return new IndustryInfoQuery()
            {
                IDs = (int[])this.IDs.Clone(),
                RankedBy = this.RankedBy,
                RankDirection = this.RankDirection,
                UseDirectSource = this.UseDirectSource,
                GetDiagnostics = this.GetDiagnostics
            };
        }

        private string MarketQuotesRankingTypeString(IndustryQuoteProperty rankedBy)
        {
            switch (rankedBy)
            {
                case IndustryQuoteProperty.Name:
                    return "coname";
                case IndustryQuoteProperty.DividendYieldPercent:
                    return "yie";
                case IndustryQuoteProperty.LongTermDeptToEquity:
                    return "qto";
                case IndustryQuoteProperty.MarketCapitalizationInMillion:
                    return "mkt";
                case IndustryQuoteProperty.NetProfitMarginPercent:
                    return "qpm";
                case IndustryQuoteProperty.OneDayPriceChangePercent:
                    return "pr1";
                case IndustryQuoteProperty.PriceEarningsRatio:
                    return "pee";
                case IndustryQuoteProperty.PriceToBookValue:
                    return "pri";
                case IndustryQuoteProperty.PriceToFreeCashFlow:
                    return "prf";
                case IndustryQuoteProperty.ReturnOnEquityPercent:
                    return "ttm";
                default:
                    return string.Empty;
            }
        }
        private string MarketQuotesRankingDirectionString(System.ComponentModel.ListSortDirection dir)
        {
            if (dir == System.ComponentModel.ListSortDirection.Ascending)
            {
                return "u";
            }
            else
            {
                return "d";
            }
        }
        private IndustryInfoDataCollection[] ConvertJson(JObject results)
        {
            var lstCollections = new List<IndustryInfoDataCollection>();
            var tempLstData = new List<IndustryInfoData>();

            foreach (var row in results["row"])
            {
                if (row["col2"] == null)
                {
                    lstCollections.Add(new IndustryInfoDataCollection() { Items = tempLstData.ToArray() });
                    tempLstData = new List<IndustryInfoData>();
                }
                else
                {
                    if (row["col2"].ToString() != "Market Cap")
                    {
                        var item = new IndustryInfoData();
                        double t;
                        item.Name = row["col0"].ToString();
                        if (double.TryParse(row["col1"].ToString(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.ChangeInPercent = t;
                        item.MarketCapInMillion = YFHelper.GetMillionValue(row["col2"].ToString());
                        if (double.TryParse(row["col3"].ToString(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.PriceToEarnings = t;
                        if (double.TryParse(row["col4"].ToString(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.ReturnOnEquityInPercent = t;
                        if (double.TryParse(row["col5"].ToString(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.DividendYieldInPercent = t;
                        if (double.TryParse(row["col6"].ToString(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.DeptToEquity = t;
                        if (double.TryParse(row["col7"].ToString(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.PriceBook = t;
                        if (double.TryParse(row["col8"].ToString(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.NetProfitMargin = t;
                        if (double.TryParse(row["col9"].ToString(), System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.PriceToFreeCashFlow = t;
                        tempLstData.Add(item);
                    }
                }
            }
            return lstCollections.ToArray();
        }
        private IndustryInfoDataCollection[] ConvertCsv(string csvText)
        {
            var table = MyHelper.CsvTextToStringTable(csvText, ',');

            var lstItems = new List<IndustryInfoData>();
            bool isHead = true;
            foreach (var row in table)
            {
                if (isHead) { isHead = false; }
                else
                {
                    if (row.Length == 10)
                    {
                        double t;
                        var item = new IndustryInfoData();
                        item.Name = row[0];
                        if (double.TryParse(row[1], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.ChangeInPercent = t;
                        item.MarketCapInMillion = YFHelper.GetMillionValue(row[2]);
                        if (double.TryParse(row[3], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.PriceToEarnings = t;
                        if (double.TryParse(row[4], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.ReturnOnEquityInPercent = t;
                        if (double.TryParse(row[5], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.DividendYieldInPercent = t;
                        if (double.TryParse(row[6], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.DeptToEquity = t;
                        if (double.TryParse(row[7], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.PriceBook = t;
                        if (double.TryParse(row[8], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.NetProfitMargin = t;
                        if (double.TryParse(row[9], System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out t)) item.PriceToFreeCashFlow = t;
                        lstItems.Add(item);
                    }
                }
            }

            return new IndustryInfoDataCollection[] { new IndustryInfoDataCollection() { Items = lstItems.ToArray() } };
        }

    }


    public class IndustryInfoResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public IndustryInfoDataCollection[] Collections { get; internal set; }

        internal IndustryInfoResult() { }
    }


}

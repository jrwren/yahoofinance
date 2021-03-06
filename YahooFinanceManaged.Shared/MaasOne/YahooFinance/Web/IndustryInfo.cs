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

    public class IndustryInfoQuery : Query<IndustryInfoResult>
    {
        public int? ID { get; set; }

        public IndustryProperty RankedBy { get; set; }

        public System.ComponentModel.ListSortDirection RankDirection { get; set; }


        public IndustryInfoQuery()
        {
            this.RankedBy = IndustryProperty.Name;
            this.RankDirection = System.ComponentModel.ListSortDirection.Ascending;
        }

        public IndustryInfoQuery(int id) : this() { this.ID = id; }


        public override QueryBase Clone()
        {
            return new IndustryInfoQuery()
            {
                ID = this.ID,
                RankedBy = this.RankedBy,
                RankDirection = this.RankDirection
            };
        }

        
        protected override IndustryInfoResult ConvertResult(System.IO.Stream stream, ConvertInfo ci)
        {
            string csvText = MyHelper.StreamToString(stream);
            IndustryInfoResult result = new IndustryInfoResult();
            result.Infos = this.ConvertCsv(csvText);
            return result;
        }

        protected override string CreateUrl()
        {
            var rankType = this.MarketQuotesRankingTypeString(this.RankedBy);
            var rankDir = this.MarketQuotesRankingDirectionString(this.RankDirection);
            var sid = string.Empty;
            if (this.ID.HasValue) { sid = "s_"; }
            else { sid = this.ID.Value.ToString(); }
            return string.Format("http://biz.yahoo.com/p/csv/{0}{1}{2}.csv", sid, rankType, rankDir);
        }

        protected override void Validate(ValidationResult result)
        {
            if (this.ID.HasValue)
            {
                if (this.ID.Value <= 0 || this.ID.Value >= 1000)
                {
                    result.Success = false;
                    result.Info.Add("ID", string.Format("ID [{0}] is invalid. Only IDs in a range from 1 to 999 are valid.", this.ID));
                }
            }
        }


        private string MarketQuotesRankingTypeString(IndustryProperty rankedBy)
        {
            switch (rankedBy)
            {
                case IndustryProperty.Name:
                    return "coname";
                case IndustryProperty.DividendYieldPercent:
                    return "yie";
                case IndustryProperty.LongTermDeptToEquity:
                    return "qto";
                case IndustryProperty.MarketCapitalizationInMillion:
                    return "mkt";
                case IndustryProperty.NetProfitMarginPercent:
                    return "qpm";
                case IndustryProperty.OneDayPriceChangePercent:
                    return "pr1";
                case IndustryProperty.PriceEarningsRatio:
                    return "pee";
                case IndustryProperty.PriceToBookValue:
                    return "pri";
                case IndustryProperty.PriceToFreeCashFlow:
                    return "prf";
                case IndustryProperty.ReturnOnEquityPercent:
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
        private IndustryInfoCollection[] ConvertJson(JObject results)
        {
            var lstCollections = new List<IndustryInfoCollection>();
            var tempLstData = new List<IndustryInfo>();

            foreach (var row in results["row"])
            {
                if (row["col2"] == null)
                {
                    lstCollections.Add(new IndustryInfoCollection() { Items = tempLstData.ToArray() });
                    tempLstData = new List<IndustryInfo>();
                }
                else
                {
                    if (row["col2"].ToString() != "Market Cap")
                    {
                        var item = new IndustryInfo();
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
        private IndustryInfoCollection ConvertCsv(string csvText)
        {
            var table = MyHelper.CsvTextToStringTable(csvText, ',');

            var lstItems = new List<IndustryInfo>();
            bool isHead = true;
            foreach (var row in table)
            {
                if (isHead) { isHead = false; }
                else
                {
                    if (row.Length == 10)
                    {
                        double t;
                        var item = new IndustryInfo();
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

            return new IndustryInfoCollection() { Items = lstItems.ToArray() };
        }

    }


    public class IndustryInfoResult : ResultBase
    {
        public IndustryInfoCollection Infos { get; internal set; }

        internal IndustryInfoResult() { }
    }


}

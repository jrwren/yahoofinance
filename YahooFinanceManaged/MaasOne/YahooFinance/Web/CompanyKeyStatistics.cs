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
    public class CompanyKeyStatisticsQuery : Query<CompanyKeyStatisticsResult>, IYqlQuery
    {
        public bool UseDirectSource { get; set; }

        public bool GetDiagnostics { get; set; }

        public string[] IDs { get; set; }

        public CompanyKeyStatisticsQuery() { }
        public CompanyKeyStatisticsQuery(string id) : this(new string[] { id }) { }
        public CompanyKeyStatisticsQuery(IEnumerable<string> ids) { this.IDs = ids.EnumToArray(); }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.IDs == null || this.IDs.Length == 0)
            {
                result.Success = false;
                result.Info.Add("IDs", "No IDs available.");
            }
            else
            {
                if (this.UseDirectSource)
                {
                    if (this.IDs[0].IsNullOrWhiteSpace())
                    {
                        result.Success = false;
                        result.Info.Add("IDs", "No valid ID available.");
                    }
                }
                else
                {
                    var validCnt = 0;
                    foreach (string id in this.IDs) { validCnt += id.IsNullOrWhiteSpace() ? 0 : 1; }
                    if (validCnt == 0)
                    {
                        result.Success = false;
                        result.Info.Add("IDs", "No valid IDs available.");
                    }
                }
            }
        }

        protected override Uri CreateUrl()
        {
            string url = string.Empty;
            if (this.UseDirectSource)
            {
                url = this.CreateUrl(this.IDs[0]);
            }
            else
            {
                string urlIn = string.Empty;
                foreach (var id in this.IDs) { if (!id.IsNullOrWhiteSpace()) urlIn += string.Format("'{0}', ", this.CreateUrl(id)); }
                urlIn = urlIn.Substring(0, urlIn.Length - 2);
                url = YFHelper.YqlUrl("*", "html",
                                       string.Format("url in ({0}) and xpath='//div[@id=\"rightcol\"]'", urlIn),
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        private string CreateUrl(string id) { return string.Format("http://finance.yahoo.com/q/ks?s={0}", Uri.EscapeDataString(id)); }

        protected override CompanyKeyStatisticsResult ConvertResult(System.IO.Stream stream)
        {
            CompanyKeyStatisticsResult result = new CompanyKeyStatisticsResult();

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                result.Data = new CompanyKeyStatisticsData[] { this.ConvertStatistics((JObject)htmlDoc.FindFirst("div", "id", "rightcol")) };
            }
            else
            {
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                result.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    JToken divTok = yqlDoc.Query.Results["div"];
                    JArray divArr = null;
                    if (divTok is JArray) { divArr = (JArray)divTok; }
                    else { divArr = new JArray(divTok); }

                    List<CompanyKeyStatisticsData> lstData = new List<CompanyKeyStatisticsData>();
                    foreach (JObject divObj in divArr) { lstData.Add(this.ConvertStatistics(divObj)); }
                    result.Diagnostics = yqlDoc.Query.Diagnostics;
                    result.Data = lstData.ToArray();
                }
            }

            return result;
        }

        public override Query<CompanyKeyStatisticsResult> Clone()
        {
            return new CompanyKeyStatisticsQuery()
            {
                GetDiagnostics = this.GetDiagnostics,
                UseDirectSource = this.UseDirectSource,
                IDs = (string[])this.IDs.Clone()
            };
        }

        private CompanyKeyStatisticsData ConvertStatistics(JObject rightcolObj)
        {
            var result = new CompanyKeyStatisticsData();

            JObject sumObject = (JObject)rightcolObj.FindFirst("table", "id", "yfncsumtab");
            JObject shortInfoObject = (JObject)rightcolObj.FindFirst("div", "id", "yfi_rt_quote_summary");

            if (sumObject == null) throw new ParseException("The [summary] object could not be load.");

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);

            JObject tr = (JObject)sumObject.FindFirst("tr", "valign", "top");

            var valuationMeasuresTable = (JObject)tr["td"][0]["table"][1];
            var fiscalYearTable = (JObject)tr["td"][0]["table"][3];
            var profitTable = (JObject)tr["td"][0]["table"][4];
            var mngtEffctTable = (JObject)tr["td"][0]["table"][5];
            var incomeTable = (JObject)tr["td"][0]["table"][6];
            var balanceTable = (JObject)tr["td"][0]["table"][7];
            var cashflowTable = (JObject)tr["td"][0]["table"][8];
            var stockPriceTable = (JObject)tr["td"][2]["table"][1];
            var shareTable = (JObject)tr["td"][2]["table"][2];
            var divSplitsTable = (JObject)tr["td"][2]["table"][3];

            var valuationMeasuresObject = new CompanyValuationMeasures(this.ConvertTable(valuationMeasuresTable));
            var fiscalYearObject = new CompanyFiscalYear(this.ConvertTable(fiscalYearTable));
            var profitObject = new CompanyProfitability(this.ConvertTable(profitTable));
            var mngtEffctObject = new CompanyManagementEffectiveness(this.ConvertTable(mngtEffctTable));
            var incomeObject = new CompanyIncomeStatement(this.ConvertTable(incomeTable));
            var balanceObject = new CompanyBalanceSheet(this.ConvertTable(balanceTable));
            var cashflowObject = new CompanyCashFlowStatement(this.ConvertTable(cashflowTable));
            var stockPriceObject = new CompanyStockPriceHistory(this.ConvertTable(stockPriceTable));
            var shareObject = new CompanyShareStatistics(this.ConvertTable(shareTable));
            var divSplitsObject = new CompanyDividendsAndSplits(this.ConvertTable(divSplitsTable));

            result.Statistics = new CompanyKeyStatistics()
            {
                ID = (result.ShortInfo != null ? result.ShortInfo.ID : null),
                ValuationMeasures = valuationMeasuresObject,
                FinancialHighlights = new CompanyFinancialHighlights()
                {
                    FiscalYear = fiscalYearObject,
                    Profitability = profitObject,
                    ManagementEffectiveness = mngtEffctObject,
                    IncomeStatement = incomeObject,
                    BalanceSheet = balanceObject,
                    CashFlowStatement = cashflowObject
                },
                TradingInfo = new CompanyTradingInfo()
                {
                    StockPriceHistory = stockPriceObject,
                    ShareStatistics = shareObject,
                    DividendsAndSplits = divSplitsObject
                }
            };

            return result;
        }

        private object[] ConvertTable(JObject table)
        {
            var lst = new List<object>();
            if (table != null)
            {
                try
                {
                    table = table.HtmlInnerTable();
                    if (table["tr"] is JArray)
                    {
                        var trs = (JArray)table["tr"];

                        int startIndex = (trs[0]["td"] is JObject) ? 1 : 0;
                        for (int i = startIndex; i < trs.Count; i++)
                        {
                            var tr = trs[i];
                            if (tr["td"] is JArray && ((JArray)tr["td"]).Count == 2)
                            {
                                lst.Add(tr["td"][1].HtmlFirstContent().ToObject(MyHelper.ConverterCulture));
                            }
                            else { lst.Add(null); }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var x = ex.Message;
                }
            }
            return lst.ToArray();
        }
    }

    public class CompanyKeyStatisticsResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }

        public CompanyKeyStatisticsData[] Data { get; internal set; }

        internal CompanyKeyStatisticsResult() { }
    }

    public class CompanyKeyStatisticsData
    {
        public CompanyKeyStatistics Statistics { get; internal set; }

        public QuotesBase ShortInfo { get; internal set; }

        internal CompanyKeyStatisticsData() { }
    }
}

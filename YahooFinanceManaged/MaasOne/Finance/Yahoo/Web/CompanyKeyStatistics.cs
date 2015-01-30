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
    public class CompanyKeyStatisticsQuery : Query<CompanyKeyStatisticsResult>, IYqlQuery
    {


        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public string ID { get; set; }

        public CompanyKeyStatisticsQuery() : this(string.Empty) { }
        public CompanyKeyStatisticsQuery(string id) { this.ID = id; }

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
            string url = string.Format("http://finance.yahoo.com/q/ks?s={0}", this.ID);
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       "url='" + url + "' and (xpath='//table[@id=\"yfncsumtab\"]' or xpath='//div[@id=\"yfi_rt_quote_summary\"]')",
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        protected override CompanyKeyStatisticsResult ConvertResult(System.IO.Stream stream)
        {
            CompanyKeyStatisticsResult result = new CompanyKeyStatisticsResult();

            JObject shortInfoObject = null;
            JObject componentsObject = null;

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                componentsObject = (JObject)htmlDoc.FindFirst("table", "id", "yfncsumtab");
                shortInfoObject = (JObject)htmlDoc.FindFirst("div", "id", "yfi_rt_quote_summary");
            }
            else
            {
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                result.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    componentsObject = (JObject)yqlDoc.Query.Results["table"];
                    shortInfoObject = (JObject)yqlDoc.Query.Results["div"];
                }
            }


            if (componentsObject == null) throw new ConvertException("The [profile] object could not be load.");

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);

            JObject tr = (JObject)componentsObject.FindFirst("tr", "valign", "top");


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


            result.Data = new CompanyKeyStatisticsData()
            {
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

            if (result.ShortInfo != null) { result.Data.ID = result.ShortInfo.ID; }
            else { result.Data.ID = this.ID; }

            return result;
        }
        public override Query<CompanyKeyStatisticsResult> Clone()
        {
            return new CompanyKeyStatisticsQuery() { ID = this.ID, GetDiagnostics = this.GetDiagnostics, UseDirectSource = this.UseDirectSource };
        }


        private object[] ConvertTable(JObject table)
        {
            var lst = new List<object>();
            if (table != null)
            {
                try
                {
                    table = MyHelper.HtmlInnerTable(table);
                    if (table["tr"] is JArray)
                    {
                        var trs = (JArray)table["tr"];

                        int startIndex = (trs[0]["td"] is JObject) ? 1 : 0;
                        for (int i = startIndex; i < trs.Count; i++)
                        {
                            var tr = trs[i];
                            if (tr["td"] is JArray && ((JArray)tr["td"]).Count == 2)
                            {
                                lst.Add(MyHelper.StringToObject(MyHelper.HtmlFirstContent(tr["td"][1]), MyHelper.ConverterCulture));
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









    /// <summary>
    /// Stores the result data
    /// </summary>
    public class CompanyKeyStatisticsResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public CompanyKeyStatisticsData Data { get; internal set; }
        public QuotesShortInfo ShortInfo { get; internal set; }
    }



}

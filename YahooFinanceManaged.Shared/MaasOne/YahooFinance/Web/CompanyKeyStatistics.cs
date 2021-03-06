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
    public class CompanyKeyStatisticsQuery : YqlQuery<CompanyKeyStatisticsResult>
    {
        public string ID { get; set; }


        public CompanyKeyStatisticsQuery() { }

        public CompanyKeyStatisticsQuery(string id) { this.ID = id; }


        public override QueryBase Clone() { return new CompanyKeyStatisticsQuery(this.ID); }


        protected override string CreateUrl() { return string.Format("http://finance.yahoo.com/q/ks?s={0}", Uri.EscapeDataString(this.ID)); }

        protected override void Validate(ValidationResult result)
        {
            if (this.ID.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Info.Add("ID", "ID is NULL or empty.");
            }
        }


        internal override CompanyKeyStatisticsResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            CompanyKeyStatisticsResult result = new CompanyKeyStatisticsResult();

            JObject rightcolObj = (JObject)yqlToken;
            
            JObject sumObject = (JObject)rightcolObj.FindFirst("table", "id", "yfncsumtab");
            JObject shortInfoObject = (JObject)rightcolObj.FindFirst("div", "id", "yfi_rt_quote_summary");

            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);

            JObject tr = (JObject)sumObject.FindFirst("tr", "valign", "top");

            JObject valuationMeasuresTable = (JObject)tr["td"][0]["table"][1];
            JObject fiscalYearTable = (JObject)tr["td"][0]["table"][3];
            JObject profitTable = (JObject)tr["td"][0]["table"][4];
            JObject mngtEffctTable = (JObject)tr["td"][0]["table"][5];
            JObject incomeTable = (JObject)tr["td"][0]["table"][6];
            JObject balanceTable = (JObject)tr["td"][0]["table"][7];
            JObject cashflowTable = (JObject)tr["td"][0]["table"][8];
            JObject stockPriceTable = (JObject)tr["td"][2]["table"][1];
            JObject shareTable = (JObject)tr["td"][2]["table"][2];
            JObject divSplitsTable = (JObject)tr["td"][2]["table"][3];

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

        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc) { return htmlDoc.FindFirst("div", "id", "rightcol"); }

        internal override string YqlXPath() { return "//div[@id=\"rightcol\"]"; }


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


    public class CompanyKeyStatisticsResult : ResultBase
    {
        public QuotesBase ShortInfo { get; internal set; }

        public CompanyKeyStatistics Statistics { get; internal set; }


        internal CompanyKeyStatisticsResult() { }
    }
}

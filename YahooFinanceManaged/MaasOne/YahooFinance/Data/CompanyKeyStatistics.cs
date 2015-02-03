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
using System.Collections.Generic;

namespace MaasOne.YahooFinance.Data
{

    /// <summary>
    /// CompanyStatisticsData is a conatiner class for several statistics of a single company.
    /// </summary>
    /// <remarks></remarks>
    public class CompanyKeyStatisticsData : IID
    {

        public string ID { get; set; }
        public CompanyValuationMeasures ValuationMeasures { get; set; }
        public CompanyFinancialHighlights FinancialHighlights { get; set; }
        public CompanyTradingInfo TradingInfo { get; set; }

    }

    public class CompanyValuationMeasures
    {

        /// <summary>
        /// The total dollar value of all outstanding shares. Computed as shares times current market price. Capitalization is a measure of corporate size.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price Per Share x Number of Shares Outstanding
        /// Intraday Value
        /// Shares outstanding is taken from the most recently filed quarterly or annual report and Market Cap is calculated using shares outstanding.</remarks>
        public double MarketCapitalisationInMillion { get; set; }
        /// <summary>
        /// Enterprise Value is a measure of theoretical takeover price, and is useful in comparisons against income statement line items above the interest expense/income lines such as revenue and EBITDA.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Market Cap + Total Debt - Total Cash &amp; Short Term Investments</remarks>
        public double EnterpriseValueInMillion { get; set; }
        /// <summary>
        /// A popular valuation ratio calculated by dividing the current market price by trailing 12-month (ttm) Earnings Per Share.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Earnings Per Share
        /// Intraday Value
        /// Trailing Twelve Months</remarks>
        public double TrailingPE { get; set; }
        /// <summary>
        /// A valuation ratio calculated by dividing the current market price by projected 12-month Earnings Per Share.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Projected Earnings Per Share
        /// Fiscal Year Ending</remarks>
        public double ForwardPE { get; set; }
        /// <summary>
        /// Forward-looking measure rather than typical earnings growth measures, which look eck in time (historical). Used to measure a stock's valuation against its projected 5-yr growth rate.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: P/E Ratio / 5-Yr Expected EPS Growth
        /// 5 years expected</remarks>
        public double PEGRatio { get; set; }
        /// <summary>
        /// A valuation ratio calculated by dividing the current market price by trailing 12-month (ttm) Total Revenues. Often used to value unprofitable companies.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Total Revenues Per Share
        /// Trailing Twelve Months</remarks>
        public double PriceToSales { get; set; }
        /// <summary>
        /// A valuation ratio calculated by dividing the current market price by the most recent quarter's (mrq) Book Value Per Share.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Book Value Per Share
        /// Most Recent Quarter</remarks>
        public double PriceToBook { get; set; }
        /// <summary>
        /// Firm value compared against revenue. Provides a more rigorous comparison than the Price/Sales ratio by removing the effects of capitalization from both sides of the ratio. Since revenue is unaffected by the interest income/expense line item, the appropriate value comparison should also remove the effects of capitalization, as EV does.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Enterprise Value / Total Revenues
        /// Trailing Twelve Months</remarks>
        public double EnterpriseValueToRevenue { get; set; }
        /// <summary>
        /// Firm value compared against EBITDA (Earnings before interest, taxes, depreciation, and amortization). See Enterprise Value/Revenue.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Enterprise Value / EBITDA
        /// Trailing Twelve Months</remarks>
        public double EnterpriseValueToEBITDA { get; set; }

        public CompanyValuationMeasures() { }
        internal CompanyValuationMeasures(object[] values)
        {
            if (values.Length == 9)
            {
                if (values[0] != null && values[0] is double) this.MarketCapitalisationInMillion = (double)values[0];
                if (values[1] != null && values[1] is double) this.EnterpriseValueInMillion = (double)values[1];
                if (values[2] != null && values[2] is double) this.TrailingPE = (double)values[2];
                if (values[3] != null && values[3] is double) this.ForwardPE = (double)values[3];
                if (values[4] != null && values[4] is double) this.PEGRatio = (double)values[4];
                if (values[5] != null && values[5] is double) this.PriceToSales = (double)values[5];
                if (values[6] != null && values[6] is double) this.PriceToBook = (double)values[6];
                if (values[7] != null && values[7] is double) this.EnterpriseValueToRevenue = (double)values[7];
                if (values[8] != null && values[8] is double) this.EnterpriseValueToEBITDA = (double)values[8];
            }
        }

    }

    public class CompanyFinancialHighlights
    {

        public CompanyFiscalYear FiscalYear { get; set; }
        public CompanyProfitability Profitability { get; set; }
        public CompanyManagementEffectiveness ManagementEffectiveness { get; set; }
        public CompanyIncomeStatement IncomeStatement { get; set; }
        public CompanyBalanceSheet BalanceSheet { get; set; }
        public CompanyCashFlowStatement CashFlowStatement { get; set; }

    }

    public class CompanyFiscalYear
    {

        /// <summary>
        /// The date of the end of the firm's accounting year.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime FiscalYearEnds { get; set; }
        /// <summary>
        /// Date for the most recent quarter end for which data is available on the Key Statistics page. This period is often abbreviated as "MRQ."
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime MostRecentQuarter { get; set; }

        public CompanyFiscalYear() { }
        internal CompanyFiscalYear(object[] values)
        {
            if (values.Length == 2)
            {
                if (values[0] != null && values[0] is DateTime) this.FiscalYearEnds = (DateTime)values[0];
                if (values[1] != null && values[1] is DateTime) this.MostRecentQuarter = (DateTime)values[1];
            }
        }

    }

    public class CompanyProfitability
    {

        /// <summary>
        /// Also known as Return on Sales, this value is the Net Income After Taxes for the trailing 12 months divided by Total Revenue for the same period and is expressed as a percentage.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: (Net Income / Total Revenues) x 100
        /// Trailing Twelve Months</remarks>
        public double ProfitMarginPercent { get; set; }
        /// <summary>
        /// This item represents the difference between the Total Revenues and the Total Operating Costs divided by Total Revenues, and is expressed as a percentage. Total Operating Costs consist of: (a) Cost of Goods Sold (b) Total (c) Selling, General &amp; Administrative Expenses (d) Total R &amp; D Expenses (e) Depreciation &amp; Amortization and (f) Total Other Operating Expenses, Total. A ratio used to measure a company's operating efficiency.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: [(Total Revenues - Total Operating Costs) / (Total Revenues)] x 100
        /// Trailing Twelve Months</remarks>
        public double OperatingMarginPercent { get; set; }

        public CompanyProfitability() { }
        internal CompanyProfitability(object[] values)
        {
            if (values.Length == 2)
            {
                if (values[0] != null && values[0] is double) this.ProfitMarginPercent = (double)values[0];
                if (values[1] != null && values[1] is double) this.OperatingMarginPercent = (double)values[1];
            }
        }

    }

    public class CompanyManagementEffectiveness
    {

        /// <summary>
        /// This ratio shows percentage of Returns to Total Assets of the company. This is a useful measure in analyzing how well a company uses its assets to produce earnings.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Earnings from Continuing Operations / Average Total Equity
        /// Trailing Twelve Months</remarks>
        public double ReturnOnAssetsPercent { get; set; }
        /// <summary>
        /// This is a measure of the return on money provided by the firms' owners. This ratio represents Earnings from Continuing Operations divided by average Total Equity and is expressed as a percentage.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: [(Earnings from Continuing Operations) / Total Common Equity] x 100
        /// Trailing Twelve Months</remarks>
        public double ReturnOnEquityPercent { get; set; }

        public CompanyManagementEffectiveness() { }
        internal CompanyManagementEffectiveness(object[] values)
        {
            if (values.Length == 2)
            {
                if (values[0] != null && values[0] is double) this.ReturnOnAssetsPercent = (double)values[0];
                if (values[1] != null && values[1] is double) this.ReturnOnEquityPercent = (double)values[1];
            }
        }

    }

    public class CompanyIncomeStatement
    {

        /// <summary>
        /// The amount of money generated by a company's business activities. Also known as Sales.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Trailing Twelve Months</remarks>
        public double RevenueInMillion { get; set; }
        /// <summary>
        /// Revenue in relation to shares.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Total Revenues / Weighted Average Shares Outstanding
        /// Trailing Twelve Months</remarks>
        public double RevenuePerShare { get; set; }
        /// <summary>
        /// The growth of Quarterly Total Revenues from the same quarter a year ago.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: [(Qtrly Total Revenues x Qtrly Total Revenues (yr ago)) / Qtrly Total Revenues (yr ago)] x 100
        /// Year Over Year</remarks>
        public double QuarterlyRevenueGrowthPercent { get; set; }
        /// <summary>
        /// This item represents Total Revenues minus Cost Of Goods Sold, Total.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Trailing Twelve Months</remarks>
        public double GrossProfitInMillion { get; set; }
        /// <summary>
        /// The accounting acronym EBITDA stands for "Earnings Before Interest, Tax, Depreciation, and Amortization."
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Trailing Twelve Months</remarks>
        public double EBITDAInMillion { get; set; }
        /// <summary>
        /// This ratio shows percentage of Net Income to Common Excluding Extra Items less Earnings Of Discontinued Operations to Total Revenues. This is the dollar amount accruing to common shareholders for dividends and retained earnings.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Net Income - Preferred Dividend and Other Adjustments - Earnings Of Discontinued Operations - Extraordinary Item &amp; Accounting Change
        /// Trailing Twelve Months</remarks>
        public double NetIncomeAvlToCommonInMillion { get; set; }
        /// <summary>
        /// This is the Adjusted Income Available to Common Stockholders (based on Generally Accepted Accounting Principles, GAAP) for the trailing 12 months divided by the trailing 12 month weighted average shares outstanding. Diluted EPS uses diluted weighted average shares in the calculation, or the weighted average shares assuming all convertible securities are exercised.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: (Net Income - Preferred Dividend and Other Adjustments)/ Weighted Average Diluted Shares Outstanding
        /// Trailing Twelve Months</remarks>
        public double DilutedEPS { get; set; }
        /// <summary>
        /// The growth of Quarterly Net Income from the same quarter a year ago.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: [(Qtrly Net Income x Qtrly Net Income (yr ago)) / Qtrly Net Income (yr ago)] x 100
        /// Year Over Year</remarks>
        public double QuaterlyEarningsGrowthPercent { get; set; }

        public CompanyIncomeStatement() { }
        internal CompanyIncomeStatement(object[] values)
        {
            if (values.Length == 8)
            {
                if (values[0] != null && values[0] is double) this.RevenueInMillion = (double)values[0];
                if (values[1] != null && values[1] is double) this.RevenuePerShare = (double)values[1];
                if (values[2] != null && values[2] is double) this.QuarterlyRevenueGrowthPercent = (double)values[2];
                if (values[3] != null && values[3] is double) this.GrossProfitInMillion = (double)values[3];
                if (values[4] != null && values[4] is double) this.EBITDAInMillion = (double)values[4];
                if (values[5] != null && values[5] is double) this.NetIncomeAvlToCommonInMillion = (double)values[5];
                if (values[6] != null && values[6] is double) this.DilutedEPS = (double)values[6];
                if (values[7] != null && values[7] is double) this.QuaterlyEarningsGrowthPercent = (double)values[7];
            }
        }

    }

    public class CompanyBalanceSheet
    {

        /// <summary>
        /// The Total Cash and Short-term Investments on the elance sheet as of the most recent quarter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Most Recent Quarter</remarks>
        public double TotalCashInMillion { get; set; }
        /// <summary>
        /// This is the Total Cash plus Short Term Investments divided by the Shares Outstanding at the end of the most recent fiscal quarter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Most Recent Quarter</remarks>
        public double TotalCashPerShare { get; set; }
        /// <summary>
        /// The Total Debt on the elance sheet as of the most recent quarter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Short Term Borrowings + Current Portion of Long Term Debt + Current Portion of Capital Lease + Long Term Debt + Long Term Capital Lease + Finance Division Debt Current + Finance Division Debt Non Current
        /// Most Recent Quarter</remarks>
        public double TotalDeptInMillion { get; set; }
        /// <summary>
        /// This ratio is Total Debt for the most recent fiscal quarter divided by Total Shareholder Equity for the same period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: [(Long-term Debt + Capital Leases + Finance Division Debt Non-Current + Short-term Borrowings + Current Portion of Long-term Debt + Current Portion of Capital Lease Obligation + Finance Division Debt Current) / (Total Common Equity + Total Preferred Equity)] x 100
        /// Most Recent Quarter</remarks>
        public double TotalDeptPerEquity { get; set; }
        /// <summary>
        /// This is the ratio of Total Current Assets for the most recent quarter divided by Total Current Liabilities for the same period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Total Current Assets / Total Current Liabilities
        /// Most Recent Quarter</remarks>
        public double CurrentRatio { get; set; }
        /// <summary>
        /// This is defined as the Common Shareholder's Equity divided by the Shares Outstanding at the end of the most recent fiscal quarter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Total Common Equity / Total Common Shares Outstanding
        /// Most Recent Quarter</remarks>
        public double BookValuePerShare { get; set; }

        public CompanyBalanceSheet() { }
        internal CompanyBalanceSheet(object[] values)
        {
            if (values.Length == 6)
            {
                if (values[0] != null && values[0] is double) this.TotalCashInMillion = (double)values[0];
                if (values[1] != null && values[1] is double) this.TotalCashPerShare = (double)values[1];
                if (values[2] != null && values[2] is double) this.TotalDeptInMillion = (double)values[2];
                if (values[3] != null && values[3] is double) this.TotalDeptPerEquity = (double)values[3];
                if (values[4] != null && values[4] is double) this.CurrentRatio = (double)values[4];
                if (values[5] != null && values[5] is double) this.BookValuePerShare = (double)values[5];
            }
        }

    }

    public class CompanyCashFlowStatement
    {

        /// <summary>
        /// Net cash used or generated in operating activities during the stated period of time. It reflects net impact of all operating activity transactions on the cash flow of the entity. This GAAP figure is taken directly from the company's Cash Flow Statement and might include significant non-recurring items.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Net Income + Depreciation and Amortization, Total + Other Amortization + Other Non-Cash Items, Total + Change in Working Capital
        /// Trailing Twelve Months</remarks>
        public double OperatingCashFlowInMillion { get; set; }
        /// <summary>
        /// This figure is a normalized item that excludes non-recurring items and also takes into consideration cash inflows from financing activities such as debt or preferred stock issuances.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: (EBIT + Interest Expense) x (1 x Tax Rate) + Depreciation &amp; Amort., Total + Other Amortization + Capital Expenditure + Sale (Purchase) of Intangible assets - Change in Net Working Capital + Pref. Dividends Paid + Total Debt Repaid + Total Debt Issued + Repurchase of Preferred + Issuance of Preferred Stock   -- [Where: Tax Rate = 0.375]
        /// Trailing Twelve Months</remarks>
        public double LeveredFreeCashFlowInMillion { get; set; }

        public CompanyCashFlowStatement() { }
        internal CompanyCashFlowStatement(object[] values)
        {
            if (values.Length == 2)
            {
                if (values[0] != null && values[0] is double) this.OperatingCashFlowInMillion = (double)values[0];
                if (values[1] != null && values[1] is double) this.LeveredFreeCashFlowInMillion = (double)values[1];
            }
        }

    }


    public class CompanyStockPriceHistory
    {

        /// <summary>
        /// The Beta used is Beta of Equity. Beta is the monthly price change of a particular company relative to the monthly price change of the S&amp;P500. The time period for Beta is 3 years (36 months) when available.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Beta { get; set; }
        /// <summary>
        /// The percentage change in price from 52 weeks ago.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double OneYearChangePercent { get; set; }
        /// <summary>
        /// The S&amp;P 500 Index's percentage change in price from 52 weeks ago.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double SP500OneYearChangePercent { get; set; }
        /// <summary>
        /// This price is the highest Price the stock traded at in the last 12 months. This could be an intraday high.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double OneYearHigh { get; set; }
        /// <summary>
        /// This price is the lowest Price the stock traded at in the last 12 months. This could be an intraday low.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double OneYearLow { get; set; }
        /// <summary>
        /// A simple moving average that is calculated by dividing the sum of the closing prices in the last 50 trading days by 50.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double FiftyDayMovingAverage { get; set; }
        /// <summary>
        /// A simple moving average that is calculated by dividing the sum of the closing prices in the last 200 trading days by 200.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double TwoHundredDayMovingAverage { get; set; }

        public CompanyStockPriceHistory() { }
        internal CompanyStockPriceHistory(object[] values)
        {
            if (values.Length == 7)
            {
                if (values[0] != null && values[0] is double) this.Beta = (double)values[0];
                if (values[1] != null && values[1] is double) this.OneYearChangePercent = (double)values[1];
                if (values[2] != null && values[2] is double) this.SP500OneYearChangePercent = (double)values[2];
                if (values[3] != null && values[3] is double) this.OneYearHigh = (double)values[3];
                if (values[4] != null && values[4] is double) this.OneYearLow = (double)values[4];
                if (values[5] != null && values[5] is double) this.FiftyDayMovingAverage = (double)values[5];
                if (values[6] != null && values[6] is double) this.TwoHundredDayMovingAverage = (double)values[6];
            }
        }

    }

    public class CompanyShareStatistics
    {

        /// <summary>
        /// This is the average daily trading volume during the last 3 months.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int AverageVolumeThreeMonth { get; set; }
        /// <summary>
        /// This is the average daily trading volume during the last 10 days.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int AverageVolumeTenDays { get; set; }
        /// <summary>
        /// This is the number of shares of common stock currently outstanding—the number of shares issued minus the shares held in treasury. This field reflects all offerings and acquisitions for stock made after the end of the previous fiscal period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double SharesOutstandingInMillion { get; set; }
        /// <summary>
        /// This is the number of freely traded shares in the hands of the public. Float is calculated as Shares Outstanding minus Shares Owned by Insiders, 5% Owners, and Rule 144 Shares.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double FloatInMillion { get; set; }
        /// <summary>
        /// This is the number of shares currently borrowed by investors for sale, but not yet returned to the owner (lender).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double PercentHeldByInsiders { get; set; }
        public double PercentHeldByInstitutions { get; set; }
        public double SharesShortInMillion { get; set; }
        /// <summary>
        /// This represents the number of days it would take to cover the Short Interest if trading continued at the average daily volume for the month. It is calculated as the Short Interest for the Current Month divided by the Average Daily Volume.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ShortRatio { get; set; }
        /// <summary>
        /// Number of shares short divided by float.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ShortPercentOfFloat { get; set; }
        /// <summary>
        /// Shares Short in the prior month. See Shares Short.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double SharesShortPriorMonthInMillion { get; set; }

        public CompanyShareStatistics() { }
        internal CompanyShareStatistics(object[] values)
        {
            if (values.Length == 10)
            {
                if (values[0] != null && values[0] is int) this.AverageVolumeThreeMonth = (int)values[0];
                if (values[1] != null && values[1] is int) this.AverageVolumeTenDays = (int)values[1];
                if (values[2] != null && values[2] is double) this.SharesOutstandingInMillion = (double)values[2];
                if (values[3] != null && values[3] is double) this.FloatInMillion = (double)values[3];
                if (values[4] != null && values[4] is double) this.PercentHeldByInsiders = (double)values[4];
                if (values[5] != null && values[5] is double) this.PercentHeldByInstitutions = (double)values[5];
                if (values[6] != null && values[6] is double) this.SharesShortInMillion = (double)values[6];
                if (values[7] != null && values[7] is double) this.ShortRatio = (double)values[7];
                if (values[8] != null && values[8] is double) this.ShortPercentOfFloat = (double)values[8];
                if (values[9] != null && values[9] is double) this.SharesShortPriorMonthInMillion = (double)values[9];
            }
        }

    }

    public class CompanyDividendsAndSplits
    {

        /// <summary>
        /// The annualized amount of dividends expected to be paid in the current fiscal year.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ForwardAnnualDividendRate { get; set; }
        /// <summary>
        /// Formula: (Forward Annual Dividend Rate / Current Market Price) x 100
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ForwardAnnualDividendYieldPercent { get; set; }
        /// <summary>
        /// The sum of all dividends paid out in the trailing 12-month period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double TrailingAnnualDividendYield { get; set; }
        /// <summary>
        /// Formula: (Trailing Annual Dividend Rate / Current Market Price) x 100
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double TrailingAnnualDividendYieldPercent { get; set; }
        /// <summary>
        /// The average Forward Annual Dividend Yield in the past 5 years.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double FiveYearAverageDividendYieldPercent { get; set; }
        /// <summary>
        /// The ratio of Earnings paid out in Dividends, expressed as a percentage.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double PayoutRatioPercent { get; set; }
        /// <summary>
        /// The payment date for a declared dividend.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime DividendDate { get; set; }
        /// <summary>
        /// The first day of trading when the seller, rather than the buyer, of a stock is entitled to the most recently announced dividend payment. The date set by the NYSE (and generally followed on other U.S. exchanges) is currently two business days before the record date. A stock that has gone ex-dividend is denoted by an x in newspaper listings on that date.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime ExDividendDate { get; set; }
        public DateTime LastSplitDate { get; set; }
        public CompanySharesSplitFactor LastSplitFactor { get; set; }

        public CompanyDividendsAndSplits() { }
        internal CompanyDividendsAndSplits(object[] values)
        {
            if (values.Length == 10)
            {
                if (values[0] != null && values[0] is double) this.ForwardAnnualDividendRate = (double)values[0];
                if (values[1] != null && values[1] is double) this.ForwardAnnualDividendYieldPercent = (double)values[1];
                if (values[2] != null && values[2] is double) this.TrailingAnnualDividendYield = (double)values[2];
                if (values[3] != null && values[3] is double) this.TrailingAnnualDividendYieldPercent = (double)values[3];
                if (values[4] != null && values[4] is double) this.FiveYearAverageDividendYieldPercent = (double)values[4];
                if (values[5] != null && values[5] is double) this.PayoutRatioPercent = (double)values[5];
                if (values[6] != null && values[6] is DateTime) this.DividendDate = (DateTime)values[6];
                if (values[7] != null && values[7] is DateTime) this.ExDividendDate = (DateTime)values[7];
                if (values[9] != null && values[8] is string) this.LastSplitFactor = new CompanySharesSplitFactor((string)values[8]);
                if (values[8] != null && values[9] is DateTime) this.LastSplitDate = (DateTime)values[9];
            }
        }

    }

    public class CompanyTradingInfo
    {

        public CompanyStockPriceHistory StockPriceHistory { get; set; }
        public CompanyShareStatistics ShareStatistics { get; set; }
        public CompanyDividendsAndSplits DividendsAndSplits { get; set; }

    }

    public class CompanySharesSplitFactor
    {

        /// <summary>
        /// Old relational value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int OldShares { get; set; }
        /// <summary>
        /// New relational value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int NewShares { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="newShares">The new number of shares after splitting (relative)</param>
        /// <param name="forOldShares">The old number of shares before splitting (relative)</param>
        /// <remarks></remarks>
        public CompanySharesSplitFactor(int newShares, int forOldShares)
        {
            this.OldShares = forOldShares;
            this.NewShares = newShares;
        }
        internal CompanySharesSplitFactor(string val)
        {
            var parts = val.Trim().Split(':');
            if (parts.Length == 2)
            {
                this.NewShares = int.Parse(parts[0]);
                this.OldShares = int.Parse(parts[1]);
            }
        }
        public override string ToString()
        {
            return string.Format("{0} : {1}", this.NewShares, this.OldShares);
        }

    }

}

using System;
using System.Collections.Generic;

namespace MaasOne.Finance.Yahoo.Web
{

    /// <summary>
    /// Provides the available sizes for chart images.
    /// </summary>
    /// <remarks></remarks>
    public enum ChartImageSize
    {
        /// <summary>
        /// Large images have a size of 800 x 355 px.
        /// </summary>
        /// <remarks></remarks>
        Large,
        /// <summary>
        /// Middle images have a size of 512 x 288 px.
        /// </summary>
        /// <remarks></remarks>
        Middle,
        /// <summary>
        /// Small images have a size of 60 x 16 px.
        /// </summary>
        /// <remarks></remarks>
        Small
    }

    /// <summary>
    /// Provides the available scales for chart images.
    /// </summary>
    /// <remarks>
    /// Arithmetic: If the scale provides values from 0 to 100: Value 25 is at 1/4 of 'world'/visible values; 50 is at 1/2; 75 is at 3/4. ///\\\ Logarithmic: If the scale provides values from 0 to 100: Value 10 is at 50/100 of 'world'/visible values; 25 is at 69/100; 50 is at 85/100. The sense of this scale is to relate the absolute changings of a chart. If a stock has a price of $10 and jumps to $20. It's a rise of $10 or 100%. If a stock price is $100 and is increasing of $10 the rise is just 10%.
    /// </remarks>
    public enum ChartScale
    {
        /// <summary>
        /// Arithmetic scale has the same proportion in y-axis to the esed values.
        /// </summary>
        /// <remarks></remarks>
        Arithmetic,
        /// <summary>
        /// Logarithmic scale has for same value differences shorter going differences in y-axis lines (based on logarithmic calculation). 
        /// </summary>
        /// <remarks></remarks>
        Logarithmic
    }

    /// <summary>
    /// Provides the time spans for showed data base.
    /// </summary>
    /// <remarks></remarks>
    public enum ChartTimeSpan
    {
        /// <summary>
        /// 1 Day
        /// </summary>
        /// <remarks></remarks>
        c1D,
        /// <summary>
        /// 5 Days
        /// </summary>
        /// <remarks></remarks>
        c5D,
        /// <summary>
        /// 3 Months
        /// </summary>
        /// <remarks></remarks>
        c3M,
        /// <summary>
        /// 6 Months
        /// </summary>
        /// <remarks></remarks>
        c6M,
        /// <summary>
        /// 1 Year
        /// </summary>
        /// <remarks></remarks>
        c1Y,
        /// <summary>
        /// 2 Years
        /// </summary>
        /// <remarks></remarks>
        c2Y,
        /// <summary>
        /// 5 Years
        /// </summary>
        /// <remarks></remarks>
        c5Y,
        /// <summary>
        /// Maximum
        /// </summary>
        /// <remarks></remarks>
        cMax
    }

    /// <summary>
    /// Provides the chart type of the chart image.
    /// </summary>
    /// <remarks></remarks>
    public enum ChartType
    {
        /// <summary>
        /// Line
        /// </summary>
        /// <remarks></remarks>
        Line,
        /// <summary>
        /// er
        /// </summary>
        /// <remarks></remarks>
        Bar,
        /// <summary>
        /// Candle
        /// </summary>
        /// <remarks></remarks>
        Candle
    }

    /// <summary>
    /// Provides the markets for limitation at ID search.
    /// </summary>
    /// <remarks></remarks>
    public enum IDSearchMarket
    {
        AllMarkets,
        France,
        Germany,
        Spain,
        UK,
        UsAndCanada
    }

    /// <summary>
    /// Provides the available sortable properties at ID search.
    /// </summary>
    /// <remarks></remarks>
    public enum IDSearchRankProperty
    {
        /// <summary>
        /// No special property
        /// </summary>
        /// <remarks></remarks>
        NoRanking,
        /// <summary>
        /// ID
        /// </summary>
        /// <remarks></remarks>
        ID,
        /// <summary>
        /// Name
        /// </summary>
        /// <remarks></remarks>
        Name,
        /// <summary>
        /// Category
        /// </summary>
        /// <remarks></remarks>
        Type,
        /// <summary>
        /// Exchange
        /// </summary>
        /// <remarks></remarks>
        Exchange
    }

    /// <summary>
    /// Provides the available intervals between received HistQuoteData items.
    /// </summary>
    /// <remarks>Daily Historical Quotes provide you with the daily open, high, low, close, and volume for each trading day in the chosen date range. Weekly Historical Quotes retrieve the open trade from the first trading day for the week, the high and low price quotes of the week, and the closing price on the last trading day of the week. The weekly volume is the average daily volume for all trading days in the reported week. Monthly Historical Quotes report the open trade from the first trading day of the month, the high and low price quotes for the month, and the closing price on the last trading day of the month. The monthly volume is the average daily volume for all trading days in the reported month.</remarks>
    public enum HistoricalQuotesInterval
    {
        /// <summary>
        /// Daily
        /// </summary>
        /// <remarks></remarks>
        Daily,
        /// <summary>
        /// Weekly
        /// </summary>
        /// <remarks></remarks>
        Weekly,
        /// <summary>
        /// Monthly
        /// </summary>
        /// <remarks></remarks>
        Monthly
    }

    /// <summary>
    /// Provides every available property of market quote data.
    /// </summary>
    /// <remarks></remarks>
    public enum IndustryQuoteProperty
    {
        /// <summary>
        /// Dividend Yield Percent
        /// </summary>
        /// <remarks></remarks>
        DividendYieldPercent,
        /// <summary>
        /// Long Term Dept To Equity
        /// </summary>
        /// <remarks></remarks>
        LongTermDeptToEquity,
        /// <summary>
        /// Market Capitalization In Million
        /// </summary>
        /// <remarks></remarks>
        MarketCapitalizationInMillion,
        /// <summary>
        /// Name
        /// </summary>
        /// <remarks></remarks>
        Name,
        /// <summary>
        /// Net Profit Margin in Percent
        /// </summary>
        /// <remarks></remarks>
        NetProfitMarginPercent,
        /// <summary>
        /// One Day Price Change Percent
        /// </summary>
        /// <remarks></remarks>
        OneDayPriceChangePercent,
        /// <summary>
        /// Price Earnings Ratio
        /// </summary>
        /// <remarks></remarks>
        PriceEarningsRatio,
        /// <summary>
        /// Price To Book Value
        /// </summary>
        /// <remarks></remarks>
        PriceToBookValue,
        /// <summary>
        /// Price To Free Cash Flow
        /// </summary>
        /// <remarks></remarks>
        PriceToFreeCashFlow,
        /// <summary>
        /// Return On Equity Percent
        /// </summary>
        /// <remarks></remarks>
        ReturnOnEquityPercent
    }

    /// <summary>
    /// The time span for the value ese of a calculated moving average. A bigger time span results in a more straightened line with less reaction to short term changings.
    /// </summary>
    /// <remarks></remarks>
    public enum ChartMovingAverageInterval
    {
        /// <summary>
        /// 5
        /// </summary>
        /// <remarks></remarks>
        m5,
        /// <summary>
        /// 10
        /// </summary>
        /// <remarks></remarks>
        m10,
        /// <summary>
        /// 20
        /// </summary>
        /// <remarks></remarks>
        m20,
        /// <summary>
        /// 50
        /// </summary>
        /// <remarks></remarks>
        m50,
        /// <summary>
        /// 100
        /// </summary>
        /// <remarks></remarks>
        m100,
        /// <summary>
        /// 200
        /// </summary>
        /// <remarks></remarks>
        m200
    }

    /// <summary>
    /// Provides the two financial option type 'Call' and 'Put'.
    /// </summary>
    /// <remarks></remarks>
    public enum QuoteOptionType
    {
        /// <summary>
        /// Call
        /// </summary>
        /// <remarks></remarks>
        Call,
        /// <summary>
        /// Put
        /// </summary>
        /// <remarks></remarks>
        Put
    }

    /// <summary>
    /// Provides different technical indicators for chart images.
    /// </summary>
    /// <remarks></remarks>
    public enum ChartTechnicalIndicator
    {
        /// <summary>
        /// Stochastic
        /// </summary>
        /// <remarks></remarks>
        Fast_Stoch,
        /// <summary>
        /// Moving-Average-Convergence-Divergence
        /// </summary>
        /// <remarks></remarks>
        MACD,
        /// <summary>
        /// Money Flow Index
        /// </summary>
        /// <remarks></remarks>
        MFI,
        /// <summary>
        /// Rate of Change
        /// </summary>
        /// <remarks></remarks>
        ROC,
        /// <summary>
        /// Relative Strength Index
        /// </summary>
        /// <remarks></remarks>
        RSI,
        /// <summary>
        /// Slow Stochastic
        /// </summary>
        /// <remarks></remarks>
        Slow_Stoch,
        /// <summary>
        /// Volume
        /// </summary>
        /// <remarks></remarks>
        Vol,
        /// <summary>
        /// Volume with Moving Average
        /// </summary>
        /// <remarks></remarks>
        Vol_MA,
        /// <summary>
        /// Williams Percent Range
        /// </summary>
        /// <remarks></remarks>
        W_R,
        /// <summary>
        /// Bollinger ends
        /// </summary>
        /// <remarks></remarks>
        Bollinger_Bands,
        /// <summary>
        /// Parabolic Stop And Reverse
        /// </summary>
        /// <remarks></remarks>
        Parabolic_SAR,
        /// <summary>
        /// Splits
        /// </summary>
        /// <remarks></remarks>
        Splits,
        /// <summary>
        /// Volume (inside chart)
        /// </summary>
        /// <remarks></remarks>
        Volume
    }

}

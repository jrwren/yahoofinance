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
    /// Provides every available property of quote data.
    /// </summary>
    /// <remarks></remarks>
    public enum QuoteProperty
    {
        /// <summary>
        /// AfterHoursChangeRealtime
        /// </summary>
        /// <remarks></remarks>
        AfterHoursChangeRealtime,

        /// <summary>
        /// Annualized Gain
        /// </summary>
        /// <remarks></remarks>
        AnnualizedGain,

        /// <summary>
        /// Ask Size
        /// </summary>
        /// <remarks></remarks>
        Ask,

        /// <summary>
        /// Ask (Realtime)
        /// </summary>
        /// <remarks></remarks>
        AskRealtime,

        /// <summary>
        /// Ask Size
        /// </summary>
        /// <remarks></remarks>
        AskSize,

        /// <summary>
        /// Average Daily Volume
        /// </summary>
        /// <remarks></remarks>
        AverageDailyVolume,

        /// <summary>
        /// Bid Size
        /// </summary>
        /// <remarks></remarks>
        Bid,

        /// <summary>
        /// Bid (Realtime)
        /// </summary>
        /// <remarks></remarks>
        BidRealtime,

        /// <summary>
        /// Bid Size
        /// </summary>
        /// <remarks></remarks>
        BidSize,

        /// <summary>
        /// Book Value
        /// </summary>
        /// <remarks></remarks>
        BookValuePerShare,

        /// <summary>
        /// Change
        /// </summary>
        /// <remarks></remarks>
        Change,

        /// <summary>
        /// Change Percent
        /// </summary>
        /// <remarks></remarks>
        Change_ChangeInPercent,

        /// <summary>
        /// Change From 50 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        ChangeFromFiftydayMovingAverage,

        /// <summary>
        /// Change From 200 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        ChangeFromTwoHundreddayMovingAverage,

        /// <summary>
        /// Change From 1 Year High
        /// </summary>
        /// <remarks></remarks>
        ChangeFromYearHigh,

        /// <summary>
        /// Change From 1 Year Low
        /// </summary>
        /// <remarks></remarks>
        ChangeFromYearLow,

        /// <summary>
        /// Change In Percent
        /// </summary>
        /// <remarks></remarks>
        ChangeInPercent,

        /// <summary>
        /// Change Percent (Realtime)
        /// </summary>
        /// <remarks></remarks>
        ChangeInPercentRealtime,

        /// <summary>
        /// Days Value Change (Realtime)
        /// </summary>
        /// <remarks></remarks>
        ChangeRealtime,

        /// <summary>
        /// Commission
        /// </summary>
        /// <remarks></remarks>
        Commission,

        /// <summary>
        /// Currency
        /// </summary>
        /// <remarks></remarks>
        Currency,

        /// <summary>
        /// Days High
        /// </summary>
        /// <remarks></remarks>
        DaysHigh,

        /// <summary>
        /// Days Low
        /// </summary>
        /// <remarks></remarks>
        DaysLow,

        /// <summary>
        /// Days Range (Realtime)
        /// </summary>
        /// <remarks></remarks>
        DaysRange,

        /// <summary>
        /// Days Range (Realtime)
        /// </summary>
        /// <remarks></remarks>
        DaysRangeRealtime,

        /// <summary>
        /// Days Value Change
        /// </summary>
        /// <remarks></remarks>
        DaysValueChange,

        /// <summary>
        /// Days Value Change (Realtime)
        /// </summary>
        /// <remarks></remarks>
        DaysValueChangeRealtime,

        /// <summary>
        /// Dividend Pay Date
        /// </summary>
        /// <remarks></remarks>
        DividendPayDate,

        /// <summary>
        /// Dividend Share
        /// </summary>
        /// <remarks></remarks>
        TrailingAnnualDividendYield,

        /// <summary>
        /// Dividend Yield
        /// </summary>
        /// <remarks></remarks>
        TrailingAnnualDividendYieldInPercent,

        /// <summary>
        /// Earnings Share
        /// </summary>
        /// <remarks></remarks>
        DilutedEPS,

        /// <summary>
        /// EBITDA
        /// </summary>
        /// <remarks></remarks>
        EBITDA,

        /// <summary>
        /// Price EPS Estimate Current Year
        /// </summary>
        /// <remarks></remarks>
        EPSEstimateCurrentYear,

        /// <summary>
        /// EPS Estimate Next Quarter
        /// </summary>
        /// <remarks></remarks>
        EPSEstimateNextQuarter,

        /// <summary>
        /// Price EPS Estimate Next Year
        /// </summary>
        /// <remarks></remarks>
        EPSEstimateNextYear,

        /// <summary>
        /// Ex Dividend Date
        /// </summary>
        /// <remarks></remarks>
        ExDividendDate,

        /// <summary>
        /// 50 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        FiftydayMovingAverage,

        /// <summary>
        /// Float Shares
        /// </summary>
        /// <remarks></remarks>
        SharesFloat,

        /// <summary>
        /// High Limit
        /// </summary>
        /// <remarks></remarks>
        HighLimit,

        /// <summary>
        /// Holdings Gain
        /// </summary>
        /// <remarks></remarks>
        HoldingsGain,

        /// <summary>
        /// Holdings Gain Percent
        /// </summary>
        /// <remarks></remarks>
        HoldingsGainPercent,

        /// <summary>
        /// Holdings Gain Percent (Realtime)
        /// </summary>
        /// <remarks></remarks>
        HoldingsGainPercentRealtime,

        /// <summary>
        /// Holdings Gain (Realtime)
        /// </summary>
        /// <remarks></remarks>
        HoldingsGainRealtime,

        /// <summary>
        /// Holdings Value (Realtime)
        /// </summary>
        /// <remarks></remarks>
        HoldingsValue,

        /// <summary>
        /// Holdings Value (Realtime)
        /// </summary>
        /// <remarks></remarks>
        HoldingsValueRealtime,

        /// <summary>
        /// Last Trade Date
        /// </summary>
        /// <remarks></remarks>
        LastTradeDate,

        /// <summary>
        /// Last Trade Price Only
        /// </summary>
        /// <remarks></remarks>
        LastTradePriceOnly,

        /// <summary>
        /// Last Trade With Time (Realtime)
        /// </summary>
        /// <remarks></remarks>
        LastTradeRealtimeWithTime,

        LastTradeSize,

        /// <summary>
        /// Last Trade Time
        /// </summary>
        /// <remarks></remarks>
        LastTradeTime,

        /// <summary>
        /// Last Trade With Time
        /// </summary>
        /// <remarks></remarks>
        LastTradeWithTime,

        /// <summary>
        /// Low Limit
        /// </summary>
        /// <remarks></remarks>
        LowLimit,

        /// <summary>
        /// Market Capitalization
        /// </summary>
        /// <remarks></remarks>
        MarketCapitalization,

        /// <summary>
        /// Market Capitalization (Realtime)
        /// </summary>
        /// <remarks></remarks>
        MarketCapRealtime,

        /// <summary>
        /// More Info
        /// </summary>
        /// <remarks></remarks>
        MoreInfo,

        /// <summary>
        /// Name
        /// </summary>
        /// <remarks></remarks>
        Name,

        /// <summary>
        /// Notes
        /// </summary>
        /// <remarks></remarks>
        Notes,

        /// <summary>
        /// 1 Year Target Price
        /// </summary>
        /// <remarks></remarks>
        OneyrTargetPrice,

        /// <summary>
        /// Open
        /// </summary>
        /// <remarks></remarks>
        Open,

        /// <summary>
        /// Order Book (Realtime)
        /// </summary>
        /// <remarks></remarks>
        OrderBookRealtime,

        /// <summary>
        /// PEG Ratio
        /// </summary>
        /// <remarks></remarks>
        PEGRatio,

        /// <summary>
        /// PE Ratio (Realtime)
        /// </summary>
        /// <remarks></remarks>
        PERatio,

        /// <summary>
        /// PE Ratio (Realtime)
        /// </summary>
        /// <remarks></remarks>
        PERatioRealtime,

        /// <summary>
        /// Percent Change From 50 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        PercentChangeFromFiftydayMovingAverage,

        /// <summary>
        /// Percent Change From 200 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        PercentChangeFromTwoHundreddayMovingAverage,

        /// <summary>
        /// Percent Change From 1 Year High
        /// </summary>
        /// <remarks></remarks>
        ChangeInPercentFromYearHigh,

        /// <summary>
        /// Percent Change From 1 Year Low
        /// </summary>
        /// <remarks></remarks>
        PercentChangeFromYearLow,

        /// <summary>
        /// Previous Close
        /// </summary>
        /// <remarks></remarks>
        PreviousClose,

        /// <summary>
        /// Price Book
        /// </summary>
        /// <remarks></remarks>
        PriceBook,

        /// <summary>
        /// Price EPS Estimate Current Year
        /// </summary>
        /// <remarks></remarks>
        PriceEPSEstimateCurrentYear,

        /// <summary>
        /// Price EPS Estimate Next Year
        /// </summary>
        /// <remarks></remarks>
        PriceEPSEstimateNextYear,

        /// <summary>
        /// Price Paid
        /// </summary>
        /// <remarks></remarks>
        PricePaid,

        /// <summary>
        /// Price Sales
        /// </summary>
        /// <remarks></remarks>
        PriceSales,

        /// <summary>
        /// Revenue (ttm)
        /// </summary>
        /// <remarks></remarks>
        Revenue,

        /// <summary>
        /// Shares Owned
        /// </summary>
        /// <remarks></remarks>
        SharesOwned,

        /// <summary>
        /// Shares Outstanding
        /// </summary>
        /// <remarks></remarks>
        SharesOutstanding,

        /// <summary>
        /// Short Ratio
        /// </summary>
        /// <remarks></remarks>
        ShortRatio,

        /// <summary>
        /// Stock Exchange
        /// </summary>
        /// <remarks></remarks>
        StockExchange,

        /// <summary>
        /// Symbol
        /// </summary>
        /// <remarks></remarks>
        Symbol,

        /// <summary>
        /// Ticker Trend
        /// </summary>
        /// <remarks></remarks>
        TickerTrend,

        /// <summary>
        /// Trade Date
        /// </summary>
        /// <remarks></remarks>
        TradeDate,

        /// <summary>
        /// Trade Links
        /// </summary>
        /// <remarks></remarks>
        TradeLinks,

        /// <summary>
        /// Additional Html for Trade Links
        /// </summary>
        /// <remarks></remarks>
        TradeLinksAdditional,

        /// <summary>
        /// 200 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        TwoHundreddayMovingAverage,

        /// <summary>
        /// Volume td
        /// </summary>
        /// <remarks></remarks>
        Volume,

        /// <summary>
        /// 1 Year High
        /// </summary>
        /// <remarks></remarks>
        YearHigh,

        /// <summary>
        /// 1 Year Low
        /// </summary>
        /// <remarks></remarks>
        YearLow,

        /// <summary>
        /// 1 Year Range
        /// </summary>
        /// <remarks></remarks>
        YearRange
    }
    
    public enum ScreenerProperty
    {
        //Stocks
        Symbol,
        Name,
        Industry,
        IndexMembership,
        RetailPrice,
        AverageVolume,
        MarketCapitalization,
        DividendYieldRatio,
        ReturnPercent,
        Beta,
        Sales,
        ProfitMargin,
        PriceEarningsRatio,
        PriceBookRatio,
        PriceSalesRatio,
        PEG,
        FiveYearGrowth,
        Growth,
        AnalystRecommend,

        //Funds
        Category,
        RankPercent,
        ManagerTenureYears,
        MorningstarRating,
        ReturnRating,
        RiskRating,
        NetAssets,
        TurnoverPercent,
        HoldingMedianMarketCap,
        MinInvest,
        FrontLoadPercent,
        ExpenseRatioPercent,
        ReturnYearToDatePercent,
        OneYearReturn,
        Annualized3YearReturnPercent,
        Annualized5YearreturnPercent,

        //Bonds
        Type,
        Issue,
        Price,
        CouponPercent,
        Maturity,
        YTMPercent,
        CurrentYieldPercent,
        FitchRating,
        Callable
    }

    public enum ETFSearchProperty
    {
        ID,
        Name,
        Category,
        Family,
        ReturnIntraday,
        ReturnYTD,
        Return3Month,
        Return1Y,
        Return3Y,
        ReturnYTDNAV,
        Return3MNAV,
        Return1YNAV,
        Return5YNAV,
        VolumeIntraday,
        Volume3MAvg,
        LastTrade,
        YearHigh,
        YearLow,
        AvgMarketCap,
        PortfolioPE,
        PortfolioPS,
        PortfolioPCash,
        PortfolioPB,
        EarningsGrowth,
        Alpha3Y,
        Beta3Y,
        RSqrd3Y,
        NetAssets,
        ExpenseRatio,
        AnnualTurnover,
        InceptDate
    }

    public enum BondType
    {
        Corporate,
        Treasury,
        TreasuryZeroCoupon,
        Municipal
    }

    public enum FundMorningstarRating
    {
        OneStar = 1,
        TwoStars = 2,
        ThreeStars = 3,
        FourStars = 4,
        FiveStars = 5
    }

    public enum BondFitchRating
    {
        AAA = 1,
        AA = 2,
        A = 3,
        BBB = 4,
        BB = 5,
        B = 6,
        CCC = 7,
        CC = 8,
        D = 9,
        NR = 10
    }

}

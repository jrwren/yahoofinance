// ******************************************************************************
// ** 
// **  Yahoo! Managed
// **  Written by Marius Häusler 2012
// **  It would be pleasant, if you contact me when you are using this code.
// **  Contact: YahooFinanceManaged@gmail.com
// **  Project Home: http://code.google.com/p/yahoo-finance-managed/
// **  
// ******************************************************************************
// **  
// **  Copyright 2012 Marius Häusler
// **  
// **  Licensed under the Apache License, Version 2.0 (the "License");
// **  you may not use this file except in compliance with the License.
// **  You may obtain a copy of the License at
// **  
// **    http://www.apache.org/licenses/LICENSE-2.0
// **  
// **  Unless required by applicable law or agreed to in writing, software
// **  distributed under the License is distributed on an "AS IS" BASIS,
// **  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// **  See the License for the specific language governing permissions and
// **  limitations under the License.
// ** 
// ******************************************************************************


namespace MaasOne.Finance.Yahoo.Screener
{

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
        AverageRec,

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

    public enum BondType
    {
        Corporate,
        Treasury,
        TreasuryZeroCoupon,
        Municipal
    }

    public enum BondPriceType
    {
        Premium = 1,
        Par = 2,
        Discount = 3
    }

    public enum UsState
    {
        AL,
        AK,
        AZ,
        AR,
        CA,
        CO,
        CT,
        DE,
        FL,
        GA,
        HI,
        ID,
        IL,
        IN,
        IA,
        KS,
        KY,
        LA,
        ME,
        MD,
        MA,
        MI,
        MN,
        MS,
        MO,
        MT,
        NE,
        NV,
        NH,
        NJ,
        NM,
        NY,
        NC,
        ND,
        OH,
        OK,
        OR,
        PA,
        RI,
        SC,
        SD,
        TN,
        TX,
        UT,
        VT,
        VA,
        WA,
        WV,
        WI,
        WY
    }

    public enum MorningstarRating
    {
        OneStar = 1,
        TwoStars = 2,
        ThreeStars = 3,
        FourStars = 4,
        FiveStars = 5
    }
    
    public enum FitchRating
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

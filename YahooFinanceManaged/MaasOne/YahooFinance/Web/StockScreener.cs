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
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.YahooFinance.Data;

namespace MaasOne.YahooFinance.Web
{



    public class StockScreenerQuery : ScreenerQuery
    {

        public StockScreenerView View { get; set; }
        public StockScreenerPagination Pagination { get; set; }

        public ScreenerProperty RankedBy { get; set; }
        public System.ComponentModel.ListSortDirection RankDirection { get; set; }

        public StockScreenerQuery()
        {
            this.View = StockScreenerView.Screened;
            this.RankedBy = ScreenerProperty.Name;
            this.RankDirection = System.ComponentModel.ListSortDirection.Ascending;
            this.Pagination = new StockScreenerPagination();
        }

        protected override void ValidateQuery(ValidationResult result)
        {
            base.ValidateQuery(result);
            if (this.Pagination == null)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("Pagination", "Pagination is NULL."));
            }
        }
        protected override Uri CreateUrl()
        {
            string url = this.GetDirectUrl();
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       "url='" + url + "' AND (xpath='html/body/table[2]/tr/td/table[1]/tr' OR xpath='html/body/table[1]/tr/td[1]/font')",
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        public override Query<ScreenerResult> Clone()
        {
            return new StockScreenerQuery()
            {
                UseDirectSource = this.UseDirectSource,
                GetDiagnostics = this.GetDiagnostics,
                Criterias = (ScreenerCriteria[])this.Criterias.Clone(),
                View = this.View,
                Pagination = this.Pagination.Clone(),
                RankedBy = this.RankedBy,
                RankDirection = this.RankDirection
            };
        }


        private string GetDirectUrl()
        {
            var url = new System.Text.StringBuilder();
            url.Append("http://screener.finance.yahoo.com/b?");
            url.AppendFormat("vw={0}", ((int)this.View).ToString());
            foreach (var c in this.Criterias) { url.Append(c.TagParameterInternal()); }
            url.AppendFormat("&{0}={1}", this.RankDirection == ListSortDirection.Ascending ? "s" : "z", YFHelper.GetScreenerPropertyTag(this.RankedBy));
            url.Append("&db=stocks");
            url.AppendFormat("&b={0}", this.Pagination.Start + 1);
            return url.ToString();
        }

    }




    public class StockScreenerPagination : StartIndexPagination
    {
        public StockScreenerPagination() { this.Count = 20; }
        public new StockScreenerPagination Clone()
        {
            return new StockScreenerPagination() { Start = this.Start, Count = this.Count };
        }
    }




    public class StockIndustryCriteria : ScreenerCriteria
    {
        public override string DisplayName { get { return "Industry"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.Industry; } }
        public int IndustryID { get; set; }

        public StockIndustryCriteria() { }

        protected override bool IsValid()
        {
            return this.IndustryID >= 100 && this.IndustryID < 1000;
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}", YFHelper.GetScreenerPropertyTag(this.Property), this.IndustryID);
        }
    }

    public class StockPriceCriteria : ScreenerMinMaxCriteria<double>
    {
        public override string DisplayName { get { return "Price"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.RetailPrice; } }

        public StockPriceCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
    }

    public class StockMarketCapCriteria : ScreenerMinMaxCriteria<int>
    {
        public override string DisplayName { get { return "Market Cap"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.MarketCapitalization; } }

        public StockMarketCapCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
    }

    public class StockDividendYieldRatioCriteria : ScreenerMinMaxCriteria<double>
    {
        public override string DisplayName { get { return "Price"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.DividendYieldRatio; } }

        public StockDividendYieldRatioCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
    }

    public class StockBetaCriteria : ScreenerMinMaxCriteria<double>
    {
        public override string DisplayName { get { return "Beta"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.Beta; } }

        public StockBetaCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
    }

    public class StockSalesRevenueCriteria : ScreenerMinMaxCriteria<int>
    {
        public override string DisplayName { get { return "Sales Revenue"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.Sales; } }

        public StockSalesRevenueCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
    }

    public class StockProfitMarginCriteria : ScreenerMinMaxCriteria<int>
    {
        public override string DisplayName { get { return "Profit Margin"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.ProfitMargin; } }

        public StockProfitMarginCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? (int?)(this.Minimum.Value + 150) : null, this.Maximum.HasValue ? (int?)(this.Maximum.Value + 150) : null);
        }
    }

    public class StockPriceEarningsRatioCriteria : ScreenerMinMaxCriteria<double>
    {
        public override string DisplayName { get { return "Price/Earnings"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.PriceEarningsRatio; } }

        public StockPriceEarningsRatioCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
    }

    public class StockPriceBookRatioCriteria : ScreenerMinMaxCriteria<double>
    {
        public override string DisplayName { get { return "Price/Book"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.PriceBookRatio; } }

        public StockPriceBookRatioCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
    }

    public class StockPriceSalesRatioCriteria : ScreenerMinMaxCriteria<double>
    {
        public override string DisplayName { get { return "Price/Sales"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.PriceSalesRatio; } }

        public StockPriceSalesRatioCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
    }

    public class StockPEGRatioCriteria : ScreenerMinMaxCriteria<double>
    {
        public override string DisplayName { get { return "Price/Earnings To Growth"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.PEG; } }

        public StockPEGRatioCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
    }

    public class StockFiveYearGrowthCriteria : ScreenerMinMaxCriteria<double>
    {
        public override string DisplayName { get { return "Est. 5 EPS Year Growth"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.FiveYearGrowth; } }

        public StockFiveYearGrowthCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }

        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? (int?)(this.Minimum.Value + 150) : null, this.Maximum.HasValue ? (int?)(this.Maximum.Value + 150) : null);
        }
    }

    public class StockAnalystRecommendCriteria : ScreenerMinMaxCriteria<double>
    {
        public override string DisplayName { get { return "Analyst Recommend"; } }

        public override ScreenerProperty Property { get { return ScreenerProperty.AnalystRecommend; } }

        public StockAnalystRecommendCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }

        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum, this.Maximum);
        }
    }
}

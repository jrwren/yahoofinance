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



    public class FundScreenerQuery : ScreenerQuery
    {

        public FundScreenerView View { get; set; }
        public FundScreenerPagination Pagination { get; set; }

        public ScreenerProperty RankedBy { get; set; }
        public System.ComponentModel.ListSortDirection RankDirection { get; set; }

        public FundScreenerQuery()
        {
            this.View = FundScreenerView.Screened;
            this.RankedBy = ScreenerProperty.Name;
            this.RankDirection = System.ComponentModel.ListSortDirection.Ascending;
            this.Pagination = new FundScreenerPagination();
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
            return new FundScreenerQuery()
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
            url.Append("http://screener.finance.yahoo.com/a?");
            url.AppendFormat("vw={0}", ((int)this.View).ToString());
            foreach (var c in this.Criterias) { url.Append(c.TagParameterInternal()); }
            url.AppendFormat("&{0}={1}", this.RankDirection == ListSortDirection.Ascending ? "s" : "z", YFHelper.GetScreenerPropertyTag(this.RankedBy));
            url.Append("&db=funds");
            url.AppendFormat("&b={0}", this.Pagination.Start + 1);
            return url.ToString();
        }

    }




    public class FundScreenerPagination : StartIndexPagination
    {
        public FundScreenerPagination() { this.Count = 20; }
        public new FundScreenerPagination Clone()
        {
            return new FundScreenerPagination() { Start = this.Start, Count = this.Count };
        }
    }




    public class FundCategory
    {

        public string ID { get; private set; }
        public string Name { get; private set; }

        public FundCategory(string curID, string curDesc)
        {
            this.ID = curID.ToUpper();
            this.Name = curDesc;
        }
        internal FundCategory(MaasOne.Resources.WorldMarketCategory orig)
        {
            this.ID = orig.ID;
            this.Name = orig.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is FundCategory)
            {
                return this.ID.Equals(((FundCategory)obj).ID) && this.Name.Equals(((FundCategory)obj).Name);
            }
            return false;
        }

        public override string ToString()
        {
            if (this.Name != string.Empty)
            {
                return this.Name;
            }
            else
            {
                return this.ID;
            }
        }

    }


    public class FundCategoryCriteria : ScreenerCriteria
    {


        public override string DisplayName { get { return "Category"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.Category; } }

        public FundCategory Category { get; set; }

        public FundCategoryCriteria() { }

        protected override bool IsValid()
        {
            return this.Category != null;
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}", YFHelper.GetScreenerPropertyTag(this.Property), this.Category.ID);
        }

    }

    public class FundNameCriteria : ScreenerCriteria
    {


        public override string DisplayName { get { return "Fund Name"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.Name; } }

        public string Name { get; set; }

        public FundNameCriteria() { }

        protected override bool IsValid()
        {
            return this.Name.IsNullOrWhiteSpace() == false;
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}", YFHelper.GetScreenerPropertyTag(this.Property), this.Name);
        }

    }

    public class FundRankInCategoryCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "Rank in Category"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.RankPercent; } }

        public FundRankInCategoryCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundManagerTenureCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "Manager Tenure (Yrs.)"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.ManagerTenureYears; } }

        public FundManagerTenureCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundNetAssetsCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "Net Assets"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.NetAssets; } }

        public FundNetAssetsCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundTurnoverCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "Turnover (%)"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.TurnoverPercent; } }

        public FundTurnoverCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundHldgMedianMarketCapCriteria : ScreenerMinMaxCriteria<double>
    {

        public override string DisplayName { get { return "Holding Median Market Cap"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.HoldingMedianMarketCap; } }

        public FundHldgMedianMarketCapCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundMinInvestCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "Min. Initial Investment"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.MinInvest; } }

        public FundMinInvestCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundFrontLoadCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "Front Load (%)"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.FrontLoadPercent; } }

        public FundFrontLoadCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundExpenseRatioCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "Total Expense (%)"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.ExpenseRatioPercent; } }

        public FundExpenseRatioCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || (this.Maximum.Value >= 0 && this.Maximum.Value <= 99)) &&
                (this.Minimum.HasValue == false || (this.Minimum.Value >= 0 && this.Maximum.Value <= 99)) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}=.{1:00}/.{2:00}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? (this.Minimum.Value / 100) : 1, this.Maximum.HasValue ? (this.Maximum.Value) : 99);
        }

    }

    public class FundReturnYTDCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "Return Year to Date (%)"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.ReturnYearToDatePercent; } }

        public FundReturnYTDCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundOneYearReturnCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "1 Year Return (%)"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.OneYearReturn; } }

        public FundOneYearReturnCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundAnnualized3YrCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "Annualized 3 Year Return (%)"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.Annualized3YearReturnPercent; } }

        public FundAnnualized3YrCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundAnnualized5YrCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "Annualized 5 Year Return (%)"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.Annualized5YearreturnPercent; } }

        public FundAnnualized5YrCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundMorningstarCriteria : ScreenerCriteria
    {

        public override string DisplayName { get { return "Morningstar Rating"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.MorningstarRating; } }
        public FundMorningstarRating? MinimumRating { get; set; }
        public FundMorningstarRating? MaximumRating { get; set; }

        public FundMorningstarCriteria() { }

        protected override bool IsValid()
        {
            return this.MinimumRating.HasValue || this.MaximumRating.HasValue;
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.GetParam(this.MinimumRating), this.GetParam(this.MaximumRating));
        }
        private string GetParam(FundMorningstarRating? val) { return val.HasValue ? ((int)val.Value).ToString() : string.Empty; }

    }


}

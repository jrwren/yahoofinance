using System;
using System.Collections.Generic;

namespace MaasOne.Finance.Yahoo.Screener
{

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
            return string.Format("&{0}={1}", YFHelper.GetPropertyTag(this.Property), this.Category.ID);
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
            return string.Format("&{0}={1}", YFHelper.GetPropertyTag(this.Property), this.Name);
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
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
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
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
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
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
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
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
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
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
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
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
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
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
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
            return string.Format("&{0}=.{1:00}/.{2:00}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? (this.Minimum.Value / 100) : 1, this.Maximum.HasValue ? (this.Maximum.Value) : 99);
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
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
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
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
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
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
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
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.Minimum.HasValue ? this.Minimum.Value : 0, this.Maximum.HasValue ? this.Maximum.Value : 100);
        }

    }

    public class FundMorningstarCriteria : ScreenerCriteria
    {

        public override string DisplayName { get { return "Morningstar Rating"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.MorningstarRating; } }
        public MorningstarRating? MinimumRating { get; set; }
        public MorningstarRating? MaximumRating { get; set; }

        public FundMorningstarCriteria() { }

        protected override bool IsValid()
        {
            return this.MinimumRating.HasValue || this.MaximumRating.HasValue;
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetPropertyTag(this.Property), this.GetParam(this.MinimumRating), this.GetParam(this.MaximumRating));
        }
        private string GetParam(MorningstarRating? val) { return val.HasValue ? ((int)val.Value).ToString() : string.Empty; }

    }


}

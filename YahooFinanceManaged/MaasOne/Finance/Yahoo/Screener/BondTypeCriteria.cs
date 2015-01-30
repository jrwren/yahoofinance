using System;
using System.Collections.Generic;

namespace MaasOne.Finance.Yahoo.Screener
{
    
    public class BondTypeCriteria : ScreenerCriteria
    {

        public override string DisplayName { get { return "Type"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.Type; } }

        public bool Treasury { get; set; }
        public bool TreasuryZeroCoupon { get; set; }
        public bool Corporate { get; set; }

        public bool Municipal { get; set; }
        public UsState? State { get; set; }

        public BondTypeCriteria() { }

        protected override bool IsValid()
        {
            return !(this.Municipal == false &&
                this.Treasury == false &&
                this.TreasuryZeroCoupon == false &&
                this.Corporate == false);
        }
        protected override string TagParameter()
        {
            var sb = new System.Text.StringBuilder();
            if (this.Treasury) sb.AppendFormat("&tt=1");
            if (this.TreasuryZeroCoupon) sb.AppendFormat("&tz=1");
            if (this.Corporate) sb.AppendFormat("&tc=1");
            if (this.Municipal) sb.AppendFormat("&tm=1");
            sb.AppendFormat("&stt={0}", this.State.HasValue ? this.State.ToString() : "-");
            return sb.ToString();
        }

    }
    
    public class BondPriceCriteria : ScreenerCriteria
    {

        public override string DisplayName { get { return "Price"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.Price; } }

        public BondPriceType Price { get; set; }

        public BondPriceCriteria() { }

        protected override bool IsValid()
        {
            return true;
        }
        protected override string TagParameter()
        {
            return string.Format("&pr={0}", ((int)this.Price).ToString());
        }

    }
    
    public class BondCouponCriteria : ScreenerMinMaxCriteria<double>
    {

        public override string DisplayName { get { return "Coupon (%)"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.CouponPercent; } }

        public BondCouponCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&cpl={0}&cpu={1}", this.Minimum.HasValue ? this.Minimum.Value : -1, this.Maximum.HasValue ? this.Maximum.Value : -1);
        }

    }
    
    public class BondCurrentYieldCriteria : ScreenerMinMaxCriteria<double>
    {

        public override string DisplayName { get { return "Current Yield (%)"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.CurrentYieldPercent; } }

        public BondCurrentYieldCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&yl={0}&yu={1}", this.Minimum.HasValue ? this.Minimum.Value : -1, this.Maximum.HasValue ? this.Maximum.Value : -1);
        }

    }

    public class BondYTMCriteria : ScreenerMinMaxCriteria<double>
    {

        public override string DisplayName { get { return "YTM (%)"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.YTMPercent; } }

        public BondYTMCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&ytl={0}&ytu={1}", this.Minimum.HasValue ? this.Minimum.Value : -1, this.Maximum.HasValue ? this.Maximum.Value : -1);
        }

    }

    public class BondMaturityCriteria : ScreenerMinMaxCriteria<int>
    {

        public override string DisplayName { get { return "Maturity"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.Maturity; } }

        public BondMaturityCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Maximum.HasValue == false || this.Maximum.Value >= 0) &&
                (this.Minimum.HasValue == false || this.Minimum.Value >= 0) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || this.Minimum.Value < this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&mtl={0}&mtu={1}", this.Minimum.HasValue ? this.Minimum.Value : -1, this.Maximum.HasValue ? this.Maximum.Value : -1);
        }

    }
        
    public class BondFitchRatingCriteria : ScreenerMinMaxCriteria<FitchRating>
    {

        public override string DisplayName { get { return "Fitch Rating"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.FitchRating; } }

        public BondFitchRatingCriteria() { }

        protected override bool IsValid()
        {
            return (this.Minimum.HasValue || this.Maximum.HasValue) &&
                (this.Minimum.HasValue != this.Maximum.HasValue || (int)this.Minimum.Value < (int)this.Maximum.Value);
        }
        protected override string TagParameter()
        {
            return string.Format("&mtl={0}&mtu={1}", this.Minimum.HasValue ? (int)this.Minimum.Value : -1, this.Maximum.HasValue ? (int)this.Maximum.Value : -1);
        }

    }

    public class BondCallableCriteria : ScreenerCriteria
    {

        public override string DisplayName { get { return "Callable"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.Callable; } }

        public bool Callable { get; set; }

        public BondCallableCriteria() { }

        protected override bool IsValid()
        {
            return true;
        }
        protected override string TagParameter()
        {
            return string.Format("&cll={0}", this.Callable ? 1 : 0);
        }

    }

}

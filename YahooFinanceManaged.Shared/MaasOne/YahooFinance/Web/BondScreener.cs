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
using System.ComponentModel;

namespace MaasOne.YahooFinance.Web
{
    public class BondScreenerQuery : ScreenerQuery
    {
        public BondScreenerPagination Pagination { get; set; }


        public BondScreenerQuery()
        {
            this.SortProperty = Data.ScreenerProperty.Maturity;
            this.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
            this.Pagination = new BondScreenerPagination();
        }


        public override QueryBase Clone()
        {
            return new BondScreenerQuery()
            {
                Criterias = (ScreenerCriteria[])this.Criterias.Clone(),
                Pagination = this.Pagination.Clone(),
                SortProperty = this.SortProperty,
                SortDirection = this.SortDirection
            };
        }


        protected override string CreateUrl()
        {
            var url = new System.Text.StringBuilder();
            url.Append("http://screener.finance.yahoo.com/z1?");
            url.AppendFormat("b={0}", this.Pagination.Start + 1);
            url.AppendFormat("&so={0}", this.SortDirection == ListSortDirection.Ascending ? "a" : "d");
            url.AppendFormat("&sf={0}", YFHelper.GetScreenerPropertyTag(this.SortProperty));

            bool found = false;

            foreach (var c in this.Criterias) { if (c is BondTypeCriteria) { found = true; url.Append(c.TagParameterInternal()); break; } }
            if (found == false) url.Append("&stt=-");
            found = false;
            foreach (var c in this.Criterias) { if (c is BondPriceCriteria) { found = true; url.Append(c.TagParameterInternal()); break; } }
            if (found == false) url.Append("&pr=0");
            found = false;
            foreach (var c in this.Criterias) { if (c is BondCouponCriteria) { found = true; url.Append(c.TagParameterInternal()); break; } }
            if (found == false) url.Append("&cpl=-1&cpu=-1");
            found = false;
            foreach (var c in this.Criterias) { if (c is BondCurrentYieldCriteria) { found = true; url.Append(c.TagParameterInternal()); break; } }
            if (found == false) url.Append("&yl=-1&yu=-1");
            found = false;
            foreach (var c in this.Criterias) { if (c is BondYTMCriteria) { found = true; url.Append(c.TagParameterInternal()); break; } }
            if (found == false) url.Append("&ytl=-1&ytu=-1");
            found = false;
            foreach (var c in this.Criterias) { if (c is BondMaturityCriteria) { found = true; url.Append(c.TagParameterInternal()); break; } }
            if (found == false) url.Append("&mtl=-1&mtu=-1");
            found = false;
            foreach (var c in this.Criterias) { if (c is BondFitchRatingCriteria) { found = true; url.Append(c.TagParameterInternal()); break; } }
            if (found == false) url.Append("&rl=-1&ru=-1");
            found = false;
            foreach (var c in this.Criterias) { if (c is BondCallableCriteria) { found = true; url.Append(c.TagParameterInternal()); break; } }
            if (found == false) url.Append("&cll=-1");

            return url.ToString();
        }

        protected override void Validate(ValidationResult result)
        {
            base.Validate(result);
            if (this.Pagination == null)
            {
                result.Success = false;
                result.Info.Add("Pagination", "Pagination is NULL.");
            }
        }
    }

    
    public class BondScreenerPagination : StartIndexPagination
    {
        public BondScreenerPagination() { this.Count = 15; }
        public new BondScreenerPagination Clone()
        {
            return new BondScreenerPagination() { Start = this.Start, Count = this.Count };
        }
    }



    public class BondTypeCriteria : ScreenerCriteria
    {

        public override string DisplayName { get { return "Type"; } }
        public override Data.ScreenerProperty Property { get { return Data.ScreenerProperty.Type; } }

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
        public override Data.ScreenerProperty Property { get { return Data.ScreenerProperty.Price; } }

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
        public override Data.ScreenerProperty Property { get { return Data.ScreenerProperty.CouponPercent; } }

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
        public override Data.ScreenerProperty Property { get { return Data.ScreenerProperty.CurrentYieldPercent; } }

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
        public override Data.ScreenerProperty Property { get { return Data.ScreenerProperty.YTMPercent; } }

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
        public override Data.ScreenerProperty Property { get { return Data.ScreenerProperty.Maturity; } }

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

    public class BondFitchRatingCriteria : ScreenerMinMaxCriteria<Data.BondFitchRating>
    {

        public override string DisplayName { get { return "Fitch Rating"; } }
        public override Data.ScreenerProperty Property { get { return Data.ScreenerProperty.FitchRating; } }

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
        public override Data.ScreenerProperty Property { get { return Data.ScreenerProperty.Callable; } }

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

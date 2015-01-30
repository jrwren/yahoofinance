using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.Finance.Yahoo.Screener;
using System.ComponentModel;

namespace MaasOne.Finance.Yahoo.Web
{

    public class BondScreenerQuery : ScreenerQuery
    {

        public BondScreenerPagination Pagination { get; set; }


        public BondScreenerQuery()
        {
            this.RankedBy = ScreenerProperty.Maturity;
            this.RankDirection = System.ComponentModel.ListSortDirection.Ascending;
            this.Pagination = new BondScreenerPagination();
        }

        protected override void ValidateQuery(ValidationResult result)
        {
            base.ValidateQuery(result);
            if (this.Pagination == null)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("Pagination", "Pagination is null."));
            }
        }
        protected override Uri CreateUrl()
        {
            string url = this.GetDirectUrl();
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       "url='" + url + "' AND (xpath='//table[@class=\"yfnc_tableout1\"]/tr/td/table/tr' OR xpath='//td[@align=\"right\"]/strong')",
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        public override Query<ScreenerResult> Clone()
        {
            return new BondScreenerQuery()
            {
                UseDirectSource = this.UseDirectSource,
                GetDiagnostics = this.GetDiagnostics,
                Criterias = (ScreenerCriteria[])this.Criterias.Clone(),
                Pagination = this.Pagination.Clone(),
                RankedBy = this.RankedBy,
                RankDirection = this.RankDirection
            };
        }



        private string GetDirectUrl()
        {
            var url = new System.Text.StringBuilder();
            url.Append("http://screener.finance.yahoo.com/z1?");
            url.AppendFormat("b={0}", this.Pagination.Start + 1);
            url.AppendFormat("&so={0}", this.RankDirection == ListSortDirection.Ascending ? "a" : "d");
            url.AppendFormat("&sf={0}", YFHelper.GetPropertyTag(this.RankedBy));
            
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



    }



    public class BondScreenerPagination : StartIndexPagination
    {
        public BondScreenerPagination() { this.Count = 15; }
        public new BondScreenerPagination Clone()
        {
            return new BondScreenerPagination() { Start = this.Start, Count = this.Count };
        }
    }



}

using System;
using System.Net;
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.Finance.Yahoo.Screener;
using MaasOne.Finance.Yahoo.Data;

namespace MaasOne.Finance.Yahoo.Web
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
                result.Info.Add(new KeyValuePair<string, string>("Pagination", "Pagination is null."));
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
            url.AppendFormat("&{0}={1}", this.RankDirection == ListSortDirection.Ascending ? "s" : "z", YFHelper.GetPropertyTag(this.RankedBy));
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



    public enum StockScreenerView
    {
        All = 0,
        Screened = 1,
        ShareData = 2,
        Performance = 3,
        SalesAndProfit = 4,
        Valuation = 5,
        AnalystEstimates = 6
    }



}

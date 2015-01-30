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
using System.Net;
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.Finance.Yahoo.Screener;
using MaasOne.Finance.Yahoo.Data;

namespace MaasOne.Finance.Yahoo.Web
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
            url.AppendFormat("&{0}={1}", this.RankDirection == ListSortDirection.Ascending ? "s" : "z", YFHelper.GetPropertyTag(this.RankedBy));
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



    public enum FundScreenerView
    {
        All = 0,
        Screened = 1,
        Overviews = 2,
        Holdings = 3,
        Ratings = 4,
        PurchasingAndFees = 5,
        Performance = 6
    }



}

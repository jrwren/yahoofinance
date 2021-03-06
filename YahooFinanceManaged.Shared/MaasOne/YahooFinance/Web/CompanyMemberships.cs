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
using MaasOne.YahooFinance.Data;

namespace MaasOne.YahooFinance.Web
{
    public class CompanyMembershipsQuery : YqlQuery<CompanyMembershipsResult>
    {
        public CompanyMembershipsPagination ETFsPagination { get; set; }

        public CompanyMembershipsPagination FundsPagination { get; set; }

        public string ID { get; private set; }

        public CompanyMembershipsPagination IndicesPagination { get; set; }


        public CompanyMembershipsQuery() { }

        public CompanyMembershipsQuery(string id)
        {
            this.ID = id;
            this.IndicesPagination = new CompanyMembershipsPagination();
            this.ETFsPagination = new CompanyMembershipsPagination();
            this.FundsPagination = new CompanyMembershipsPagination();
        }


        public override QueryBase Clone()
        {
            return new CompanyMembershipsQuery(this.ID)
            {
                IndicesPagination = this.IndicesPagination.Clone(),
                ETFsPagination = this.ETFsPagination.Clone(),
                FundsPagination = this.FundsPagination.Clone()
            };
        }


        protected override string CreateUrl()
        {
            return string.Format("http://finance.yahoo.com/q/ct?s={0}&e={1}&f={2}&i={3}",
                Uri.EscapeDataString(this.ID),
                this.ETFsPagination != null ? Math.Abs(this.ETFsPagination.Page) + 1 : 1,
                this.FundsPagination != null ? Math.Abs(this.FundsPagination.Page) + 1 : 1,
                this.IndicesPagination != null ? Math.Abs(this.IndicesPagination.Page) + 1 : 1
                );
        }

        protected override void Validate(ValidationResult result)
        {
            if (this.ID.IsNullOrWhiteSpace() == true)
            {
                result.Success = false;
                result.Info.Add("ID", "ID is NULL or empty.");
            }
        }


        internal override CompanyMembershipsResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            CompanyMembershipsResult result = new CompanyMembershipsResult();

            JObject rightcolObj = (JObject)yqlToken;

            JObject sumObject = (JObject)rightcolObj.FindFirst("table", "id", "yfncsumtab");
            JObject shortInfoObject = (JObject)rightcolObj.FindFirst("div", "id", "yfi_rt_quote_summary");
            
            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);
            
            JObject tr = (JObject)sumObject.FindFirst("tr", "valign", "top");

            JObject td = (JObject)tr["td"][0];

            if (td["table"] is JArray)
            {
                var tables = (JArray)td["table"];
                if (tables.Count >= 2)
                {
                    JObject indexTable = (JObject)tables[1];
                    result.Indices = this.GetItems(indexTable);
                }
                if (tables.Count >= 4)
                {
                    JObject etfTable = (JObject)tables[3];
                    result.ETFs = this.GetItems(etfTable);
                }
                if (tables.Count >= 6)
                {
                    JObject fundTable = (JObject)tables[5];
                    result.Funds = this.GetItems(fundTable);
                }
            }

            if (td["div"] != null)
            {
                JArray pagingObjects = td["div"] is JArray ? (JArray)td["div"] : new JArray(td["div"]);
                foreach (JToken pagingObject in pagingObjects)
                {
                    string pageStr = pagingObject["small"]["content"].HtmlFirstContent();
                    if (pageStr.Contains("Indices")) { result.IndicesPagination = YFHelper.HtmlConvertPagination(pageStr); }
                    else if (pageStr.Contains("ETFs")) { result.ETFsPagination = YFHelper.HtmlConvertPagination(pageStr); }
                    else if (pageStr.Contains("Mutual Funds")) { result.FundsPagination = YFHelper.HtmlConvertPagination(pageStr); }
                }
            }
            if (result.IndicesPagination == null) result.IndicesPagination = new LimitedPagination(0, result.Indices.Length, result.Indices.Length - 1);
            if (result.ETFsPagination == null) result.ETFsPagination = new LimitedPagination(0, result.ETFs.Length, result.ETFs.Length - 1);
            if (result.FundsPagination == null) result.FundsPagination = new LimitedPagination(0, result.Funds.Length, result.Funds.Length - 1);

            return result;
        }

        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc) { return htmlDoc.FindFirst("div", "id", "rightcol"); }

        internal override string YqlXPath() { return "//div[@id=\"rightcol\"]"; }


        private MembershipItem[] GetItems(JObject table)
        {
            var items = new List<MembershipItem>();
            if (table != null)
            {
                table = table.HtmlInnerTable();
                if (table["tr"] is JArray)
                {
                    var trs = (JArray)table["tr"];
                    for (int i = 1; i < trs.Count; i++)
                    {
                        var tds = (JArray)trs[i]["td"];
                        var item = new MembershipItem();
                        for (int n = 0; n < tds.Count; n++)
                        {
                            try
                            {
                                switch (n)
                                {
                                    case 0: item.ID = tds[n].HtmlFirstContent(); break;
                                    case 1: item.Name = tds[n].HtmlFirstContent(); break;
                                    case 2: item.LastTradePriceOnly = tds[n].HtmlFirstContent().ParseDouble(); break;
                                    case 3:
                                        item.ChangeInPercent = tds[n].HtmlFirstContent().Replace("%", "").ParseDouble();
                                        item.ChangeInPercent *= (tds[n]["span"]["img"]["class"].HtmlFirstContent() == "neg_arrow") ? -1 : 1;
                                        break;
                                }
                            }
                            catch { }
                        }
                        items.Add(item);
                    }
                }
            }
            return items.ToArray();
        }
    }


    public class CompanyMembershipsPagination : Pagination
    {
        public override int Start
        {
            get { return this.Page * this.Count; }
            protected set { throw new NotSupportedException(); }
        }

        public int Page { get; set; }


        public CompanyMembershipsPagination() { this.Count = 10; }


        public new CompanyMembershipsPagination Clone() { return new CompanyMembershipsPagination() { Page = this.Page }; }
    }


    public class CompanyMembershipsResult : ResultBase
    {
        public MembershipItem[] ETFs { get; internal set; }

        public LimitedPagination ETFsPagination { get; internal set; }

        public MembershipItem[] Funds { get; internal set; }

        public LimitedPagination FundsPagination { get; internal set; }

        public MembershipItem[] Indices { get; internal set; }

        public LimitedPagination IndicesPagination { get; internal set; }

        public QuotesBase ShortInfo { get; internal set; }


        internal CompanyMembershipsResult() { }
    }
}

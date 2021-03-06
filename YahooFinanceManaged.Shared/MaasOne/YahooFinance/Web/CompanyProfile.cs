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
using Newtonsoft.Json.Linq.JsonPath;
using MaasOne.YahooFinance.Data;

namespace MaasOne.YahooFinance.Web
{
    public class CompanyProfileQuery : YqlQuery<CompanyProfileResult>
    {
        public string ID { get; set; }


        public CompanyProfileQuery() { }

        public CompanyProfileQuery(string id) { this.ID = id; }


        public override QueryBase Clone() { return new CompanyProfileQuery(this.ID); }


        protected override void Validate(ValidationResult result)
        {
            if (this.ID.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Info.Add("ID", "ID is NULL or empty.");
            }
        }

        protected override string CreateUrl() { return string.Format("http://finance.yahoo.com/q/pr?s={0}", Uri.EscapeDataString(this.ID)); }


        internal override CompanyProfileResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            CompanyProfileResult result = new CompanyProfileResult();

            JObject rightcolObj = (JObject)yqlToken;

            JObject shortInfoObject = (JObject)rightcolObj.FindFirst("div", "id", "yfi_rt_quote_summary");
            JObject sumObject = (JObject)rightcolObj.FindFirst("table", "id", "yfncsumtab");


            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);


            result.Profile = new CompanyProfile();

            JObject tr = (JObject)sumObject.FindFirst("tr", "valign", "top");

            JToken tdl = tr["td"][0];

            result.Profile.ID = result.ShortInfo.ID;
            result.Profile.CompanyName = tdl[(ci.IsDirectSource ? "b" : "strong")].HtmlFirstContent();
            result.Profile.Address = tdl["p"][0].HtmlFirstContent();
            result.Profile.BusinessSummary = tdl["p"][1].HtmlFirstContent();


            JToken details = tdl["table"][1]["tr"]["td"]["table"];
            result.Profile.Details = new CompanyProfileDetails();

            string secLnk = details["tr"][1]["td"][1].HtmlFirstLink();
            if (secLnk != null)
            {
                int? secID = secLnk.Substring(0, secLnk.LastIndexOf("conameu")).Substring(secLnk.LastIndexOf('/') + 1).ParseInt();
                if (secID.HasValue)
                {
                    result.Profile.Details.Sector = new Sector(secID.Value, details["tr"][1]["td"][1]["a"].HtmlFirstContent());

                }
            }

            string indLnk = details["tr"][2]["td"][1].HtmlFirstLink();
            if (indLnk != null)
            {
                int? indID = indLnk.Substring(0, indLnk.LastIndexOf(".")).Substring(indLnk.LastIndexOf("/") + 1).ParseInt();
                if (indID.HasValue)
                {
                    result.Profile.Details.Industry = new Industry(indID.Value, details["tr"][2]["td"][1]["a"].HtmlFirstContent());
                }
            }

            result.Profile.Details.FullTimeEmployees = details["tr"][3]["td"][1].HtmlFirstContent().ParseInt();


            JToken tdr = tr["td"][2];

            result.Profile.CorporateGovernance = tdr["table"][1]["tr"][0]["td"].HtmlFirstContent();
            JArray ke = (JArray)tdr["table"][3]["tr"]["td"]["table"]["tr"];
            List<CompanyProfileExecutivePerson> lst = new List<CompanyProfileExecutivePerson>();
            if (ke.Count > 1)
            {
                for (int i = 1; i < ke.Count; i++)
                {
                    JObject ktr = (JObject)ke[i];
                    CompanyProfileExecutivePerson ep = new CompanyProfileExecutivePerson();
                    ep.Name = ktr["td"][0].HtmlFirstContent();
                    ep.Age = ((JObject)ktr["td"][0]).FindFirst("content").HtmlFirstContent().ParseInt();
                    ep.Position = (ci.IsDirectSource ? ktr["td"][0]["small"] : ktr["td"][0]["p"]["small"]).HtmlFirstContent();
                    object pay = ktr["td"][1].HtmlFirstContent().ToObject();
                    if (pay != null) ep.Pay = Convert.ToInt64(pay);
                    object exercised = ktr["td"][2].HtmlFirstContent().ToObject();
                    if (exercised != null) ep.Exercised = Convert.ToInt64(exercised);
                    lst.Add(ep);
                }
                result.Profile.KeyExecutives = lst.ToArray();
            }

            return result;
        }

        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc) { return htmlDoc.FindFirst("div", "id", "rightcol"); }

        internal override string YqlXPath() { return "//div[@id=\"rightcol\"]"; }
    }


    public class CompanyProfileResult : ResultBase
    {
        public CompanyProfile Profile { get; internal set; }

        public QuotesBase ShortInfo { get; internal set; }


        internal CompanyProfileResult() { }
    }
}

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
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq.JsonPath;
using MaasOne.YahooFinance.Data;
/*
#if  (NET20)
using Newtonsoft.Json.Utilities.LinqBridge;
#else
using System.Linq;
#endif
*/

namespace MaasOne.YahooFinance.Web
{

    public class CompanyProfileQuery : Query<CompanyProfileResult>, IYqlQuery
    {

        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public string ID { get; set; }

        public CompanyProfileQuery() : this(string.Empty) { }
        public CompanyProfileQuery(string id) { this.ID = id; }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (string.IsNullOrEmpty(this.ID))
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("ID", "No ID available."));
            }
        }
        protected override Uri CreateUrl()
        {
            string url = string.Format("http://finance.yahoo.com/q/pr?s={0}", this.ID);
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       "url='" + url + "' and (xpath='//table[@id=\"yfncsumtab\"]' or xpath='//div[@id=\"yfi_rt_quote_summary\"]')",
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        protected override CompanyProfileResult ConvertResult(System.IO.Stream stream)
        {
            CompanyProfileResult result = new CompanyProfileResult();

            JObject shortInfoObject = null;
            JObject profileObject = null;

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                profileObject = (JObject)htmlDoc.FindFirst("table", "id", "yfncsumtab");
                shortInfoObject = (JObject)htmlDoc.FindFirst("div", "id", "yfi_rt_quote_summary");
            }
            else
            {
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                result.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    profileObject = (JObject)yqlDoc.Query.Results["table"];
                    shortInfoObject = (JObject)yqlDoc.Query.Results["div"];
                }
            }


            if (profileObject == null) throw new ParseException("The [profile] object could not be load.");

            result.Data = new CompanyProfileData();

            JObject tr = (JObject)profileObject.FindFirst("tr", "valign", "top");

            var tdl = tr["td"][0];

            result.Data.ID = this.ID.ToUpper().Trim();
            result.Data.CompanyName = tdl[(this.UseDirectSource ? "b" : "strong")].HtmlFirstContent();
            result.Data.Address = tdl["p"][0].HtmlFirstContent();
            result.Data.BusinessSummary = tdl["p"][1].HtmlFirstContent();


            var details = tdl["table"][1]["tr"]["td"]["table"];
            result.Data.Details = new CompanyProfileDetails();

            var secLnk = details["tr"][1]["td"][1].HtmlFirstLink();
            if (secLnk != null)
            {
                var secID = secLnk.Substring(0, secLnk.LastIndexOf("conameu")).Substring(secLnk.LastIndexOf('/') + 1).ParseInt();
                if (secID.HasValue)
                {
                    result.Data.Details.Sector = new Sector(secID.Value, details["tr"][1]["td"][1]["a"].HtmlFirstContent());

                }
            }

            var indLnk = details["tr"][2]["td"][1].HtmlFirstLink();
            if (indLnk != null)
            {
                var indID = indLnk.Substring(0, indLnk.LastIndexOf(".")).Substring(indLnk.LastIndexOf("/") + 1).ParseInt();
                if (indID.HasValue)
                {
                    result.Data.Details.Industry = new Industry(indID.Value, details["tr"][2]["td"][1]["a"].HtmlFirstContent());
                }
            }

            result.Data.Details.FullTimeEmployees = details["tr"][3]["td"][1].HtmlFirstContent().ParseInt();


            var tdr = tr["td"][2];

            result.Data.CorporateGovernance = tdr["table"][1]["tr"][0]["td"].HtmlFirstContent();
            var ke = (JArray)tdr["table"][3]["tr"]["td"]["table"]["tr"];
            List<CompanyProfileExecutivePerson> lst = new List<CompanyProfileExecutivePerson>();
            if (ke.Count > 1)
            {
                for (var i = 1; i < ke.Count; i++)
                {
                    var ktr = (JObject)ke[i];
                    var ep = new CompanyProfileExecutivePerson();
                    ep.Name = ktr["td"][0].HtmlFirstContent();
                    ep.Age = ((JObject)ktr["td"][0]).FindFirst("content").HtmlFirstContent().ParseInt();
                    ep.Position = (UseDirectSource ? ktr["td"][0]["small"] : ktr["td"][0]["p"]["small"]).HtmlFirstContent();
                    var pay = ktr["td"][1].HtmlFirstContent().ToObject();
                    if (pay != null) ep.Pay = Convert.ToInt64(pay);
                    var exercised = ktr["td"][2].HtmlFirstContent().ToObject();
                    if (exercised != null) ep.Exercised = Convert.ToInt64(exercised);
                    lst.Add(ep);
                }
                result.Data.KeyExecutives = lst.ToArray();
            }


            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);

            return result;
        }
        public override Query<CompanyProfileResult> Clone()
        {
            return new CompanyProfileQuery(this.ID) { GetDiagnostics = this.GetDiagnostics, UseDirectSource = this.UseDirectSource };
        }

    }

    /// <summary>
    /// Stores the result data
    /// </summary>
    public class CompanyProfileResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public CompanyProfileData Data { get; internal set; }
        public QuotesBaseData ShortInfo { get; internal set; }
        internal CompanyProfileResult() { }
    }

}

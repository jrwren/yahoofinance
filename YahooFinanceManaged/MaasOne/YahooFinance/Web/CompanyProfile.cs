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
    public class CompanyProfileQuery : Query<CompanyProfileResult>, IYqlQuery
    {
        public bool UseDirectSource { get; set; }

        public bool GetDiagnostics { get; set; }

        public string[] IDs { get; set; }

        public CompanyProfileQuery() { }
        public CompanyProfileQuery(string id) : this(new string[] { id }) { }
        public CompanyProfileQuery(IEnumerable<string> ids) { this.IDs = ids.EnumToArray(); }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.IDs == null || this.IDs.Length == 0)
            {
                result.Success = false;
                result.Info.Add("IDs", "No IDs available.");
            }
            else
            {
                if (this.UseDirectSource)
                {
                    if (this.IDs[0].IsNullOrWhiteSpace())
                    {
                        result.Success = false;
                        result.Info.Add("IDs", "No valid ID available.");
                    }
                }
                else
                {
                    var validCnt = 0;
                    foreach (string id in this.IDs) { validCnt += id.IsNullOrWhiteSpace() ? 0 : 1; }
                    if (validCnt == 0)
                    {
                        result.Success = false;
                        result.Info.Add("IDs", "No valid IDs available.");
                    }
                }
            }
        }

        protected override Uri CreateUrl()
        {
            string url = string.Empty;
            if (this.UseDirectSource)
            {
                url = this.CreateUrl(this.IDs[0]);
            }
            else
            {
                string urlIn = string.Empty;
                foreach (var id in this.IDs) { if (!id.IsNullOrWhiteSpace()) urlIn += string.Format("'{0}', ", this.CreateUrl(id)); }
                urlIn = urlIn.Substring(0, urlIn.Length - 2);
                url = YFHelper.YqlUrl("*", "html",
                                       string.Format("url in ({0}) and xpath='//div[@id=\"rightcol\"]'", urlIn),
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        private string CreateUrl(string id) { return string.Format("http://finance.yahoo.com/q/pr?s={0}", Uri.EscapeDataString(id)); }

        protected override CompanyProfileResult ConvertResult(System.IO.Stream stream)
        {
            CompanyProfileResult result = new CompanyProfileResult();

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                result.Data = new CompanyProfileData[] { this.ConvertProfile((JObject)htmlDoc.FindFirst("div", "id", "rightcol")) };
            }
            else
            {
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                result.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    JToken divTok = yqlDoc.Query.Results["div"];
                    JArray divArr = null;
                    if (divTok is JArray) { divArr = (JArray)divTok; }
                    else { divArr = new JArray(divTok); }

                    List<CompanyProfileData> lstData = new List<CompanyProfileData>();
                    foreach (JObject divObj in divArr) { lstData.Add(this.ConvertProfile(divObj)); }
                    result.Diagnostics = yqlDoc.Query.Diagnostics;
                    result.Data = lstData.ToArray();
                }
            }

            return result;
        }

        public override Query<CompanyProfileResult> Clone()
        {
            return new CompanyProfileQuery()
            {
                GetDiagnostics = this.GetDiagnostics,
                UseDirectSource = this.UseDirectSource,
                IDs = (string[])this.IDs.Clone()
            };
        }

        private CompanyProfileData ConvertProfile(JObject rightcolObj)
        {
            JObject shortInfoObject = (JObject)rightcolObj.FindFirst("div", "id", "yfi_rt_quote_summary");
            JObject sumObject = (JObject)rightcolObj.FindFirst("table", "id", "yfncsumtab");

            if (sumObject == null) throw new ParseException("The [summary] object could not be load.");

            var result = new CompanyProfileData();


            result.ShortInfo = YFHelper.HtmlConvertShortInfo(shortInfoObject);


            result.Profile = new CompanyProfile();

            JObject tr = (JObject)sumObject.FindFirst("tr", "valign", "top");

            var tdl = tr["td"][0];

            result.Profile.ID = result.ShortInfo.ID;
            result.Profile.CompanyName = tdl[(this.UseDirectSource ? "b" : "strong")].HtmlFirstContent();
            result.Profile.Address = tdl["p"][0].HtmlFirstContent();
            result.Profile.BusinessSummary = tdl["p"][1].HtmlFirstContent();


            var details = tdl["table"][1]["tr"]["td"]["table"];
            result.Profile.Details = new CompanyProfileDetails();

            var secLnk = details["tr"][1]["td"][1].HtmlFirstLink();
            if (secLnk != null)
            {
                var secID = secLnk.Substring(0, secLnk.LastIndexOf("conameu")).Substring(secLnk.LastIndexOf('/') + 1).ParseInt();
                if (secID.HasValue)
                {
                    result.Profile.Details.Sector = new Sector(secID.Value, details["tr"][1]["td"][1]["a"].HtmlFirstContent());

                }
            }

            var indLnk = details["tr"][2]["td"][1].HtmlFirstLink();
            if (indLnk != null)
            {
                var indID = indLnk.Substring(0, indLnk.LastIndexOf(".")).Substring(indLnk.LastIndexOf("/") + 1).ParseInt();
                if (indID.HasValue)
                {
                    result.Profile.Details.Industry = new Industry(indID.Value, details["tr"][2]["td"][1]["a"].HtmlFirstContent());
                }
            }

            result.Profile.Details.FullTimeEmployees = details["tr"][3]["td"][1].HtmlFirstContent().ParseInt();


            var tdr = tr["td"][2];

            result.Profile.CorporateGovernance = tdr["table"][1]["tr"][0]["td"].HtmlFirstContent();
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
                result.Profile.KeyExecutives = lst.ToArray();
            }



            return result;
        }
    }

    public class CompanyProfileResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }

        public CompanyProfileData[] Data { get; internal set; }

        internal CompanyProfileResult() { }
    }

    public class CompanyProfileData
    {
        public CompanyProfile Profile { get; internal set; }

        public QuotesBase ShortInfo { get; internal set; }

        internal CompanyProfileData() { }
    }
}

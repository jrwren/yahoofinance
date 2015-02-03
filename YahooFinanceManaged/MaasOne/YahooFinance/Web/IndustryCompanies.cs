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
using MaasOne.YahooFinance.Data;

namespace MaasOne.YahooFinance.Web
{
    public class IndustryCompaniesQuery : Query<IndustryCompaniesResult>, IYqlQuery
    {

        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public Industry Industry { get; set; }

        public IndustryCompaniesQuery() { }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.Industry == null)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("Industry", "Industry is NULL."));
            }
        }
        protected override Uri CreateUrl()
        {
            string url = string.Format("http://biz.yahoo.com/ic/ind_{0}_cl_all.html", this.Industry.ID);
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       string.Format("url='{0}' and xpath='//td[@valign=\"top\" and @width=\"50%\"]/table'", url),
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        protected override IndustryCompaniesResult ConvertResult(System.IO.Stream stream)
        {
            IndustryCompaniesResult res = new IndustryCompaniesResult();

            JObject leftTableObject = null;
            JObject rightTableObject = null;

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                var tds2 = new List<JObject>();
                var tds = htmlDoc.Find("td", "valign", "top");
                foreach (var td in tds)
                {
                    if (td["width"] != null && td["width"].ToString() == "50%")
                    {
                        tds2.Add((JObject)td);
                    }
                }
                if (tds2.Count == 2)
                {
                    leftTableObject = tds2[0];
                    rightTableObject = tds2[1];
                }
            }
            else
            {
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                res.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    leftTableObject = (JObject)yqlDoc.Query.Results["table"][0];
                    rightTableObject = (JObject)yqlDoc.Query.Results["table"][1];
                }
            }

            if (leftTableObject == null || rightTableObject == null) throw new ParseException("The [table] object could not be load.");

            var lst = new List<IndustryCompany>();

            var links = new List<JToken>();
            links.AddRange(leftTableObject.Find("a"));
            links.AddRange(rightTableObject.Find("a"));

            foreach (var lnkObj in links)
            {
                var lnk = ((JObject)lnkObj).HtmlFirstLink();
                var id = MyHelper.GetRestTagValue(lnk, "s");
                if (id != null)
                {
                    lst[lst.Count - 1].ID = id;
                }
                else
                {
                    var item = new IndustryCompany();
                    item.Name = ((JObject)lnkObj).HtmlFirstContent();
                    item.IndustryComponentID = lnk.Substring(0, lnk.LastIndexOf(".")).Substring(lnk.LastIndexOf("/" + 1)).ParseInt().Value;
                    lst.Add(item);
                }
            }

            res.Companies = lst.ToArray();

            return res;
        }
        public override Query<IndustryCompaniesResult> Clone() { return new IndustryCompaniesQuery(); }


    }




    public class IndustryCompaniesResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public IndustryCompany[] Companies { get; internal set; }
        internal IndustryCompaniesResult() { }
    }
}

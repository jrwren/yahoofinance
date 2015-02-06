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
    public class IndustryListQuery : Query<IndustryListResult>, IYqlQuery
    {

        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public IndustryListQuery() { }

        protected override void ValidateQuery(ValidationResult result) { }
        protected override Uri CreateUrl()
        {
            string url = "http://biz.yahoo.com/ic/ind_index.html";
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       string.Format("url='{0}' and xpath='//td[@valign=\"top\" and @width=\"50%\"]/table'", url),
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        protected override IndustryListResult ConvertResult(System.IO.Stream stream)
        {
            IndustryListResult res = new IndustryListResult();

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

            var lstSec = new List<Sector>();
            var lstInd = new List<Industry>();
            this.SetSectorsAndIndustries(leftTableObject, lstSec, lstInd);
            this.SetSectorsAndIndustries(rightTableObject, lstSec, lstInd);
            res.Sectors = lstSec.ToArray();
            res.Industries = lstInd.ToArray();

            return res;
        }
        public override Query<IndustryListResult> Clone() { return new IndustryListQuery(); }


        private void SetSectorsAndIndustries(JObject table, List<Sector> lstSec, List<Industry> lstInd)
        {
            foreach (var row in table["tr"])
            {
                if (row["td"] is JArray)
                {
                    //Industry
                    var indLnk = row["td"][1].HtmlFirstLink();
                    int indID = int.Parse(indLnk.Substring(0, indLnk.LastIndexOf(".")).Substring(indLnk.LastIndexOf("/") + 1));
                    lstInd.Add(new Industry(indID, row["td"][1].HtmlFirstContent()));
                }
                else if (row["td"]["font"] != null || row["td"]["a"] != null)
                {
                    //Sector
                    lstSec.Add(new Sector(lstSec.Count + 1, row["td"].HtmlFirstContent()));
                }
            }
        }




    }




    public class IndustryListResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public Sector[] Sectors { get; internal set; }
        public Industry[] Industries { get; internal set; }
        public IndustryListResult() { }
    }
}

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
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.Finance.Yahoo.Data;

namespace MaasOne.Finance.Yahoo.Web
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
                                       "url='" + url + "' and xpath='//td[@valign=\"top\" and @width=\"50%\"]/table'",
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

            if (leftTableObject == null || rightTableObject == null) throw new ConvertException("The [table] object could not be load.");

            var lstSec = new List<Sector>();
            var lstInd = new List<Industry>();
            this.SetSecAndInd(leftTableObject, lstSec, lstInd);
            this.SetSecAndInd(rightTableObject, lstSec, lstInd);
            res.Sectors = lstSec.ToArray();
            res.Industries = lstInd.ToArray();

            return res;
        }
        public override Query<IndustryListResult> Clone() { return new IndustryListQuery(); }


        private void SetSecAndInd(JObject table, List<Sector> lstSec, List<Industry> lstInd)
        {
            foreach (var row in table["tr"])
            {
                if (row["td"] is JArray)
                {
                    //Industry
                    var indLnk = MyHelper.HtmlFirstLink(row["td"][1]);
                    int indID = int.Parse(indLnk.Substring(0, indLnk.LastIndexOf(".")).Substring(indLnk.LastIndexOf("/") + 1));
                    lstInd.Add(new Industry(indID, MyHelper.HtmlFirstContent(row["td"][1]), lstSec[lstSec.Count - 1]));
                }
                else if (row["td"]["font"] != null || row["td"]["a"] != null)
                {
                    //Sector
                    lstSec.Add(new Sector(lstSec.Count + 1, MyHelper.HtmlFirstContent(row["td"])));
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

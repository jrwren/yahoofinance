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
    public class AnalyseUpDowngradesQuery : Query<AnalyseUpDowngradesResult>, IYqlQuery
    {


        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public string ID { get; set; }

        public AnalyseUpDowngradesQuery() : this(string.Empty) { }
        public AnalyseUpDowngradesQuery(string id) { this.ID = id; }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.ID.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("ID", "No ID available."));
            }
        }
        protected override Uri CreateUrl()
        {
            string url = string.Format("http://finance.yahoo.com/q/ud?s={0}", this.ID);
            if (this.UseDirectSource == false)
            {
                url = YFHelper.YqlUrl("*", "html",
                                       "url='" + url + "' and (xpath='//div[@id=\"content\"]')",
                                       true, this.GetDiagnostics, null);
            }
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        protected override AnalyseUpDowngradesResult ConvertResult(System.IO.Stream stream)
        {
            AnalyseUpDowngradesResult result = new AnalyseUpDowngradesResult();

            JObject componentsObject = null;

            if (this.UseDirectSource == true)
            {
                string htmlText = MyHelper.StreamToString(stream);
                JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
                componentsObject = (JObject)htmlDoc.FindFirst("div", "id", "content");
            }
            else
            {
                YqlResponse yqlDoc = MyHelper.DeserializeJson<YqlResponse>(stream);
                if (yqlDoc == null) throw new Exception("Cannot read YQL response data.");
                result.Diagnostics = yqlDoc.Query.Diagnostics;
                if (yqlDoc.Query.Results != null)
                {
                    componentsObject = (JObject)yqlDoc.Query.Results["div"];
                }
            }

            if (componentsObject == null) throw new ParseException("The [profile] object could not be load.");

            var table = ((JObject)componentsObject["table"]["tr"][3]["td"]["table"][2]).HtmlInnerTable();

            var lst = new List<AnalyseUpDowngradesItem>();
            if (table["tr"] is JArray)
            {
                var trs = (JArray)table["tr"];

                int startIndex = (trs[0]["th"] != null) ? 1 : 0;
                for (int i = startIndex; i < trs.Count; i++)
                {
                    var utr = trs[i];
                    if (utr["td"] is JArray && ((JArray)utr["td"]).Count == 5)
                    {
                        var itm = new AnalyseUpDowngradesItem();
                        itm.Date = utr["td"][0].HtmlFirstContent().ParseDate().Value;
                        itm.ResearchFirm = utr["td"][1].HtmlFirstContent();
                        var action = utr["td"][2].HtmlFirstContent();
                        switch (action)
                        {
                            case "Initiated": itm.Action = AnalyseUpDowngradeAction.Initiated; break;
                            case "Upgrade": itm.Action = AnalyseUpDowngradeAction.Upgrade; break;
                            case "Downgrade": itm.Action = AnalyseUpDowngradeAction.Downgrade; break;
                        }
                        itm.From = utr["td"][3].HtmlFirstContent();
                        itm.To = utr["td"][4].HtmlFirstContent();

                        lst.Add(itm);
                    }
                }
            }

            result.History = lst.ToArray();

            return result;
        }
        public override Query<AnalyseUpDowngradesResult> Clone()
        {
            return new AnalyseUpDowngradesQuery() { ID = this.ID, GetDiagnostics = this.GetDiagnostics, UseDirectSource = this.UseDirectSource };
        }
        
    }



    /// <summary>
    /// Stores the result data
    /// </summary>
    public class AnalyseUpDowngradesResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public AnalyseUpDowngradesItem[] History { get; internal set; }
        public QuotesBaseData ShortInfo { get; internal set; }
        internal AnalyseUpDowngradesResult() { }
    }



}

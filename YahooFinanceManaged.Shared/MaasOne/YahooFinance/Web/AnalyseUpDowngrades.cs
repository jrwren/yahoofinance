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
    public class AnalyseUpDowngradesQuery : YqlQuery<AnalyseUpDowngradesResult>
    {
        public string ID { get; private set; }


        public AnalyseUpDowngradesQuery() { }

        public AnalyseUpDowngradesQuery(string id) { this.ID = id; }


        public override QueryBase Clone() { return new AnalyseUpDowngradesQuery(this.ID); }

        protected override string CreateUrl() { return string.Format("http://finance.yahoo.com/q/ud?s={0}", Uri.EscapeDataString(this.ID)); }

        protected override void Validate(ValidationResult result)
        {
            if (this.ID.IsNullOrWhiteSpace() == true)
            {
                result.Success = false;
                result.Info.Add("ID", "ID is NULL or empty.");
            }
        }


        internal override AnalyseUpDowngradesResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            AnalyseUpDowngradesResult result = new AnalyseUpDowngradesResult();

            JObject contentObj = (JObject)yqlToken;

            JObject table = ((JObject)contentObj["table"]["tr"][3]["td"]["table"][2]).HtmlInnerTable();

            List<AnalyseUpDowngrade> lst = new List<AnalyseUpDowngrade>();
            if (table["tr"] is JArray)
            {
                JArray trs = (JArray)table["tr"];

                int startIndex = (trs[0]["th"] != null) ? 1 : 0;
                for (int i = startIndex; i < trs.Count; i++)
                {
                    JToken utr = trs[i];
                    if (utr["td"] is JArray && ((JArray)utr["td"]).Count == 5)
                    {
                        AnalyseUpDowngrade itm = new AnalyseUpDowngrade();
                        itm.Date = utr["td"][0].HtmlFirstContent().ParseDate().Value;
                        itm.ResearchFirm = utr["td"][1].HtmlFirstContent();
                        string action = utr["td"][2].HtmlFirstContent();
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
            JObject centerObj = ((JObject)contentObj.FindFirst("center"));
            JToken[] links = centerObj.Find("a");
            foreach (JToken lnk in links)
            {
                try
                {
                    string tag = MyHelper.GetRestTagValue(lnk["href"].ToString(), ".sym");
                    if (tag.IsNullOrWhiteSpace() == false)
                    { result.ID = tag; break; }
                }
                catch (Exception ex) { }
            }

            return result;
        }

        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc) { return htmlDoc.FindFirst("div", "id", "content"); }

        internal override string YqlXPath() { return "//div[@id=\"content\"]"; }
    }


    public class AnalyseUpDowngradesResult : ResultBase, IID
    {
        public AnalyseUpDowngrade[] History { get; internal set; }

        public string ID { get; internal set; }


        internal AnalyseUpDowngradesResult() { }
    }
}

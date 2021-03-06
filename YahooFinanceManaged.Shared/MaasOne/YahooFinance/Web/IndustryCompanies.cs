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
    public class IndustryCompaniesQuery : YqlQuery<IndustryCompaniesResult>
    {
        public int Industry { get; set; }


        public IndustryCompaniesQuery() { }

        public IndustryCompaniesQuery(int industry) { this.Industry = industry; }


        public override QueryBase Clone()
        {
            return new IndustryCompaniesQuery(this.Industry);
        }


        protected override string CreateUrl() { return string.Format("http://biz.yahoo.com/ic/{0}_cl_all.html", this.Industry); }

        protected override void Validate(ValidationResult result)
        {
            if (this.Industry <= 0 || this.Industry >= 1000)
            {
                result.Success = false;
                result.Info.Add("Industry", "Industry is invalid.");
            }
        }


        internal override IndustryCompaniesResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            IndustryCompaniesResult result = new IndustryCompaniesResult();

            JObject tableObj = (JObject)yqlToken;

            JObject leftTableObject = (JObject)tableObj["td"][0];
            JObject rightTableObject = (JObject)tableObj["td"][2];

            if (leftTableObject == null || rightTableObject == null) throw new ParseException("The [table] object could not be load.");

            List<IndustryCompany> lst = new List<IndustryCompany>();

            List<JToken> links = new List<JToken>();
            links.AddRange(leftTableObject.Find("a"));
            links.AddRange(rightTableObject.Find("a"));

            foreach (JToken lnkObj in links)
            {
                string lnk = ((JObject)lnkObj).HtmlFirstLink();
                string id = MyHelper.GetRestTagValue(lnk, "s");
                if (id != null)
                {
                    lst[lst.Count - 1].ID = id;
                }
                else
                {
                    IndustryCompany item = new IndustryCompany();
                    item.Name = ((JObject)lnkObj).HtmlFirstContent();
                    item.IndustryComponentID = lnk.Substring(0, lnk.LastIndexOf(".")).Substring(lnk.LastIndexOf("/") + 1).ParseInt().Value;
                    lst.Add(item);
                }
            }

            result.Companies = lst.ToArray();

            return result;
        }

        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc) { return htmlDoc["html"]["body"]["table"]["tr"]["td"]["table"][5]["tr"]["td"][2]["table"][1]["tr"][3]; }

        internal override string YqlXPath() { return "html/body/table/tr/td/table[6]/tr/td[3]/table[2]/tr[4]"; }
    }


    public class IndustryCompaniesResult : ResultBase
    {
        public IndustryCompany[] Companies { get; internal set; }
        internal IndustryCompaniesResult() { }
    }
}

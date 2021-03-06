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
    public class IndustryListQuery : YqlQuery<IndustryListResult>
    {

        public IndustryListQuery() { }


        public override QueryBase Clone() { return new IndustryListQuery(); }


        protected override string CreateUrl() { return "http://biz.yahoo.com/ic/ind_index.html"; }

        protected override void Validate(ValidationResult result) { }


        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc)
        {
            return htmlDoc["html"]["body"]["table"][0]["tr"]["td"]["table"][5]["tr"]["td"][0]["table"][1]["tr"][3];
        }

        internal override IndustryListResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            IndustryListResult res = new IndustryListResult();

            JObject leftTableObject = (JObject)yqlToken["td"][0]["table"];
            JObject rightTableObject = (JObject)yqlToken["td"][2]["table"];

            List<Sector> lstSec = new List<Sector>();
            List<Industry> lstInd = new List<Industry>();
            this.SetSectorsAndIndustries(leftTableObject, lstSec, lstInd);
            this.SetSectorsAndIndustries(rightTableObject, lstSec, lstInd);
            res.Sectors = lstSec.ToArray();
            res.Industries = lstInd.ToArray();

            return res;
        }

        internal override string YqlXPath()
        {
            return "html/body/table[1]/tr/td/table[6]/tr/td[1]/table[2]/tr[4]";
        }


        private void SetSectorsAndIndustries(JObject table, List<Sector> lstSec, List<Industry> lstInd)
        {
            foreach (JToken row in table["tr"])
            {
                if (row["td"] is JArray)
                {
                    //Industry
                    string indLnk = row["td"][1].HtmlFirstLink();
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


    public class IndustryListResult : ResultBase
    {
        public Sector[] Sectors { get; internal set; }
        public Industry[] Industries { get; internal set; }
        public IndustryListResult() { }
    }
}

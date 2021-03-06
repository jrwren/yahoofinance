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

    public class IDSearchInstantQuery : Query<IDSearchInstantResult>
    {
        public Culture Culture { get; set; }

        public string LookupText { get; set; }


        public IDSearchInstantQuery()
        {
            this.Culture = Cultures.UnitedStates_English;
            this.LookupText = string.Empty;
        }

        public IDSearchInstantQuery(string lookupText)
            : this()
        {
            this.LookupText = lookupText;
        }


        public override QueryBase Clone()
        {
            return new IDSearchInstantQuery()
            {
                Culture = this.Culture,
                LookupText = this.LookupText
            };
        }


        protected override IDSearchInstantResult ConvertResult(System.IO.Stream stream, ConvertInfo ci)
        {
            IDSearchInstantResult result = new IDSearchInstantResult();

            string jsonTxt = MyHelper.StreamToString(stream);
            jsonTxt = jsonTxt.Substring(0, jsonTxt.LastIndexOf("}") + 1).Substring(jsonTxt.IndexOf("{"));

            InstantResponse resp = JsonConvert.DeserializeObject<InstantResponse>(jsonTxt);

            List<IDSearchInstantItem> lst = new List<IDSearchInstantItem>();
            foreach (var res in resp.ResultSet.Result)
            {
                IDSearchInstantItem isd = new IDSearchInstantItem()
                {
                    ID = res.symbol,
                    Name = res.name,
                    Exchange = res.exch
                };
                switch (res.type.ToUpper())
                {
                    case "S": isd.Type = FinanceType.Stock; break;
                    case "I": isd.Type = FinanceType.Index; break;
                    case "F": isd.Type = FinanceType.Future; break;
                    case "E": isd.Type = FinanceType.ETF; break;
                    case "M": isd.Type = FinanceType.Fund; break;
                }
                lst.Add(isd);
            }
            result.Items = lst.ToArray();

            return result;
        }

        protected override string CreateUrl()
        {
            Culture cult = this.Culture != null ? this.Culture : Cultures.UnitedStates_English;
            System.Text.StringBuilder url = new System.Text.StringBuilder();
            /*
            url.Append("http://d.yimg.com/autoc.finance.yahoo.com/autoc?query=")
            url.Append(Uri.EscapeDataString(search.Trim))
            url.Append("&callback=YAHOO.Finance.SymbolSuggest.ssCallback")
            */
            url.Append("http://d.yimg.com/aq/autoc?query=");
            url.Append(Uri.EscapeDataString(this.LookupText.Trim()));
            url.Append(YFHelper.CultureToParameters(cult));
            url.Append("&callback=YAHOO.util.ScriptNodeDataSource.callbacks");
            return url.ToString();
        }

        protected override void Validate(ValidationResult result)
        {
            if (this.LookupText.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Info.Add("Query", "Query is empty.");
            }
        }


        private class InstantResponse
        {
            public InstantResultSet ResultSet { get; set; }
        }

        private class InstantResultSet
        {
            public string Query { get; set; }
            public InstantIDResult[] Result { get; set; }
        }

        private class InstantIDResult
        {
            public string symbol { get; set; }
            public string name { get; set; }
            public string exch { get; set; }
            public string type { get; set; }
            public string exchDisp { get; set; }
            public string typeDisp { get; set; }
        }
    }


    public class IDSearchInstantResult : ResultBase
    {
        /// <summary>
        /// Gets the received ID search results.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IDSearchInstantItem[] Items { get; internal set; }


        internal IDSearchInstantResult() { }
    }
}

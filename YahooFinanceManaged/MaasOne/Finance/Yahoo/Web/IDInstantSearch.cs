﻿// **************************************************************************************************
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

    public class IDInstantSearchQuery : Query<IDInstantSearchResult>, ILookupQuery
    {

        public string LookupText { get; set; }
        public Culture Culture { get; set; }

        public IDInstantSearchQuery() : this(string.Empty) { this.Culture = DefaultData.Cultures.UnitedStates_English; }
        public IDInstantSearchQuery(string lookupText) { this.LookupText = lookupText; }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.LookupText.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("Query", "Query is empty."));
            }
        }
        protected override Uri CreateUrl()
        {
            Culture cult = this.Culture != null ? this.Culture : DefaultData.Cultures.UnitedStates_English;
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
            return new Uri(url.ToString(), UriKind.RelativeOrAbsolute);
        }
        protected override IDInstantSearchResult ConvertResult(System.IO.Stream stream)
        {
            IDInstantSearchResult result = null;

            var jsonTxt = MyHelper.StreamToString(stream);
            jsonTxt = jsonTxt.Substring(0, jsonTxt.LastIndexOf("}") + 1).Substring(jsonTxt.IndexOf("{"));

            InstantResponse resp = JsonConvert.DeserializeObject<InstantResponse>(jsonTxt);
            if (resp != null)
            {
                result = new IDInstantSearchResult();
                List<IDInstantSearchData> lst = new List<IDInstantSearchData>();
                foreach (var res in resp.ResultSet.Result)
                {
                    var isd = new IDInstantSearchData()
                    {
                        ID = res.symbol,
                        Name = res.name,
                        Exchange = res.exch
                    };
                    switch (res.type.ToUpper())
                    {
                        case "S": isd.Type = SecurityType.Stock; break;
                        case "I": isd.Type = SecurityType.Index; break;
                        case "F": isd.Type = SecurityType.Future; break;
                        case "E": isd.Type = SecurityType.ETF; break;
                        case "M": isd.Type = SecurityType.Fund; break;
                    }
                    lst.Add(isd);
                }
                result.Items = lst.ToArray();
            }

            return result;
        }
        public override Query<IDInstantSearchResult> Clone()
        {
            return new IDInstantSearchQuery() { Culture = this.Culture, LookupText = this.LookupText };
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




    /// <summary>
    /// Stores the result data
    /// </summary>
    public class IDInstantSearchResult
    {
        /// <summary>
        /// Gets the received ID search results.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IDInstantSearchData[] Items { get; internal set; }

        internal IDInstantSearchResult() { }
    }


}

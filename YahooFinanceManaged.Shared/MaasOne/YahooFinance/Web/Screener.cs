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
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.YahooFinance.Data;

namespace MaasOne.YahooFinance.Web
{



    public abstract class ScreenerQuery : YqlQuery<ScreenerResult>
    {
        public ScreenerCriteria[] Criterias { get; set; }

        public ListSortDirection SortDirection { get; set; }

        public ScreenerProperty SortProperty { get; set; }


        protected ScreenerQuery() { }


        protected override void Validate(ValidationResult result)
        {
            if (this.Criterias == null || this.Criterias.Length == 0)
            {
                result.Success = false;
                result.Info.Add("Criterias", "No criterias found.");
            }
            else
            {
                foreach (var c in this.Criterias)
                {
                    if (c.IsValidInternal() == false)
                    {
                        result.Success = false;
                        result.Info.Add("Criteria: [" + c.GetType().Name + "]", "Invalid.");
                    }
                }
            }
        }


        internal override ScreenerResult YqlConvertToken(JToken yqlToken, ConvertInfo ci)
        {
            ScreenerResult result = new ScreenerResult();

            JObject bodyObject = (JObject)yqlToken;

            JArray tableObject = null;
            JObject paginationObject = null;


            if (this is BondScreenerQuery)
            {
                tableObject = (JArray)bodyObject.FindFirst("table", "class", "yfnc_tableout1")["tr"]["td"]["table"]["tr"];

                var tds = bodyObject.Find("td", "align", "right");
                foreach (var td in tds)
                {
                    if (td is JObject && td[ci.IsDirectSource ? "b" : "strong"] != null) { paginationObject = (JObject)td["b"]; break; }
                }
            }
            else
            {
                tableObject = (JArray)bodyObject["table"][1]["tr"]["td"]["table"][0]["tr"];
                paginationObject = (JObject)bodyObject["table"][0]["tr"][0]["td"][0]["font"];
            }

            var lstHeaders = new List<ScreenerProperty?>();
            var lst = new List<ScreenerItem>();

            if (tableObject.Count > 0)
            {
                var headerObj = (JObject)tableObject[0];
                foreach (var td in headerObj["td"])
                {
                    ScreenerProperty? prp = null;
                    var tag = td.HtmlFirstLink();
                    if (tag != null)
                    {

                        if (tag.IndexOf("s=") > -1) { tag = tag.Substring(tag.IndexOf("s=") + 2); }
                        else if (tag.IndexOf("z=") > -1) { tag = tag.Substring(tag.IndexOf("z=") + 2); }
                        else if (tag.IndexOf("sf=") > -1) { tag = tag.Substring(tag.IndexOf("sf=") + 3); }

                        if (tag.IndexOf("&") > -1) tag = tag.Substring(0, tag.IndexOf("&"));

                        foreach (ScreenerProperty sp in Enum.GetValues(typeof(ScreenerProperty)))
                        {
                            if (YFHelper.ContainsScreenerPropertyTag(sp) && tag == YFHelper.GetScreenerPropertyTag(sp))
                            {
                                prp = sp;
                                break;
                            }
                        }
                    }
                    lstHeaders.Add(prp);
                }
            }



            if (lstHeaders.Count > 0 && tableObject.Count > 1)
            {
                for (int i = 1; i < tableObject.Count; i++)
                {
                    var tr = (JObject)tableObject[i];
                    var tds = (JArray)tr["td"];
                    if (tds.Count == lstHeaders.Count)
                    {
                        var item = new ScreenerItem();
                        for (int n = 0; n < lstHeaders.Count; n++)
                        {
                            var td = tds[n];
                            if (lstHeaders[n].HasValue)
                            {
                                object value = null;
                                string valueStr = string.Empty;
                                if (lstHeaders[n].Value == ScreenerProperty.Maturity)
                                { valueStr = ((JObject)td).FindFirst(ci.IsDirectSource ? "div" : "font")["nobr"].ToString(); }
                                else if (ci.IsDirectSource == false && lstHeaders[n].Value == ScreenerProperty.Issue)
                                { valueStr = ((JObject)td).FindFirst("div").HtmlFirstContent(); }
                                else
                                { valueStr = td.HtmlFirstContent(true); }
                                if (valueStr != null)
                                {
                                    if (lstHeaders[n].Value == ScreenerProperty.Symbol)
                                    {
                                        value = valueStr.Replace("\n>", "");
                                        item.ID = value.ToString();
                                    }
                                    else if (lstHeaders[n].Value == ScreenerProperty.Name)
                                    {
                                        item.Name = valueStr;
                                    }
                                    else if (lstHeaders[n].Value == ScreenerProperty.Industry)
                                    {
                                        try
                                        {
                                            var indLnk = td.HtmlFirstLink();
                                            int indID = int.Parse(indLnk.Substring(0, indLnk.LastIndexOf(".")).Substring(indLnk.LastIndexOf("/") + 1));
                                            value = new KeyValuePair<int, string>(indID, valueStr);
                                        }
                                        catch (Exception ex) { }
                                    }
                                    else if (valueStr.EndsWith("M") || valueStr.StartsWith("B") || valueStr.StartsWith("K"))
                                    {
                                        try { value = YFHelper.GetMillionValue(valueStr.Replace("$", "")) * 1000000; }
                                        catch (Exception ex) { }
                                    }
                                    else if (lstHeaders[n].Value == ScreenerProperty.Type)
                                    {
                                        switch (valueStr)
                                        {
                                            case "Treas": value = BondType.Treasury; break;
                                            case "Zero": value = BondType.TreasuryZeroCoupon; break;
                                            case "Corp": value = BondType.Corporate; break;
                                            case "Muni": value = BondType.Municipal; break;
                                        }
                                    }
                                    else if (lstHeaders[n].Value == ScreenerProperty.Issue)
                                    {
                                        try
                                        {
                                            var indLnk = td.HtmlFirstLink();
                                            var indID = indLnk.Substring(0, indLnk.IndexOf("&")).Substring(indLnk.IndexOf("ce=") + 3);
                                            value = new KeyValuePair<string, string>(indID, valueStr);
                                        }
                                        catch (Exception ex) { }
                                    }
                                    else if (lstHeaders[n].Value == ScreenerProperty.FitchRating)
                                    {
                                        if (valueStr == "Not Rated") { value = BondFitchRating.NR; }
                                        else
                                        {
                                            foreach (BondFitchRating r in Enum.GetValues(typeof(BondFitchRating))) { if (r.ToString() == valueStr) { value = r; break; } }
                                        }
                                    }
                                    else if (lstHeaders[n].Value == ScreenerProperty.Callable)
                                    {
                                        if (valueStr == "Yes") { value = true; }
                                        else { value = false; }
                                    }


                                    if (value == null && valueStr != "N/A")
                                    {
                                        value = valueStr.Replace("%", "").Replace("$", "").ToObject();
                                    }
                                }
                                else if (lstHeaders[n].Value == ScreenerProperty.MorningstarRating)
                                {
                                    try
                                    {
                                        var lnk = (td["img"]["src"]).ToString();
                                        value = (FundMorningstarRating)int.Parse(lnk.Substring(0, lnk.LastIndexOf("x.")).Substring(lnk.LastIndexOf("/stars") + 6));
                                    }
                                    catch (Exception ex)
                                    {
                                        value = -1;
                                    }
                                }



                                item.Values.Add(lstHeaders[n].Value, value);

                            }
                            else
                            {
                                //More Info                               
                                if (item.Values.ContainsKey(ScreenerProperty.Symbol) && item.Values[ScreenerProperty.Symbol] == null)
                                {
                                    var id = string.Empty;
                                    var ass = td["font"]["a"];
                                    foreach (var a in ass)
                                    {
                                        if (a.HtmlFirstContent() == "Quote")
                                        {
                                            var lnk = a.HtmlFirstLink();
                                            lnk = lnk.Substring(lnk.IndexOf("s=") + 2);
                                            if (lnk.IndexOf("&") > -1) lnk = lnk.Substring(0, lnk.IndexOf("&"));
                                            id = lnk;
                                            break;
                                        }
                                    }
                                    if (string.IsNullOrEmpty(id) == false) item.Values[ScreenerProperty.Symbol] = id;
                                }
                            }
                        }
                        lst.Add(item);
                    }
                }
            }

            result.Items = lst.ToArray();

            var paginationStr = paginationObject.HtmlFirstContent();
            if (string.IsNullOrEmpty(paginationStr) == false)
            {
                paginationStr = paginationStr.Replace("(Showing ", "").Replace("to", "").Replace("-", "").Replace("of", "").Replace(")", "").Replace("|", "");
                var parts = paginationStr.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3)
                {
                    result.Pagination = new LimitedPagination(int.Parse(parts[0]) - 1, int.Parse(parts[1]) - int.Parse(parts[0]) + 1, int.Parse(parts[2]));
                }
            }
            else
            {
                ci.IsIntegrityComplete = false;
                ci.IntegrityMessages.Add("No pagination available.");
            }

            return result;
        }

        internal override JToken YqlTokenFromDirectSource(JObject htmlDoc) { return htmlDoc["html"]["body"]; }

        internal override string YqlXPath() { return "/html/body"; }
    }


    public class ScreenerResult : ResultBase
    {
        public LimitedPagination Pagination { get; internal set; }
        public ScreenerItem[] Items { get; internal set; }
        internal ScreenerResult() { }
    }


    public abstract class ScreenerCriteria
    {

        public abstract string DisplayName { get; }
        public abstract ScreenerProperty Property { get; }

        protected ScreenerCriteria() { }

        protected abstract string TagParameter();
        protected abstract bool IsValid();

        internal string TagParameterInternal() { return this.TagParameter(); }
        internal bool IsValidInternal() { return this.IsValid(); }

    }

    public abstract class ScreenerMinMaxCriteria<T> : ScreenerCriteria where T : struct
    {
        public T? Minimum { get; set; }
        public T? Maximum { get; set; }
        protected ScreenerMinMaxCriteria() { }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}/{2}", YFHelper.GetScreenerPropertyTag(this.Property), this.Minimum, this.Maximum);
        }
    }


}

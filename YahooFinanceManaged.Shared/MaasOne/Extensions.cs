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
using System.IO;
using System.Text;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using MaasOne;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#if  (NET20)
using Newtonsoft.Json.Utilities.LinqBridge;
#else
using System.Linq;
#endif

namespace MaasOne
{
#if !(NETFX_CORE)
    [Browsable(false)]
#endif
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class YFExtensions
    {
        public static string[] ToIdArray(this IEnumerable<MaasOne.YahooFinance.IID> items) { return ToIdArray(items, false); }
        public static string[] ToIdArray(this IEnumerable<MaasOne.YahooFinance.IID> items, bool ignoreNullItems)
        {
            List<string> result = new List<string>();
            foreach (MaasOne.YahooFinance.IID iid in items) { if (iid != null || ignoreNullItems == false) result.Add(iid != null ? iid.ID : null); }
            return result.ToArray();
        }

        internal static JToken FindFirst(this JObject root, string nodeName) { return FindFirst(root, nodeName, null, null); }
        internal static JToken FindFirst(this JObject root, string nodeName, string propertyName, object propertyValue)
        {
            var res = Find(root, nodeName, propertyName, propertyValue, true);
            if (res.Length > 0) return res[0];
            return null;
        }

        internal static JToken[] Find(this JObject root, string nodeName) { return root.Find(nodeName, null, null); }
        internal static JToken[] Find(this JObject root, string nodeName, string propertyName, object propertyValue) { return root.Find(nodeName, propertyName, propertyValue, false); }
        private static JToken[] Find(this JObject root, string nodeName, string propertyName, object propertyValue, bool returnFirst)
        {
            List<JToken> lst = new List<JToken>();
            if (root != null)
            {
                IEnumerable<JToken> tokens = root.Descendants().OfType<JProperty>().Where(p => p.Name == nodeName).Select(p => p.Value);
                if (string.IsNullOrEmpty(propertyName))
                {
                    foreach (JToken tok in tokens)
                    {
                        if (tok is JArray)
                        {
                            foreach (var subTok in (JArray)tok)
                            {
                                lst.Add(subTok);
                                if (returnFirst) break;
                            }
                        }
                        else
                        {
                            lst.Add(tok);
                        }
                        if (returnFirst) break;
                    }
                }
                else
                {
                    foreach (var d in tokens)
                    {
                        if (d is JObject)
                        {
                            JToken dValObj = d[propertyName];
                            if (dValObj != null && dValObj is JValue && ((JValue)dValObj).Value.Equals(propertyValue))
                            {
                                lst.Add(d);
                                if (returnFirst) break;
                            }
                        }
                        else if (d is JArray)
                        {
                            foreach (JToken dObj in (JArray)d)
                            {
                                if (!(dObj is JValue))
                                {
                                    JToken dValObj = dObj[propertyName];
                                    if (dValObj != null && dValObj is JValue && ((JValue)dValObj).Value.Equals(propertyValue))
                                    {
                                        lst.Add(dObj);
                                        if (returnFirst) break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return lst.ToArray();
        }

        internal static JObject HtmlInnerTable(this JObject table)
        {
            if (table["tr"] is JObject && table["tr"]["td"] is JObject && table["tr"]["td"]["table"] is JObject)
            {
                table = HtmlInnerTable((JObject)table["tr"]["td"]["table"]);
            }
            return table;
        }

        private static readonly string[] ContentTags = new string[] { "content", "p", "a", "span", "b", "strong", "small", "font", "div" };
        private static readonly string[] LinkTags = new string[] { "href", "a", "p", "span", "b", "strong", "small", "font", "div" };
        private static readonly char[] TrimChars = new char[] { ',', ' ', '\n', '\r', '\t' };
        internal static string HtmlFirstLink(this JToken t, bool includeDiv = false)
        {
            if (t == null) return null;
            if (t is JValue) return null;
            foreach (JProperty p in t.Children<JProperty>())
            {
                if ((includeDiv || p.Name != "div") && LinkTags.EnumContains(p.Name))
                {
                    if (p.Value is JValue && p.Name == "href") return p.Value.ToString().Trim(TrimChars);
                    string result = HtmlFirstLink(p.Value, includeDiv);
                    if (result != null) return result;
                }
            }
            return null;
        }
        internal static string HtmlFirstContent(this JToken t, bool includeDiv = false)
        {
            if (t != null)
            {
                if (t is JValue) return t.ToString().Trim(TrimChars);
                foreach (JProperty p in t.Children<JProperty>())
                {
                    if ((includeDiv || p.Name != "div") && ContentTags.EnumContains(p.Name))
                    {
                        var result = HtmlFirstContent(p.Value, includeDiv);
                        if (result != null) return result;
                    }
                }
            }
            return null;
        }


        internal static double? ParseDouble(this string s) { return s.ParseDouble(MyHelper.ConverterCulture); }
        internal static double? ParseDouble(this string s, CultureInfo c)
        {
            if (c == null) throw new ArgumentException("Culture is NULL.", "c");
            if (s.IsNullOrWhiteSpace()) return null;
            double tmp;
            return double.TryParse(s.Trim(TrimChars), NumberStyles.Any, c, out tmp) ? tmp : (double?)null;
        }
        internal static int? ParseInt(this string s) { return s.ParseInt(MyHelper.ConverterCulture); }
        internal static int? ParseInt(this string s, CultureInfo c)
        {
            if (c == null) throw new ArgumentException("Culture is NULL.", "c");
            if (s.IsNullOrWhiteSpace()) return null;
            int tmp;
            return int.TryParse(s.Trim(TrimChars), NumberStyles.Any, c, out tmp) ? tmp : (int?)null;
        }
        internal static DateTime? ParseDate(this string s) { return s.ParseDate(MyHelper.ConverterCulture); }
        internal static DateTime? ParseDate(this string s, CultureInfo c)
        {
            if (c == null) throw new ArgumentException("Culture is NULL.", "c");
            if (s.IsNullOrWhiteSpace()) return null;
            DateTime tmp;
            return DateTime.TryParse(s.Trim(TrimChars), c, DateTimeStyles.None, out tmp) ? tmp : (DateTime?)null;
        }

        internal static T EnumItemAt<T>(this IEnumerable<T> values, int index)
        {
            int cnt = 0;
            foreach (T itm in values)
            {
                if (cnt == index) return itm;
                cnt++;
            }
            return default(T);
        }
        internal static bool EnumContains<T>(this IEnumerable<T> values, T item)
        {
            foreach (T i in values) { if (i.Equals(item)) return true; }
            return false;
        }
        internal static T[] EnumToArray<T>(this IEnumerable<T> values)
        {
            if (values != null)
            {
                if (values is T[])
                {
                    return (T[])values;
                }
                else if (values is List<T>)
                {
                    return ((List<T>)values).ToArray();
                }
                else
                {
                    var result = new T[values.Count()]; int i = 0;
                    foreach (T t in values) { result[i] = t; i++; }
                    return result;
                }
            }
            else
            {
                return new T[] { };
            }
        }

        internal static object ToObject(this string s) { return s.ToObject(MyHelper.ConverterCulture); }
        internal static object ToObject(this string s, System.Globalization.CultureInfo c)
        {
            if (c == null) throw new ArgumentException("Culture is NULL.", "c");
            if (s.IsNullOrWhiteSpace()) return null;
            string value = s.Replace("%", "").Replace("\"", "").Replace("<b>", "").Replace("</b>", "").Replace("N/A", "").Trim(TrimChars);
            if (value != string.Empty)
            {
                if (value == "-") { return string.Empty; }

                double db;
                if (value.EndsWith("M") || value.EndsWith("B") || value.EndsWith("K"))
                {
                    string val2 = value.Substring(0, value.Length - 1);
                    if (double.TryParse(val2, System.Globalization.NumberStyles.Any, c, out db))
                    {
                        return (long)(MaasOne.YahooFinance.YFHelper.GetMillionValue(value) * 1000000);
                    }
                }

                if (value.Contains(c.NumberFormat.NumberDecimalSeparator) && double.TryParse(value, System.Globalization.NumberStyles.Any, c, out db))
                {
                    return db;
                }

                long l;
                if (long.TryParse(value, NumberStyles.Any, c, out l))
                {
                    int i;
                    if (int.TryParse(value, NumberStyles.Any, c, out i) && l == i)
                    {
                        return i;
                    }
                    return l;
                }

                System.DateTime dt;
                if (value.IndexOf(':') != value.LastIndexOf(':') && System.DateTime.TryParse(value, c, DateTimeStyles.AssumeUniversal, out dt))
                {
                    return dt;
                }

                return s;
            }
            return null;
        }
    }
}

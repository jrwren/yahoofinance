// ******************************************************************************
// ** 
// **  MaasOne WebServices
// **  Written by Marius Häusler 2012
// **  It would be pleasant, if you contact me when you are using this code.
// **  Contact: YahooFinanceManaged@gmail.com
// **  Project Home: http://code.google.com/p/yahoo-finance-managed/
// **  
// ******************************************************************************
// **  
// **  Copyright 2012 Marius Häusler
// **  
// **  Licensed under the Apache License, Version 2.0 (the "License");
// **  you may not use this file except in compliance with the License.
// **  You may obtain a copy of the License at
// **  
// **    http://www.apache.org/licenses/LICENSE-2.0
// **  
// **  Unless required by applicable law or agreed to in writing, software
// **  distributed under the License is distributed on an "AS IS" BASIS,
// **  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// **  See the License for the specific language governing permissions and
// **  limitations under the License.
// ** 
// ******************************************************************************
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
#if !(NET20)
using System.Xml.Linq;
#endif

namespace MaasOne
{

    internal static class MyHelper
    {

        public static readonly System.Globalization.CultureInfo ConverterCulture = new System.Globalization.CultureInfo("en-US");

        public static string StreamToString(Stream stream) { return StreamToString(stream, null); }
        public static string StreamToString(Stream stream, Encoding encoding)
        {
            if (stream != null)
            {
                if (encoding == null) encoding = System.Text.Encoding.UTF8;
                if (stream.CanSeek) stream.Seek(0, System.IO.SeekOrigin.Begin);
                if (stream.CanRead)
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream, encoding))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            return null;
        }
        public static byte[] StreamToBytes(Stream stream)
        {
            if (stream != null)
            {
                byte[] result = new byte[Convert.ToInt32(stream.Length) + 1];
                stream.Read(result, 0, Convert.ToInt32(stream.Length));
                return result;
            }
            return null;
        }
        public static MemoryStream CopyStream(Stream stream)
        {
            System.IO.MemoryStream copy = null;
            if (stream != null && stream.CanRead)
            {
                if (stream.CanSeek) stream.Seek(0, System.IO.SeekOrigin.Begin);
                copy = new System.IO.MemoryStream();
                byte[] buffer = new byte[4096];
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0) { copy.Write(buffer, 0, read); }
                copy.Seek(0, System.IO.SeekOrigin.Begin);
            }
            return copy;
        }


        public static string CharEnumToString(IEnumerable<char> arr)
        {
            string s = string.Empty;
            if (arr != null)
            {
                foreach (char c in arr)
                {
                    s += c;
                }
            }
            return s;
        }
        public static T GetEnumItemAt<T>(IEnumerable<T> values, int index)
        {
            int cnt = 0;
            foreach (T itm in values)
            {
                if (cnt == index) return itm;
                cnt++;
            }
            return default(T);
        }

        public static object StringToObject(string str, System.Globalization.CultureInfo ci)
        {
            if (str == null) return null;
            string value = str.Replace("%", "").Replace("\"", "").Replace("<b>", "").Replace("</b>", "").Replace("N/A", "").Trim();
            if (value != string.Empty)
            {
                double dbl;
                if (value.EndsWith("M") || value.EndsWith("B") || value.EndsWith("K"))
                {
                    string val2 = value.Substring(0, value.Length - 1);
                    if (double.TryParse(val2, System.Globalization.NumberStyles.Any, ci, out dbl))
                    {
                        return Finance.Yahoo.YFHelper.GetMillionValue(value);
                    }
                }

                if (value == "-") { return string.Empty; }

                if (value.Contains(" - "))
                {
                    String[] values = value.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                    List<object> results = new List<object>();
                    foreach (String v in values)
                    {
                        results.Add(StringToObject(v, ci));
                    }
                    if (results.Count == 0) { return string.Empty; }
                    else if (results.Count == 0) { return results[0]; }
                    else { return results.ToArray(); }
                }

                if (value.Contains(ci.NumberFormat.NumberDecimalSeparator) && double.TryParse(value, System.Globalization.NumberStyles.Any, ci, out dbl))
                {
                    return dbl;
                }
                else
                {
                    int lng = 0;
                    if (int.TryParse(value, NumberStyles.Any, ci, out lng))
                    {
                        return lng;
                    }
                    else
                    {
                        System.DateTime dte;
                        if (value.IndexOf(':') != value.LastIndexOf(':') && System.DateTime.TryParse(value, ci, System.Globalization.DateTimeStyles.AdjustToUniversal, out dte))
                        {
                            return dte;
                        }
                        else
                        {
                            return str;
                        }
                    }
                }

            }
            else
            {
                return string.Empty;
            }
        }
        public static string ObjectToString(object value, System.Globalization.CultureInfo ci)
        {
            if (value != null)
            {
                if (value is double)
                {
                    return Convert.ToDouble(value).ToString(ci);
                }
                else if (value is System.DateTime)
                {
                    return Convert.ToDateTime(value).ToString(ci);
                }
                else if (value is object[])
                {
                    object[] values = (object[])value;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (object obj in values)
                    {
                        sb.Append(ObjectToString(obj, ci));
                        if (!object.ReferenceEquals(obj, values[values.Length - 1]))
                            sb.Append(" - ");
                    }
                    return sb.ToString();
                }
                else
                {
                    return value.ToString();
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static string[][] CsvTextToStringTable(string csv, char delimiter)
        {
            string[] rows = csv.Split(new String[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            List<string[]> lst = new List<string[]>();
            foreach (string row in rows)
            {
                if (row.Trim() != string.Empty)
                    lst.Add(CsvRowToStringArray(row.Trim(), delimiter));
            }
            return lst.ToArray();
        }

        public static string[] CsvRowToStringArray(string row, char delimiter, bool withQuoteMarks = true)
        {
            if (withQuoteMarks)
            {
                List<string> lstParts = new List<string>();
                int actualIndex = 0;
                int tempStartIndex = 0;
                bool waitForNextQuoteMark = false;

                while (!(actualIndex == row.Length))
                {
                    if (row[actualIndex] == '\"')
                    {
                        waitForNextQuoteMark = !waitForNextQuoteMark;
                    }
                    else if (row[actualIndex] == delimiter)
                    {
                        if (!waitForNextQuoteMark)
                        {
                            lstParts.Add(ClearCsvString(row.Substring(tempStartIndex, actualIndex - tempStartIndex)));
                            tempStartIndex = actualIndex + 1;
                        }
                    }
                    actualIndex += 1;
                    if (actualIndex == row.Length)
                    {
                        string s = ClearCsvString(row.Substring(tempStartIndex, actualIndex - tempStartIndex));
                        lstParts.Add(s);
                    }
                }
                return lstParts.ToArray();
            }
            else
            {
                return row.Split(delimiter);
            }
        }
        private static string ClearCsvString(string csv)
        {
            if (csv.Length > 0)
            {
                string result = csv;
                if (result.StartsWith("\""))
                    result = result.Substring(1);
                if (result.EndsWith("\""))
                    result = result.Substring(0, result.Length - 1);
                return result.Replace("\"\"", "\"");
            }
            else
            {
                return csv;
            }
        }

        public static T DeserializeJson<T>(Stream stream)
        {
            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var sr = new System.IO.StreamReader(stream))
            using (var jsonTextReader = new Newtonsoft.Json.JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }

        public static double ParseToDouble(string s)
        {
            double v = 0;
            double.TryParse(s, System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out v);
            return v;
        }
        public static DateTime ParseToDateTime(string s)
        {
            DateTime d = new DateTime();
            DateTime.TryParse(s, MyHelper.ConverterCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out d);
            return d;
        }

        private static char[] TrimChars = new char[] { ' ', ',', '\n', '\r', '\t' };
        public static double? ParseDouble(string s, CultureInfo c = null)
        {
            if (c == null) c = MyHelper.ConverterCulture;
            double tmp;
            return double.TryParse(s.Trim(TrimChars), NumberStyles.Any, c, out tmp) ? tmp : new double?();
        }
        public static int? ParseInt(string s, CultureInfo c = null)
        {
            if (c == null) c= MyHelper.ConverterCulture;
            int tmp;
            return int.TryParse(s.Trim(TrimChars), NumberStyles.Any, c, out tmp) ? tmp : new int?();
        }

        public static JObject HtmlInnerTable(JObject table)
        {
            if (table["tr"] is JObject && table["tr"]["td"] is JObject && table["tr"]["td"]["table"] is JObject)
            {
                table = (JObject)table["tr"]["td"]["table"];
            }
            return table;
        }

        private static string[] ContentTags = new string[] { "content", "p", "a", "span", "b", "strong", "small", "font", "div" };


        public static string HtmlFirstContent(JToken t, bool includeDiv = false)
        {
            if (t != null)
            {
                if (t is JValue) return t.ToString().Trim(new char[] { ',', ' ', '\n', '\r', '\t' });
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
        public static string HtmlFirstLink(JToken t, bool includeDiv = false)
        {
            if (t.HasValues)
            {
                if (t["href"] != null) { return t["href"].ToString(); }
                if (t["a"] != null) { return HtmlFirstLink(t["a"], includeDiv); }
                if (t["p"] != null) { return HtmlFirstLink(t["p"], includeDiv); }
                if (t["span"] != null) { return HtmlFirstLink(t["span"], includeDiv); }
                if (t["b"] != null) { return HtmlFirstLink(t["b"], includeDiv); }
                if (t["small"] != null) { return HtmlFirstLink(t["small"], includeDiv); }
                if (t["strong"] != null) { return HtmlFirstLink(t["strong"], includeDiv); }
                if (t["font"] != null) { return HtmlFirstLink(t["font"], includeDiv); }
                if (includeDiv && t["div"] != null) { return HtmlFirstLink(t["div"], includeDiv); }
            }
            return null;
        }

    }

}

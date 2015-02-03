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
using System.IO;
using MaasOne.Net;
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

        public static string GetRestTagValue(string url, string tag)
        {
            url = Uri.UnescapeDataString(url);
            if (!url.IsNullOrWhiteSpace())
            {
                var i = url.LastIndexOf(tag + "=");
                if (i > -1)
                {
                    var iNext = url.LastIndexOf("&");
                    return url.Substring(0, iNext > -1 ? iNext : url.Length).Substring(i + tag.Length + 2);
                }
            }
            return null;
        }

    }

}

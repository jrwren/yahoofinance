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
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MaasOne.Finance.Yahoo.Data;

namespace MaasOne.Finance.Yahoo.Web
{

    public class QuotesQuery : Query<QuotesResult>, IYqlQuery
    {

        public bool UseDirectSource { get; set; }
        public bool GetDiagnostics { get; set; }

        public string[] IDs { get; set; }
        public QuoteProperty[] Properties { get; set; }

        public QuotesQuery() : this(new string[] { }, new QuoteProperty[] { }) { }
        public QuotesQuery(IID managedID) : this(managedID.ID) { }
        public QuotesQuery(string unmanagedID) : this(new string[] { unmanagedID }) { }
        public QuotesQuery(IEnumerable<IID> managedIDs) : this(YFHelper.IIDsToStrings(managedIDs)) { }
        public QuotesQuery(IEnumerable<string> unmanagedIDs)
            : this(unmanagedIDs, new QuoteProperty[] { QuoteProperty.Symbol, 
                                                       QuoteProperty.LastTradePriceOnly, 
                                                       QuoteProperty.DaysHigh, 
                                                       QuoteProperty.DaysLow, 
                                                       QuoteProperty.Volume, 
                                                       QuoteProperty.LastTradeDate, 
                                                       QuoteProperty.LastTradeTime }) { }
        public QuotesQuery(IID managedID, IEnumerable<QuoteProperty> properties) : this(managedID.ID, properties) { }
        public QuotesQuery(string unmanagedID, IEnumerable<QuoteProperty> properties) : this(new string[] { unmanagedID }, properties) { }
        public QuotesQuery(IEnumerable<IID> managedIDs, IEnumerable<QuoteProperty> properties) : this(YFHelper.IIDsToStrings(managedIDs), properties) { }
        public QuotesQuery(IEnumerable<string> unmanagedIDs, IEnumerable<QuoteProperty> properties)
        {
            this.IDs = unmanagedIDs.EnumToArray();
            this.Properties = properties.EnumToArray();
            this.UseDirectSource = false;
            this.GetDiagnostics = true;
        }


        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.IDs == null || this.IDs.Length == 0)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("IDs", "No IDs available."));
            }

            if (this.Properties == null || this.Properties.Length == 0)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("Properties", "No properties available."));
            }
        }
        protected override Uri CreateUrl()
        {
            System.Text.StringBuilder ids = new System.Text.StringBuilder();
            foreach (string s in this.IDs)
            {
                ids.Append(YFHelper.CleanYqlParam(s));
                ids.Append(',');
            }
            if (ids.Length > 0) ids.Remove(ids.Length - 1, 1);
            String csvUrl = "http://download.finance.yahoo.com/d/quotes.csv?s=" + ids.ToString() + "&f=" + this.CsvQuotePropertyTags(this.Properties) + "&e=.csv";

            if (this.UseDirectSource == true)
            {
                return new Uri(Uri.EscapeUriString(csvUrl), UriKind.RelativeOrAbsolute);
            }
            else
            {
                var clm = new System.Text.StringBuilder();
                foreach (var prp in this.Properties)
                {
                    clm.Append("_" + ((int)prp).ToString());
                    clm.Append(',');
                }
                if (clm.Length > 0) clm.Remove(clm.Length - 1, 1);
                return new Uri(YFHelper.YqlUrl("*", "csv", string.Format("url='{0}' and columns='{1}'", csvUrl, clm.ToString()), true, this.GetDiagnostics, null), UriKind.RelativeOrAbsolute);
            }
        }
        protected override QuotesResult ConvertResult(System.IO.Stream stream)
        {
            if (stream == null) return null;
            var quotesData = new List<QuotesData>();
            var str = MyHelper.StreamToString(stream);
            YqlDiagnostics diag = null;

            if (this.UseDirectSource == true)
            {
                quotesData.AddRange(ToQuotesData(str, ',', this.Properties, new System.Globalization.CultureInfo("en-US")));
            }
            else
            {
                YqlResponse res = JsonConvert.DeserializeObject<YqlResponse>(str);
                if (res != null)
                {
                    if (res.Query.Diagnostics != null)
                    {
                        diag = res.Query.Diagnostics;
                    }
                    if (res.Query.Results != null)
                    {
                        JObject jo = (JObject)res.Query.Results;

                        if (res.Query.Count > 0)
                        {
                            var rows = jo["row"];

                            JArray rowArr = null;
                            if (rows is JArray) { rowArr = (JArray)rows; }
                            else { rowArr = new JArray((JObject)rows); }

                            foreach (JObject row in rowArr)
                            {
                                var qd = new QuotesData();
                                foreach (var col in row)
                                {
                                    var cki = (QuoteProperty)Convert.ToInt32(col.Key.Substring(1));
                                    foreach (QuoteProperty prp in Enum.GetValues(typeof(QuoteProperty)))
                                    {
                                        if (cki == prp)
                                        {
                                            qd[prp] = QuoteStringToObject(col.Value.ToString(), prp, MyHelper.ConverterCulture);
                                            break;
                                        }
                                    }
                                }
                                quotesData.Add(qd);
                            }
                        }
                    }
                }
            }

            return new QuotesResult() { Items = quotesData.ToArray(), Diagnostics = diag };
        }
        public override Query<QuotesResult> Clone()
        {
            return new QuotesQuery()
            {
                IDs = (string[])this.IDs.Clone(),
                Properties = (QuoteProperty[])this.Properties.Clone(),
                UseDirectSource = this.UseDirectSource,
                GetDiagnostics = this.GetDiagnostics
            };
        }

        private readonly Dictionary<QuoteProperty, string> QuotePropertyTags = new Dictionary<QuoteProperty, string>();
        private string CsvQuotePropertyTags(QuoteProperty[] properties)
        {
            if (QuotePropertyTags.Count == 0)
            {
                QuotePropertyTags.Add(QuoteProperty.Ask, "a0");
                QuotePropertyTags.Add(QuoteProperty.AverageDailyVolume, "a2");
                QuotePropertyTags.Add(QuoteProperty.AskSize, "a5");
                QuotePropertyTags.Add(QuoteProperty.Bid, "b0");
                QuotePropertyTags.Add(QuoteProperty.AskRealtime, "b2");
                QuotePropertyTags.Add(QuoteProperty.BidRealtime, "b3");
                QuotePropertyTags.Add(QuoteProperty.BookValuePerShare, "b4");
                QuotePropertyTags.Add(QuoteProperty.BidSize, "b6");
                QuotePropertyTags.Add(QuoteProperty.Change_ChangeInPercent, "c");
                QuotePropertyTags.Add(QuoteProperty.Change, "c1");
                QuotePropertyTags.Add(QuoteProperty.Commission, "c3");
                QuotePropertyTags.Add(QuoteProperty.Currency, "c4");
                QuotePropertyTags.Add(QuoteProperty.ChangeRealtime, "c6");
                QuotePropertyTags.Add(QuoteProperty.AfterHoursChangeRealtime, "c8");
                QuotePropertyTags.Add(QuoteProperty.TrailingAnnualDividendYield, "d0");
                QuotePropertyTags.Add(QuoteProperty.LastTradeDate, "d1");
                QuotePropertyTags.Add(QuoteProperty.TradeDate, "d2");
                QuotePropertyTags.Add(QuoteProperty.DilutedEPS, "e0");
                QuotePropertyTags.Add(QuoteProperty.EPSEstimateCurrentYear, "e7");
                QuotePropertyTags.Add(QuoteProperty.EPSEstimateNextYear, "e8");
                QuotePropertyTags.Add(QuoteProperty.EPSEstimateNextQuarter, "e9");
                QuotePropertyTags.Add(QuoteProperty.TradeLinksAdditional, "f0");
                QuotePropertyTags.Add(QuoteProperty.SharesFloat, "f6");
                QuotePropertyTags.Add(QuoteProperty.DaysLow, "g0");
                QuotePropertyTags.Add(QuoteProperty.HoldingsGainPercent, "g1");
                QuotePropertyTags.Add(QuoteProperty.AnnualizedGain, "g3");
                QuotePropertyTags.Add(QuoteProperty.HoldingsGain, "g4");
                QuotePropertyTags.Add(QuoteProperty.HoldingsGainPercentRealtime, "g5");
                QuotePropertyTags.Add(QuoteProperty.HoldingsGainRealtime, "g6");
                QuotePropertyTags.Add(QuoteProperty.DaysHigh, "h0");
                QuotePropertyTags.Add(QuoteProperty.MoreInfo, "i");
                QuotePropertyTags.Add(QuoteProperty.OrderBookRealtime, "i5");
                QuotePropertyTags.Add(QuoteProperty.YearLow, "j0");
                QuotePropertyTags.Add(QuoteProperty.MarketCapitalization, "j1");
                QuotePropertyTags.Add(QuoteProperty.SharesOutstanding, "j2");
                QuotePropertyTags.Add(QuoteProperty.MarketCapRealtime, "j3");
                QuotePropertyTags.Add(QuoteProperty.EBITDA, "j4");
                QuotePropertyTags.Add(QuoteProperty.ChangeFromYearLow, "j5");
                QuotePropertyTags.Add(QuoteProperty.PercentChangeFromYearLow, "j6");
                QuotePropertyTags.Add(QuoteProperty.YearHigh, "k0");
                QuotePropertyTags.Add(QuoteProperty.LastTradeRealtimeWithTime, "k1");
                QuotePropertyTags.Add(QuoteProperty.ChangeInPercentRealtime, "k2");
                QuotePropertyTags.Add(QuoteProperty.LastTradeSize, "k3");
                QuotePropertyTags.Add(QuoteProperty.ChangeFromYearHigh, "k4");
                QuotePropertyTags.Add(QuoteProperty.ChangeInPercentFromYearHigh, "k5");
                QuotePropertyTags.Add(QuoteProperty.LastTradeWithTime, "l0");
                QuotePropertyTags.Add(QuoteProperty.LastTradePriceOnly, "l1");
                QuotePropertyTags.Add(QuoteProperty.HighLimit, "l2");
                QuotePropertyTags.Add(QuoteProperty.LowLimit, "l3");
                QuotePropertyTags.Add(QuoteProperty.DaysRange, "m");
                QuotePropertyTags.Add(QuoteProperty.DaysRangeRealtime, "m2");
                QuotePropertyTags.Add(QuoteProperty.FiftydayMovingAverage, "m3");
                QuotePropertyTags.Add(QuoteProperty.TwoHundreddayMovingAverage, "m4");
                QuotePropertyTags.Add(QuoteProperty.ChangeFromTwoHundreddayMovingAverage, "m5");
                QuotePropertyTags.Add(QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage, "m6");
                QuotePropertyTags.Add(QuoteProperty.ChangeFromFiftydayMovingAverage, "m7");
                QuotePropertyTags.Add(QuoteProperty.PercentChangeFromFiftydayMovingAverage, "m8");
                QuotePropertyTags.Add(QuoteProperty.Name, "n0");
                QuotePropertyTags.Add(QuoteProperty.Notes, "n4");
                QuotePropertyTags.Add(QuoteProperty.Open, "o0");
                QuotePropertyTags.Add(QuoteProperty.PreviousClose, "p0");
                QuotePropertyTags.Add(QuoteProperty.PricePaid, "p1");
                QuotePropertyTags.Add(QuoteProperty.ChangeInPercent, "p2");
                QuotePropertyTags.Add(QuoteProperty.PriceSales, "p5");
                QuotePropertyTags.Add(QuoteProperty.PriceBook, "p6");
                QuotePropertyTags.Add(QuoteProperty.ExDividendDate, "q0");
                QuotePropertyTags.Add(QuoteProperty.PERatio, "r0");
                QuotePropertyTags.Add(QuoteProperty.DividendPayDate, "r1");
                QuotePropertyTags.Add(QuoteProperty.PERatioRealtime, "r2");
                QuotePropertyTags.Add(QuoteProperty.PEGRatio, "r5");
                QuotePropertyTags.Add(QuoteProperty.PriceEPSEstimateCurrentYear, "r6");
                QuotePropertyTags.Add(QuoteProperty.PriceEPSEstimateNextYear, "r7");
                QuotePropertyTags.Add(QuoteProperty.Symbol, "s0");
                QuotePropertyTags.Add(QuoteProperty.SharesOwned, "s1");
                QuotePropertyTags.Add(QuoteProperty.Revenue, "s6");
                QuotePropertyTags.Add(QuoteProperty.ShortRatio, "s7");
                QuotePropertyTags.Add(QuoteProperty.LastTradeTime, "t1");
                QuotePropertyTags.Add(QuoteProperty.TradeLinks, "t6");
                QuotePropertyTags.Add(QuoteProperty.TickerTrend, "t7");
                QuotePropertyTags.Add(QuoteProperty.OneyrTargetPrice, "t8");
                QuotePropertyTags.Add(QuoteProperty.Volume, "v0");
                QuotePropertyTags.Add(QuoteProperty.HoldingsValue, "v1");
                QuotePropertyTags.Add(QuoteProperty.HoldingsValueRealtime, "v7");
                QuotePropertyTags.Add(QuoteProperty.YearRange, "w0");
                QuotePropertyTags.Add(QuoteProperty.DaysValueChange, "w1");
                QuotePropertyTags.Add(QuoteProperty.DaysValueChangeRealtime, "w4");
                QuotePropertyTags.Add(QuoteProperty.StockExchange, "x0");
                QuotePropertyTags.Add(QuoteProperty.TrailingAnnualDividendYieldInPercent, "y0");
            }


            System.Text.StringBuilder symbols = new System.Text.StringBuilder();
            if (properties != null && properties.Length > 0)
            {
                foreach (QuoteProperty qp in properties)
                {
                    symbols.Append(QuotePropertyTags[qp]);
                }
            }
            return symbols.ToString();
        }

        private QuoteProperty[] CheckPropertiesOfQuotesData(IEnumerable<QuotesData> quotes, IEnumerable<QuoteProperty> properties)
        {
            List<QuoteProperty> lstProperties = new List<QuoteProperty>();
            if (properties == null)
            {
                return GetAllActiveProperties(quotes);
            }
            else
            {
                lstProperties.AddRange(properties);
                if (lstProperties.Count == 0)
                {
                    return GetAllActiveProperties(quotes);
                }
                else
                {
                    return lstProperties.ToArray();
                }
            }
        }
        private QuoteProperty[] GetAllActiveProperties(IEnumerable<QuotesData> quotes)
        {
            List<QuoteProperty> lst = new List<QuoteProperty>();
            if (quotes != null)
            {
                foreach (QuoteProperty qp in Enum.GetValues(typeof(QuoteProperty)))
                {
                    bool valueIsNotNull = false;
                    foreach (QuotesData quote in quotes)
                    {
                        if (quote[qp] != null)
                        {
                            valueIsNotNull = true;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                    if (valueIsNotNull)
                        lst.Add(qp);
                }
            }
            return lst.ToArray();
        }




        private QuoteProperty[] mAlternateQuoteProperties = new QuoteProperty[] {
		QuoteProperty.LastTradeSize,
		QuoteProperty.BidSize,
		QuoteProperty.AskSize

	};
        /// <summary>
        /// Converts a list of quote data to a CSV formatted text
        /// </summary>
        /// <param name="quotes">The list of quote values</param>
        /// <param name="delimiter">The delimiter of the CSV text</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted data string in CSV format</returns>
        /// <remarks></remarks>
        public string FromQuotesData(IEnumerable<QuotesData> quotes, char delimiter, System.Globalization.CultureInfo culture = null)
        {
            return FromQuotesData(quotes, delimiter, null, culture);
        }
        /// <summary>
        /// Converts a list of quote data to a CSV formatted text
        /// </summary>
        /// <param name="quotes">The list of quote values</param>
        /// <param name="delimiter">The delimiter of the CSV text</param>
        /// <param name="properties">The used properties of the items</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted data string in CSV format</returns>
        /// <remarks></remarks>
        private string FromQuotesData(IEnumerable<QuotesData> quotes, char delimiter, IEnumerable<QuoteProperty> properties, System.Globalization.CultureInfo culture = null)
        {
            if (quotes != null)
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                if (culture != null)
                    ci = culture;

                QuoteProperty[] prpts = CheckPropertiesOfQuotesData(quotes, properties);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                foreach (QuoteProperty qp in prpts)
                {
                    sb.Append(qp.ToString());
                    sb.Append(delimiter);
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();

                foreach (QuotesData q in quotes)
                {
                    if (q != null)
                    {
                        System.Text.StringBuilder sbQ = new System.Text.StringBuilder();
                        foreach (QuoteProperty qp in prpts)
                        {
                            object o = MyHelper.ObjectToString(q[qp], ci);
                            if (o is string)
                            {
                                if (o.ToString() == string.Empty)
                                {
                                    sbQ.Append("\"N/A\"");
                                }
                                else
                                {
                                    sbQ.Append("\"");
                                    sbQ.Append(q[qp].ToString().Replace("\"", "\"\""));
                                    sbQ.Append("\"");
                                }
                            }
                            else
                            {
                                sbQ.Append(MyHelper.ObjectToString(q[qp], ci));
                            }
                            sbQ.Append(delimiter);
                        }
                        if (sbQ.Length > 0)
                            sbQ.Remove(sbQ.Length - 1, 1);
                        sb.AppendLine(sbQ.ToString());
                    }
                }
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Tries to read a list of quote data from a CSV formatted text (incl. Header)
        /// </summary>
        /// <param name="csvText">The CSV formatted text</param>
        /// <param name="delimiter">The delimiter of the CSV text</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted quote values or Nothing</returns>
        /// <remarks></remarks>
        private QuotesData[] ToQuotesData(string csvText, char delimiter, System.Globalization.CultureInfo culture = null)
        {
            List<QuoteProperty> properties = new List<QuoteProperty>();
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
            if (culture != null)
                ci = culture;

            if (csvText != string.Empty)
            {
                string[] rows = csvText.Split(new string[] {
				"\r",
				"\n"
			}, StringSplitOptions.RemoveEmptyEntries);
                string[] headerParts = MyHelper.CsvRowToStringArray(rows[0], delimiter);
                foreach (string part in headerParts)
                {
                    foreach (QuoteProperty qp in Enum.GetValues(typeof(QuoteProperty)))
                    {
                        if (qp.ToString() == part.Trim())
                        {
                            properties.Add(qp);
                            break;
                        }
                    }
                }
                if (properties.Count != headerParts.Length)
                    return null;
            }

            return ToQuotesData(csvText, delimiter, properties.ToArray(), ci, true);
        }
        private QuotesData[] ToQuotesData(string csvText, char delimiter, QuoteProperty[] properties, System.Globalization.CultureInfo culture, bool hasHeader = false)
        {
            List<QuotesData> quotes = new List<QuotesData>();
            if (csvText != string.Empty & culture != null & (properties != null && properties.Length > 0))
            {
                if (properties.Length > 0)
                {
                    string[][] table = MyHelper.CsvTextToStringTable(csvText, delimiter);
                    int start = 0;
                    if (hasHeader)
                        start = 1;

                    if (table.Length > 0)
                    {
                        if (!(table[0].Length == properties.Length))
                        {
                            String[][] semicolTable = MyHelper.CsvTextToStringTable(csvText, Convert.ToChar((delimiter == ';' ? ',' : ';')));
                            if (semicolTable[0].Length == properties.Length)
                            {
                                table = semicolTable;
                            }
                        }
                        if (table.Length > 0 && table.Length > start)
                        {
                            for (int i = start; i <= table.Length - 1; i++)
                            {
                                QuotesData qd = CsvRowToQuotesData(table[i], properties, culture);
                                if (qd != null)
                                    quotes.Add(qd);
                            }
                        }
                    }
                }
            }
            return quotes.ToArray();
        }

        private QuotesData CsvRowToQuotesData(string[] rowItems, QuoteProperty[] properties, System.Globalization.CultureInfo culture)
        {
            if (rowItems.Length > 0)
            {
                QuotesData quote = null;
                if (rowItems.Length == properties.Length)
                {
                    quote = new QuotesData();
                    for (int i = 0; i <= properties.Length - 1; i++)
                    {
                        quote[properties[i]] = QuoteStringToObject(rowItems[i], properties[i], culture);
                    }
                }
                else
                {

                    if (rowItems.Length > 1)
                    {
                        List<QuoteProperty> alternateProperties = new List<QuoteProperty>();
                        foreach (QuoteProperty qp in properties)
                        {
                            foreach (QuoteProperty q in mAlternateQuoteProperties)
                            {
                                if (qp == q)
                                {
                                    alternateProperties.Add(qp);
                                    break;
                                }
                            }
                        }


                        if (alternateProperties.Count > 0)
                        {
                            List<KeyValuePair<QuoteProperty, int>[]> lst = new List<KeyValuePair<QuoteProperty, int>[]>();
                            int[][] permutations = MaxThreePerm(alternateProperties.Count, Math.Min(rowItems.Length - properties.Length + 1, 3));
                            foreach (int[] perm in permutations)
                            {
                                List<KeyValuePair<QuoteProperty, int>> lst2 = new List<KeyValuePair<QuoteProperty, int>>();
                                for (int i = 0; i <= alternateProperties.Count - 1; i++)
                                {
                                    lst2.Add(new KeyValuePair<QuoteProperty, int>(alternateProperties[i], perm[i]));
                                }
                                lst.Add(lst2.ToArray());
                            }

                            foreach (KeyValuePair<QuoteProperty, int>[] combination in lst)
                            {
                                String[] newRowItems = CsvNewRowItems(rowItems, properties, combination);

                                try
                                {
                                    if (newRowItems.Length > 0)
                                    {
                                        quote = new QuotesData();
                                        for (int i = 0; i <= properties.Length - 1; i++)
                                        {
                                            quote[properties[i]] = QuoteStringToObject(rowItems[i], properties[i], culture);
                                        }
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                }
                            }

                        }
                    }
                }
                return quote;
            }
            else
            {
                return null;
            }
        }

        private string[] CsvNewRowItems(string[] oldItems, QuoteProperty[] properties, KeyValuePair<QuoteProperty, int>[] multipleItemProperties)
        {
            System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
            List<string> newRowItems = new List<string>();
            int itemsCount = properties.Length;
            foreach (KeyValuePair<QuoteProperty, int> q in multipleItemProperties)
            {
                itemsCount += q.Value - 1;
            }
            if (itemsCount == oldItems.Length)
            {
                int actualIndex = 0;
                foreach (QuoteProperty qp in properties)
                {
                    Nullable<KeyValuePair<QuoteProperty, int>> alternatProperty = null;
                    foreach (KeyValuePair<QuoteProperty, int> q in multipleItemProperties)
                    {
                        if (q.Key == qp)
                        {
                            alternatProperty = q;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                    if (!alternatProperty.HasValue)
                    {
                        newRowItems.Add(oldItems[actualIndex]);
                    }
                    else
                    {
                        string newRowItem = string.Empty;

                        for (int i = actualIndex; i <= (actualIndex + alternatProperty.Value.Value - 1); i++)
                        {
                            int @int = 0;
                            if (int.TryParse(oldItems[i], System.Globalization.NumberStyles.Integer, convCulture, out @int) && (oldItems[i] == @int.ToString() || oldItems[i] == "000"))
                            {
                                newRowItem += oldItems[i];
                            }
                            else
                            {
                                newRowItem = string.Empty;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                        if (newRowItem != string.Empty)
                        {
                            newRowItems.Add(newRowItem);
                            actualIndex += alternatProperty.Value.Value - 1;
                        }
                    }
                    actualIndex += 1;
                }
            }
            return newRowItems.ToArray();
        }

        private object QuoteStringToObject(string value, QuoteProperty prp, System.Globalization.CultureInfo culture)
        {
            object o = MyHelper.StringToObject(value, culture);
            if (prp == QuoteProperty.Name && o is Array && ((Array)o).Length > 0)
            {
                Array arr = (Array)o;
                string s = "";
                for (int i = 0; i < arr.Length; i++)
                {
                    s += arr.GetValue(i).ToString();
                    if (i != arr.Length - 1)
                    {
                        s += " - ";
                    }
                }
                return s;
            }
            else
            {
                return o;
            }
        }
        private int[][] MaxThreePerm(int propertyCount, int maxCount)
        {
            List<int[]> lst = new List<int[]>();
            for (int i = 1; i <= maxCount; i++)
            {
                if (propertyCount > 1)
                {
                    for (int n = 1; n <= maxCount; n++)
                    {
                        if (propertyCount > 2)
                        { for (int m = 1; m <= maxCount; m++) { lst.Add(new int[] { i, n, m }); } }
                        else
                        { lst.Add(new int[] { i, n }); }
                    }
                }
                else
                { lst.Add(new int[] { i }); }
            }
            return lst.ToArray();
        }


    }



    /// <summary>
    /// Stores the result data
    /// </summary>
    public class QuotesResult : IYqlResult
    {
        public YqlDiagnostics Diagnostics { get; internal set; }
        public QuotesData[] Items { get; internal set; }
        internal QuotesResult() { }
    }


}

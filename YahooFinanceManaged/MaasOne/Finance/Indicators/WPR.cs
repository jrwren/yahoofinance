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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;


namespace MaasOne.Finance.Indicators
{



    public class WPR : IHighLowValueIndicator
    {

        public string Name
        {
            get { return "Williams Percent Range"; }
        }

        public bool IsRealative
        {
            get { return true; }
        }

        public double ScaleMaximum
        {
            get { return 100; }
        }

        public double ScaleMinimum
        {
            get { return 0; }
        }

        public int Period { get; set; }


        /// <summary>
        /// Calculate values of Williams Percent Range for historic quote values.
        /// </summary>
        /// <param name="values">An unsorted IEnumerable of HistQuoteData.</param>
        /// <returns>The sorted dictionaries. 1) WPR values; 2) Quote values (Close)</returns>
        /// <remarks></remarks>
        public System.Collections.Generic.Dictionary<System.DateTime, double>[] Calculate(System.Collections.Generic.IEnumerable<HighLowValue> values)
        {
            Dictionary<System.DateTime, double> wprResult = new Dictionary<System.DateTime, double>();

            List<HighLowValue> quoteValues = new List<HighLowValue>(values);
            quoteValues.Sort(new HistQuotesSorter());

            wprResult.Add(quoteValues[0].Date, 50);
            for (int i = 1; i <= quoteValues.Count - 1; i++)
            {
                int periodlength = Math.Min(i, this.Period + 1);

                double ll = quoteValues[i - periodlength].Range.Minimum;
                double hh = quoteValues[i - periodlength].Range.Maximum;

                for (int s = i - periodlength; s <= i; s++)
                {
                    if (quoteValues[s].Range.Minimum < ll)
                        ll = quoteValues[s].Range.Minimum;
                    if (quoteValues[s].Range.Maximum > hh)
                        hh = quoteValues[s].Range.Maximum;
                }
                wprResult.Add(quoteValues[i].Date, ((hh - quoteValues[i].Value) / (hh - ll)) * 100);
            }
            return new Dictionary<System.DateTime, double>[] { wprResult, QuotesConverter.ConvertHistQuotesToSingleValues(quoteValues) };
        }

        public override string ToString()
        {
            return this.Name + " " + this.Period;
        }

    }


}

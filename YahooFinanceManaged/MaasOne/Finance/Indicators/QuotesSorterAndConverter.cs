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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;


namespace MaasOne.Finance.Indicators
{


    internal class QuotesSorter : IComparer<KeyValuePair<System.DateTime, double>>
    {

        public int Compare(KeyValuePair<System.DateTime, double> x, KeyValuePair<System.DateTime, double> y)
        {
            if (x.Key > y.Key)
            {
                return 1;
            }
            else if (x.Key < y.Key)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }


    internal class HistQuotesSorter : IComparer<HighLowValue>
    {

        public int Compare(HighLowValue x, HighLowValue y)
        {
            if (x.Date > y.Date)
            {
                return 1;
            }
            else if (x.Date < y.Date)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }



    public abstract class QuotesConverter
    {

        public static Dictionary<System.DateTime, double> ConvertHistQuotesToSingleValues(IEnumerable<HighLowValue> quotes)
        {
            Dictionary<System.DateTime, double> dict = new Dictionary<System.DateTime, double>();
            foreach (HighLowValue hq in quotes)
            {
                dict.Add(hq.Date, hq.Value);
            }
            return dict;
        }

        private QuotesConverter() { }
    }


}

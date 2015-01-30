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


    public class Momentum : ISingleValueIndicator
    {


        public string Name { get { return "Momentum"; } }
        public bool IsRealative { get { return true; } }
        public double ScaleMaximum { get { return double.PositiveInfinity; } }
        public double ScaleMinimum { get { return 0; } }
        public int Period { get; set; }

        public Momentum()
        {
            this.Period = 14;
        }

        public Dictionary<System.DateTime, double>[] Calculate(IEnumerable<KeyValuePair<System.DateTime, double>> values)
        {
            Dictionary<System.DateTime, double> momResult = new Dictionary<System.DateTime, double>();

            List<KeyValuePair<System.DateTime, double>> quoteValues = new List<KeyValuePair<System.DateTime, double>>(values);
            quoteValues.Sort(new QuotesSorter());

            if (quoteValues.Count > 0)
            {
                for (int i = 0; i <= quoteValues.Count - 1; i++)
                {
                    if (i >= this.Period)
                    {
                        momResult.Add(quoteValues[i].Key, (quoteValues[i].Value / quoteValues[i - this.Period].Value) * 100);
                    }
                    else
                    {
                        momResult.Add(quoteValues[i].Key, (quoteValues[i].Value / quoteValues[0].Value) * 100);
                    }
                }
            }

            return new Dictionary<System.DateTime, double>[] { momResult };
        }

        public override string ToString()
        {
            return this.Name + " " + this.Period;
        }
    }

}

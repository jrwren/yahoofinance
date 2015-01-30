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

    public interface IIndicator
    {
        string Name { get; }
        double ScaleMinimum { get; }
        double ScaleMaximum { get; }
        int Period { get; set; }
        bool IsRealative { get; }
    }

    public interface ISingleValueIndicator : IIndicator
    {
        Dictionary<System.DateTime, double>[] Calculate(IEnumerable<KeyValuePair<System.DateTime, double>> values);
    }

    public interface IHighLowValueIndicator : IIndicator
    {
        Dictionary<System.DateTime, double>[] Calculate(IEnumerable<HighLowValue> values);
    }


    /// <summary>
    /// Stores informations about one historic trading period (day, week or month).
    /// </summary>
    /// <remarks></remarks>
    public class HighLowValue
    {

        public System.DateTime Date { get; set; }
        public Range<double> Range { get; set; }
        public double Value { get; set; }

    }

}

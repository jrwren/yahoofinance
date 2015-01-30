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

	/// <summary>
	/// Bollinger Bands Indicator
	/// </summary>
	/// <remarks></remarks>
	public class BB : StD
	{

		public override string Name {
			get { return "Bollinger Bands"; }
		}

		public override bool IsRealative {
			get { return false; }
		}

		public override Dictionary<System.DateTime, double>[] Calculate(IEnumerable<KeyValuePair<System.DateTime, double>> values)
		{
			Dictionary<System.DateTime, double> bbResultUpper = new Dictionary<System.DateTime, double>();
			Dictionary<System.DateTime, double> bbResultLower = new Dictionary<System.DateTime, double>();

			Dictionary<DateTime, double>[] baseResults = base.Calculate(values);
			Dictionary<System.DateTime, double> stdResult = baseResults[0];
			Dictionary<System.DateTime, double> maResult = baseResults[1];
			List<KeyValuePair<System.DateTime, double>> histQuotes = new List<KeyValuePair<System.DateTime, double>>(baseResults[2]);

			if (histQuotes.Count > 0) {
				foreach (KeyValuePair<DateTime, double> hq in histQuotes) {
					bbResultUpper.Add(hq.Key, maResult[hq.Key] + stdResult[hq.Key]);
					bbResultLower.Add(hq.Key, maResult[hq.Key] - stdResult[hq.Key]);
				}
			}

			return new Dictionary<System.DateTime, double>[] {
				bbResultUpper,
				bbResultLower,
				stdResult,
				maResult,
				baseResults[2]
			};
		}


		public override string ToString()
		{
			return this.Name + " " + this.Period;
		}
	}

}

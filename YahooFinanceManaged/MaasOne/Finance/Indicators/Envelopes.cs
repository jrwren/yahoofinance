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
	/// Envelopes Indicator
	/// </summary>
	/// <remarks></remarks>
	public class Env : MA
	{

		public override string Name {
			get { return "Envelopes"; }
		}

		private bool mIsBufferFixed = false;
		public bool IsBufferFixed {
			get { return mIsBufferFixed; }
			set { mIsBufferFixed = value; }
		}
		private double mPercentBuffer = 0.05;
		public double PercentBuffer {
			get { return mPercentBuffer; }
			set { mPercentBuffer = value; }
		}
		private double mFixedBuffer = 1;
		public double FixedBuffer {
			get { return mFixedBuffer; }
			set { mFixedBuffer = value; }
		}


		public override Dictionary<System.DateTime, double>[] Calculate(IEnumerable<KeyValuePair<System.DateTime, double>> values)
		{
			Dictionary<System.DateTime, double> envResultUpper = new Dictionary<System.DateTime, double>();
			Dictionary<System.DateTime, double> envResultLower = new Dictionary<System.DateTime, double>();

			Dictionary<DateTime, double>[] baseResults = base.Calculate(values);
			Dictionary<System.DateTime, double> maResult = baseResults[0];
			List<KeyValuePair<System.DateTime, double>> histQuotes = new List<KeyValuePair<System.DateTime, double>>(baseResults[1]);

			if (this.IsBufferFixed) {
				foreach (KeyValuePair<DateTime, double> maRes in maResult) {
					envResultUpper.Add(maRes.Key, maRes.Value + this.FixedBuffer);
					envResultLower.Add(maRes.Key, maRes.Value - this.FixedBuffer);
				}
			} else {
				foreach (KeyValuePair<DateTime, double> maRes in maResult) {
					envResultUpper.Add(maRes.Key, maRes.Value * (1 + this.PercentBuffer));
					envResultLower.Add(maRes.Key, maRes.Value * (1 - this.PercentBuffer));
				}
			}

			return new Dictionary<System.DateTime, double>[] {
				envResultUpper,
				envResultLower,
				maResult,
				baseResults[1]
			};
		}

		public override string ToString()
		{
			return this.Name + " " + this.Period;
		}

	}


}

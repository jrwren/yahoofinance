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


	public class MACD : EMA
	{

		public override string Name {
			get { return "Moving Average Convergence/Divergence"; }
		}

		public override bool IsRealative {
			get { return true; }
		}

		public int PeriodFast { get; set; }
		public int PeriodSlow { get; set; }


		public MACD()
		{
			base.Period = 9;
			this.PeriodFast = 12;
			this.PeriodSlow = 26;
		}


		public override System.Collections.Generic.Dictionary<System.DateTime, double>[] Calculate(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.DateTime, double>> values)
		{
			int basePeriod = base.Period;
			Dictionary<System.DateTime, double> macdValues = new Dictionary<System.DateTime, double>();
			//Dim signalLineValues As New Dictionary(Of Date, Double)

			base.Period = this.PeriodFast;
			Dictionary<DateTime, double> ema12values = base.Calculate(values)[0];

			base.Period = this.PeriodSlow;
			Dictionary<DateTime, double> ema26values = base.Calculate(values)[0];

			List<KeyValuePair<System.DateTime, double>> closeValues = new List<KeyValuePair<System.DateTime, double>>(values);

			System.DateTime d = default(System.DateTime);
			for (int i = 0; i <= closeValues.Count - 1; i++) {
				d = closeValues[i].Key;
				macdValues.Add(d, ema12values[d] - ema26values[d]);
			}

			base.Period = basePeriod;
			Dictionary<DateTime, double> ema9values = base.Calculate(macdValues)[0];

			return new Dictionary<System.DateTime, double>[] {
				macdValues,
				ema9values
			};
		}

		public override string ToString()
		{
			return this.Name + " " + this.Period;
		}

	}

}

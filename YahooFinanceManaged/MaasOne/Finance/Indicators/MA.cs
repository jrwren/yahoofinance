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
	/// Moving Average Indicator
	/// </summary>
	/// <remarks></remarks>
	public class MA : ISingleValueIndicator
	{

		public virtual string Name {
			get { return "Moving Average"; }
		}

		public virtual bool IsRealative {
			get { return false; }
		}

		public double ScaleMaximum {
			get { return double.PositiveInfinity; }
		}

		public double ScaleMinimum {
			get { return double.NegativeInfinity; }
		}


		public int Period { get; set; }

		public MA() {
			this.Period = 12;
		}

		/// <summary>
		/// Calculate values of Moving Average for historic quote values.
		/// </summary>
		/// <param name="values">An unsorted IEnumerable of HistQuoteData.</param>
		/// <returns>The sorted dictionaries. 1) MA values; 2) Quote values.</returns>
		/// <remarks></remarks>
		public virtual Dictionary<System.DateTime, double>[] Calculate(IEnumerable<KeyValuePair<System.DateTime, double>> values)
		{
			Dictionary<System.DateTime, double> maResult = new Dictionary<System.DateTime, double>();

			List<KeyValuePair<System.DateTime, double>> quoteValues = new List<KeyValuePair<System.DateTime, double>>(values);
			quoteValues.Sort(new QuotesSorter());

			Dictionary<System.DateTime, double> valDict = new Dictionary<System.DateTime, double>();
			foreach (KeyValuePair<System.DateTime, double> item in quoteValues) {
				valDict.Add(item.Key, item.Value);
			}

			if (quoteValues.Count > 0) {
				double ave = 0;
				for (int i = 0; i <= quoteValues.Count - 1; i++) {
					ave = 0;
					if (i + 1 - this.Period >= 0) {
						for (int n = i + 1 - this.Period; n <= i; n++) {
							ave += quoteValues[n].Value;
						}
						ave = ave / this.Period;
					} else {
						for (int n = 0; n <= i; n++) {
							ave += quoteValues[n].Value;
						}
						ave = ave / (i + 1);
					}
					maResult.Add(quoteValues[i].Key, ave);
				}
			}

			return new Dictionary<System.DateTime, double>[] {
				maResult,
				valDict
			};
		}

		public override string ToString()
		{
			return this.Name + " " + this.Period;
		}

	}




}

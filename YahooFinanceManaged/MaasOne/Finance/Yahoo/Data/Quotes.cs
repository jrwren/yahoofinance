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
using System.Collections.Generic;

namespace MaasOne.Finance.Yahoo.Data
{


    /// <summary>
    /// Stores informations of different quote values. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class QuotesData : IQuotePrice
    {

        private object[] mValues = new object[88];

        /// <summary>
        /// Gets or sets the value of a specfic property
        /// </summary>
        /// <param name="prp">Gets or sets the property you want to get or set</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlIgnore]
        public object this[QuoteProperty prp]
        {
            get { return mValues[(int)prp]; }
            set { mValues[(int)prp] = value; }
        }


        /// <summary>
        /// The ID of the QuotesData
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID
        {
            get { return this.Values<string>(QuoteProperty.Symbol); }
            set { mValues[(int)QuoteProperty.Symbol] = value; }
        }
        /// <summary>
        /// Gets or sets the name of the QuotesData
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Represents the value of QuoteProperty.Name</remarks>
        public string Name
        {
            get { return this.Values<string>(QuoteProperty.Name); }
            set { mValues[(int)QuoteProperty.Name] = value; }
        }
        /// <summary>
        /// The price value of the QuoteBaseData
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double LastTradePriceOnly
        {
            get { return this.Values<double>(QuoteProperty.LastTradePriceOnly); }
            set { mValues[(int)QuoteProperty.LastTradePriceOnly] = value; }
        }
        /// <summary>
        /// The change of the price in relation to close value of the previous day
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Change
        {
            get { return this.Values<double>(QuoteProperty.Change); }
            set { mValues[(int)QuoteProperty.Change] = value; }
        }
        /// <summary>
        /// The trade volume of the day
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long Volume
        {
            get { return this.Values<long>(QuoteProperty.Volume); }
            set { mValues[(int)QuoteProperty.Volume] = value; }
        }
        /// <summary>
        /// The date value of the data
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime LastTradeDate
        {
            get { return this.Values<DateTime>(QuoteProperty.LastTradeDate); }
            set { mValues[(int)QuoteProperty.LastTradeDate] = value; }
        }
        /// <summary>
        /// The time value of the data
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime LastTradeTime
        {
            get { return this.Values<DateTime>(QuoteProperty.LastTradeTime); }
            set { mValues[(int)QuoteProperty.LastTradeTime] = value; }
        }
        /// <summary>
        /// Gets or sets the opening price value of the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Open
        {
            get { return this.Values<double>(QuoteProperty.Open); }
            set { mValues[(int)QuoteProperty.Open] = value; }
        }
        /// <summary>
        /// Gets or sets the highest value of the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double DaysHigh
        {
            get { return this.Values<double>(QuoteProperty.DaysHigh); }
            set { mValues[(int)QuoteProperty.DaysHigh] = value; }
        }
        /// <summary>
        /// Gets or sets the lowest price value of the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double DaysLow
        {
            get { return this.Values<double>(QuoteProperty.DaysLow); }
            set { mValues[(int)QuoteProperty.DaysLow] = value; }
        }
        /// <summary>
        /// Currency
        /// </summary>
        public string Currency
        {
            get { return this.Values<string>(QuoteProperty.Currency); }
            set { this[QuoteProperty.Currency] = value; }
        }



        /// <summary>
        /// The calculated close price of the last trading day
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>[LastTradePriceOnly] - [Change]</remarks>
        public double PreviewClose { get { return this.LastTradePriceOnly - this.Change; } }
        /// <summary>
        /// The calculated price change in percent
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>[Change] / [PreviewClose]</remarks>
        public double ChangeInPercent
        {
            get
            {
                if (this.PreviewClose != 0)
                { return (this.Change / this.PreviewClose) * 100; }
                else
                { return 0; }
            }
        }

        public QuotesData() { }
        public QuotesData(string id) { this.ID = id; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prp"></param>
        /// <returns></returns>
        public object Values(QuoteProperty prp) { return mValues[(int)prp]; }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prp"></param>
        /// <returns></returns>
        public T Values<T>(QuoteProperty prp)
        {
            object val = mValues[(int)prp];
            if (val != null && val is T) return (T)val;
            return default(T);
        }

        public QuotesData Clone()
        {
            QuotesData cln = new QuotesData();
            foreach (QuoteProperty qp in Enum.GetValues(typeof(QuoteProperty)))
            {
                if (this[qp] is object[])
                {
                    object[] obj = (object[])this[qp];
                    object[] newObj = new object[obj.Length];
                    if (obj.Length > 0)
                    {
                        for (int i = 0; i <= obj.Length - 1; i++)
                        {
                            newObj[i] = obj[i];
                        }
                    }
                    cln[qp] = newObj;
                }
                else
                {
                    cln[qp] = this[qp];
                }
            }
            return cln;
        }

        public bool HasSameID(IID iid) { return iid != null && this.ID.Equals(iid.ID); }
        public void UpdateLastTradePriceOnly(IQuotePrice actualQuote)
        {
            if (actualQuote == null) throw new ArgumentNullException("quote", "The quote is null.");
            if (this.HasSameID(actualQuote) == false) throw new ArgumentNullException("quote.ID", "The ID is different.");
            this[QuoteProperty.LastTradePriceOnly] = actualQuote.LastTradePriceOnly;
        }

    }
}

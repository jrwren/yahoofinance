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

namespace MaasOne.YahooFinance.Data
{

    public class QuotesBaseData : IQuotePrice
    {

        /// <summary>
        /// Gets or sets the ID property value
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
        public virtual string ID { get; set; }
        /// <summary>
        /// Gets or sets the Title property value
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
        public virtual string Name { get; set; }
        /// <summary>
        /// Gets or sets the LastTradePriceOnly property value
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
        public virtual double? LastTradePriceOnly { get; set; }
        /// <summary>
        /// Gets or sets the Change property value
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
        public virtual double? Change { get; set; }
        /// <summary>
        /// Gets or sets the ChangeInPercent property value
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
        public virtual double? ChangeInPercent { get; set; }

        public void UpdateLastTradePriceOnly(IQuotePrice update)
        {
            if (update == null) throw new ArgumentNullException("quote", "The quote is NULL.");
            if (this.ID.Equals(update.ID) == false) throw new ArgumentNullException("quote.ID", "The ID is different.");
            this.LastTradePriceOnly = update.LastTradePriceOnly;
        }

    }




    /// <summary>
    /// Stores informations of different quote values. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class QuotesData : QuotesBaseData
    {

        public Dictionary<QuoteProperty, object> Values { get; private set; }

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
            get
            {
                if (this.Values.ContainsKey(prp)) return this.Values[prp];
                return null;
            }
            set
            {
                if (this.Values.ContainsKey(prp))
                {
                    if (value == null) { this.Values.Remove(prp); }
                    else { this.Values[prp] = value; }
                }
                else if (value != null) { this.Values.Add(prp, value); }
            }
        }



        public override string ID
        {
            get { return (string)this[QuoteProperty.Symbol]; }
            set { this[QuoteProperty.Symbol] = value; }
        }
        public override string Name
        {
            get { return (string)this[QuoteProperty.Name]; }
            set { this[QuoteProperty.Name] = value; }
        }

        public override double? LastTradePriceOnly
        {
            get { return this.Value<double>(QuoteProperty.LastTradePriceOnly); }
            set { this[QuoteProperty.LastTradePriceOnly] = value; }
        }
        public override double? Change
        {
            get { return this.Value<double>(QuoteProperty.Change); }
            set { this[QuoteProperty.Change] = value; }
        }
        public override double? ChangeInPercent
        {
            get { return this.Value<double>(QuoteProperty.Change_ChangeInPercent); }
            set { this[QuoteProperty.Change_ChangeInPercent] = value; }
        }


        public QuotesData() { this.Values = new Dictionary<QuoteProperty, object>(); }
        public QuotesData(string id) : this() { this.Values[QuoteProperty.Symbol] = id; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prp"></param>
        /// <returns></returns>
        public Nullable<T> Value<T>(QuoteProperty prp) where T : struct
        {
            if (this.Values.ContainsKey(prp))
            {
                object val = this.Values[prp];
                if (val != null)
                {
                    if (val is T)
                    { return (T)val; }
                    else
                    { throw new ArgumentException("The value was found but is not of type " + typeof(T).Name + ". Type is " + val.GetType().Name + ".", "T"); }
                }
            }
            return null;
        }

        public QuotesData Clone()
        {
            QuotesData cln = new QuotesData();
            foreach (QuoteProperty qp in this.Values.Keys)
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

        public bool IDEquals(IID iid) { return iid != null && this.ID.Equals(iid.ID); }

    }



    /// <summary>
    /// Stores informations about one historic trading period (day, week or month).
    /// </summary>
    /// <remarks></remarks>
    public class QuotesPeriodData
    {

        /// <summary>
        /// The first value in trading period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute]
        public double Open { get; set; }
        /// <summary>
        /// The highest value in trading period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute]
        public double High { get; set; }
        /// <summary>
        /// The lowest value in trading period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute]
        public double Low { get; set; }
        /// <summary>
        /// The last value in trading period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute]
        public double Close { get; set; }

    }

    public class QuotesHistoricalData : QuotesPeriodData
    {

        /// <summary>
        /// The startdate of the period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute]
        public System.DateTime TradingDate { get; set; }
        /// <summary>
        /// The last value in trading period in relation to share splits.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute]
        public double CloseAdjusted { get; set; }
        /// <summary>
        /// The traded volume.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute]
        public long Volume { get; set; }
        /// <summary>
        /// The close value of the previous HistQuoteData in chain.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute]
        public double PreviousClose { get; set; }

    }



    public class QuotesHistoricalDataCollection : IEnumerable<QuotesHistoricalData>, IID
    {

        public List<QuotesHistoricalData> Items { get; private set; }

        /// <summary>
        /// The ID of the historic quotes owning stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute]
        public string ID { get; set; }

        public QuotesHistoricalDataCollection() { this.Items = new List<QuotesHistoricalData>(); }
        public QuotesHistoricalDataCollection(string id) : this(id, null) { }
        public QuotesHistoricalDataCollection(string id, IEnumerable<QuotesHistoricalData> items)
            : this()
        {
            this.ID = id;
            if (items != null)
            {
                this.Items.AddRange(items);
                this.Sort();
            }
        }

        public void Sort()
        {
            this.Items.Sort(new HistQuotesSorter());
            if (this.Items.Count > 0)
            {
                this.Items[0].PreviousClose = 0;
                for (int i = 1; i < this.Items.Count; i++)
                {
                    this.Items[i].PreviousClose = this.Items[i - 1].Close;
                }
            }
        }

        public IEnumerator<QuotesHistoricalData> GetEnumerator() { return this.Items.GetEnumerator(); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return this.Items.GetEnumerator(); }


        private class HistQuotesSorter : IComparer<QuotesHistoricalData>
        {
            public int Compare(QuotesHistoricalData x, QuotesHistoricalData y) { return DateTime.Compare(x.TradingDate, y.TradingDate); }
        }

        private double[] GetOpenValues()
        {
            double[] result = new double[this.Items.Count];
            if (this.Items.Count > 0)
                for (int i = 0; i < this.Items.Count; i++) result[i] = this.Items[i].Open;
            return result;
        }
        private double[] GetCloseValues()
        {
            double[] result = new double[this.Items.Count];
            if (this.Items.Count > 0)
                for (int i = 0; i < this.Items.Count; i++) result[i] = this.Items[i].Close;
            return result;
        }
        private double[] GetCloseAdjustedValues()
        {
            double[] result = new double[this.Items.Count];
            if (this.Items.Count > 0)
                for (int i = 0; i < this.Items.Count; i++) result[i] = this.Items[i].CloseAdjusted;
            return result;
        }
        private double[] GetHighValues()
        {
            double[] result = new double[this.Items.Count];
            if (this.Items.Count > 0)
                for (int i = 0; i < this.Items.Count; i++) result[i] = this.Items[i].High;
            return result;
        }
        private double[] GetLowValues()
        {
            double[] result = new double[this.Items.Count];
            if (this.Items.Count > 0)
                for (int i = 0; i < this.Items.Count; i++) result[i] = this.Items[i].Low;
            return result;
        }

    }





    /// <summary>
    /// Stores informations of quote options.
    /// </summary>
    /// <remarks></remarks>
    public class QuotesOptionsData : IQuotePrice
    {

        /// <summary>
        /// Call/Put Indicator
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Web.QuotesOptionsType Type { get; set; }
        /// <summary>
        ///  The stated price per share for which underlying stock can be purchased (in the case of a call) or sold (in the case of a put) by the option holder upon exercise of the option contract.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Strike { get; set; }
        /// <summary>
        /// The basic parts of new option symbol are: Root symbol + Expiration Year(yy)+ Expiration Month(mm)+ Expiration Day(dd) + Call/Put Indicator (C or P) + Strike price
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID { get; set; }
        /// <summary>
        /// The price of the last trade made for option contract.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double? LastTradePriceOnly { get; set; }
        /// <summary>
        /// The Bid price is the price you get if you were to write (sell) an option.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Bid { get; set; }
        /// <summary>
        /// The Ask price is the price you have to pay to purchase an option.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Ask { get; set; }
        /// <summary>
        /// The change in price for the day. This is the difference between the last trade and the previous day's closing price (Prev Close). The change is reported as "0" if the option hasn't traded today.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Change { get; set; }
        /// <summary>
        /// The change in percent for the day. This is the difference between the last trade and the previous day's closing price (Prev Close). The change is reported as "0" if the option hasn't traded today.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ChangeInPercent { get; set; }
        /// <summary>
        /// The volume indicates the number of option contracts that have traded for the current day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Volume { get; set; }
        /// <summary>
        /// The total number of derivative contracts traded that have not yet been liquidated either by an offsetting derivative transaction or by delivery.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int OpenInterest { get; set; }
        public double ImpliedVoyatilityPercent { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public QuotesOptionsData() { }
        internal QuotesOptionsData(Web.QuotesOptionsType type, object[] values)
        {
            this.Type = type;
            if (values.Length == 10)
            {
                if (values[0] != null && values[0] is double) this.Strike = (double)values[0];
                if (values[1] != null && values[1] is string) this.ID = (string)values[1];
                if (values[2] != null && values[2] is double) this.LastTradePriceOnly = (double)values[2];
                if (values[3] != null && values[3] is double) this.Bid = (double)values[3];
                if (values[4] != null && values[4] is double) this.Ask = (double)values[4];
                if (values[5] != null && values[5] is double) this.Change = (double)values[5];
                if (values[6] != null && values[6] is double) this.ChangeInPercent = (double)values[6];
                if (values[7] != null && values[7] is int) this.Volume = (int)values[7];
                if (values[8] != null && values[8] is int) this.OpenInterest = (int)values[8];
                if (values[9] != null && values[9] is int) this.OpenInterest = (int)values[9];
            }
        }

        public override string ToString() { return this.ID; }

    }


    public class QuotesOrderBookItem
    {
        public double Price { get; set; }
        public int Size { get; set; }
    }




}

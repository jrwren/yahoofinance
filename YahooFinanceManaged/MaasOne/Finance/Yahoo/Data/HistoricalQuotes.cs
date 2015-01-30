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


    public class HistoricalQuotesDataCollection : List<HistoricalQuotesData>, IID
    {

        /// <summary>
        /// The ID of the historic quotes owning stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute]
        public string ID { get; set; }

        public HistoricalQuotesDataCollection() : this(string.Empty, null) { }
        public HistoricalQuotesDataCollection(string id) : this(id, null) { }
        public HistoricalQuotesDataCollection(string id, IEnumerable<HistoricalQuotesData> items)
        {
            this.ID = id;
            if (items != null)
            {
                base.AddRange(items);
                this.Sort();
            }
        }

        public void Sort()
        {
            base.Sort(new HistQuotesSorter());
            if (base.Count > 0)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (i > 0)
                    {
                        base[i].PreviousClose = base[i - 1].Close;
                    }
                }
            }
        }


        private class HistQuotesSorter : IComparer<HistoricalQuotesData>
        {
            public int Compare(HistoricalQuotesData x, HistoricalQuotesData y) { return DateTime.Compare(x.TradingDate, y.TradingDate); }
        }

    }



    /// <summary>
    /// Stores informations about one historic trading period (day, week or month).
    /// </summary>
    /// <remarks></remarks>
    public class HistoricalQuotesData
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

}

using System;
using System.Collections.Generic;

namespace MaasOne.Finance.Yahoo.Data
{


    /// <summary>
    /// Stores informations of quote options.
    /// </summary>
    /// <remarks></remarks>
    public class QuoteOptionsData : IQuotePrice
    {

        /// <summary>
        /// Call/Put Indicator
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Web.QuoteOptionType Type { get; set; }
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
        public double LastTradePriceOnly { get; set; }
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
        public QuoteOptionsData() { }
        internal QuoteOptionsData(Web.QuoteOptionType type, object[] values)
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


}

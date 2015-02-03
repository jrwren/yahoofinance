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

namespace MaasOne.YahooFinance
{

    public class TradingTimeInfo
    {

        /// <summary>
        /// The time when trading starts
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>By setting a value, the date is not important, only hour and minute</remarks>
        public DateTime OpeningTimeLocal { get; private set; }
        /// <summary>
        /// The time when trading ends
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>By setting a value, the date is not important, only hour and minute. If time value is smaler than opening, trading ends on the next day. 24 hours trading is maximum</remarks>
        public DateTime ClosingTimeLocal { get; private set; }
        /// <summary>
        /// The timespan of active trading for each trading day
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeSpan TradingSpan { get { return this.ClosingTimeLocal - this.OpeningTimeLocal; } }
#if (PORTABLE40 || PORTABLE45)
        /// <summary>
        /// The system's timezone ID of the stock exchange.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string TimeZoneID { get; private set; }
#else
        /// <summary>
        /// The timezone of the stock exchange.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeZoneInfo TimeZone { get; private set; }
#endif
        /// <summary>
        /// The data response delay to realtime of yahoo servers
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int DelayMinutes { get; private set; }
        /// <summary>
        /// The days when trading is active
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DayOfWeek[] TradingDays { get; private set; }
        /// <summary>
        /// Days without active trading time.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime[] NonTradingDays { get; private set; }
        public TradingTimeInfo(DateTime openingTimeLocal, DateTime closingTimeLocal, string timeZoneID, int delayMinutes) : this(openingTimeLocal, closingTimeLocal, timeZoneID, delayMinutes, null, null) { }
        public TradingTimeInfo(DateTime openingTimeLocal, DateTime closingTimeLocal, string timeZoneID, int delayMinutes, IEnumerable<DayOfWeek> tradingDays, IEnumerable<System.DateTime> nonTradingDays)
        {
            this.OpeningTimeLocal = openingTimeLocal;
            this.ClosingTimeLocal = closingTimeLocal;

            if (this.OpeningTimeLocal >= this.ClosingTimeLocal) throw new ArgumentException("The opening time must be earlier than the closing time.", "OpeningTimeLocal");
            if (delayMinutes < 0 || delayMinutes >= 1440) throw new ArgumentException("The delay in minutes must be minimum 0 and maximum 1440.", "delayMinutes");
            if (this.TradingSpan.TotalMinutes < 0 || this.TradingSpan.TotalMinutes > 1440) throw new ArgumentException("The trading span must be within 24 hours.", "tradingSpan");
#if (PORTABLE40 || PORTABLE45)             
            this.TimeZoneID = timeZoneID;
#else
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
            if (timeZone == null) throw new ArgumentNullException("The time zone object is NULL.", "timeZone");
            this.TimeZone = timeZone;
#endif
            this.DelayMinutes = delayMinutes;
            this.TradingDays = (tradingDays != null ? tradingDays.EnumToArray() : new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
            this.NonTradingDays = (nonTradingDays != null ? nonTradingDays.EnumToArray() : new System.DateTime[0]);
        }
        internal TradingTimeInfo(MaasOne.Resources.WorldMarketStockExchangeTradingTime orig)
            : this(orig.OpeningTimeLocal, orig.ClosingTimeLocal, orig.TimeZone, orig.DelayMinutes)
        {
            var ds = new List<DayOfWeek>();
            var values = orig.TradingDays.Split(' ');
            foreach (var v in values) { foreach (DayOfWeek dow in Enum.GetValues(typeof(DayOfWeek))) { if (dow.ToString().StartsWith(v)) { ds.Add(dow); break; } } }
            this.TradingDays = ds.ToArray();
            var nds = new List<DateTime>();
            if (orig.NonTradingDays != null) { foreach (var v in orig.NonTradingDays) { nds.Add(v.Date); } }
            this.NonTradingDays = nds.ToArray();
        }

#if !(PORTABLE40 || PORTABLE45)
        public DateTime ExchangeLocalTimeNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, this.TimeZone);
        }

        public bool IsActiveNow() { return this.IsActiveUTC(DateTime.UtcNow); }
        public bool IsActiveMachineLocal(DateTime time) { return this.IsActiveUTC(time.ToUniversalTime()); }
        /// <summary>
        /// Returns if trading is active at a specific UTC DateTime.
        /// </summary>
        /// <param name="time">The UTC DateTime</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsActiveUTC(DateTime time)
        {
            return this.IsActiveExchangeLocal(TimeZoneInfo.ConvertTimeFromUtc(time, this.TimeZone));
        }
#endif
        /// <summary>
        /// Returns if trading is active at a specific datetime in relation to stock exchange's current timezone.
        /// </summary>
        /// <param name="time">The DateTime of the stock exchange's local time zone.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsActiveExchangeLocal(DateTime localTime)
        {
            bool isHoliday = false;
            if (this.NonTradingDays != null && this.NonTradingDays.Length > 0)
            {
                foreach (System.DateTime h in this.NonTradingDays)
                {
                    if (h.Date == localTime.Date)
                    {
                        isHoliday = true;
                        break;
                    }
                }
            }

            if (!isHoliday && (this.TradingDays == null || this.TradingDays.EnumContains(localTime.DayOfWeek)))
            {
                return (localTime > this.OpeningTimeLocal & localTime < this.ClosingTimeLocal);
            }
            else
            {
                return false;
            }
        }

    }
}

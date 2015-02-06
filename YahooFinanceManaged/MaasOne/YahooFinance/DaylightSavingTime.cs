#region "License"
// *********************************************************************************************
// **                                                                                         **
// **  Yahoo! Finance Managed                                                                 **
// **                                                                                         **
// **  Copyright (c) Marius Häusler 2009-2015                                                 **
// **                                                                                         **
// **  Licensed under GNU Lesser General Public License (LGPL) (Version 2.1, February 1999).  **
// **                                                                                         **
// **  License: https://www.gnu.org/licenses/old-licenses/lgpl-2.1.txt                        **
// **                                                                                         **
// **  Project: https://yahoofinance.codeplex.com/                                            **
// **                                                                                         **
// **  Contact: maasone@live.com                                                              **
// **                                                                                         **
// *********************************************************************************************
#endregion
using System;
using System.Collections.Generic;
using System.Text;


namespace MaasOne.YahooFinance
{
    /// <summary>
    /// Class for saving informations about the fix dates for Daylight Saving Time in a specific year.
    /// </summary>
    /// <remarks></remarks>
    public class DaylightSavingTime
    {

        private int mYear;
        private DateTime mStartDate;

        private DateTime mEndDate;
        public int Year
        {
            get { return mYear; }
        }
        public DateTime StartDate
        {
            get { return mStartDate; }
        }
        public DateTime EndDate
        {
            get { return mEndDate; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="startDate">The start date of Daylight Saving Time</param>
        /// <param name="endDate">The end date of Daylight Saving Time</param>
        /// <remarks>In case of countries in southern hemisphere the start date is higher then the end date. In this case the object contains the end date of the period that starts one year before. The start date is the begin of the period that will go on into the next year.</remarks>
        public DaylightSavingTime(DateTime startDate, DateTime endDate)
        {
            if (startDate.Year != endDate.Year)
            {
                throw new ArgumentException("The year of [startDate] is not the same year like [endDate] parameter.", "startDate");
            }
            else
            {
                mYear = startDate.Year;
                mStartDate = startDate;
                mEndDate = endDate;
            }
        }

    }
}

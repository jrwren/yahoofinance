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
using System.Text;


namespace MaasOne.Finance.Yahoo
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

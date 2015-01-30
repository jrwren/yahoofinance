#if (NET20)
//Author: Arman Ghazanchyan
//Created date: 09/04/2007
//Last updated: 09/17/2007
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.Win32;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
    /// <summary>
    /// Represents a time zone and provides access to all system time zones.
    /// </summary>
    [DebuggerDisplay("{_displayName}")]
    public class TimeZoneInfo : IComparer<TimeZoneInfo>
    {
        
        private string _id;
        private TimeZoneInformation _tzi = new TimeZoneInformation();

        private string _displayName;
#region " STRUCTURES "

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;

            public ushort wMilliseconds;
            /// <summary>
            /// Sets the member values of the time structure.
            /// </summary>
            /// <param name="info">A byte array that contains the information of a time.</param>
            [DebuggerHidden()]
            public void SetInfo(byte[] info)
            {
                if (info.Length != Marshal.SizeOf(this))
                {
                    throw new ArgumentException("Information size is incorrect", "info");
                }
                this.wYear = BitConverter.ToUInt16(info, 0);
                this.wMonth = BitConverter.ToUInt16(info, 2);
                this.wDayOfWeek = BitConverter.ToUInt16(info, 4);
                this.wDay = BitConverter.ToUInt16(info, 6);
                this.wHour = BitConverter.ToUInt16(info, 8);
                this.wMinute = BitConverter.ToUInt16(info, 10);
                this.wSecond = BitConverter.ToUInt16(info, 12);
                this.wMilliseconds = BitConverter.ToUInt16(info, 14);
            }

            /// <summary>
            /// Determines whether the specified System.Object 
            /// is equal to the current System.Object.
            /// </summary>
            /// <param name="obj">The System.Object to compare 
            /// with the current System.Object.</param>
            [DebuggerHidden()]
            public override bool Equals(object obj)
            {
                if (object.ReferenceEquals(this.GetType(), obj.GetType()))
                {
                    SYSTEMTIME objSt = (SYSTEMTIME)obj;
                    if (this.wDay != objSt.wDay || this.wDayOfWeek != objSt.wDayOfWeek || this.wHour != objSt.wHour || this.wMilliseconds != objSt.wMilliseconds || this.wMinute != objSt.wMinute || this.wMonth != objSt.wMonth || this.wSecond != objSt.wSecond || this.wYear != objSt.wYear)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }

        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct TimeZoneInformation
        {
            public int bias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string standardName;
            public SYSTEMTIME standardDate;
            public int standardBias;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string daylightName;
            public SYSTEMTIME daylightDate;

            public int daylightBias;
            /// <summary>
            /// Sets the member values of bias, standardBias, 
            /// daylightBias, standardDate, daylightDate of the structure.
            /// </summary>
            /// <param name="info">A byte array that contains the 
            /// information of the Tzi windows registry key.</param>
            [DebuggerHidden()]
            public void SetBytes(byte[] info)
            {
                if (info.Length != 44)
                {
                    throw new ArgumentException("Information size is incorrect", "info");
                }
                this.bias = BitConverter.ToInt32(info, 0);
                this.standardBias = BitConverter.ToInt32(info, 4);
                this.daylightBias = BitConverter.ToInt32(info, 8);
                byte[] helper = new byte[16];
                Array.Copy(info, 12, helper, 0, 16);
                this.standardDate.SetInfo(helper);
                Array.Copy(info, 28, helper, 0, 16);
                this.daylightDate.SetInfo(helper);
            }

            /// <summary>
            /// Determines whether the specified System.Object 
            /// is equal to the current System.Object.
            /// </summary>
            /// <param name="obj">The System.Object to compare 
            /// with the current System.Object.</param>
            [DebuggerHidden()]
            public override bool Equals(object obj)
            {
                if (object.ReferenceEquals(this.GetType(), obj.GetType()))
                {
                    TimeZoneInformation objTzi = (TimeZoneInformation)obj;
                    if (this.bias != objTzi.bias || this.daylightBias != objTzi.daylightBias || this.daylightName != objTzi.daylightName || this.standardBias != objTzi.standardBias || this.standardName != objTzi.standardName || !this.daylightDate.Equals(objTzi.daylightDate) || !this.standardDate.Equals(objTzi.standardDate))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }

        }

        #endregion

#region " API METHODS "

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetTimeZoneInformation(ref TimeZoneInformation lpTimeZoneInformation);

        #endregion

#region " CLASS PROPERTIES "

        /// <summary>
        /// Gets the display name of the time zone.
        /// </summary>
        public string DisplayName
        {
            [DebuggerHidden()]
            get
            {
                this.Refresh();
                return this._displayName;
            }
        }

        /// <summary>
        /// Gets the daylight saving name of the time zone.
        /// </summary>
        public string DaylightName
        {
            [DebuggerHidden()]
            get
            {
                this.Refresh();
                if (this.GetDaylightChanges(this.CurrentTime.Year).Delta == TimeSpan.Zero)
                {
                    return this._tzi.standardName;
                }
                else
                {
                    return this._tzi.daylightName;
                }
            }
        }

        /// <summary>
        /// Gets the standard name of the time zone.
        /// </summary>
        public string StandardName
        {
            [DebuggerHidden()]
            get
            {
                this.Refresh();
                return this._tzi.standardName;
            }
        }

        /// <summary>
        /// Gets the current date and time of the time zone.
        /// </summary>
        public DateTime CurrentTime
        {
            [DebuggerHidden()]
            get { return new DateTime(DateTime.UtcNow.Ticks + this.CurrentUtcOffset.Ticks, DateTimeKind.Local); }
        }

        /// <summary>
        /// Gets the current UTC (Coordinated Universal Time) offset of the time zone.
        /// </summary>
        public TimeSpan CurrentUtcOffset
        {
            [DebuggerHidden()]
            get
            {
                if (this.IsDaylightSavingTime())
                {
                    return new TimeSpan(0, -(this._tzi.bias + this._tzi.daylightBias), 0);
                }
                else
                {
                    return new TimeSpan(0, -this._tzi.bias, 0);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current time zone for this computer system.
        /// </summary>
        public static TimeZoneInfo CurrentTimeZone
        {
            [DebuggerHidden()]
            get { return new TimeZoneInfo(TimeZone.CurrentTimeZone.StandardName); }
            [DebuggerHidden()]
            set
            {
                value.Refresh();
                if (!TimeZoneInfo.SetTimeZoneInformation(ref value._tzi))
                {
                    //Throw a Win32Exception
                    throw new System.ComponentModel.Win32Exception();
                }
            }
        }

        /// <summary>
        /// Gets the standard UTC (Coordinated Universal Time) offset of the time zone.
        /// </summary>
        public TimeSpan StandardUtcOffset
        {
            [DebuggerHidden()]
            get
            {
                this.Refresh();
                return new TimeSpan(0, -this._tzi.bias, 0);
            }
        }

        /// <summary>
        /// Gets the id of the time zone.
        /// </summary>
        public string Id
        {
            [DebuggerHidden()]
            get
            {
                this.Refresh();
                return this._id;
            }
        }

        #endregion

#region " CLASS CONSTRUCTORS "

        /// <param name="standardName">A time zone standard name.</param>
        [DebuggerHidden()]
        public TimeZoneInfo(string standardName)
        {
            this.SetValues(standardName);
        }

        [DebuggerHidden()]
        private TimeZoneInfo()
        {
        }

        #endregion

#region " CLASS METHODS "

        /// <summary>
        /// Gets an array of all time zones on the system.
        /// </summary>
        [DebuggerHidden()]
        public static TimeZoneInfo[] GetTimeZones()
        {
            List<TimeZoneInfo> tzInfos = new List<TimeZoneInfo>();
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", false);
            if ((key != null))
            {
                foreach (string zoneName in key.GetSubKeyNames())
                {
                    TimeZoneInfo tzi = new TimeZoneInfo();
                    tzi._id = zoneName;
                    tzi.SetValues();
                    tzInfos.Add(tzi);
                }
                TimeZoneInfo.Sort(tzInfos);
            }
            else
            {
                throw new KeyNotFoundException("Cannot find the windows registry key (Time Zone).");
            }
            return tzInfos.ToArray();
        }

        /// <summary>
        /// Sorts the elements in a list(Of TimeZoneInfo) 
        /// object based on standard UTC offset or display name.
        /// </summary>
        /// <param name="tzInfos">A time zone list to sort.</param>
        [DebuggerHidden()]
        public static void Sort(List<TimeZoneInfo> tzInfos)
        {
            tzInfos.Sort(new TimeZoneInfo());
        }

        /// <summary>
        /// Sorts the elements in an entire one-dimensional TimeZoneInfo 
        /// array based on standard UTC offset or display name.
        /// </summary>
        /// <param name="tzInfos">A time zone array to sort.</param>
        [DebuggerHidden()]
        public static void Sort(TimeZoneInfo[] tzInfos)
        {
            Array.Sort(tzInfos, new TimeZoneInfo());
        }

        /// <summary>
        /// Gets a TimeZoneInfo.Object from standard name.
        /// </summary>
        /// <param name="standardName">A time zone standard name.</param>
        [DebuggerHidden()]
        public static TimeZoneInfo FromStandardName(string standardName)
        {
            return new TimeZoneInfo(standardName);
        }

        /// <summary>
        /// Gets a TimeZoneInfo.Object from Id.
        /// </summary>
        /// <param name="id">A time zone id that corresponds 
        /// to the windows registry time zone key.</param>
        [DebuggerHidden()]
        public static TimeZoneInfo FindSystemTimeZoneById(string id)
        {
            if ((id != null))
            {
                if (id != string.Empty)
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", false);
                    if ((key != null))
                    {
                        RegistryKey subKey = key.OpenSubKey(id, false);
                        if ((subKey != null))
                        {
                            TimeZoneInfo tzi = new TimeZoneInfo();
                            tzi._id = subKey.Name;
                            tzi._displayName = subKey.GetValue("Display").ToString();
                            tzi._tzi.daylightName = subKey.GetValue("Dlt").ToString();
                            tzi._tzi.standardName = subKey.GetValue("Std").ToString();
                            tzi._tzi.SetBytes((Byte[])subKey.GetValue("Tzi"));
                            return tzi;
                        }
                    }
                    else
                    {
                        throw new KeyNotFoundException("Cannot find the windows registry key (Time Zone).");
                    }
                }
                throw new ArgumentException("Unknown time zone.", "id");
            }
            else
            {
                throw new ArgumentNullException("id", "Value cannot be null.");
            }
        }

        public static DateTime ConvertTimeFromUtc(DateTime utcTime, TimeZoneInfo timeZone)
        {
            return utcTime.Add(timeZone.CurrentUtcOffset);
        }

        /// <summary>
        /// Returns the daylight saving time for a particular year.
        /// </summary>
        /// <param name="year">The year to which the daylight 
        /// saving time period applies.</param>
        [DebuggerHidden()]
        public System.Globalization.DaylightTime GetDaylightChanges(int year)
        {
            TimeZoneInformation tzi = new TimeZoneInformation();
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", false);
            if ((key != null))
            {
                RegistryKey subKey = key.OpenSubKey(this._id, false);
                if ((subKey != null))
                {
                    RegistryKey subKey1 = subKey.OpenSubKey("Dynamic DST", false);
                    if ((subKey1 != null))
                    {
                        if (Array.IndexOf(subKey1.GetValueNames(), Convert.ToString(year)) != -1)
                        {
                            tzi.SetBytes((Byte[])subKey1.GetValue(Convert.ToString(year)));
                        }
                        else
                        {
                            this.Refresh();
                            tzi = this._tzi;
                        }
                    }
                    else
                    {
                        this.Refresh();
                        tzi = this._tzi;
                    }
                }
                else
                {
                    throw new Exception("Unknown time zone.");
                }
            }
            else
            {
                throw new KeyNotFoundException("Cannot find the windows registry key (Time Zone).");
            }
            DateTime dStart = default(DateTime);
            DateTime dEnd = default(DateTime);
            dStart = this.GetStartDate(tzi, year);
            dEnd = this.GetEndDate(tzi, year);
            if (dStart != System.DateTime.MinValue && dEnd != System.DateTime.MinValue)
            {
                return new DaylightTime(dStart, dEnd, new TimeSpan(0, -this._tzi.daylightBias, 0));
            }
            else
            {
                return new DaylightTime(dStart, dEnd, new TimeSpan(0, 0, 0));
            }
        }

        /// <summary>
        /// Returns a value indicating whether this time 
        /// zone is within a daylight saving time period.
        /// </summary>
        [DebuggerHidden()]
        public bool IsDaylightSavingTime()
        {
            DateTime dUtcNow = DateTime.UtcNow.AddMinutes(-(this._tzi.bias));
            DateTime sUtcNow = DateTime.UtcNow.AddMinutes(-(this._tzi.bias + this._tzi.daylightBias));
            DaylightTime dt = default(DaylightTime);

            if (this._tzi.daylightDate.wMonth <= this._tzi.standardDate.wMonth)
            {
                //Daylight saving time starts and ends in the same year
                dt = this.GetDaylightChanges(dUtcNow.Year);
                if (dt.Delta != TimeSpan.Zero)
                {
                    if (dUtcNow >= dt.Start && sUtcNow < dt.End)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                //Daylight saving time starts and ends in diferent years
                dt = this.GetDaylightChanges(sUtcNow.Year);
                if (dt.Delta != TimeSpan.Zero)
                {
                    if (dUtcNow < dt.Start && sUtcNow >= dt.End)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Creates and returns a date and time object.
        /// </summary>
        /// <param name="wYear">The year of the date.</param>
        /// <param name="wMonth">The month of the date.</param>
        /// <param name="wDay">The week day in the month.</param>
        /// <param name="wDayOfWeek">The day of the week.</param>
        /// <param name="wHour">The hour of the date.</param>
        /// <param name="wMinute">The minute of the date.</param>
        /// <param name="wSecond">The seconds of the date.</param>
        /// <param name="wMilliseconds">The milliseconds of the date.</param>
        [DebuggerHidden()]
        private DateTime CreateDate(int wYear, int wMonth, int wDay, int wDayOfWeek, int wHour, int wMinute, int wSecond, int wMilliseconds)
        {

            if (wDay < 1 || wDay > 5)
            {
                throw new ArgumentOutOfRangeException("wDat", wDay, "The value is out of acceptable range (1 to 5).");
            }
            if (wDayOfWeek < 0 || wDayOfWeek > 6)
            {
                throw new ArgumentOutOfRangeException("wDayOfWeek", wDayOfWeek, "The value is out of acceptable range (0 to 6).");
            }
            int daysInMonth = System.DateTime.DaysInMonth(wYear, wMonth);
            int fDayOfWeek = (int)new DateTime(wYear, wMonth, 1).DayOfWeek;
            int occurre = 1;
            int day = 1;
            if (fDayOfWeek != wDayOfWeek)
            {
                if (wDayOfWeek == 0)
                {
                    day += 7 - fDayOfWeek;
                }
                else
                {
                    if (wDayOfWeek > fDayOfWeek)
                    {
                        day += wDayOfWeek - fDayOfWeek;
                    }
                    else if (wDayOfWeek < fDayOfWeek)
                    {
                        day = wDayOfWeek + fDayOfWeek;
                    }
                }
            }
            while (occurre < wDay && day <= daysInMonth - 7)
            {
                day += 7;
                occurre += 1;
            }
            return new DateTime(wYear, wMonth, day, wHour, wMinute, wSecond, wMilliseconds, DateTimeKind.Local);
        }

        /// <summary>
        /// Gets the starting daylight saving date and time for specified thime zone.
        /// </summary>
        [DebuggerHidden()]
        private DateTime GetStartDate(TimeZoneInformation tzi, int year)
        {
            if (tzi.daylightDate.wMonth != 0)
            {
                if (tzi.daylightDate.wYear == 0)
                {
                    return this.CreateDate(year, tzi.daylightDate.wMonth, tzi.daylightDate.wDay, tzi.daylightDate.wDayOfWeek, tzi.daylightDate.wHour, tzi.daylightDate.wMinute, tzi.daylightDate.wSecond, tzi.daylightDate.wMilliseconds);
                }
                else
                {
                    return new DateTime(tzi.daylightDate.wYear, tzi.daylightDate.wMonth, tzi.daylightDate.wDay, tzi.daylightDate.wHour, tzi.daylightDate.wMinute, tzi.daylightDate.wSecond, tzi.daylightDate.wMilliseconds, DateTimeKind.Local);
                }
            }
            else
            {
                return new DateTime();
            }
        }

        /// <summary>
        /// Gets the end date of the daylight saving time for specified thime zone.
        /// </summary>
        [DebuggerHidden()]
        private DateTime GetEndDate(TimeZoneInformation tzi, int year)
        {
            if (tzi.standardDate.wMonth != 0)
            {
                if (tzi.standardDate.wYear == 0)
                {
                    return this.CreateDate(year, tzi.standardDate.wMonth, tzi.standardDate.wDay, tzi.standardDate.wDayOfWeek, tzi.standardDate.wHour, tzi.standardDate.wMinute, tzi.standardDate.wSecond, tzi.standardDate.wMilliseconds);
                }
                else
                {
                    return new DateTime(tzi.standardDate.wYear, tzi.standardDate.wMonth, tzi.standardDate.wDay, tzi.standardDate.wHour, tzi.standardDate.wMinute, tzi.standardDate.wSecond, tzi.standardDate.wMilliseconds, DateTimeKind.Local);
                }
            }
            return new DateTime();
        }

        /// <summary>
        /// Refreshes the information of the time zone object.
        /// </summary>
        [DebuggerHidden()]
        public void Refresh()
        {
            this.SetValues();
        }

        /// <summary>
        /// Sets the time zone object's information.
        /// </summary>
        [DebuggerHidden()]
        private void SetValues()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", false);
            if ((key != null))
            {
                RegistryKey subKey = key.OpenSubKey(this._id, false);
                if ((subKey != null))
                {
                    this._displayName = Convert.ToString(subKey.GetValue("Display"));
                    this._tzi.daylightName = Convert.ToString(subKey.GetValue("Dlt"));
                    this._tzi.standardName = Convert.ToString(subKey.GetValue("Std"));
                    this._tzi.SetBytes((Byte[])subKey.GetValue("Tzi"));
                }
                else
                {
                    throw new Exception("Unknown time zone.");
                }
            }
            else
            {
                throw new KeyNotFoundException("Cannot find the windows registry key (Time Zone).");
            }
        }

        /// <summary>
        /// Sets the time zone object's information.
        /// </summary>
        /// <param name="standardName">A time zone standard name.</param>
        [DebuggerHidden()]
        private void SetValues(string standardName)
        {
            if ((standardName != null))
            {
                bool exist = false;
                if (standardName != string.Empty)
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", false);
                    if ((key != null))
                    {
                        foreach (string zoneName in key.GetSubKeyNames())
                        {
                            RegistryKey subKey = key.OpenSubKey(zoneName, false);
                            if (Convert.ToString(subKey.GetValue("Std")) == standardName)
                            {
                                this._id = zoneName;
                                this._displayName = Convert.ToString(subKey.GetValue("Display"));
                                this._tzi.daylightName = Convert.ToString(subKey.GetValue("Dlt"));
                                this._tzi.standardName = Convert.ToString(subKey.GetValue("Std"));
                                this._tzi.SetBytes((Byte[])subKey.GetValue("Tzi"));
                                exist = true;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                    }
                    else
                    {
                        throw new KeyNotFoundException("Cannot find the windows registry key (Time Zone).");
                    }
                }
                if (!exist)
                {
                    throw new ArgumentException("Unknown time zone.", "standardName");
                }
            }
            else
            {
                throw new ArgumentNullException("id", "Value cannot be null.");
            }
        }

        /// <summary>
        /// Returns a System.String that represents the current TimeZoneInfo object.
        /// </summary>
        [DebuggerHidden()]
        public override string ToString()
        {
            return this.DisplayName;
        }

        /// <summary>
        /// Determines whether the specified System.Object 
        /// is equal to the current System.Object.
        /// </summary>
        /// <param name="obj">The System.Object to compare 
        /// with the current System.Object.</param>
        [DebuggerHidden()]
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this.GetType(), obj.GetType()))
            {
                TimeZoneInfo objTzi = (TimeZoneInfo)obj;
                if (this._displayName != objTzi._displayName || this._id != objTzi._id || !this._tzi.Equals(objTzi._tzi))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Compares two specified TimeZoneInfo.Objects 
        /// based on standard UTC offset or display name.
        /// </summary>
        /// <param name="x">The first TimeZoneInfo.Object.</param>
        /// <param name="y">The second TimeZoneInfo.Object.</param>
        [DebuggerHidden()]
        public virtual int Compare(TimeZoneInfo x, TimeZoneInfo y)
        {
            if (x._tzi.bias == y._tzi.bias)
            {
                return x._displayName.CompareTo(y._displayName);
            }
            if (x._tzi.bias > y._tzi.bias)
            {
                return -1;
            }
            if (x._tzi.bias < y._tzi.bias)
            {
                return 1;
            }
            return -1;
        }

        #endregion

    }

}
#endif
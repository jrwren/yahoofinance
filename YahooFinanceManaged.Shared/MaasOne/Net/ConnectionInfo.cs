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
using System.Net;

namespace MaasOne.Net
{
    /// <summary>
    /// Provides information about a download process.
    /// </summary>
    /// <remarks></remarks>
    public partial class ConnectionInfo
    {
        internal ConnectionInfo(ConnectionState state,Exception exception, int timeout, int size, DateTime startTime, DateTime endTime)
        {
            this.State = state;
            this.Exception = exception;
            this.SizeInBytes = size;
            this.EndTime = endTime;
            this.StartTime = startTime;
            this.TimeSpan = this.EndTime - this.StartTime;
            this.Timeout = timeout;
        }


        /// <summary>
        /// Gets the end time of the download operation.
        /// </summary>
        public DateTime EndTime { get; private set; }

        /// <summary>
        /// Gets the exception object when a download operation fails. Otherwise it is NULL.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the average speed in KiloBytes per second of the download operation.
        /// </summary>
        public double KBPerSecond
        {
            get
            {
                if (this.TimeSpan.TotalMilliseconds != 0)
                {
                    return this.SizeInBytes / this.TimeSpan.TotalMilliseconds;
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Gets the size of downloaded data in bytes.
        /// </summary>
        public int SizeInBytes { get; private set; }

        /// <summary>
        /// Gets the start time of the download operation.
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// Gets the connection status.
        /// </summary>
        public ConnectionState State { get; private set; }
       
        /// <summary>
        /// Gets the setted timeout for download operation in milliseconds.
        /// </summary>
        public int Timeout { get; private set; }

        /// <summary>
        /// Gets the time span of start and end time.
        /// </summary>
        public TimeSpan TimeSpan { get; private set; }
    }
}

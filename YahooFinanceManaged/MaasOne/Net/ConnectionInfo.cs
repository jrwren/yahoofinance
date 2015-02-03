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
using System.Net;
using System.Collections.Generic;


namespace MaasOne.Net
{

    /// <summary>
    /// Provides information about a download process.
    /// </summary>
    /// <remarks></remarks>
    public partial class ConnectionInfo
    {


        private Exception mException = null;
        private int mSizeInBytes = 0;
        private DateTime mStartTime;
        private DateTime mEndTime;
        private TimeSpan mTimeSpan;

        /// <summary>
        /// Indicates the connection status.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ConnectionState State
        {
            get
            {
                if (mException == null)
                {
                    return ConnectionState.Success;
                }
                else
                {
                    if (mException is WebException)
                    {
                        WebException exp = (WebException)mException;
                        if (exp.Status == WebExceptionStatus.RequestCanceled) { return ConnectionState.Canceled; }
#if !(PORTABLE40 || PORTABLE45)
                        else if (exp.Status == WebExceptionStatus.Timeout) { return ConnectionState.Timeout; }
#endif
                        else { return ConnectionState.ErrorOccured; }
                    }
                    else
                    {
                        return ConnectionState.ErrorOccured;
                    }

                }
            }
        }
        /// <summary>
        /// If an exception occurs during download process, the exception object will be stored here. If no exception occurs, this property is null/Nothing.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Exception Exception
        {
            get { return mException; }
        }

#if !(PORTABLE40)
        /// <summary>
        /// The setted timeout for download process in milliseconds.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Timeout
        {
            get { return mTimeout; }
        }
        private int mTimeout = 0;
#endif
        /// <summary>
        /// The start time of download process, independent to individual preparation of passed parameters for start downloading.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime StartTime
        {
            get { return mStartTime; }
        }
        /// <summary>
        /// The end time of the download process, independent to post-processing actions like parsing.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DateTime EndTime
        {
            get { return mEndTime; }
        }
        /// <summary>
        /// The time span of start and end time.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeSpan TimeSpan
        {
            get { return mTimeSpan; }
        }
        /// <summary>
        /// The size of downloaded data in bytes.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int SizeInBytes
        {
            get { return mSizeInBytes; }
        }
        public double KBPerSecond
        {
            get
            {
                if (mTimeSpan.TotalMilliseconds != 0)
                {
                    return mSizeInBytes / mTimeSpan.TotalMilliseconds;
                }
                else
                {
                    return -1;
                }
            }
        }


#if (PORTABLE40)
        internal ConnectionInfo(Exception exception, int size, DateTime startTime, DateTime endTime)
#else
        internal ConnectionInfo(Exception exception, int timeout, int size, DateTime startTime, DateTime endTime)
#endif
        {
            mException = exception;
            mSizeInBytes = size;
            mEndTime = endTime;
            mStartTime = startTime;
            mTimeSpan = mEndTime - mStartTime;
#if !(PORTABLE40)
            mTimeout = timeout;
#endif
        }

        public ConnectionInfo Clone()
        {
#if (PORTABLE40)
            return new ConnectionInfo(this.Exception, this.SizeInBytes, this.StartTime, this.EndTime);
#else
            return new ConnectionInfo(this.Exception, this.Timeout, this.SizeInBytes, this.StartTime, this.EndTime);
#endif
        }

    }

}

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

namespace MaasOne.Net
{
    /// <summary>
    /// Provides the user argument object that was passed at the beginning of a download operation.
    /// </summary>
    public abstract class DownloadEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the user argument that was passed at the beginning of download operation.
        /// </summary>
        public object UserArgs { get; private set; }


        protected DownloadEventArgs(object userArgs) { this.UserArgs = userArgs; }
    }


    public class DownloadCompletedEventArgs : DownloadEventArgs
    {
        public ResponseBase ResponseBase { get; private set; }


        internal DownloadCompletedEventArgs(object userArgs, ResponseBase response)
            : base(userArgs)
        {
            this.ResponseBase = response;
        }

    }

    /// <summary>
    /// Provides properties for a completed, asynchronous download operation.
    /// </summary>
    public class YqlDownloadCompletedEventArgs<T> : DownloadCompletedEventArgs where T : ResultBase
    {
        /// <summary>
        /// Gets the response of the download operation.
        /// </summary>
        public YqlResponse<T> Response { get { return (YqlResponse<T>)base.ResponseBase; } }


        internal YqlDownloadCompletedEventArgs(object userArgs, YqlResponse<T> response) : base(userArgs, response) { }
    }

    /// <summary>
    /// Provides properties for a completed, asynchronous download operation.
    /// </summary>
    public class DownloadCompletedEventArgs<T> : DownloadCompletedEventArgs where T : ResultBase
    {
        /// <summary>
        /// Gets the response of the download operation.
        /// </summary>
        public Response<T> Response { get { return (Response<T>)base.ResponseBase; } }


        internal DownloadCompletedEventArgs(object userArgs, Response<T> response) : base(userArgs, response) { }
    }
}

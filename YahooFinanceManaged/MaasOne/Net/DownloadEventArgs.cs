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
    /// Base event argument class for asynchronous download processes. Provides the individual user argument that was passed at the beginning of download process.
    /// </summary>
    /// <remarks></remarks>
    public abstract class DownloadEventArgs : EventArgs, IDownloadEventArgs
    {

        /// <summary>
        /// Gets the user argument that were passed when the download was started.
        /// </summary>
        /// <value></value>
        /// <returns>An object defined by the user</returns>
        /// <remarks></remarks>
        public object UserArgs { get; private set; }

        protected DownloadEventArgs(object userArgs) { this.UserArgs = userArgs; }

    }

    /// <summary>
    /// Base event class for completed asynchronous download processes that provides the response of the download.
    /// </summary>
    /// <remarks></remarks>
    public class DownloadCompletedEventArgs<T> : DownloadEventArgs, IDownloadCompletedEventArgs where T : class
    {
        /// <summary>
        /// Gets the response of the download process.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Response<T> Response { get; private set; }

        internal DownloadCompletedEventArgs(object userArgs, Response<T> resp) : base(userArgs) { this.Response = resp; }

        IResponse IDownloadCompletedEventArgs.GetResponse() { return this.Response; }

    }

}

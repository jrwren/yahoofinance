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


namespace System.Net
{
    /// <summary>
    /// Base event class for asynchronous download processes and provides the individual user argument. This class must be inherited.
    /// </summary>
    /// <remarks></remarks>
    public abstract class DownloadEventArgs : EventArgs
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
    /// Base event class for completed asynchronous download processes that provides additionally the response of the download. This class must be inherited.
    /// </summary>
    /// <remarks></remarks>
    public class DownloadCompletedEventArgs<T> : DownloadEventArgs, IDownloadCompletedEventArgs where T : class
    {

        /// <summary>
        /// 
        /// </summary>
        public Query<T> Query { get; private set; }
        /// <summary>
        /// Gets the response of the download process.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Response<T> Response { get; private set; }

        internal DownloadCompletedEventArgs(object userArgs, Response<T> resp, Query<T> query)
            : base(userArgs)
        {
            this.Response = resp;
            this.Query = query;
        }

        public IResponse GetResponse() { return this.Response; }

    }

}

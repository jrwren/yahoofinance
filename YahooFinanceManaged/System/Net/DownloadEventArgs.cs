// ******************************************************************************
// ** 
// **  MaasOne WebServices
// **  Written by Marius Häusler 2012
// **  It would be pleasant, if you contact me when you are using this code.
// **  Contact: YahooFinanceManaged@gmail.com
// **  Project Home: http://code.google.com/p/yahoo-finance-managed/
// **  
// ******************************************************************************
// **  
// **  Copyright 2012 Marius Häusler
// **  
// **  Licensed under the Apache License, Version 2.0 (the "License");
// **  you may not use this file except in compliance with the License.
// **  You may obtain a copy of the License at
// **  
// **    http://www.apache.org/licenses/LICENSE-2.0
// **  
// **  Unless required by applicable law or agreed to in writing, software
// **  distributed under the License is distributed on an "AS IS" BASIS,
// **  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// **  See the License for the specific language governing permissions and
// **  limitations under the License.
// ** 
// ******************************************************************************
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

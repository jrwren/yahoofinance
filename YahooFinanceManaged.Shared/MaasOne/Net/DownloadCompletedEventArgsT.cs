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
    /// Provides properties for a completed, asynchronous download operation.
    /// </summary>
    public class DownloadCompletedEventArgs<T> : DownloadCompletedEventArgs, IQueryDownloadCompletedEventArgs where T : ResultBase
    {
        internal DownloadCompletedEventArgs(object userArgs, Response<T> response) : base(userArgs, response) { }



        /// <summary>
        /// Gets the response of the download operation.
        /// </summary>
        public Response<T> Response { get { return (Response<T>)base.ResponseBase; } }


        IQueryResponse IQueryDownloadCompletedEventArgs.Response { get { return this.Response; } }
    }
}

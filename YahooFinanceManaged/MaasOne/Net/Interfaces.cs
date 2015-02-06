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

    public interface IQuery
    {
        IQuery GetClone();
    }

    public delegate void AsyncIDownloadCompletedEventHandler(object sender, IDownloadCompletedEventArgs e);
    public partial interface IDownload
    {
        event AsyncIDownloadCompletedEventHandler AsyncIDownloadCompleted;

        IQuery GetDefaultQuery { get; }
#if !(PORTABLE40)
        IWebProxy Proxy { get; set; }
        int Timeout { get; set; }
#endif

        /// <summary>
        /// Starts an asynchronous download.
        /// </summary>
        /// <param name="userArgs">Individual user arguments.</param>
        void DownloadAsync(object userArgs);

#if (NET45 || PORTABLE45)
        System.Threading.Tasks.Task<IResponse> GetResponseTaskAsync();
#endif
#if (NET20 || NET45)
        IResponse GetResponse();
#endif
    }

    public interface IResponse
    {
        ConnectionInfo Connection { get; }
        IQuery QueryObj { get; }
        object ResultObj { get; }
    }

    public interface IDownloadEventArgs
    {
        object UserArgs { get; }
    }

    public interface IDownloadCompletedEventArgs : IDownloadEventArgs
    {
        IResponse GetResponse();
    }



}

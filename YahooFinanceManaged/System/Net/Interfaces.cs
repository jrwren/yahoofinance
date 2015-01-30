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

    public interface IQuery
    {
        IQuery GetCloneDeep();
    }

    public delegate void AsyncIDownloadCompletedEventHandler(object sender, IDownloadCompletedEventArgs e);
    public partial interface IDownload
    {


        event AsyncIDownloadCompletedEventHandler AsyncIDownloadCompletedEvent;

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

    public interface IDownloadCompletedEventArgs
    {

        object UserArgs { get; }

        IResponse GetResponse();

    }



}

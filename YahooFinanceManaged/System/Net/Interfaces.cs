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

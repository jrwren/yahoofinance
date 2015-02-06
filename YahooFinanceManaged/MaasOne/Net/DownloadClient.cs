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
using System.ComponentModel;
using System.Threading;
#if (NET45 || PORTABLE45)
using System.Threading.Tasks;
#endif


namespace MaasOne.Net
{
    /// <summary>
    /// This class provides methods and properties for downloading data and controlling these download processes.
    /// </summary>
    /// <remarks></remarks>
    public class DownloadClient<T> : IDownload where T : class
    {

        #region "Properties"
        /// <summary>
        /// Gets a collection of active download operations.
        /// </summary>
        public DownloadSessionCollection ActiveSessions { get; private set; }

        public Query<T> DefaultQuery { get; set; }
        IQuery IDownload.GetDefaultQuery { get { return this.DefaultQuery; } }

#if !(PORTABLE40)
        /// <summary>
        /// Gets or sets the used proxy setting for download.
        /// </summary>
        /// <value></value>
        /// <returns>The actual setted proxy</returns>
        /// <remarks>Default value are proxy settings from Internet Explorer/Windows Internet Settings.</remarks>
        public IWebProxy Proxy { get; set; }

        private int mTimeout = 30000;
        /// <summary>
        /// Gets or sets the time in miliseconds when the download will cancel if the time reached before it is completed.
        /// </summary>
        /// <value>Timeout in miliseconds. If value is '-1', the next download wont have a timeout.</value>
        /// <returns>The timeout in miliseconds.</returns>
        /// <remarks></remarks>
        public int Timeout
        {
            get { return mTimeout; }
            set { mTimeout = Math.Min(Math.Max(0, value), 300000); }
        }
#endif

        #endregion


        #region "Constructor"

        public DownloadClient()
        {
            this.DefaultQuery = null;
#if !(PORTABLE40)
            this.Timeout = 30000;
            this.Proxy = null;
#endif
            this.ActiveSessions = new DownloadSessionCollection();
        }
        public DownloadClient(Query<T> defaultQuery) : this() { this.DefaultQuery = defaultQuery; }

        #endregion


        #region "WRAsync"

        public delegate void AsyncDownloadCompletedEventHandler(object sender, DownloadCompletedEventArgs<T> e);

        public event AsyncDownloadCompletedEventHandler AsyncDownloadCompleted;
        public event AsyncIDownloadCompletedEventHandler AsyncIDownloadCompleted;

        /// <summary>
        /// Starts an asynchronous download.
        /// </summary>
        /// <param name="userArgs">Individual user arguments.</param>
        public void DownloadAsync(object userArgs) { this.DownloadAsync(this.DefaultQuery, userArgs); }
        public void DownloadAsync(Query<T> query, object userArgs)
        {
            query = this.CheckQuery(query);
            HttpWebRequest wr = this.CreateWebRequest(query);

            DateTime startTime = DateTime.Now;
            this.AddDownload(wr, query, startTime, userArgs);
            try
            {
                startTime = DateTime.Now;
                AsyncDownloadArgs asyncArgs = new AsyncDownloadArgs(wr, query, userArgs, startTime);
                IAsyncResult res = wr.BeginGetResponse(new AsyncCallback(this.ResponseDownloadCompleted), asyncArgs);
#if !(PORTABLE40 || PORTABLE45)
                System.Threading.ThreadPool.RegisterWaitForSingleObject(res.AsyncWaitHandle, new System.Threading.WaitOrTimerCallback(this.ResponseDownloadTimeout), asyncArgs, this.Timeout, true);
#endif
            }
            catch (Exception ex)
            {
                System.Net.WebException dlException = this.GetOrCreateWebException(ex);
#if (PORTABLE40)
                ConnectionInfo conn = new ConnectionInfo(dlException, 0, startTime, System.DateTime.Now);
#else
                ConnectionInfo conn = new ConnectionInfo(dlException, this.Timeout, 0, startTime, System.DateTime.Now);
#endif
                if (AsyncDownloadCompleted != null) this.AsyncDownloadCompleted(this, new DownloadCompletedEventArgs<T>(userArgs, new Response<T>(conn, null, query)));
                if (AsyncIDownloadCompleted != null) this.AsyncIDownloadCompleted(this, new DownloadCompletedEventArgs<T>(userArgs, new Response<T>(conn, null, query)));
            }
        }

        private void ResponseDownloadCompleted(IAsyncResult result)
        {
            DateTime endTime = DateTime.Now;
            AsyncDownloadArgs asyncArgs = (AsyncDownloadArgs)result.AsyncState;
            Exception dlException = null;
            int size = 0;
            T res = null;

            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)asyncArgs.WR.EndGetResponse(result))
                {
#if !(PORTABLE40 || PORTABLE45)
                    if (!asyncArgs.TimedOut)
                    {
#endif
                        try
                        {
                            using (System.IO.Stream stream = resp.GetResponseStream())
                            {
                                using (System.IO.MemoryStream ms = MyHelper.CopyStream(stream))
                                {
                                    endTime = System.DateTime.Now;
#if !(PORTABLE40 || PORTABLE45)
                                    resp.Close();
#else
                                    resp.Dispose();
#endif
                                    if (ms != null && ms.CanSeek) int.TryParse(ms.Length.ToString(), out size);
                                    try
                                    { res = asyncArgs.Query.ConvertResultInternal(ms); }
                                    catch (Exception ex)
                                    { dlException = this.GetOrCreateParseException(ex); }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            dlException = this.GetOrCreateWebException(ex);
                            endTime = System.DateTime.Now;
                        }
#if !(PORTABLE40 || PORTABLE45)
                    }
                    else
                    {
                        dlException = new WebException("Timeout Exception.", null, WebExceptionStatus.Timeout, resp);
                        endTime = System.DateTime.Now;
                    }
#endif
                }
            }
            catch (Exception ex)
            {
                dlException = this.GetOrCreateWebException(ex);
            }

            this.RemoveDownload(asyncArgs.WR);

            if (AsyncDownloadCompleted != null || AsyncIDownloadCompleted != null)
            {
#if (PORTABLE40)            
                ConnectionInfo conn = new ConnectionInfo(dlException, size, asyncArgs.StartTime, endTime);
#else
                ConnectionInfo conn = new ConnectionInfo(dlException, asyncArgs.Timeout, size, asyncArgs.StartTime, endTime);
#endif
                DownloadCompletedEventArgs<T> args = new DownloadCompletedEventArgs<T>(asyncArgs.UserArgs, new Response<T>(conn, res, asyncArgs.Query));
#if (PORTABLE40 || PORTABLE45)
                if (AsyncDownloadCompleted != null) AsyncDownloadCompleted(this, args);
                if (AsyncIDownloadCompleted != null) AsyncIDownloadCompleted(this, args);
#else
                AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(this);
                if (AsyncDownloadCompleted != null) asyncOp.Post(new SendOrPostCallback(delegate(object obj) { AsyncDownloadCompleted(this, (DownloadCompletedEventArgs<T>)obj); }), args);
                if (AsyncIDownloadCompleted != null) asyncOp.Post(new SendOrPostCallback(delegate(object obj) { AsyncIDownloadCompleted(this, (IDownloadCompletedEventArgs)obj); }), args);
#endif
            }
        }

#if !(PORTABLE40 || PORTABLE45)
        private void ResponseDownloadTimeout(object state, bool timedOut)
        {
            if (timedOut)
            {
                AsyncDownloadArgs asyncArgs = (AsyncDownloadArgs)state;
                asyncArgs.TimedOut = true;
                asyncArgs.WR.Abort();
            }
        }
#endif

        private class AsyncDownloadArgs : DownloadEventArgs
        {
            public DateTime StartTime { get; private set; }
            public HttpWebRequest WR { get; private set; }
            public Query<T> Query { get; private set; }
#if !(PORTABLE40)
            public int Timeout { get; private set; }
            public bool TimedOut { get; set; }
#endif

            public AsyncDownloadArgs(HttpWebRequest wr, Query<T> query, object userArgs, DateTime st)
                : base(userArgs)
            {
                this.WR = wr;
                this.Query = query;
                this.StartTime = st;
            }
        }

        private bool DeepUserArgsEqual(object argsX, object argsY)
        {
            return object.ReferenceEquals(this.GetDeepUserArgs(argsX), this.GetDeepUserArgs(argsY));
        }
        private object GetDeepUserArgs(object args)
        {
            if (args != null && args is DownloadEventArgs) { return this.GetDeepUserArgs(((DownloadEventArgs)args).UserArgs); }
            else { return args; }
        }

        #endregion




        #region "HCTaskAsync"

#if (NET45 || PORTABLE45)
        async Task<IResponse> IDownload.GetResponseTaskAsync() { return await this.DownloadTaskAsync(); }
        public async Task<Response<T>> DownloadTaskAsync() { return await this.DownloadTaskAsync(this.DefaultQuery); }
        public async Task<Response<T>> DownloadTaskAsync(Query<T> query)
        {
            query = this.CheckQuery(query);

            System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
            hc.Timeout = TimeSpan.FromMilliseconds(this.Timeout);

            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime;

            Exception dlException = null;
            int size = -1;
            T result = null;

            this.AddDownload(hc, query, startTime, null);

            try
            {
                startTime = DateTime.Now;
                using (System.Net.Http.HttpResponseMessage resp = await hc.GetAsync(query.GetUrlInternal()))
                {
                    using (System.IO.Stream stream = await resp.Content.ReadAsStreamAsync())
                    {
                        using (System.IO.MemoryStream ms = MaasOne.MyHelper.CopyStream(stream))
                        {
                            endTime = DateTime.Now;
                            resp.Dispose();
                            if (ms != null && ms.CanSeek) int.TryParse(ms.Length.ToString(), out size);
                            try
                            { result = query.ConvertResultInternal(ms); }
                            catch (Exception ex)
                            { dlException = this.GetOrCreateParseException(ex); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dlException = this.GetOrCreateWebException(ex);
                endTime = DateTime.Now;
            }

            this.RemoveDownload(hc);

            ConnectionInfo conn = new ConnectionInfo(dlException, (int)hc.Timeout.TotalMilliseconds, size, startTime, endTime);

            return new Response<T>(conn, result, query);
        }
#endif

        #endregion




        #region "WRSync"

#if (NET20 || NET45)
        IResponse IDownload.GetResponse() { return this.Download(); }
        public Response<T> Download() { return this.Download(this.DefaultQuery); }
        public Response<T> Download(Query<T> query)
        {
            query = this.CheckQuery(query);

            HttpWebRequest wr = this.CreateWebRequest(query);

            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime;
            Exception dlException = null;
            int size = -1;
            T result = null;

            try
            {
                this.AddDownload(wr, query, startTime, null);
                startTime = DateTime.Now;
                using (HttpWebResponse resp = (HttpWebResponse)wr.GetResponse())
                {
                    using (System.IO.Stream stream = resp.GetResponseStream())
                    {
                        using (System.IO.MemoryStream ms = MaasOne.MyHelper.CopyStream(stream))
                        {
                            resp.Close();
                            endTime = DateTime.Now;
                            if (ms != null && ms.CanSeek) int.TryParse(ms.Length.ToString(), out size);
                            try
                            { result = query.ConvertResultInternal(ms); }
                            catch (Exception ex)
                            { dlException = this.GetOrCreateParseException(ex); }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                dlException = this.GetOrCreateWebException(ex);
                endTime = DateTime.Now;
            }

            this.RemoveDownload(wr);

            ConnectionInfo conn = new ConnectionInfo(dlException, wr.Timeout, size, startTime, endTime);

            return new Response<T>(conn, result, query);
        }
#endif

        #endregion




        #region "Internal"

        private Query<T> CheckQuery(Query<T> query)
        {
            if (query == null) { throw new ArgumentNullException("query", "The Query for downloading " + typeof(T).Name + " is NULL."); }
            query = query.Clone();
            ValidationResult valid = new ValidationResult();
            query.ValidateQueryInternal(valid);
            if (valid.Success == false) throw valid.CreateException();
            return query;
        }
        private HttpWebRequest CreateWebRequest(Query<T> query)
        {
            Uri url = query.GetUrlInternal();
            if (url == null) throw new Exception("The created URL is NULL.");
            if (url.IsAbsoluteUri == false) throw new Exception("The URI is not absolute.");
            HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(url);
            wr.Method = "GET";
#if !(PORTABLE40 || PORTABLE45)
            wr.Timeout = this.Timeout;
            wr.ReadWriteTimeout = this.Timeout;
#endif
#if !(PORTABLE40)
            if (this.Proxy != null) wr.Proxy = this.Proxy;
#endif
            return wr;
        }
        private WebException GetOrCreateWebException(Exception ex)
        {
            if (ex is WebException) { return (WebException)ex; }
            else { return new System.Net.WebException("An Exception was thrown during download process. See InnerException for more details.", ex, System.Net.WebExceptionStatus.UnknownError, null); }
        }
        private ParseException GetOrCreateParseException(Exception ex)
        {
            if (ex is ParseException) { return (ParseException)ex; }
            else { return new ParseException(ex); }
        }

        private void AddDownload(object clientObject, Query<T> query, DateTime startTime, object args)
        {
            this.ActiveSessions.Add(new Net.DownloadSession(clientObject, query, args));
        }
        private bool RemoveDownload(object clientObject)
        {
            return this.ActiveSessions.Remove(clientObject);
        }


        #endregion

    }

}

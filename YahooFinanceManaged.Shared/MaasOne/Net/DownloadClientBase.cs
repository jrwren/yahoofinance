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
using System.Collections.Generic;

namespace MaasOne.Net
{
    /// <summary>
    /// Provides methods and properties for downloading data and controlling these download operations.
    /// </summary>
    /// <remarks></remarks>
    public abstract class DownloadClientBase
    {
        private int timeout = 30000;



        public DownloadClientBase()
        {
            this.ActiveOperations = new DownloadOperationCollection();
        }



        /// <summary>
        /// Gets a collection of active download operations.
        /// </summary>
        public DownloadOperationCollection ActiveOperations { get; private set; }

#if !(SILVERLIGHT)
        /// <summary>
        /// Gets or sets the network proxy to use to access this Internet resource.
        /// </summary>
        public IWebProxy Proxy { get; set; }
#endif

        /// <summary>
        /// Gets or sets the length of time, in milliseconds, before the request times out.
        /// </summary>
        public int Timeout
        {
            get { return this.timeout; }
            set { this.timeout = Math.Min(Math.Max(0, value), 300000); }
        }



#if !(NETFX_CORE || SILVERLIGHT)
        /// <summary>
        /// Starts a data download and conversion operation.
        /// </summary>
        /// <param name="query">The query that is used for creating the request.</param>
        /// <exception cref="System.ArgumentException">Thrown when the passed query is invalid.</exception>
        /// <returns>The response object containing the generic result data and additional information.</returns>
        protected StreamResponse Download(Uri url)
        {
            DateTime startTime = DateTime.UtcNow;
            ConnectionState state = ConnectionState.Success;
            Exception dlException = null;
            int size = -1;
            System.IO.Stream result = null;

            HttpWebRequest wr = this.CreateWebRequest(url);

            try
            {
                this.AddDownload(wr, startTime, null);

                using (HttpWebResponse resp = (HttpWebResponse)wr.GetResponse())
                {
                    using (System.IO.Stream stream = resp.GetResponseStream())
                    {
                        result = MaasOne.MyHelper.CopyStream(stream);
                        resp.Close();
                        if (result != null && result.CanSeek) int.TryParse(result.Length.ToString(), out size);
                    }
                }
            }
            catch (WebException ex)
            {
                state = ConnectionState.ErrorOccured;
                if (ex.Status == WebExceptionStatus.RequestCanceled)
                { state = ConnectionState.Canceled; }
                else if (ex.Status == WebExceptionStatus.Timeout)
                { state = ConnectionState.Timeout; }
                dlException = ex;
            }
            catch (Exception ex)
            {
                state = ConnectionState.ErrorOccured;
                dlException = this.GetOrCreateWebException(ex);
            }

            this.RemoveDownload(wr);

            ConnectionInfo conn = new ConnectionInfo(state, dlException, wr.Timeout, size, startTime, DateTime.UtcNow);

            return new StreamResponse(conn, result);
        }
#endif

        /// <summary>
        /// Starts an asynchronous data download and conversion operation.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the <see cref="DefaultQuery"/> is invalid.</exception>
        /// <param name="query">The query that is used for creating the request.</param>
        /// <param name="userArgs">Individual user arguments that will be passed to the <see cref="AsyncDownloadCompletedEventHandler{T}"/> when <see cref="AsyncDownloadCompleted"/> event is fired.</param>
        /// <remarks><see cref="AsyncDownloadCompleted"/> event is fired when download and conversion operation is completed.</remarks>
        protected void DownloadAsync(Uri url, object userArgs)
        {
            DateTime startTime = DateTime.UtcNow;

            HttpWebRequest wr = this.CreateWebRequest(url);

            try
            {
                this.AddDownload(wr, startTime, userArgs);

                AsyncDownloadArgs asyncArgs = new AsyncDownloadArgs(wr, userArgs, startTime, this.Timeout);

                IAsyncResult res = wr.BeginGetResponse(new AsyncCallback(this.ResponseDownloadCompleted), asyncArgs);
#if !(NETFX_CORE)
                System.Threading.ThreadPool.RegisterWaitForSingleObject(res.AsyncWaitHandle,
                                                                        new System.Threading.WaitOrTimerCallback((object state, bool timedOut) =>
                                                                        { if (timedOut) this.TimeoutAsync(state); }),
                                                                        asyncArgs,
                                                                        this.Timeout,
                                                                        true);
#else
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    if (res.AsyncWaitHandle.WaitOne(asyncArgs.Timeout) == false)
                        this.TimeoutAsync(asyncArgs);
                    return;
                });
#endif
            }
            catch (Exception ex)
            {
                System.Net.WebException dlException = this.GetOrCreateWebException(ex);
                ConnectionInfo conn = new ConnectionInfo(ConnectionState.ErrorOccured, dlException, this.Timeout, -1, startTime, DateTime.UtcNow);
                this.RaiseAsyncDownloadCompleted(new StreamResponse(conn, null), userArgs);
            }
        }

#if !(NET20 || NET35 || NET40 || SILVERLIGHT)
        /// <summary>
        /// Starts an async data download and conversion operation.
        /// </summary>
        /// <param name="query">The query that is used for creating the request.</param>
        /// <exception cref="System.ArgumentException">Thrown when the <see cref="DefaultQuery"/> is invalid.</exception>
        /// <returns>The task object representing the async download operation.</returns>
        protected async System.Threading.Tasks.Task<StreamResponse> DownloadTaskAsync(Uri url)
        {
            DateTime startTime = DateTime.UtcNow;

            System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
            hc.Timeout = TimeSpan.FromMilliseconds(this.Timeout);


            ConnectionState state = ConnectionState.Success;
            Exception dlException = null;
            int size = -1;
            System.IO.Stream result = null;

            this.AddDownload(hc, startTime, null);

            var cts = new System.Threading.CancellationTokenSource();
            try
            {
                startTime = DateTime.Now;
                using (System.Net.Http.HttpResponseMessage resp = await hc.GetAsync(url, cts.Token))
                {
                    using (System.IO.Stream stream = await resp.Content.ReadAsStreamAsync())
                    {
                        result = MaasOne.MyHelper.CopyStream(stream);

                        resp.Dispose();
                        if (result != null && result.CanSeek) int.TryParse(result.Length.ToString(), out size);
                    }
                }
            }
            catch (WebException ex)
            {
                state = ConnectionState.ErrorOccured;
                if (ex.Status == WebExceptionStatus.RequestCanceled)
                { state = ConnectionState.Canceled; }
                dlException = ex;
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                { state = ConnectionState.Canceled; }
                else
                { state = ConnectionState.Timeout; }
                dlException = this.GetOrCreateWebException(ex);
            }
            catch (Exception ex)
            {
                state = ConnectionState.ErrorOccured;
                dlException = this.GetOrCreateWebException(ex);
            }

            this.RemoveDownload(hc);

            ConnectionInfo conn = new ConnectionInfo(state, dlException, (int)hc.Timeout.TotalMilliseconds, size, startTime, DateTime.UtcNow);

            return new StreamResponse(conn, result);
        }
#endif

        protected HttpWebRequest CreateWebRequest(Uri url)
        {
            if (url == null) throw new Exception("URI is NULL.");
            if (url.IsAbsoluteUri == false) throw new Exception("URI is not absolute.");

            HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(url);
            wr.Method = "GET";
#if !(NETFX_CORE || SILVERLIGHT)
            wr.Timeout = this.Timeout;
            wr.ReadWriteTimeout = this.Timeout;
#endif
#if !(SILVERLIGHT)
            if (this.Proxy != null) wr.Proxy = this.Proxy;
#endif

            return wr;
        }

        protected WebException GetOrCreateWebException(Exception ex)
        {
            if (ex is WebException) { return (WebException)ex; }
            else { return new System.Net.WebException("An Exception was thrown during download operation. See InnerException for more details.", ex, System.Net.WebExceptionStatus.UnknownError, null); }
        }

        protected ParseException GetOrCreateParseException(Exception ex)
        {
            if (ex is ParseException) { return (ParseException)ex; }
            else { return new ParseException(ex); }
        }


        protected abstract void RaiseAsyncDownloadCompleted(StreamResponse response, object userArgs);


        private void AddDownload(object clientObject, DateTime startTime, object args) { this.ActiveOperations.Add(new Net.DownloadOperation(clientObject, args)); }

        private bool RemoveDownload(object clientObject) { return this.ActiveOperations.Remove(clientObject); }

        private void ResponseDownloadCompleted(IAsyncResult result)
        {
            AsyncDownloadArgs asyncArgs = (AsyncDownloadArgs)result.AsyncState;
            ConnectionState state = ConnectionState.Success;
            Exception dlException = null;
            int size = 0;
            System.IO.Stream res = null;

            if (!asyncArgs.TimedOut)
            {
                try
                {
                    using (HttpWebResponse resp = (HttpWebResponse)asyncArgs.WR.EndGetResponse(result))
                    {
                        using (System.IO.Stream stream = resp.GetResponseStream())
                        {
                            res = MyHelper.CopyStream(stream);
#if !(NETFX_CORE)
                            resp.Close();
#else
                            resp.Dispose();
#endif
                            if (res != null && res.CanSeek) int.TryParse(res.Length.ToString(), out size);
                        }
                    }
                }
                catch (WebException ex)
                {
                    state = ConnectionState.ErrorOccured;
                    if (ex.Status == WebExceptionStatus.RequestCanceled)
                        state = ConnectionState.Canceled;
                    dlException = ex;
                }
                catch (Exception ex)
                {
                    state = ConnectionState.ErrorOccured;
                    dlException = this.GetOrCreateWebException(ex);
                }
            }
            else
            {
                state = ConnectionState.Timeout;
            }


            this.RemoveDownload(asyncArgs.WR);

            ConnectionInfo conn = new ConnectionInfo(state, dlException, asyncArgs.Timeout, size, asyncArgs.StartTime, DateTime.UtcNow);

            StreamResponse strResp = new StreamResponse(conn, res);
#if !(NETFX_CORE)
            System.ComponentModel.AsyncOperationManager.CreateOperation(this).Post(new System.Threading.SendOrPostCallback(delegate(object obj)
            {
                object[] arr = (object[])obj;
                RaiseAsyncDownloadCompleted((StreamResponse)arr[0], arr[1]);

            }), new object[] {
                    strResp,
                    asyncArgs.UserArgs
                });
#else
            RaiseAsyncDownloadCompleted(strResp, asyncArgs.UserArgs);
#endif
        }

        private void TimeoutAsync(object asyncArgs)
        {
            AsyncDownloadArgs aa = (AsyncDownloadArgs)asyncArgs;
            aa.TimedOut = true;
            aa.WR.Abort();
        }



        private class AsyncDownloadArgs : DownloadEventArgs
        {
            public DateTime StartTime { get; private set; }
            public HttpWebRequest WR { get; private set; }
            public int Timeout { get; private set; }
            public bool TimedOut { get; set; }

            public AsyncDownloadArgs(HttpWebRequest wr, object userArgs, DateTime st, int to)
                : base(userArgs)
            {
                this.WR = wr;
                this.StartTime = st;
                this.Timeout = to;
            }
        }
    }
}

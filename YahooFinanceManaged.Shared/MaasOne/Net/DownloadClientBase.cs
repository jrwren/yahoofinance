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
        /// <summary>
        /// Gets a collection of active download operations.
        /// </summary>
        public DownloadOperationCollection ActiveOperations { get; private set; }

#if !(PCL40)
        /// <summary>
        /// Gets or sets the network proxy to use to access this Internet resource.
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Gets or sets the length of time, in milliseconds, before the request times out.
        /// </summary>
        public int Timeout
        {
            get { return mTimeout; }
            set { mTimeout = Math.Min(Math.Max(0, value), 360000); }
        }
        private int mTimeout = 30000;
#endif


        public DownloadClientBase()
        {
            this.ActiveOperations = new DownloadOperationCollection();
        }



        #region "Sync"

#if !(PCL40 || PCL45)
        /// <summary>
        /// Starts a data download and conversion operation.
        /// </summary>
        /// <param name="query">The query that is used for creating the request.</param>
        /// <exception cref="System.ArgumentException">Thrown when the passed query is invalid.</exception>
        /// <returns>The response object containing the generic result data and additional information.</returns>
        protected StreamResponse Download(Uri url)
        {
            DateTime startTime = DateTime.UtcNow;
            DateTime endTime = startTime;
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
                        endTime = DateTime.UtcNow;
                        if (result != null && result.CanSeek) int.TryParse(result.Length.ToString(), out size);
                    }
                }
            }
            catch (Exception ex)
            {
                dlException = this.GetOrCreateWebException(ex);
                endTime = DateTime.UtcNow;
            }

            this.RemoveDownload(wr);

            ConnectionInfo conn = new ConnectionInfo(dlException, wr.Timeout, size, startTime, endTime);

            return new StreamResponse(conn, result);
        }
#endif

        #endregion



        #region "Async"

        /// <summary>
        /// Starts an asynchronous data download and conversion operation.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the <see cref="DefaultQuery"/> is invalid.</exception>
        /// <param name="query">The query that is used for creating the request.</param>
        /// <param name="userArgs">Individual user arguments that will be passed to the <see cref="AsyncDownloadCompletedEventHandler{T}"/> when <see cref="AsyncDownloadCompleted"/> event is fired.</param>
        /// <remarks><see cref="AsyncDownloadCompleted"/> event is fired when download and conversion operation is completed.</remarks>
        protected void DownloadAsync(Uri url, QueryBase[] queries, object userArgs)
        {
            DateTime startTime = DateTime.UtcNow;

            HttpWebRequest wr = this.CreateWebRequest(url);

            try
            {
                this.AddDownload(wr, startTime, userArgs);

                AsyncDownloadArgs asyncArgs = new AsyncDownloadArgs(wr, queries, userArgs, startTime);
#if !(PCL40 || PCL45)
                asyncArgs.Timeout = this.Timeout;
#endif
                IAsyncResult res = wr.BeginGetResponse(new AsyncCallback(this.ResponseDownloadCompleted), asyncArgs);
#if !(PCL40 || PCL45)
                System.Threading.ThreadPool.RegisterWaitForSingleObject(res.AsyncWaitHandle, new System.Threading.WaitOrTimerCallback(this.ResponseDownloadTimeout), asyncArgs, this.Timeout, true);
#endif
            }
            catch (Exception ex)
            {
                System.Net.WebException dlException = this.GetOrCreateWebException(ex);
#if (PCL40)
                ConnectionInfo conn = new ConnectionInfo(dlException, 0, startTime, DateTime.UtcNow);
#else
                ConnectionInfo conn = new ConnectionInfo(dlException, this.Timeout, 0, startTime, DateTime.UtcNow);
#endif
                this.RaiseAsyncDownloadCompleted(new StreamResponse(conn, null), queries, userArgs);
            }
        }

        private void ResponseDownloadCompleted(IAsyncResult result)
        {
            AsyncDownloadArgs asyncArgs = (AsyncDownloadArgs)result.AsyncState;
            DateTime endTime = DateTime.UtcNow;
            Exception dlException = null;
            int size = 0;
            System.IO.Stream res = null;

            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)asyncArgs.WR.EndGetResponse(result))
                {
#if !(PCL40 || PCL45)
                    if (!asyncArgs.TimedOut)
                    {
#endif
                        using (System.IO.Stream stream = resp.GetResponseStream())
                        {
                            res = MyHelper.CopyStream(stream);
#if !(PCL40 || PCL45)
                            resp.Close();
#else
                            resp.Dispose();
#endif
                            endTime = DateTime.UtcNow;
                            if (res != null && res.CanSeek) int.TryParse(res.Length.ToString(), out size);
                        }
#if !(PCL40 || PCL45)
                    }
                    else
                    {
                        dlException = new WebException("Timeout Exception.", null, WebExceptionStatus.Timeout, resp);
                        endTime = DateTime.UtcNow;
                    }
#endif
                }
            }
            catch (Exception ex)
            {
                dlException = this.GetOrCreateWebException(ex);
                endTime = DateTime.UtcNow;
            }

            this.RemoveDownload(asyncArgs.WR);


#if (PCL40)
            ConnectionInfo conn = new ConnectionInfo(dlException, size, asyncArgs.StartTime, endTime);
#else
            ConnectionInfo conn = new ConnectionInfo(dlException, asyncArgs.Timeout, size, asyncArgs.StartTime, endTime);
#endif
            StreamResponse strResp = new StreamResponse(conn, res);
#if (PCL40 || PCL45)
            RaiseAsyncDownloadCompleted(strResp, asyncArgs.Queries, asyncArgs.UserArgs);
#else
            System.ComponentModel.AsyncOperationManager.CreateOperation(this).Post(new System.Threading.SendOrPostCallback(delegate(object obj)
            {
                object[] arr = (object[])obj;
                RaiseAsyncDownloadCompleted((StreamResponse)arr[0], (QueryBase[])arr[1], arr[2]);

            }), new object[] {
                    strResp,
                    asyncArgs.Queries,
                    asyncArgs.UserArgs
                });
#endif
        }

#if !(PCL40 || PCL45)
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
        protected abstract void RaiseAsyncDownloadCompleted(StreamResponse response, QueryBase[] queries, object userArgs);

        private class AsyncDownloadArgs : DownloadEventArgs
        {
            public DateTime StartTime { get; private set; }
            public HttpWebRequest WR { get; private set; }
            public QueryBase[] Queries { get; private set; }
#if !(PCL40)
            public int Timeout { get; internal set; }
            public bool TimedOut { get; set; }
#endif
            public AsyncDownloadArgs(HttpWebRequest wr, QueryBase[] queries, object userArgs, DateTime st)
                : base(userArgs)
            {
                this.WR = wr;
                this.Queries = queries;
                this.StartTime = st;
            }
        }

        #endregion



        #region "TaskAsync"

#if !(NET20 || PCL40)
        /// <summary>
        /// Starts an async data download and conversion operation.
        /// </summary>
        /// <param name="query">The query that is used for creating the request.</param>
        /// <exception cref="System.ArgumentException">Thrown when the <see cref="DefaultQuery"/> is invalid.</exception>
        /// <returns>The task object representing the async download operation.</returns>
        protected async System.Threading.Tasks.Task<StreamResponse> DownloadTaskAsync(Uri url)
        {

            System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
            hc.Timeout = TimeSpan.FromMilliseconds(this.Timeout);

            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime;

            Exception dlException = null;
            int size = -1;
            System.IO.Stream result = null;

            this.AddDownload(hc, startTime, null);

            try
            {
                startTime = DateTime.Now;
                using (System.Net.Http.HttpResponseMessage resp = await hc.GetAsync(url))
                {
                    using (System.IO.Stream stream = await resp.Content.ReadAsStreamAsync())
                    {
                        result = MaasOne.MyHelper.CopyStream(stream);

                        endTime = DateTime.Now;
                        resp.Dispose();
                        if (result != null && result.CanSeek) int.TryParse(result.Length.ToString(), out size);
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

            return new StreamResponse(conn, result);
        }
#endif

        #endregion



        #region "Private"

        protected HttpWebRequest CreateWebRequest(Uri url)
        {
            if (url == null) throw new Exception("URI is NULL.");
            if (url.IsAbsoluteUri == false) throw new Exception("URI is not absolute.");

            HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(url);
            wr.Method = "GET";
#if !(PCL40 || PCL45)
            wr.Timeout = this.Timeout;
            wr.ReadWriteTimeout = this.Timeout;
#endif
#if !(PCL40)
            if (this.Proxy != null) wr.Proxy = this.Proxy;
#endif

            return wr;
        }

        private WebException GetOrCreateWebException(Exception ex)
        {
            if (ex is WebException) { return (WebException)ex; }
            else { return new System.Net.WebException("An Exception was thrown during download operation. See InnerException for more details.", ex, System.Net.WebExceptionStatus.UnknownError, null); }
        }

        protected ParseException GetOrCreateParseException(Exception ex)
        {
            if (ex is ParseException) { return (ParseException)ex; }
            else { return new ParseException(ex); }
        }

        private void AddDownload(object clientObject, DateTime startTime, object args)
        {
            this.ActiveOperations.Add(new Net.DownloadOperation(clientObject, args));
        }

        private bool RemoveDownload(object clientObject)
        {
            return this.ActiveOperations.Remove(clientObject);
        }

        #endregion
    }
}

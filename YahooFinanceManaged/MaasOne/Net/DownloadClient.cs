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
using Newtonsoft.Json.Linq;

namespace MaasOne.Net
{
    public delegate void AsyncDownloadCompletedEventHandler(object sender, DownloadCompletedEventArgs e);

    /// <summary>
    /// Delegate for completed, asynchronous and generic download operations.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="sender">The <see cref="DownloadClient{T}"/> that was used for executing the download operation.</param>
    /// <param name="e">The event argument for the completed download operation.</param>
    public delegate void AsyncDownloadCompletedEventHandler<T>(object sender, DownloadCompletedEventArgs<T> e) where T : ResultBase;

    public delegate void YqlAsyncDownloadCompletedEventHandler<T>(object sender, YqlDownloadCompletedEventArgs<T> e) where T : ResultBase;

    public interface IQueryDownload
    {
#if !(PORTABLE40)
        IWebProxy Proxy { get; set; }

        int Timeout { get; set; }
#endif

        DownloadOperationCollection ActiveOperations { get; }

        QueryBase DefaultQuery { get; set; }


#if (NET20 || NET45)
        IQueryResponse Download();

        IQueryResponse Download(QueryBase query);
#endif


        event AsyncDownloadCompletedEventHandler AsyncDownloadCompletedBase;

        void DownloadAsync(object userArgs);

        void DownloadAsync(QueryBase query, object userArgs);


#if (NET45 || PORTABLE45)
        System.Threading.Tasks.Task<IQueryResponse> DownloadTaskAsync();

        System.Threading.Tasks.Task<IQueryResponse> DownloadTaskAsync(QueryBase query);
#endif


        bool IsCorrespondingType(QueryBase query);
    }

    public class DownloadClient<T> : DownloadClientBase, IQueryDownload where T : ResultBase
    {
        /// <summary>
        /// Gets or sets the default query object that will be used when a download method without query parameter is called.
        /// </summary>
        public Query<T> DefaultQuery { get; set; }
        QueryBase IQueryDownload.DefaultQuery
        {
            get { return this.DefaultQuery; }
            set
            {
                if (((IQueryDownload)this).IsCorrespondingType(value) == false) throw new ArgumentException("Query type is not corresponding.", "value");
                this.DefaultQuery = (Query<T>)value;
            }
        }


        public DownloadClient() { }

        public DownloadClient(Query<T> defaultQuery) : this() { this.DefaultQuery = defaultQuery; }


#if (NET20 || NET45)
        IQueryResponse IQueryDownload.Download() { return ((IQueryDownload)this).Download(((IQueryDownload)this).DefaultQuery); }

        IQueryResponse IQueryDownload.Download(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return this.Download((Query<T>)query);
        }

        /// <summary>
        /// Starts a data download and conversion operation.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the <see cref="DefaultQuery"/> is invalid.</exception>
        /// <returns>The <see cref="Response{T}"/> containing the generic result data and additional information.</returns>
        /// <remarks>The query object of <see cref="DefaultQuery"/> property is used for creating the request.</remarks>
        public Response<T> Download() { return this.Download(this.DefaultQuery); }

        public Response<T> Download(Query<T> query)
        {
            query = this.CheckQuery(query);
            Uri url = this.CreateUrl(query);

            Response<T> response = null;

            using (StreamResponse resp = this.Download(url))
            {
                response = this.ConvertResponse(resp, query);
            }

            return response;
        }
#endif


        /// <summary>
        /// Occurs when an asynchronous download and conversion operation completes.
        /// </summary>
        public event AsyncDownloadCompletedEventHandler<T> AsyncDownloadCompleted;

        /// <summary>
        /// Occurs when an asynchronous download and conversion operation completes.
        /// </summary>
        public event AsyncDownloadCompletedEventHandler AsyncDownloadCompletedBase;

        /// <summary>
        /// Starts an asynchronous data download and conversion operation.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the <see cref="DefaultQuery"/> is invalid.</exception>
        /// <param name="userArgs">Individual user arguments that will be passed to the <see cref="AsyncDownloadCompletedEventHandler{T}"/> when <see cref="AsyncDownloadCompleted"/> event is fired.</param>
        /// <remarks>The query object of <see cref="DefaultQuery"/> property is used for creating the request. <see cref="AsyncDownloadCompleted"/> event is fired when download and conversion operation is completed.</remarks>
        public void DownloadAsync(object userArgs) { this.DownloadAsync(this.DefaultQuery, userArgs); }

        void IQueryDownload.DownloadAsync(QueryBase query, object userArgs)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            this.DownloadAsync((Query<T>)query, userArgs);
        }

        public void DownloadAsync(Query<T> query, object userArgs)
        {
            query = this.CheckQuery(query);
            Uri url = this.CreateUrl(query);
            base.DownloadAsync(url, new QueryBase[] { query }, userArgs);
        }

        protected override void RaiseAsyncDownloadCompleted(StreamResponse response, QueryBase[] queries, object userArgs)
        {
            Query<T> query = (Query<T>)queries[0];
            Response<T> resp = null;

            using (response)
            {
                resp = this.ConvertResponse(response, query);
            }
            if (AsyncDownloadCompletedBase != null) this.AsyncDownloadCompletedBase(this, new DownloadCompletedEventArgs(userArgs, resp));
            if (AsyncDownloadCompleted != null) this.AsyncDownloadCompleted(this, new DownloadCompletedEventArgs<T>(userArgs, resp));
        }


#if (NET45 || PORTABLE45)

        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync() { return await ((IQueryDownload)this).DownloadTaskAsync(((IQueryDownload)this).DefaultQuery); }

        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return await this.DownloadTaskAsync((Query<T>)query);
        }

        /// <summary>
        /// Starts an async data download and conversion operation.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the <see cref="DefaultQuery"/> is invalid.</exception>
        /// <returns>The task object representing the async download operation.</returns>
        /// <remarks>The query object of <see cref="DefaultQuery"/> property is used for creating the request.</remarks>
        public async System.Threading.Tasks.Task<Response<T>> DownloadTaskAsync() { return await this.DownloadTaskAsync(this.DefaultQuery); }

        public async System.Threading.Tasks.Task<Response<T>> DownloadTaskAsync(Query<T> query)
        {
            query = this.CheckQuery(query);
            Uri url = this.CreateUrl(query);

            Response<T> response = null;

            using (StreamResponse resp = await this.DownloadTaskAsync(url))
            {
                response = this.ConvertResponse(resp, query);
            }

            return response;
        }
#endif


        bool IQueryDownload.IsCorrespondingType(QueryBase query) { return query is Query<T>; }


        private Query<T> CheckQuery(Query<T> query)
        {
            if (query == null) { throw new ArgumentNullException("query", "Query<" + typeof(T).Name + "> is NULL."); }
            query = (Query<T>)query.Clone();
            ValidationResult valid = query.Validate();
            if (valid.Success == false) throw valid.CreateException();
            return query;
        }

        private Response<T> ConvertResponse(StreamResponse response, Query<T> query)
        {
            Exception exc = null;
            T result = default(T);
            if (response.Connection.State == ConnectionState.Success)
            {
                try
                {
                    QueryBase.ConvertInfo ci = new QueryBase.ConvertInfo(true);
                    result = query.ConvertResultInternal(response.Result, ci);
                    result.Integrity = new DataIntegrityInfo(ci.IsIntegrityComplete, ci.IntegrityMessages.ToArray());
                }
                catch (Exception ex)
                {
                    exc = this.GetOrCreateParseException(ex);
                }
            }
            else
            {
                exc = response.Connection.Exception;
            }

            return new Response<T>(new ConnectionInfo(
                       exc,
#if !(PORTABLE40)
 response.Connection.Timeout,
#endif
 response.Connection.SizeInBytes,
                       response.Connection.StartTime,
                       response.Connection.EndTime), result, query);
        }

        private Uri CreateUrl(Query<T> query) { return new Uri(query.CreateUrlInternal(), UriKind.Absolute); }
    }







    public class YqlDownloadClient<T> : DownloadClientBase, IQueryDownload where T : ResultBase
    {
        public YqlQuery<T>[] DefaultQueries { get; set; }
        QueryBase IQueryDownload.DefaultQuery
        {
            get { return this.DefaultQueries != null && this.DefaultQueries.Length > 0 ? this.DefaultQueries[0] : null; }
            set
            {
                if (value != null)
                {
                    if (((IQueryDownload)this).IsCorrespondingType(value) == false) throw new ArgumentException("Query type is not corresponding.", "value");
                    this.DefaultQueries = new YqlQuery<T>[] { (YqlQuery<T>)value };
                }
                else
                {
                    this.DefaultQueries = new YqlQuery<T>[0]; 
                }
            }
        }

        public bool GetDiagnostics { get; set; }

        
        public YqlDownloadClient() { }

        public YqlDownloadClient(YqlQuery<T>[] defaultQueries) : this() { this.DefaultQueries = defaultQueries; }



#if (NET20 || NET45)
        IQueryResponse IQueryDownload.Download() { return ((IQueryDownload)this).Download(((IQueryDownload)this).DefaultQuery); }

        IQueryResponse IQueryDownload.Download(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return this.Download((YqlQuery<T>)query);
        }

        public YqlResponse<T> Download() { return this.Download(this.DefaultQueries); }

        public YqlResponse<T> Download(YqlQuery<T> query) { return this.Download(new YqlQuery<T>[] { query }); }

        public YqlResponse<T> Download(IEnumerable<YqlQuery<T>> queries)
        {
            YqlQuery<T>[] queriesArr = this.CheckQueries(queries);
            Uri url = this.CreateUrl(queriesArr);

            YqlResponse<T> response = null;

            using (StreamResponse resp = this.Download(url))
            {
                response = this.ConvertResponse(resp, queriesArr);
            }

            return response;
        }
#endif


        /// <summary>
        /// Occurs when an asynchronous download and conversion operation completes.
        /// </summary>
        public event YqlAsyncDownloadCompletedEventHandler<T> AsyncDownloadCompleted;

        /// <summary>
        /// Occurs when an asynchronous download and conversion operation completes.
        /// </summary>
        public event AsyncDownloadCompletedEventHandler AsyncDownloadCompletedBase;

        void IQueryDownload.DownloadAsync(QueryBase query, object userArgs)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            this.DownloadAsync((YqlQuery<T>)query, userArgs);
        }

        public void DownloadAsync(object userArgs) { this.DownloadAsync(this.DefaultQueries, userArgs); }

        public void DownloadAsync(YqlQuery<T> query, object userArgs) { this.DownloadAsync(new YqlQuery<T>[] { query }, userArgs); }

        public void DownloadAsync(IEnumerable<YqlQuery<T>> queries, object userArgs)
        {
            YqlQuery<T>[] queriesArr = this.CheckQueries(queries);
            Uri url = this.CreateUrl(queriesArr);
            base.DownloadAsync(url, queriesArr, userArgs);
        }

        protected override void RaiseAsyncDownloadCompleted(StreamResponse response, QueryBase[] queries, object userArgs)
        {
            YqlQuery<T>[] yqlQueries = (YqlQuery<T>[])queries;
            YqlResponse<T> resp = null;

            using (response)
            {
                resp = this.ConvertResponse(response, yqlQueries);
            }

            if (AsyncDownloadCompletedBase != null) this.AsyncDownloadCompletedBase(this, new DownloadCompletedEventArgs(userArgs, resp));
            if (AsyncDownloadCompleted != null) this.AsyncDownloadCompleted(this, new YqlDownloadCompletedEventArgs<T>(userArgs, resp));
        }


#if (NET45 || PORTABLE45)
        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync() { return await ((IQueryDownload)this).DownloadTaskAsync(((IQueryDownload)this).DefaultQuery); }

        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return await this.DownloadTaskAsync((YqlQuery<T>)query);
        }

        public async System.Threading.Tasks.Task<YqlResponse<T>> DownloadTaskAsync() { return await this.DownloadTaskAsync(this.DefaultQueries); }

        public async System.Threading.Tasks.Task<YqlResponse<T>> DownloadTaskAsync(YqlQuery<T> query) { return await this.DownloadTaskAsync(new YqlQuery<T>[] { query }); }

        public async System.Threading.Tasks.Task<YqlResponse<T>> DownloadTaskAsync(IEnumerable<YqlQuery<T>> queries)
        {
            YqlQuery<T>[] queriesArr = this.CheckQueries(queries);
            Uri url = this.CreateUrl(queriesArr);

            YqlResponse<T> response = null;

            using (StreamResponse resp = await base.DownloadTaskAsync(url))
            {
                response = this.ConvertResponse(resp, queriesArr);
            }

            return response;
        }
#endif


        bool IQueryDownload.IsCorrespondingType(QueryBase query) { return query is YqlQuery<T>; }


        private YqlQuery<T>[] CheckQueries(IEnumerable<YqlQuery<T>> queries)
        {
            ValidationResult valid = new ValidationResult();
            List<YqlQuery<T>> lstQueries = new List<YqlQuery<T>>();

            if (queries == null)
            {
                valid.Success = false;
                valid.Info.Add("queries", "queries is NULL.");
            }
            else
            {
                foreach (var q in queries) { if (q != null)  lstQueries.Add((YqlQuery<T>)q); }
                if (lstQueries.Count == 0)
                {
                    valid.Success = false;
                    valid.Info.Add("queries", "No Query available.");
                }
                else
                {
                    string xpath = string.Empty;
                    foreach (var query in lstQueries)
                    {
                        if (xpath.IsNullOrWhiteSpace()) xpath = query.YqlXPath();
                        if (xpath != query.YqlXPath())
                        {
                            valid.Success = false;
                            valid.Info.Add("queries", "XPath is different.");
                        }
                        query.ValidateInternal(valid);
                    }
                }
            }

            if (valid.Success == false) throw valid.CreateException();
            return lstQueries.ToArray();
        }

        private YqlResponse<T> ConvertResponse(StreamResponse response, YqlQuery<T>[] queries)
        {
            Exception exc = null;
            YqlDiagnostics diag = null;
            List<T> results = null;
            if (response.Connection.State == ConnectionState.Success)
            {
                try
                {
                    System.Resources.YqlResponse yqlDoc = MyHelper.DeserializeJson<System.Resources.YqlResponse>(response.Result);
                    if (yqlDoc == null)
                    {
                        exc = new ParseException("Cannot read YQL response data.");
                    }
                    else
                    {
                        results = new List<T>();
                        diag = yqlDoc.Query.Diagnostics;
                        if (yqlDoc.Query.Count > 0)
                        {
                            JArray tokArr = null;
                            foreach (JProperty rootTok in ((JObject)yqlDoc.Query.Results).Children<JProperty>())
                            {
                                if (rootTok.Value is JArray) { tokArr = (JArray)rootTok.Value; }
                                else { tokArr = new JArray(rootTok.Value); }

                                break;
                            }

                            if (tokArr.Count > 0)
                            {
                                for (int i = 0; i < tokArr.Count; i++)
                                {
                                    YqlQuery<T> query = (tokArr.Count == queries.Length) ? queries[i] : queries[0];
                                    QueryBase.ConvertInfo ci = new QueryBase.ConvertInfo(false);
                                    T result = query.YqlConvertToken(tokArr[i], ci);
                                    result.Integrity = new DataIntegrityInfo(ci.IsIntegrityComplete, ci.IntegrityMessages.ToArray());
                                    results.Add(result);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    results = null;
                    exc = this.GetOrCreateParseException(ex);
                }
            }
            else
            {
                exc = response.Connection.Exception;
            }
            return new YqlResponse<T>(new ConnectionInfo(
                        exc,
#if !(PORTABLE40)
 response.Connection.Timeout,
#endif
 response.Connection.SizeInBytes,
                        response.Connection.StartTime,
                        response.Connection.EndTime),
                    results != null ? results.ToArray() : null,
                    queries,
                    diag);
        }

        private Uri CreateUrl(IEnumerable<YqlQuery<T>> queries)
        {
            YqlQuery<T>[] queriesArr = this.CheckQueries(queries);

            string urlIn = string.Empty;
            foreach (var q in queries) { urlIn += string.Format("'{0}', ", q.CreateUrlInternal()); }
            urlIn = urlIn.Substring(0, urlIn.Length - 2);
            string url = MaasOne.YahooFinance.YFHelper.YqlUrl("*", "html",
                                    string.Format("url in ({0}) and xpath='{1}'", urlIn, queriesArr[0].YqlXPath()),
                                    true, this.GetDiagnostics, null);

            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
    }






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

#if !(PORTABLE40)
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

#if (NET20 || NET45)
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
                IAsyncResult res = wr.BeginGetResponse(new AsyncCallback(this.ResponseDownloadCompleted), asyncArgs);
#if !(PORTABLE40 || PORTABLE45)
                System.Threading.ThreadPool.RegisterWaitForSingleObject(res.AsyncWaitHandle, new System.Threading.WaitOrTimerCallback(this.ResponseDownloadTimeout), asyncArgs, this.Timeout, true);
#endif
            }
            catch (Exception ex)
            {
                System.Net.WebException dlException = this.GetOrCreateWebException(ex);
#if (PORTABLE40)
                ConnectionInfo conn = new ConnectionInfo(dlException, 0, startTime, DateTime.UtcNow);
#else
                ConnectionInfo conn = new ConnectionInfo(dlException, this.Timeout, 0, startTime, DateTime.UtcNow);
#endif

                this.RaiseAsyncDownloadCompleted(new StreamResponse(conn, null), queries, userArgs);
            }
        }

        private void ResponseDownloadCompleted(IAsyncResult result)
        {
            DateTime endTime = DateTime.UtcNow;
            AsyncDownloadArgs asyncArgs = (AsyncDownloadArgs)result.AsyncState;
            Exception dlException = null;
            int size = 0;
            System.IO.Stream res = null;

            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)asyncArgs.WR.EndGetResponse(result))
                {
#if !(PORTABLE40 || PORTABLE45)
                    if (!asyncArgs.TimedOut)
                    {
#endif
                        using (System.IO.Stream stream = resp.GetResponseStream())
                        {
                            res = MyHelper.CopyStream(stream);
#if !(PORTABLE40 || PORTABLE45)
                            resp.Close();
#else
                            resp.Dispose();
#endif
                            endTime = System.DateTime.UtcNow;
                            if (res != null && res.CanSeek) int.TryParse(res.Length.ToString(), out size);
                        }
#if !(PORTABLE40 || PORTABLE45)
                    }
                    else
                    {
                        dlException = new WebException("Timeout Exception.", null, WebExceptionStatus.Timeout, resp);
                        endTime = System.DateTime.UtcNow;
                    }
#endif
                }
            }
            catch (Exception ex)
            {
                dlException = this.GetOrCreateWebException(ex);
                endTime = System.DateTime.UtcNow;
            }

            this.RemoveDownload(asyncArgs.WR);


#if (PORTABLE40)
            ConnectionInfo conn = new ConnectionInfo(dlException, size, asyncArgs.StartTime, endTime);
#else
            ConnectionInfo conn = new ConnectionInfo(dlException, asyncArgs.Timeout, size, asyncArgs.StartTime, endTime);
#endif
            var strResp = new StreamResponse(conn, res);
#if (PORTABLE40 || PORTABLE45)
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
        protected abstract void RaiseAsyncDownloadCompleted(StreamResponse response, QueryBase[] queries, object userArgs);

        private class AsyncDownloadArgs : DownloadEventArgs
        {
            public DateTime StartTime { get; private set; }
            public HttpWebRequest WR { get; private set; }
            public QueryBase[] Queries { get; private set; }
#if !(PORTABLE40)
            public int Timeout { get; private set; }
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

#if (NET45 || PORTABLE45)
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

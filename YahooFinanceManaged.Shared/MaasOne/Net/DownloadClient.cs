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
    /// <summary>
    /// Delegate for completed, asynchronous and generic download operations.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="sender">The <see cref="DownloadClient{T}"/> that was used for executing the download operation.</param>
    /// <param name="e">The event argument for the completed download operation.</param>
    public delegate void AsyncDownloadCompletedEventHandler<T>(object sender, DownloadCompletedEventArgs<T> e) where T : ResultBase;


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



#if !(PORTABLE40 || PORTABLE45)
        /// <summary>
        /// Starts a data download and conversion operation.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the <see cref="DefaultQuery"/> is invalid.</exception>
        /// <returns>The <see cref="Response{T}"/> containing the generic result data and additional information.</returns>
        /// <remarks>The query object of <see cref="DefaultQuery"/> property is used for creating the request.</remarks>
        public Response<T> Download() { return this.Download(this.DefaultQuery); }

        public Response<T> Download(Query<T> query)
        {
            query = this.CheckAndCloneQuery(query);
            Uri url = this.CreateUrl(query);

            Response<T> response = null;

            using (StreamResponse resp = this.Download(url))
            {
                response = this.ConvertResponse(resp, query);
            }

            return response;
        }


        IQueryResponse IQueryDownload.Download() { return ((IQueryDownload)this).Download(((IQueryDownload)this).DefaultQuery); }

        IQueryResponse IQueryDownload.Download(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return this.Download((Query<T>)query);
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

        public void DownloadAsync(Query<T> query, object userArgs)
        {
            query = this.CheckAndCloneQuery(query);
            Uri url = this.CreateUrl(query);
            base.DownloadAsync(url, new QueryBase[] { query }, userArgs);
        }


        void IQueryDownload.DownloadAsync(QueryBase query, object userArgs)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            this.DownloadAsync((Query<T>)query, userArgs);
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



#if !(NET20 || PORTABLE40)
        /// <summary>
        /// Starts an async data download and conversion operation.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when the <see cref="DefaultQuery"/> is invalid.</exception>
        /// <returns>The task object representing the async download operation.</returns>
        /// <remarks>The query object of <see cref="DefaultQuery"/> property is used for creating the request.</remarks>
        public async System.Threading.Tasks.Task<Response<T>> DownloadTaskAsync() { return await this.DownloadTaskAsync(this.DefaultQuery); }

        public async System.Threading.Tasks.Task<Response<T>> DownloadTaskAsync(Query<T> query)
        {
            query = this.CheckAndCloneQuery(query);
            Uri url = this.CreateUrl(query);

            Response<T> response = null;

            using (StreamResponse resp = await this.DownloadTaskAsync(url))
            {
                response = this.ConvertResponse(resp, query);
            }

            return response;
        }


        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync() { return await ((IQueryDownload)this).DownloadTaskAsync(((IQueryDownload)this).DefaultQuery); }

        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return await this.DownloadTaskAsync((Query<T>)query);
        }
#endif



        bool IQueryDownload.IsCorrespondingType(Type queryType) { return typeof(Query<T>).IsAssignableFrom(queryType); }

        bool IQueryDownload.IsCorrespondingType(QueryBase query) { return ((IQueryDownload)this).IsCorrespondingType(query.GetType()); }


        private Query<T> CheckAndCloneQuery(Query<T> query)
        {
            if (query == null) { throw new ArgumentException("Query<" + typeof(T).Name + "> is NULL.", "query"); }
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

        private Uri CreateUrl(Query<T> query)
        {
            try
            { return new Uri(query.CreateUrlInternal(), UriKind.Absolute); }
            catch (Exception ex)
            { throw new ArgumentException("Cannot create the URI. See InnerException for more details.", ex); }
        }
    }


    /// <summary>
    /// Provides methods for creating a web request and converting the response to the aim data format.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    public abstract class Query<T> : QueryBase where T : ResultBase
    {
        /// <summary>
        /// Converts the received <see cref="System.IO.Stream"/> to the generic result object.
        /// </summary>
        /// <param name="stream">The <see cref="System.IO.Stream"/> received from the data source.</param>
        /// <returns></returns>
        protected abstract T ConvertResult(System.IO.Stream stream, ConvertInfo ci);
        internal T ConvertResultInternal(System.IO.Stream stream, ConvertInfo ci) { return this.ConvertResult(stream, ci); }
    }


    public class Response<T> : ResponseBase, IQueryResponse where T : ResultBase
    {
        /// <summary>
        /// Gets the cloned query that was used to create the request.
        /// </summary>
        public Query<T> Query { get; private set; }

        /// <summary>
        /// Gets the received data.
        /// </summary>
        public T Result { get { return (T)base.ResultBase; } }


        QueryBase IQueryResponse.Query { get { return this.Query; } }

        ResultBase IQueryResponse.Result { get { return this.Result; } }


        internal Response(ConnectionInfo connInfo, T result, Query<T> query)
            : base(connInfo, result)
        {
            this.Query = query;
        }
    }


    /// <summary>
    /// Provides properties for a completed, asynchronous download operation.
    /// </summary>
    public class DownloadCompletedEventArgs<T> : DownloadCompletedEventArgs, IQueryDownloadCompletedEventArgs where T : ResultBase
    {
        /// <summary>
        /// Gets the response of the download operation.
        /// </summary>
        public Response<T> Response { get { return (Response<T>)base.ResponseBase; } }


        IQueryResponse IQueryDownloadCompletedEventArgs.Response { get { return this.Response; } }


        internal DownloadCompletedEventArgs(object userArgs, Response<T> response) : base(userArgs, response) { }
    }
}

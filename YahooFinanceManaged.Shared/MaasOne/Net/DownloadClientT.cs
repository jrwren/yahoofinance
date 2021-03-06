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
#if (NETFX_CORE)
using System.Reflection;
#endif

namespace MaasOne.Net
{
    public class DownloadClient<T> : DownloadClientBase, IQueryDownload where T : ResultBase
    {
        public DownloadClient() { }

        public DownloadClient(Query<T> defaultQuery) : this() { this.DefaultQuery = defaultQuery; }



        /// <summary>
        /// Delegate for completed, asynchronous and generic download operations.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="sender">The <see cref="DownloadClient{T}"/> that was used for executing the download operation.</param>
        /// <param name="e">The event argument for the completed download operation.</param>
        public delegate void AsyncDownloadCompletedEventHandler<T>(object sender, DownloadCompletedEventArgs<T> e) where T : ResultBase;



        /// <summary>
        /// Occurs when an asynchronous download and conversion operation completes.
        /// </summary>
        public event AsyncDownloadCompletedEventHandler<T> AsyncDownloadCompleted;

        /// <summary>
        /// Occurs when an asynchronous download and conversion operation completes.
        /// </summary>
        public event AsyncDownloadCompletedEventHandler AsyncDownloadCompletedBase;



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



#if !(SILVERLIGHT || NETFX_CORE)
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
#endif

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
            AsyncArgs args = new AsyncArgs() { Query = query, UserArgs = userArgs };
            base.DownloadAsync(url, args);
        }

        private class AsyncArgs
        {
            public Query<T> Query { get; set; }
            public object UserArgs { get; set; }
        }

#if !(NET20 || NET35 || NET40 || SILVERLIGHT)
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
#endif


#if !(SILVERLIGHT || NETFX_CORE)
        IQueryResponse IQueryDownload.Download() { return ((IQueryDownload)this).Download(((IQueryDownload)this).DefaultQuery); }

        IQueryResponse IQueryDownload.Download(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return this.Download((Query<T>)query);
        }
#endif

        void IQueryDownload.DownloadAsync(QueryBase query, object userArgs)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            this.DownloadAsync((Query<T>)query, userArgs);
        }

#if !(NET20 || NET35 || NET40 || SILVERLIGHT)
        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync() { return await ((IQueryDownload)this).DownloadTaskAsync(((IQueryDownload)this).DefaultQuery); }

        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return await this.DownloadTaskAsync((Query<T>)query);
        }
#endif

        bool IQueryDownload.IsCorrespondingType(Type queryType)
        {
#if !(NETFX_CORE && !SILVERLIGHT)
            return typeof(YqlQuery<T>).IsAssignableFrom(queryType);
#else
            return typeof(YqlQuery<T>).GetTypeInfo().IsAssignableFrom(queryType.GetTypeInfo());
#endif
        }

        bool IQueryDownload.IsCorrespondingType(QueryBase query) { return ((IQueryDownload)this).IsCorrespondingType(query.GetType()); }


        protected override void RaiseAsyncDownloadCompleted(StreamResponse response, object userArgs)
        {
            AsyncArgs args = (AsyncArgs)userArgs;
            Response<T> resp = null;

            using (response)
            {
                resp = this.ConvertResponse(response, args.Query);
            }

            if (AsyncDownloadCompletedBase != null) this.AsyncDownloadCompletedBase(this, new DownloadCompletedEventArgs(args.UserArgs, resp));
            if (AsyncDownloadCompleted != null) this.AsyncDownloadCompleted(this, new DownloadCompletedEventArgs<T>(args.UserArgs, resp));
        }


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
            ConnectionState state = response.Connection.State;
            T result = default(T);

            if (state == ConnectionState.Success)
            {
                try
                {
                    QueryBase.ConvertInfo ci = new QueryBase.ConvertInfo(true);
                    result = query.ConvertResultInternal(response.Result, ci);
                    result.Integrity = new DataIntegrityInfo(ci.IsIntegrityComplete, ci.IntegrityMessages.ToArray());
                }
                catch (Exception ex)
                {
                    state = ConnectionState.ErrorOccured;
                    exc = this.GetOrCreateParseException(ex);
                }
            }
            else
            {
                exc = response.Connection.Exception;
            }

            return new Response<T>(new ConnectionInfo(
                state,
                exc,
                response.Connection.Timeout,
                response.Connection.SizeInBytes,
                response.Connection.StartTime,
                response.Connection.EndTime), 
                    result, 
                    query);
        }

        private Uri CreateUrl(Query<T> query)
        {
            try
            { return new Uri(query.CreateUrlInternal(), UriKind.Absolute); }
            catch (Exception ex)
            { throw new ArgumentException("Cannot create the URI. See InnerException for more details.", ex); }
        }
    }
}

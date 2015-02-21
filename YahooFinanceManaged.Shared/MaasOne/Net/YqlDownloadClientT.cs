using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
#if (NETFX_CORE)
using System.Reflection;
#endif
namespace MaasOne.Net
{
    public class YqlDownloadClient<T> : DownloadClientBase, IQueryDownload where T : ResultBase
    {
        public YqlDownloadClient() { }

        public YqlDownloadClient(YqlQuery<T>[] defaultQueries) : this() { this.DefaultQueries = defaultQueries; }



        public delegate void YqlAsyncDownloadCompletedEventHandler<T>(object sender, YqlDownloadCompletedEventArgs<T> e) where T : ResultBase;



        /// <summary>
        /// Occurs when an asynchronous download and conversion operation completes.
        /// </summary>
        public event YqlAsyncDownloadCompletedEventHandler<T> AsyncDownloadCompleted;

        /// <summary>
        /// Occurs when an asynchronous download and conversion operation completes.
        /// </summary>
        public event AsyncDownloadCompletedEventHandler AsyncDownloadCompletedBase;



        public YqlQuery<T>[] DefaultQueries { get; set; }

        public bool GetDiagnostics { get; set; }


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



#if !(SILVERLIGHT || NETFX_CORE)
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

        public void DownloadAsync(object userArgs) { this.DownloadAsync(this.DefaultQueries, userArgs); }

        public void DownloadAsync(YqlQuery<T> query, object userArgs) { this.DownloadAsync(new YqlQuery<T>[] { query }, userArgs); }

        public void DownloadAsync(IEnumerable<YqlQuery<T>> queries, object userArgs)
        {
            YqlQuery<T>[] queriesArr = this.CheckQueries(queries);
            Uri url = this.CreateUrl(queriesArr);
            AsyncArgs args = new AsyncArgs() { Queries = queriesArr, UserArgs = userArgs };
            base.DownloadAsync(url, args);
        }

        private class AsyncArgs
        {
            public YqlQuery<T>[] Queries { get; set; }
            public object UserArgs { get; set; }
        }

#if !(SILVERLIGHT || NET20 || NET35 || NET40)
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


#if !(SILVERLIGHT || NETFX_CORE)
        IQueryResponse IQueryDownload.Download() { return ((IQueryDownload)this).Download(((IQueryDownload)this).DefaultQuery); }

        IQueryResponse IQueryDownload.Download(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return this.Download((YqlQuery<T>)query);
        }
#endif

        void IQueryDownload.DownloadAsync(QueryBase query, object userArgs)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            this.DownloadAsync((YqlQuery<T>)query, userArgs);
        }

#if !(SILVERLIGHT || NET20 || NET35 || NET40)
        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync() { return await ((IQueryDownload)this).DownloadTaskAsync(((IQueryDownload)this).DefaultQuery); }

        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return await this.DownloadTaskAsync((YqlQuery<T>)query);
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
            YqlResponse<T> resp = null;

            using (response)
            {
                resp = this.ConvertResponse(response, args.Queries);
            }

            if (AsyncDownloadCompletedBase != null) this.AsyncDownloadCompletedBase(this, new DownloadCompletedEventArgs(args.UserArgs, resp));
            if (AsyncDownloadCompleted != null) this.AsyncDownloadCompleted(this, new YqlDownloadCompletedEventArgs<T>(args.UserArgs, resp));
        }


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
                    for (int i = 0; i < lstQueries.Count; i++)
                    {
                        YqlQuery<T> query = lstQueries[i];
                        if (xpath.IsNullOrWhiteSpace()) xpath = query.YqlXPath();
                        if (query.YqlXPath().IsNullOrWhiteSpace() || xpath != query.YqlXPath())
                        {
                            valid.Success = false;
                            valid.Info.Add("queries[" + i + "]", "XPath is different.");
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
            ConnectionState state = response.Connection.State;
            if (state == ConnectionState.Success)
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
                    state = ConnectionState.ErrorOccured;
                    exc = this.GetOrCreateParseException(ex);
                }
            }
            else
            {
                exc = response.Connection.Exception;
            }
            return new YqlResponse<T>(new ConnectionInfo(
                state,
                exc,
                response.Connection.Timeout,
                response.Connection.SizeInBytes,
                response.Connection.StartTime,
                response.Connection.EndTime),
                    results != null ? results.ToArray() : null,
                    queries,
                    diag);
        }

        private Uri CreateUrl(IEnumerable<YqlQuery<T>> queries)
        {
            try
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
            catch (Exception ex)
            { throw new ArgumentException("Cannot create the URI. See InnerException for more details.", ex); }
        }
    }
}

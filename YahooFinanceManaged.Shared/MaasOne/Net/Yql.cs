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
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MaasOne.Net
{
    public delegate void YqlAsyncDownloadCompletedEventHandler<T>(object sender, YqlDownloadCompletedEventArgs<T> e) where T : ResultBase;


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



#if !(PORTABLE40 || PORTABLE45)
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


        IQueryResponse IQueryDownload.Download() { return ((IQueryDownload)this).Download(((IQueryDownload)this).DefaultQuery); }

        IQueryResponse IQueryDownload.Download(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return this.Download((YqlQuery<T>)query);
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

        public void DownloadAsync(object userArgs) { this.DownloadAsync(this.DefaultQueries, userArgs); }

        public void DownloadAsync(YqlQuery<T> query, object userArgs) { this.DownloadAsync(new YqlQuery<T>[] { query }, userArgs); }

        public void DownloadAsync(IEnumerable<YqlQuery<T>> queries, object userArgs)
        {
            YqlQuery<T>[] queriesArr = this.CheckQueries(queries);
            Uri url = this.CreateUrl(queriesArr);
            base.DownloadAsync(url, queriesArr, userArgs);
        }


        void IQueryDownload.DownloadAsync(QueryBase query, object userArgs)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            this.DownloadAsync((YqlQuery<T>)query, userArgs);
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



#if !(NET20 || PORTABLE40)
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


        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync() { return await ((IQueryDownload)this).DownloadTaskAsync(((IQueryDownload)this).DefaultQuery); }

        async System.Threading.Tasks.Task<IQueryResponse> IQueryDownload.DownloadTaskAsync(QueryBase query)
        {
            if (((IQueryDownload)this).IsCorrespondingType(query) == false) throw new ArgumentException("Query type is not corresponding.", "query");
            return await this.DownloadTaskAsync((YqlQuery<T>)query);
        }
#endif



        bool IQueryDownload.IsCorrespondingType(Type queryType) { return typeof(YqlQuery<T>).IsAssignableFrom(queryType); }

        bool IQueryDownload.IsCorrespondingType(QueryBase query) { return ((IQueryDownload)this).IsCorrespondingType(query.GetType()); }



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


    public abstract class YqlQuery<T> : Query<T> where T : ResultBase
    {
        internal YqlQuery() { }


        protected override T ConvertResult(System.IO.Stream stream, ConvertInfo ci)
        {
            string htmlText = MyHelper.StreamToString(stream);
            JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
            JToken yqlToken = this.YqlTokenFromDirectSource(htmlDoc);
            return this.YqlConvertToken(yqlToken, ci);
        }


        internal abstract T YqlConvertToken(JToken yqlToken, ConvertInfo ci);

        internal abstract JToken YqlTokenFromDirectSource(JObject htmlDoc);

        internal abstract string YqlXPath();
    }


    public class YqlResponse<T> : ResponseBase, IQueryResponse where T : ResultBase
    {
        public YqlDiagnostics Diagnostics { get; private set; }

        /// <summary>
        /// Gets the cloned query that was used to create the request.
        /// </summary>
        public YqlQuery<T>[] Queries { get; private set; }

        /// <summary>
        /// Gets the received data.
        /// </summary>
        public T[] Results { get { return (T[])base.ResultBase; } }


        QueryBase IQueryResponse.Query { get { return this.Queries.Length > 0 ? this.Queries[0] : null; } }

        ResultBase IQueryResponse.Result { get { return this.Results.Length > 0 ? this.Results[0] : null; } }


        internal YqlResponse(ConnectionInfo connInfo, T[] results, YqlQuery<T>[] queries, YqlDiagnostics diagnostics)
            : base(connInfo, results)
        {
            this.Queries = queries != null ? queries : new YqlQuery<T>[0];
            this.Diagnostics = diagnostics;
        }
    }


    /// <summary>
    /// Provides properties for a completed, asynchronous download operation.
    /// </summary>
    public class YqlDownloadCompletedEventArgs<T> : DownloadCompletedEventArgs, IQueryDownloadCompletedEventArgs where T : ResultBase
    {
        /// <summary>
        /// Gets the response of the download operation.
        /// </summary>
        public YqlResponse<T> Response { get { return (YqlResponse<T>)base.ResponseBase; } }


        IQueryResponse IQueryDownloadCompletedEventArgs.Response { get { return this.Response; } }


        internal YqlDownloadCompletedEventArgs(object userArgs, YqlResponse<T> response) : base(userArgs, response) { }
    }



    /// <summary>
    /// Provides properties to set the start index and count number for a YQL query in results queue.
    /// </summary>
    public interface IYqlIndexQuery
    {
        /// <summary>
        /// The results queue start index.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// The total number of results.
        /// </summary>
        int Count { get; set; }
    }


    public class YqlDiagnostics
    {
        [JsonProperty("publiclycallable")]
        public bool PubliclyCallable { get; set; }

        [JsonProperty("url"), JsonConverter(typeof(System.Resources.SingleOrArrayConverter<YqlDiagUrl>))]
        public YqlDiagUrl[] Url { get; set; }

        [JsonProperty("user-time")]
        public int UserTime { get; set; }

        [JsonProperty("service-time")]
        public int ServiceTime { get; set; }

        [JsonProperty("build-version")]
        public string BuildVersion { get; set; }


        internal YqlDiagnostics() { }
    }


    public class YqlDiagUrl
    {
        [JsonProperty("execution-start-time")]
        public int ExecutionStartTime { get; set; }

        [JsonProperty("execution-stop-time")]
        public int ExecutionStopTime { get; set; }

        [JsonProperty("execution-time")]
        public int ExecutionTime { get; set; }

        [JsonProperty("content")]
        public Uri Content { get; set; }


        internal YqlDiagUrl() { }
    }
}

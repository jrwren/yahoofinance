using System;
using System.Collections.Generic;
using System.Text;

namespace MaasOne.Net
{
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
}

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

namespace MaasOne.Net
{
    /// <summary>
    /// Provides the result data and additional information of a download operation.
    /// </summary>   
    public abstract class ResponseBase
    {
        /// <summary>
        /// Gets the result of the download operation.
        /// </summary>
        /// <value>The value can be a single object or an array of objects depending on the implementing class.</value>
        public abstract object ResultBase { get; }

        /// <summary>
        /// Gets the connection information of the download operation.
        /// </summary>
        public ConnectionInfo Connection { get; protected set; }


        internal ResponseBase(ConnectionInfo connInfo)
        {
            this.Connection = connInfo;
        }
    }

    public interface IQueryResponse
    {
        QueryBase Query { get; }
        ResultBase Result { get; }
        ConnectionInfo Connection { get; }
    }


    public class Response<T> : ResponseBase, IQueryResponse where T : ResultBase
    {
        /// <summary>
        /// Gets the cloned query that was used to create the request.
        /// </summary>
        public Query<T> Query { get; private set; }

        QueryBase IQueryResponse.Query { get { return this.Query; } }

        public override object ResultBase { get { return this.Result; } }

        ResultBase IQueryResponse.Result { get { return this.Result; } }

        /// <summary>
        /// Gets the received data.
        /// </summary>
        public T Result { get; private set; }


        internal Response(ConnectionInfo connInfo, T result, Query<T> query)
            : base(connInfo)
        {
            this.Result = result;
            this.Query = query;
        }
    }

    public class YqlResponse<T> : ResponseBase, IQueryResponse where T : ResultBase
    {
        public YqlDiagnostics Diagnostics { get; private set; }

        /// <summary>
        /// Gets the cloned query that was used to create the request.
        /// </summary>
        public YqlQuery<T>[] Queries { get; private set; }

        QueryBase IQueryResponse.Query { get { return this.Queries[0]; } }

        public override object ResultBase { get { return this.Results; } }

        ResultBase IQueryResponse.Result { get { return this.Results.Length > 0 ? this.Results[0] : null; } }

        /// <summary>
        /// Gets the received data.
        /// </summary>
        public T[] Results { get; private set; }

        internal YqlResponse(ConnectionInfo connInfo, T[] results, YqlQuery<T>[] queries, YqlDiagnostics diagnostics)
            : base(connInfo)
        {
            this.Results = results;
            this.Queries = queries;
            this.Diagnostics = diagnostics;
        }

    }



}

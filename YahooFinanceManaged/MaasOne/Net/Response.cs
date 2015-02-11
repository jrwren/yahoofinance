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


    public class Response<T> : ResponseBase where T : ResultBase
    {
        /// <summary>
        /// Gets the cloned query that was used to create the request.
        /// </summary>
        public Query<T> Query { get; private set; }

        public override object ResultBase { get { return this.Result; } }

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

    public class YqlResponse<T> : ResponseBase where T : ResultBase
    {
        public YqlDiagnostics Diagnostics { get; private set; }

        /// <summary>
        /// Gets the cloned query that was used to create the request.
        /// </summary>
        public Query<T>[] Queries { get; private set; }

        public override object ResultBase { get { return this.Results; } }

        /// <summary>
        /// Gets the received data.
        /// </summary>
        public T[] Results { get; private set; }

        internal YqlResponse(ConnectionInfo connInfo, T[] results, Query<T>[] queries, YqlDiagnostics diagnostics)
            : base(connInfo)
        {
            this.Results = results;
            this.Queries = queries;
            this.Diagnostics = diagnostics;
        }

    }



}

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
    public class Response<T> : ResponseBase, IQueryResponse where T : ResultBase
    {
        internal Response(ConnectionInfo connInfo, T result, Query<T> query)
            : base(connInfo, result)
        {
            this.Query = query;
        }



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
    }
}

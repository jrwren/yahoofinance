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
    /// Base response class for download processes.
    /// </summary>
    /// <remarks></remarks>
    public class Response<T> : IResponse where T : class
    {
        private ConnectionInfo mConnection = null;
        private T mResult;
        private Query<T> mQuery;

        /// <summary>
        /// Gets connection information of the download process.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ConnectionInfo Connection { get { return mConnection; } }
        /// <summary>
        /// Gets the received managed data.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public T Result { get { return mResult; } }
        object IResponse.ResultObj { get { return mResult; } }

        public Query<T> Query { get { return mQuery; } }
        IQuery IResponse.QueryObj { get { return mQuery; } }

        internal Response(ConnectionInfo connInfo, T result, Query<T> query)
        {
            mConnection = connInfo;
            mResult = result;
            mQuery = query;
        }


    }
}

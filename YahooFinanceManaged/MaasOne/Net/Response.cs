// **************************************************************************************************
// **  
// **  Yahoo! Finance Managed
// **  Written by Marius Häusler 2015
// **  It would be pleasant, if you could contact me when you are using this code.
// **  Contact: maasone@live.com
// **  Project Home: https://yahoofinance.codeplex.com/
// **  
// **************************************************************************************************
// **  
// **  Copyright @ Marius Häusler
// **  
// **  Licensed under GNU Lesser General Public License (LGPL) (Version 2.1, February 1999).
// **  
// **  License Text: https://yahoofinance.codeplex.com/license
// **  
// **  
// **************************************************************************************************
using System;


namespace MaasOne.Net
{
    /// <summary>
    /// Base response class for download processes. This class must be inherited.
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

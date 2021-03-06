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
        internal ResponseBase(ConnectionInfo connInfo, object result)
        {
            this.Connection = connInfo;
            this.ResultBase = result;
        }



        /// <summary>
        /// Gets the connection information of the download operation.
        /// </summary>
        public ConnectionInfo Connection { get; protected set; }

        /// <summary>
        /// Gets the result of the download operation.
        /// </summary>
        /// <value>The value can be a single object or an array of objects depending on the implementing class.</value>
        public object ResultBase { get; protected set; }
    }
}

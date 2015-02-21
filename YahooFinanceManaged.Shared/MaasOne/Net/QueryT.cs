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
    /// Provides methods for creating a web request and converting the response to the aim data format.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    public abstract class Query<T> : QueryBase where T : ResultBase
    {
        public Query() { }



        internal T ConvertResultInternal(System.IO.Stream stream, ConvertInfo ci) { return this.ConvertResult(stream, ci); }


        /// <summary>
        /// Converts the received <see cref="System.IO.Stream"/> to the generic result object.
        /// </summary>
        /// <param name="stream">The <see cref="System.IO.Stream"/> received from the data source.</param>
        /// <returns></returns>
        protected abstract T ConvertResult(System.IO.Stream stream, ConvertInfo ci);
    }
}

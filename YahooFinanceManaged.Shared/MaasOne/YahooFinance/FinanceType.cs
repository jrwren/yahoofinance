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
using System.Text;


namespace MaasOne.YahooFinance
{
    /// <summary>
    /// Provides the financial type of the security in general like "Stock" or "Index".
    /// </summary>
    /// <remarks></remarks>
    public enum FinanceType
    {
        /// <summary>
        /// All
        /// </summary>
        /// <remarks></remarks>
        Any,
        /// <summary>
        /// Stock
        /// </summary>
        /// <remarks></remarks>
        Stock,
        /// <summary>
        /// Fund
        /// </summary>
        /// <remarks></remarks>
        Fund,
        /// <summary>
        /// Exchange Traded Fund
        /// </summary>
        /// <remarks></remarks>
        ETF,
        /// <summary>
        /// Index
        /// </summary>
        /// <remarks></remarks>
        Index,
        /// <summary>
        /// Future
        /// </summary>
        /// <remarks></remarks>
        Future,
        /// <summary>
        /// Warrant
        /// </summary>
        /// <remarks></remarks>
        Warrant,
        /// <summary>
        /// Currency
        /// </summary>
        /// <remarks></remarks>
        Currency
    }
}

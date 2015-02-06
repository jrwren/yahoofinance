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
    /// Describes the end state of a connection.
    /// </summary>
    /// <remarks></remarks>
    public enum ConnectionState
    {
        /// <summary>
        /// Download process completed successfully without timeout, errors or cancelations
        /// </summary>
        /// <remarks></remarks>
        Success,
        /// <summary>
        /// Download process was canceled by user interaction
        /// </summary>
        /// <remarks></remarks>
        Canceled,
#if !(PORTABLE40)
        /// <summary>
        /// Download process reached the setted timeout span
        /// </summary>
        /// <remarks></remarks>
        Timeout,
#endif
        /// <summary>
        /// An Error occured during download process
        /// </summary>
        /// <remarks></remarks>
        ErrorOccured
    }

}

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

namespace MaasOne.Net
{
    /// <summary>
    /// Describes the state of a completed download operation.
    /// </summary>
    /// <remarks></remarks>
    public enum ConnectionState
    {
        /// <summary>
        /// Download operation completed successfully without timeout, errors or cancelation.
        /// </summary>
        /// <remarks></remarks>
        Success,

        /// <summary>
        /// Download operation was canceled by user interaction.
        /// </summary>
        /// <remarks></remarks>
        Canceled,

        /// <summary>
        /// Download operation reached the setted timeout span.
        /// </summary>
        /// <remarks></remarks>
        Timeout,
        /// <summary>
        /// An Error occured during download operation.
        /// </summary>
        /// <remarks></remarks>
        ErrorOccured
    }
}

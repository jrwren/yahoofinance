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


namespace System.Net
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

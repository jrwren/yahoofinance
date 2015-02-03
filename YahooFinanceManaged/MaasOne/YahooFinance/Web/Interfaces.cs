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

namespace MaasOne.YahooFinance.Web
{

    /// <summary>
    /// Provides properties for a webservice query.
    /// </summary>
    public interface ITextSearchQuery
    {
        /// <summary>
        /// The query text.
        /// </summary>
        string LookupText { get; set; }
    }


    /// <summary>
    /// Provides properties to set the start index and count number for a query in results queue.
    /// </summary>
    public interface IResultIndexSettings
    {
        /// <summary>
        /// The results queue start index.
        /// </summary>
        int Index { get; set; }
        /// <summary>
        /// The total number of results.
        /// </summary>
        int Count { get; set; }
    }

}

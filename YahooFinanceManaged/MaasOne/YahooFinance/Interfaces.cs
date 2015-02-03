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
using System.Collections.Generic;
using System.Text;


namespace MaasOne.YahooFinance
{

    /// <summary>
    /// Interface for Yahoo! ID. Can be used for downloading informations from Yahoo! Finance.
    /// </summary>
    /// <remarks></remarks>
    public interface IID
    {
        /// <summary>
        /// The valid Yahoo! ID.
        /// </summary>
        /// <value></value>
        /// <returns>The full ID built by the implementing class.</returns>
        /// <remarks></remarks>
        string ID { get; }
    }


    public interface IQuotePrice : IID
    {
        double? LastTradePriceOnly { get; }
    }


    
}

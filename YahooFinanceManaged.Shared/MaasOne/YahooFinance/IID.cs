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
}

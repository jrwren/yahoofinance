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
    /// Provides the user argument object that was passed at the beginning of a download operation.
    /// </summary>
    public abstract class DownloadEventArgs : EventArgs
    {
        protected DownloadEventArgs(object userArgs) { this.UserArgs = userArgs; }



        /// <summary>
        /// Gets the user argument that was passed at the beginning of download operation.
        /// </summary>
        public object UserArgs { get; private set; }
    }
}

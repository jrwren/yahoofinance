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
using System.Net;

namespace MaasOne.Net
{
    /// <summary>
    /// Provides information about a running download operation.
    /// </summary>
    public class DownloadOperation
    {
        internal DownloadOperation(object clientObject, object userArgs)
        {
            this.ClientObject = clientObject;
            this.UserArgs = userArgs;
        }



        /// <summary>
        /// Gets the user argument object, if the download operation is asynchronous.
        /// </summary>
        public object UserArgs { get; private set; }


        internal object ClientObject { get; private set; }




        /// <summary>
        /// Stops this download operation.
        /// </summary>
        public void Cancel()
        {
            if (this.ClientObject is HttpWebRequest) { ((HttpWebRequest)this.ClientObject).Abort(); }
#if !(NET20 || NET35 || NET40 || SILVERLIGHT)
            if (this.ClientObject is System.Net.Http.HttpClient) { ((System.Net.Http.HttpClient)this.ClientObject).CancelPendingRequests(); }
#endif
        }


        internal void Cleanup()
        {
            this.UserArgs = null;
            this.ClientObject = null;
        }
    }
}


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

namespace MaasOne.Net
{
    public class StreamResponse : ResponseBase, IDisposable
    {
        public System.IO.Stream Result { get { return (System.IO.Stream)base.ResultBase; } }      


        internal StreamResponse(ConnectionInfo connInfo, System.IO.Stream result) : base(connInfo, result) { }


        private bool mDisposed = false;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!mDisposed)
            {
                if (disposing)
                {
                    if (this.Result != null)
                    {
                        this.Result.Dispose();
                        this.ResultBase = null;
                    }
                }
                this.Connection = null;
                mDisposed = true;
            }
        }
    }

}

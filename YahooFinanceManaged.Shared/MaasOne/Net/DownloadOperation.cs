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
using System.Collections.Generic;

namespace MaasOne.Net
{
    /// <summary>
    /// Provides information about a running download operation.
    /// </summary>
    public class DownloadOperation
    {
      
        /// <summary>
        /// Gets the user argument object, if the download operation is asynchronous.
        /// </summary>
        public object UserArgs { get; private set; }

        internal object ClientObject { get; private set; }


        internal DownloadOperation(object clientObject,  object userArgs)
        {
            this.ClientObject = clientObject;
            this.UserArgs = userArgs;
        }


        /// <summary>
        /// Stops this download operation.
        /// </summary>
        public void Cancel()
        {
            if (this.ClientObject is HttpWebRequest) { ((HttpWebRequest)this.ClientObject).Abort(); }
#if !(NET20 || PCL40)
            if (this.ClientObject is System.Net.Http.HttpClient) { ((System.Net.Http.HttpClient)this.ClientObject).CancelPendingRequests(); }
#endif
        }

        private bool IsDisposed { get; set; }
        internal virtual void Dispose()
        {
            if (this.IsDisposed == false)
            {
                this.IsDisposed = true;
                this.UserArgs = null;
                this.ClientObject = null;
            }
        }
    }
    
    public class DownloadOperationCollection : IEnumerable<DownloadOperation>
    {
        private readonly List<DownloadOperation> mItems = new List<DownloadOperation>();

        public DownloadOperation[] Items { get { return this.mItems.ToArray(); } }

        public DownloadOperation this[int index] { get { return this.mItems[index]; } }

        public int Count { get { return this.mItems.Count; } }


        internal DownloadOperationCollection() { }


        public void CancelAll()
        {
            foreach (DownloadOperation ds in this.mItems) { ds.Cancel(); }
        }

        public IEnumerator<DownloadOperation> GetEnumerator()
        {
            return mItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mItems.GetEnumerator();
        }

        internal void Add(DownloadOperation item)
        {
            this.mItems.Add(item);
        }

        internal bool Remove(DownloadOperation item)
        {
            return this.mItems.Remove(item);
        }

        internal bool Remove(object clientObject)
        {
            foreach (DownloadOperation ds in this.mItems)
            {
                if (ds.ClientObject == clientObject)
                {
                    return this.Remove(ds);
                }
            }
            return false;
        }
    }
}


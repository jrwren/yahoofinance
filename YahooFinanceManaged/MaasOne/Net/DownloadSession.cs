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


    public class DownloadSessionCollection : IEnumerable<DownloadSession>
    {

        private readonly List<DownloadSession> mItems = new List<DownloadSession>();

        public DownloadSession[] Items { get { return this.mItems.ToArray(); } }
        public DownloadSession this[int index] { get { return this.mItems[index]; } }
        public int Count { get { return this.mItems.Count; } }

        internal DownloadSessionCollection() { }

        public void CancelAll()
        {
            foreach (DownloadSession ds in this.mItems)
            {
                ds.Cancel();
            }
        }

        public IEnumerator<DownloadSession> GetEnumerator()
        {
            return mItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mItems.GetEnumerator();
        }



        internal void Add(DownloadSession item)
        {
            this.mItems.Add(item);
        }
        internal bool Remove(DownloadSession item)
        {
            return this.mItems.Remove(item);
        }
        internal bool Remove(object clientObject)
        {
            foreach (DownloadSession ds in this.mItems)
            {
                if (ds.ClientObject == clientObject)
                {
                    return this.Remove(ds);
                }
            }
            return false;
        }

    }



    public class DownloadSession
    {
        public IQuery Query { get; private set; }
        public object UserArgs { get; private set; }
        internal object ClientObject { get; private set; }

        public void Cancel()
        {
            if (this.ClientObject is HttpWebRequest) { ((HttpWebRequest)this.ClientObject).Abort(); }
#if (NET45 || PORTABLE45)
            if (this.ClientObject is System.Net.Http.HttpClient) { ((System.Net.Http.HttpClient)this.ClientObject).CancelPendingRequests(); }
#endif
        }

        internal DownloadSession(object clientObject, IQuery query, object userArgs)
        {
            this.ClientObject = clientObject;
            this.Query = query;
            this.UserArgs = userArgs;
        }


        private bool IsDisposed { get; set; }
        internal virtual void Dispose()
        {
            if (this.IsDisposed == false)
            {
                this.IsDisposed = true;
                this.Query = null;
                this.UserArgs = null;
                this.ClientObject = null;
            }
        }
    }


}


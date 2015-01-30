using System;
using System.Collections.Generic;

namespace System.Net
{


    public class DownloadSessionCollection<T> where T : class
    {

        private readonly List<DownloadSession<T>> mItems = new List<DownloadSession<T>>();
        public DownloadSession<T>[] Items { get { return this.mItems.ToArray(); } }
        public DownloadSession<T> this[int index] { get { return this.mItems[index]; } }

        public int Count { get { return this.mItems.Count; } }

        public void CancelAll()
        {

            foreach (DownloadSession<T> ds in this.mItems)
            {
                ds.Cancel();
            }

        }

        internal void Add(DownloadSession<T> item)
        {
            this.mItems.Add(item);
        }
        internal bool Remove(DownloadSession<T> item)
        {
            return this.mItems.Remove(item);
        }
        internal bool Remove(object clientObject)
        {
            foreach (DownloadSession<T> ds in this.mItems)
            {
                if (ds.ClientObject == clientObject)
                {
                    return this.Remove(ds);
                }
            }
            return false;
        }
        internal void RemoveAt(int index)
        {
            this.mItems.RemoveAt(index);
        }
    }



    public class DownloadSession : IDisposable
    {
        public object UserArgs { get; private set; }
        internal object ClientObject { get; private set; }

        public void Cancel()
        {
            if (this.ClientObject is HttpWebRequest)
            {
                var wr = (HttpWebRequest)this.ClientObject;
                wr.Abort();
            }
#if (NET45 || PORTABLE45)
            if (this.ClientObject is System.Net.Http.HttpClient)
            {
                var hc = (System.Net.Http.HttpClient)this.ClientObject;
                hc.CancelPendingRequests();
            }
#endif
        }

        internal DownloadSession(object clientObject, object userArgs)
        {
            this.ClientObject = clientObject;
            this.UserArgs = userArgs;
        }


        internal bool IsDisposed { get; set; }
        public virtual void Dispose()
        {
            if (this.IsDisposed == false)
            {
                this.IsDisposed = true;
                this.UserArgs = null;
                this.ClientObject = null;
            }
        }
    }

    public class DownloadSession<T> : DownloadSession where T : class
    {
        public Query<T> Query { get; private set; }


        internal DownloadSession(object clientObject, Query<T> query, object userArgs)
            : base(clientObject, userArgs)
        {
            this.Query = query;
        }


        public override void Dispose()
        {
            if (this.IsDisposed == false)
            {
                this.Query = null;
                base.Dispose();
            }
        }
    }

}


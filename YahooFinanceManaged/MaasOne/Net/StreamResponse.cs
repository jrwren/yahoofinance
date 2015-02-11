using System;
using System.Collections.Generic;

namespace MaasOne.Net
{
    public class StreamResponse : ResponseBase, IDisposable
    {
        public System.IO.Stream Result { get; private set; }

        public override object ResultBase { get { return this.Result; } }


        internal StreamResponse(ConnectionInfo connInfo, System.IO.Stream result) : base(connInfo) { this.Result = result; }


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
                        this.Result = null;
                    }
                }
                this.Connection = null;
                mDisposed = true;
            }
        }
    }
}

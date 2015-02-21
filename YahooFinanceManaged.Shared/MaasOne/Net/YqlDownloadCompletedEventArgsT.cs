using System;

namespace MaasOne.Net
{
    /// <summary>
    /// Provides properties for a completed, asynchronous download operation.
    /// </summary>
    public class YqlDownloadCompletedEventArgs<T> : DownloadCompletedEventArgs, IQueryDownloadCompletedEventArgs where T : ResultBase
    {
        /// <summary>
        /// Gets the response of the download operation.
        /// </summary>
        public YqlResponse<T> Response { get { return (YqlResponse<T>)base.ResponseBase; } }


        IQueryResponse IQueryDownloadCompletedEventArgs.Response { get { return this.Response; } }


        internal YqlDownloadCompletedEventArgs(object userArgs, YqlResponse<T> response) : base(userArgs, response) { }
    }
}

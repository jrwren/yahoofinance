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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MaasOne.Net
{

    public delegate void AsyncDownloadCompletedEventHandler(object sender, DownloadCompletedEventArgs e);

    public interface IQueryDownload
    {
#if !(PCL40)
        IWebProxy Proxy { get; set; }

        int Timeout { get; set; }
#endif

        DownloadOperationCollection ActiveOperations { get; }

        QueryBase DefaultQuery { get; set; }


#if !(PCL40 || PCL45)
        IQueryResponse Download();

        IQueryResponse Download(QueryBase query);
#endif


        event AsyncDownloadCompletedEventHandler AsyncDownloadCompletedBase;

        void DownloadAsync(object userArgs);

        void DownloadAsync(QueryBase query, object userArgs);


#if !(NET20 || PCL40)
        System.Threading.Tasks.Task<IQueryResponse> DownloadTaskAsync();

        System.Threading.Tasks.Task<IQueryResponse> DownloadTaskAsync(QueryBase query);
#endif


        bool IsCorrespondingType(Type queryType);
        bool IsCorrespondingType(QueryBase queryObject);
    }

    public interface IQueryResponse
    {
        QueryBase Query { get; }
        ResultBase Result { get; }
        ConnectionInfo Connection { get; }
    }

    public interface IQueryDownloadCompletedEventArgs
    {
        object UserArgs { get; }
        IQueryResponse Response { get; }
    }


    public abstract class QueryBase
    {
        /// <summary>
        /// Checks this query for validity.
        /// </summary>
        /// <returns>A boolean value indicating the success state.</returns>
        public bool IsValid() { return this.Validate().Success; }

        /// <summary>
        /// Checks this query for validity and returns detailed information.
        /// </summary>
        /// <returns>The <see cref="MaasOne.Net.ValidationResult"/> object that indicates the validity status.</returns>
        public ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();
            this.Validate(result);
            return result;
        }

        /// <summary>
        /// The override method checks whether the query is able to create a valid URL with <see cref="CreateUrl()"/> method.
        /// </summary>
        /// <param name="result">The clean <see cref="MaasOne.Net.ValidationResult"/> object that indicates the validity status.</param>
        protected abstract void Validate(ValidationResult result);
        internal void ValidateInternal(ValidationResult result) { this.Validate(result); }

        /// <summary>
        /// Creates the URL that is used for downloading data.
        /// </summary>
        /// <returns>The URL of the data source.</returns>
        protected abstract string CreateUrl();
        internal string CreateUrlInternal() { return this.CreateUrl(); }

        /// <summary>
        /// Creates a shallow copy of this query.
        /// </summary>
        /// <returns>The shallow copy of this query.</returns>
        public abstract QueryBase Clone();


        protected internal class ConvertInfo
        {
            public bool IsDirectSource { get; private set; }
            public bool IsIntegrityComplete { get; set; }
            public List<string> IntegrityMessages { get; private set; }

            internal ConvertInfo(bool isDirectSource)
            {
                this.IsDirectSource = isDirectSource;
                this.IsIntegrityComplete = true;
                this.IntegrityMessages = new List<string>();
            }
        }
    }

}

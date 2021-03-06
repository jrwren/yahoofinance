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
            public System.Collections.Generic.List<string> IntegrityMessages { get; private set; }

            internal ConvertInfo(bool isDirectSource)
            {
                this.IsDirectSource = isDirectSource;
                this.IsIntegrityComplete = true;
                this.IntegrityMessages = new System.Collections.Generic.List<string>();
            }
        }
    }




    public delegate void AsyncDownloadCompletedEventHandler(object sender, DownloadCompletedEventArgs e);




 




  




  
}

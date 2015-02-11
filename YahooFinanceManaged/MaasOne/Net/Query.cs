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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            public List<string> IntegrityMessages { get; private set; }

            internal ConvertInfo(bool isDirectSource)
            {
                this.IsDirectSource = isDirectSource;
                this.IsIntegrityComplete = true;
                this.IntegrityMessages = new List<string>();
            }
        }
    }


    /// <summary>
    /// Provides methods for creating a web request and converting the response to the aim data format.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    public abstract class Query<T> : QueryBase where T : ResultBase
    {
        /// <summary>
        /// Converts the received <see cref="System.IO.Stream"/> to the generic result object.
        /// </summary>
        /// <param name="stream">The <see cref="System.IO.Stream"/> received from the data source.</param>
        /// <returns></returns>
        protected abstract T ConvertResult(System.IO.Stream stream, ConvertInfo ci);
        internal T ConvertResultInternal(System.IO.Stream stream, ConvertInfo ci) { return this.ConvertResult(stream, ci); }
    }

    public abstract class YqlQuery<T> : Query<T> where T : ResultBase
    {
        internal YqlQuery() { }


        protected override T ConvertResult(System.IO.Stream stream, ConvertInfo ci)
        {
            string htmlText = MyHelper.StreamToString(stream);
            JObject htmlDoc = HtmlToJsonParser.Parse(htmlText);
            JToken yqlToken = this.YqlTokenFromDirectSource(htmlDoc);
            return this.YqlConvertToken(yqlToken, ci);
        }


        internal abstract T YqlConvertToken(JToken yqlToken, ConvertInfo ci);

        internal abstract JToken YqlTokenFromDirectSource(JObject htmlDoc);

        internal abstract string YqlXPath();
    }
}

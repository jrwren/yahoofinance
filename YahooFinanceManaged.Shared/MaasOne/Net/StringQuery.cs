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
    /// <summary>
    /// Provides the settings and methods that are needed for downloading a string resource.
    /// </summary>
    public class StringQuery : Query<StringResult>
    {
        public StringQuery() { }

        public StringQuery(string url) : this(new Uri(url, UriKind.RelativeOrAbsolute)) { }

        public StringQuery(Uri url) { this.Url = url; this.Encoding = System.Text.Encoding.UTF8; }



        /// <summary>
        /// Gets or sets the URL of the string resource.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets the encoding of the string resource.
        /// </summary>
        public System.Text.Encoding Encoding { get; set; }



        /// <summary>
        /// Creates a shallow copy of this query.
        /// </summary>
        /// <returns>The shallow copy of this query.</returns>
        public override QueryBase Clone()
        {
            return new StringQuery(this.Url);
        }


        /// <summary>
        /// Converts the received <see cref="System.IO.Stream"/> to the generic result object.
        /// </summary>
        /// <param name="stream">The <see cref="System.IO.Stream"/> received from the data source.</param>
        /// <returns></returns>
        protected override StringResult ConvertResult(System.IO.Stream stream, ConvertInfo ci)
        {
            string result = string.Empty;
            using (var r = new System.IO.StreamReader(stream, this.Encoding))
            {
                result = r.ReadToEnd();
            }
            return new StringResult(result);
        }

        /// <summary>
        /// Creates the URL that is used for downloading data.
        /// </summary>
        /// <returns>The URL of the data source.</returns>
        protected override string CreateUrl()
        {
            return this.Url.AbsoluteUri;
        }

        /// <summary>
        /// Checks whether the query is able to create a valid URL with <see cref="CreateUrl()"/> method.
        /// </summary>
        /// <param name="result">The clean <see cref="MaasOne.Net.ValidationResult"/> object that indicates the validity status.</param>
        protected override void Validate(ValidationResult result)
        {
            if (this.Url == null)
            {
                result.Success = false;
                result.Info.Add("Url", "The Url is NULL.");
            }
        }
    }
}

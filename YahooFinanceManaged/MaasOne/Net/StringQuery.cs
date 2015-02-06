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
    public class StringQuery : Query<string>
    {

        public Uri Url { get; set; }
        public System.Text.Encoding Encoding { get; set; }

        public StringQuery() { }
        public StringQuery(string url) : this(new Uri(url, UriKind.RelativeOrAbsolute)) { }
        public StringQuery(Uri url) { this.Url = url; this.Encoding = System.Text.Encoding.UTF8; }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.Url == null)
            {
                result.Success = false;
                result.Info.Add("Url", "The Url is NULL.");
            }
        }
        protected override Uri CreateUrl()
        {
            return this.Url;
        }
        protected override string ConvertResult(System.IO.Stream stream)
        {           
            string result = string.Empty;
            using (var r = new System.IO.StreamReader(stream, this.Encoding))
            {
                result = r.ReadToEnd();
            }
            return result;
        }
        public override Query<string> Clone()
        {
            return new StringQuery(this.Url);
        }

    }
}

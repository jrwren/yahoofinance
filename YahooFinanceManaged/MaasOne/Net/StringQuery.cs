// **************************************************************************************************
// **  
// **  Yahoo! Finance Managed
// **  Written by Marius Häusler 2015
// **  It would be pleasant, if you could contact me when you are using this code.
// **  Contact: maasone@live.com
// **  Project Home: https://yahoofinance.codeplex.com/
// **  
// **************************************************************************************************
// **  
// **  Copyright @ Marius Häusler
// **  
// **  Licensed under GNU Lesser General Public License (LGPL) (Version 2.1, February 1999).
// **  
// **  License Text: https://yahoofinance.codeplex.com/license
// **  
// **  
// **************************************************************************************************
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
                result.Info.Add(new KeyValuePair<string, string>("Url", "The Url is NULL."));
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

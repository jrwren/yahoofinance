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
using System.Net;
using System.Collections.Generic;

namespace MaasOne.Rss
{

    public class FeedQuery : Query<Feed>
    {

        public Uri Url { get; set; }

        public FeedQuery() { }
        public FeedQuery(string url) : this(new Uri(url)) { }
        public FeedQuery(Uri url) { this.Url = url; }

        protected override void ValidateQuery(ValidationResult result)
        {
            if (this.Url == null)
            {
                result.Success = false;
                result.Info.Add(new KeyValuePair<string, string>("Url", "The Url is null."));
            }
        }
        protected override Uri CreateUrl() { return this.Url; }
        protected override Feed ConvertResult(System.IO.Stream stream) { return (Feed)new System.Xml.Serialization.XmlSerializer(typeof(Feed)).Deserialize(stream); }
        public override Query<Feed> Clone() { return new FeedQuery(this.Url); }
        
    }

}

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

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
using MaasOne.Net;
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
                result.Info.Add("Url", "The Url is NULL.");
            }
        }
        protected override Uri CreateUrl() { return this.Url; }
        protected override Feed ConvertResult(System.IO.Stream stream) { return (Feed)new System.Xml.Serialization.XmlSerializer(typeof(Feed)).Deserialize(stream); }
        public override Query<Feed> Clone() { return new FeedQuery(this.Url); }
        
    }

}

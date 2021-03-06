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

namespace MaasOne.Rss
{
    public class FeedQuery : Query<Feed>
    {
        public FeedQuery() { }

        public FeedQuery(string url) : this(new Uri(url)) { }

        public FeedQuery(Uri url) { this.Url = url; }



        public Uri Url { get; set; }



        public override QueryBase Clone() { return new FeedQuery(this.Url); }


        protected override Feed ConvertResult(System.IO.Stream stream, ConvertInfo ci) { return (Feed)new System.Xml.Serialization.XmlSerializer(typeof(Feed)).Deserialize(stream); }

        protected override string CreateUrl() { return this.Url.AbsoluteUri; }

        protected override void Validate(ValidationResult result)
        {
            if (this.Url == null)
            {
                result.Success = false;
                result.Info.Add("Url", "Url is NULL.");
            }
        }
    }
}

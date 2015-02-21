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
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.Net;
using MaasOne.Net.UnitTest;
using MaasOne.Rss;

namespace MaasOne.Rss.UnitTest
{
    [TestClass]
    public class RssFeedTest : DownloadTest<Feed>
    {
        public override Query<Feed>[] CreateQueries()
        {
            FeedQuery query = new FeedQuery();
            query.Url = new Uri("http://finance.yahoo.com/rss/2.0/headline?s=yhoo&region=US&lang=en-US", UriKind.RelativeOrAbsolute);

            return new Query<Feed>[] { query };
        }

        public override void CheckResult(Feed result, Query<Feed> responseQuery)
        {
            var query = (FeedQuery)responseQuery;

            Assert.IsNotNull(result);
        }
    }
}

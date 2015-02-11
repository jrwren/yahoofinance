using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.Rss;
using MaasOne.Net;

namespace MaasOne.UnitTest.YahooFinance
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

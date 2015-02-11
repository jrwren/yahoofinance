using System;
using MaasOne.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class CompanyEventsTest : DownloadTest<CompanyEventsResult>
    {
        public override Query<CompanyEventsResult>[] CreateQueries()
        {
            return new Query<CompanyEventsResult>[]{
                new CompanyEventsQuery("YHOO"),
                new CompanyEventsQuery("MSFT"),
                new CompanyEventsQuery("GOOG")
            };
        }

        public override void CheckResult(CompanyEventsResult result, Query<CompanyEventsResult> responseQuery)
        {
            var query = (CompanyEventsQuery)responseQuery;

            Assert.IsNotNull(result.ShortInfo);

            Assert.AreNotEqual(0, result.RecentEvents);
            Assert.AreNotEqual(0, result.UpcomingEvents);
            foreach (var ev in result.RecentEvents)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(ev.Title));
            }
        }
    }
}

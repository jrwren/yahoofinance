using System;
using MaasOne.Net;
using MaasOne;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;
using MaasOne.YahooFinance.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class HistoricalQuotesTest : DownloadTest<HistoricalQuotesResult>
    {
        public override Query<HistoricalQuotesResult>[] CreateQueries()
        {
            HistoricalQuotesQuery query = new HistoricalQuotesQuery();
            query.ID = "YHOO";
            query.FromDate = new DateTime(2014, 12, 1);
            query.ToDate = new DateTime(2014, 12, 31);
            query.Interval = HistoricalQuotesInterval.Daily;

            return new Query<HistoricalQuotesResult>[] { query };
        }

        public override void CheckResult(HistoricalQuotesResult result, Query<HistoricalQuotesResult> responseQuery)
        {
            Assert.AreNotEqual(0, result.Chain.Items.Count);
            foreach (var item in result.Chain)
            {
                Assert.AreNotEqual(0, item.Close);
            }
        }
    }
}

using System;
using MaasOne.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class CompanyKeyStatisticsTest : DownloadTest<CompanyKeyStatisticsResult>
    {
        public override Query<CompanyKeyStatisticsResult>[] CreateQueries()
        {
            return new Query<CompanyKeyStatisticsResult>[]{
                new CompanyKeyStatisticsQuery("YHOO"),
                new CompanyKeyStatisticsQuery("MSFT"),
                new CompanyKeyStatisticsQuery("GOOG")
            };
        }

        public override void CheckResult(CompanyKeyStatisticsResult result, Query<CompanyKeyStatisticsResult> responseQuery)
        {
            var query = (CompanyKeyStatisticsQuery)responseQuery;

            Assert.IsNotNull(result.Statistics);
            Assert.IsNotNull(result.ShortInfo);
            if (result.Statistics != null)
            {
                Assert.IsNotNull(result.Statistics.FinancialHighlights);
                if (result.Statistics.FinancialHighlights != null)
                {

                }
            }
        }
    }
}

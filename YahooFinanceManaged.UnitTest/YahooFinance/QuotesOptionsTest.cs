using System;
using MaasOne.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class QuotesOptionsTest : DownloadTest<QuotesOptionsResult>
    {
        public override Query<QuotesOptionsResult>[] CreateQueries()
        {
            return new YqlQuery<QuotesOptionsResult>[]{
                new QuotesOptionsQuery("YHOO"),
                new QuotesOptionsQuery("MSFT"),
                new QuotesOptionsQuery("GOOG")
            };
        }

        public override void CheckResult(QuotesOptionsResult result, Query<QuotesOptionsResult> responseQuery)
        {
            var query = (QuotesOptionsQuery)responseQuery;

            Assert.IsNotNull(result.ShortInfo);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ShortInfo.ID));
            Assert.AreNotEqual(0, result.PutChain.Length);
            Assert.AreNotEqual(0, result.CallChain.Length);
            if (result.PutChain.Length > 0)
            {
                foreach (var hi in result.PutChain)
                {
                    Assert.IsFalse(string.IsNullOrWhiteSpace(hi.ID));
                }
            }
        }
    }
}

using System;
using MaasOne.Net;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;


namespace MaasOne.UnitTest.YahooFinance
{

    [TestClass]
    public class IDSearchTest : DownloadTest<IDSearchResult>
    {
        private string[] Queries = new string[] { "micro", "yaho", "googl", "bas", "germany" };
        private Random rnd = new Random(DateTime.Now.Millisecond);

        public override Query<IDSearchResult>[] CreateQueries()
        {
            IDSearchQuery query = new IDSearchQuery();
            query.LookupText = "micros"; //Queries[rnd.Next(0, Queries.Length - 1)];
            query.Server = YahooServer.USA;

            return new Query<IDSearchResult>[] { query };
        }

        public override void CheckResult(IDSearchResult result, Query<IDSearchResult> responseQuery)
        {
            Assert.AreNotEqual(0, result.Items.Length);
            foreach (var item in result.Items)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(item.ID));
            }
        }
    }
}


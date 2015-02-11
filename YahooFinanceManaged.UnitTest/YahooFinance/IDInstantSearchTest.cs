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
    public class IDSearchInstantTest : DownloadTest<IDSearchInstantResult>
    {
        private string[] Queries = new string[] { "micro", "yaho", "googl", "bas", "germany" };
        private Random rnd = new Random(DateTime.Now.Millisecond);

        public override Query<IDSearchInstantResult>[] CreateQueries()
        {
            IDSearchInstantQuery query = new IDSearchInstantQuery();
            query.LookupText = "micros"; //Queries[rnd.Next(0, Queries.Length - 1)];
            return new Query<IDSearchInstantResult>[] { query };
        }

        public override void CheckResult(IDSearchInstantResult result, Query<IDSearchInstantResult> responseQuery)
        {
            Assert.AreNotEqual(result.Items.Length, 0);
            foreach (var item in result.Items)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(item.ID));
            }
        }
    }
}

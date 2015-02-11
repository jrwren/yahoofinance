using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;
using MaasOne.Net;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class IndustryInfoTest : DownloadTest<IndustryInfoResult>
    {
        public override Query<IndustryInfoResult>[] CreateQueries()
        {
            return new Query<IndustryInfoResult>[]{
                new IndustryInfoQuery(3),
                new IndustryInfoQuery(6),
                new IndustryInfoQuery(112)
            };
        }

        public override void CheckResult(IndustryInfoResult result, Query<IndustryInfoResult> responseQuery)
        {
            Assert.AreNotEqual(0, result.Infos.Items.Length);
            foreach (var item in result.Infos.Items)
            {
                Assert.IsFalse(string.IsNullOrEmpty(item.Name), "Item invalid.");
            }
        }
    }
}

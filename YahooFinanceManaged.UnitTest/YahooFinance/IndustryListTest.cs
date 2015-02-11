using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;
using MaasOne.Net;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class IndustryListTest : DownloadTest<IndustryListResult>
    {
        public override Query<IndustryListResult>[] CreateQueries()
        {
            return new Query<IndustryListResult>[] { new IndustryListQuery() };
        }

        public override void CheckResult(IndustryListResult result, Query<IndustryListResult> responseQuery)
        {
 
            Assert.AreNotEqual(0, result.Sectors.Length);
            Assert.AreNotEqual(0, result.Industries.Length);

            var wm = WorldMarket.LoadInternalSource();

            Assert.AreEqual(wm.GetAllSectors().Length, result.Sectors.Length);
            Assert.AreEqual(wm.GetAllIndustries().Length, result.Industries.Length);
     
        }
    }
}

using System;
using MaasOne.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class AnalyseUpDowngradesTest : DownloadTest<AnalyseUpDowngradesResult>
    {
        public override Query<AnalyseUpDowngradesResult>[] CreateQueries()
        {
            return new Query<AnalyseUpDowngradesResult>[] { 
                new AnalyseUpDowngradesQuery("YHOO"),
                new AnalyseUpDowngradesQuery("MSFT"),
                new AnalyseUpDowngradesQuery("GOOG")
            };
        }

        public override void CheckResult(AnalyseUpDowngradesResult result, Query<AnalyseUpDowngradesResult> responseQuery)
        {
            var query = (AnalyseUpDowngradesQuery)responseQuery;
            
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ID));
            Assert.AreNotEqual(0, result.History.Length);
            if (result.History.Length > 0)
            {
                foreach (var hi in result.History)
                {
                    Assert.IsFalse(string.IsNullOrWhiteSpace(hi.ResearchFirm));
                }
            }
        }
    }
}

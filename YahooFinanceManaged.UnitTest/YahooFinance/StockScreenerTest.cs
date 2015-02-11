using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;
using MaasOne.YahooFinance.Data;
using MaasOne.Net;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class StockScreenerTest : DownloadTest<ScreenerResult>
    {
        public override Query<ScreenerResult>[] CreateQueries()
        {
            StockScreenerQuery query = new StockScreenerQuery();

            var criterias = new List<ScreenerCriteria>();

            var c1 = new StockIndustryCriteria();
            c1.IndustryID = 112;
            criterias.Add(c1);

            query.Criterias = criterias.ToArray();
            query.View = StockScreenerView.All;

            return new Query<ScreenerResult>[] { query };
        }

        public override void CheckResult(ScreenerResult result, Query<ScreenerResult> responseQuery)
        {
            Assert.IsTrue(result.Items.Length > 0, "No Items");

            foreach (var item in result.Items)
            {
                Assert.IsFalse(string.IsNullOrEmpty(item.Name), "Item invalid.");
            }
        }
    }
}

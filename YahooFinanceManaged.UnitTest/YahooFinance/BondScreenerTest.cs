using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.Net;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;
using MaasOne.YahooFinance.Data;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class BondScreenerTest : DownloadTest<ScreenerResult>
    {

        public override Query<ScreenerResult>[] CreateQueries()
        {
            BondScreenerQuery query = new BondScreenerQuery();
            var criterias = new List<ScreenerCriteria>();

            var c1 = new BondPriceCriteria();
            c1.Price = BondPriceType.Premium;
            criterias.Add(c1);

            query.Criterias = criterias.ToArray();
            return new Query<ScreenerResult>[] { query };
        }

        public override void CheckResult(ScreenerResult result, Query<ScreenerResult> responseQuery)
        {
            Assert.IsTrue(result.Items.Length > 0, "No Items");
            foreach (var item in result.Items)
            {
                Assert.IsTrue(item.Values.ContainsKey(ScreenerProperty.Issue), "Item invalid.");
                Assert.IsTrue(item.Values.ContainsKey(ScreenerProperty.Type), "Item invalid.");
                if (item.Values.ContainsKey(ScreenerProperty.Issue) && item.Values.ContainsKey(ScreenerProperty.Type))
                {

                }
            }
        }

    }
}

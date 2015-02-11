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
    public class FundScreenerTest : DownloadTest<ScreenerResult>
    {

        public override Query<ScreenerResult>[] CreateQueries()
        {
            FundScreenerQuery query = new FundScreenerQuery();

            var criterias = new List<ScreenerCriteria>();

            var c1 = new FundMorningstarCriteria();
            c1.MinimumRating = FundMorningstarRating.ThreeStars;
            c1.MaximumRating = FundMorningstarRating.FourStars;
            criterias.Add(c1);

            query.Criterias = criterias.ToArray();
            query.View = FundScreenerView.All;

            return new Query<ScreenerResult>[] { query };
        }

        public override void CheckResult(ScreenerResult result, Query<ScreenerResult> responseQuery)
        {
            Assert.AreNotEqual(0, result.Items.Length);
            foreach (var item in result.Items)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(item.Name));
            }
        }

    }
}

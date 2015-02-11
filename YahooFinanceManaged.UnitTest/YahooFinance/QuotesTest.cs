using System;
using MaasOne.Net;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;
using MaasOne.YahooFinance.Data;

namespace MaasOne.UnitTest.YahooFinance
{

    [TestClass]
    public class QuotesTest : DownloadTest<QuotesResult>
    {

        public override Query<QuotesResult>[] CreateQueries()
        {
            string[] ids = new string[] { "MSFT", "YHOO" };
            QuoteProperty[] properties = new QuoteProperty[] {
                QuoteProperty.Symbol, 
                QuoteProperty.LastTradePriceOnly, 
                QuoteProperty.DaysHigh, 
                QuoteProperty.DaysLow, 
                QuoteProperty.Volume, 
                QuoteProperty.LastTradeDate, 
                QuoteProperty.LastTradeTime };
            QuotesQuery query = new QuotesQuery();
            query.IDs = ids;
            query.Properties = properties;

            return new Query<QuotesResult>[] { query };
        }

        public override void CheckResult(QuotesResult result, Query<QuotesResult> responseQuery)
        {
            var query = (QuotesQuery)responseQuery;
            Assert.AreEqual(query.IDs.Length, result.Items.Length);

            foreach (var q in result.Items)
            {
                Assert.AreEqual(query.Properties.Length, q.Values.Keys.Count);

                if (query.Properties.Length == q.Values.Keys.Count)
                {
                    for (int i = 0; i < query.Properties.Length; i++)
                    {
                        Assert.IsTrue(q.Values.ContainsKey(query.Properties[i]));
                    }
                }
                Assert.IsNotNull(q[QuoteProperty.Symbol]);
                Assert.IsNotNull(q[QuoteProperty.LastTradePriceOnly]);
            }



            Assert.IsNotNull(result.Items[1][QuoteProperty.LastTradePriceOnly]);
        }

    }
}

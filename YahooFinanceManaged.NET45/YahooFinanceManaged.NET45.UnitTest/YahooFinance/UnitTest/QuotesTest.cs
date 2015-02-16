#region "License"
// *********************************************************************************************
// **                                                                                         **
// **  Yahoo! Finance Managed                                                                 **
// **                                                                                         **
// **  Copyright (c) Marius Häusler 2009-2015                                                 **
// **                                                                                         **
// **  Licensed under GNU Lesser General Public License (LGPL) (Version 2.1, February 1999).  **
// **                                                                                         **
// **  License: https://www.gnu.org/licenses/old-licenses/lgpl-2.1.txt                        **
// **                                                                                         **
// **  Project: https://yahoofinance.codeplex.com/                                            **
// **                                                                                         **
// **  Contact: maasone@live.com                                                              **
// **                                                                                         **
// *********************************************************************************************
#endregion
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.Net;
using MaasOne.Net.UnitTest;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;
using MaasOne.YahooFinance.Data;

namespace MaasOne.YahooFinance.UnitTest
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

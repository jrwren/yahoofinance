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

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class HistoricalQuotesTest : DownloadTest<HistoricalQuotesResult>
    {
        public override Query<HistoricalQuotesResult>[] CreateQueries()
        {
            HistoricalQuotesQuery query = new HistoricalQuotesQuery();
            query.ID = "YHOO";
            query.FromDate = new DateTime(2014, 12, 1);
            query.ToDate = new DateTime(2014, 12, 31);
            query.Interval = HistoricalQuotesInterval.Daily;

            return new Query<HistoricalQuotesResult>[] { query };
        }

        public override void CheckResult(HistoricalQuotesResult result, Query<HistoricalQuotesResult> responseQuery)
        {
            Assert.AreNotEqual(0, result.Chain.Items.Count);
            foreach (var item in result.Chain)
            {
                Assert.AreNotEqual(0, item.Close);
            }
        }
    }
}

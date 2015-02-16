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
    public class CompanyKeyStatisticsTest : DownloadTest<CompanyKeyStatisticsResult>
    {
        public override Query<CompanyKeyStatisticsResult>[] CreateQueries()
        {
            return new Query<CompanyKeyStatisticsResult>[]{
                new CompanyKeyStatisticsQuery("YHOO"),
                new CompanyKeyStatisticsQuery("MSFT"),
                new CompanyKeyStatisticsQuery("GOOG")
            };
        }

        public override void CheckResult(CompanyKeyStatisticsResult result, Query<CompanyKeyStatisticsResult> responseQuery)
        {
            var query = (CompanyKeyStatisticsQuery)responseQuery;

            Assert.IsNotNull(result.Statistics);
            Assert.IsNotNull(result.ShortInfo);
            if (result.Statistics != null)
            {
                Assert.IsNotNull(result.Statistics.FinancialHighlights);
                if (result.Statistics.FinancialHighlights != null)
                {

                }
            }
        }
    }
}

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
    public class CompanyEventsTest : DownloadTest<CompanyEventsResult>
    {
        public override Query<CompanyEventsResult>[] CreateQueries()
        {
            return new Query<CompanyEventsResult>[]{
                new CompanyEventsQuery("YHOO"),
                new CompanyEventsQuery("MSFT"),
                new CompanyEventsQuery("GOOG")
            };
        }

        public override void CheckResult(CompanyEventsResult result, Query<CompanyEventsResult> responseQuery)
        {
            var query = (CompanyEventsQuery)responseQuery;

            Assert.IsNotNull(result.ShortInfo);

            Assert.AreNotEqual(0, result.RecentEvents);
            Assert.AreNotEqual(0, result.UpcomingEvents);
            foreach (var ev in result.RecentEvents)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(ev.Title));
            }
        }
    }
}

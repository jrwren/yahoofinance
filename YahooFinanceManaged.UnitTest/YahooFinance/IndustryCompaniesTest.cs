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
using MaasOne.Net;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class IndustryCompaniesTest : DownloadTest<IndustryCompaniesResult>
    {
        public override Query<IndustryCompaniesResult>[] CreateQueries()
        {
            return new Query<IndustryCompaniesResult>[]{
                new IndustryCompaniesQuery(112),
                new IndustryCompaniesQuery(132),
                new IndustryCompaniesQuery(824)
            };
        }

        public override void CheckResult(IndustryCompaniesResult result, Query<IndustryCompaniesResult> responseQuery)
        {
            var query = (IndustryCompaniesQuery)responseQuery;
            Assert.AreNotEqual(0, result.Companies.Length);
        }
    }
}

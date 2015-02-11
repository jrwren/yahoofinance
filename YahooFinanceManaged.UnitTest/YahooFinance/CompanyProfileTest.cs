﻿#region "License"
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class CompanyProfileTest : DownloadTest<CompanyProfileResult>
    {
        public override Query<CompanyProfileResult>[] CreateQueries()
        {
            return new Query<CompanyProfileResult>[]{
                new CompanyProfileQuery("YHOO"),
                new CompanyProfileQuery("MSFT"),
                new CompanyProfileQuery("GOOG")
            };
        }

        public override void CheckResult(CompanyProfileResult result, Query<CompanyProfileResult> responseQuery)
        {
            var query = (CompanyProfileQuery)responseQuery;
            Assert.IsNotNull(result.ShortInfo);
            Assert.IsNotNull(result.Profile);
            if (result.Profile != null)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(result.Profile.Address));
            }
        }
    }
}

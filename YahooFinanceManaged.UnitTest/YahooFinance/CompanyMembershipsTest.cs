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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.YahooFinance;
using MaasOne.YahooFinance.Web;
using MaasOne.Net;

namespace MaasOne.UnitTest.YahooFinance
{
    [TestClass]
    public class CompanyMembershipsTest : DownloadTest<CompanyMembershipsResult>
    {
        public override Query<CompanyMembershipsResult>[] CreateQueries()
        {
            return new Query<CompanyMembershipsResult>[]{
                new CompanyMembershipsQuery("YHOO"),
                new CompanyMembershipsQuery("MSFT"),
                new CompanyMembershipsQuery("GOOG")
            };
        }

        public override void CheckResult(CompanyMembershipsResult result, Query<CompanyMembershipsResult> responseQuery)
        {
            var query = (CompanyMembershipsQuery)responseQuery;

            Assert.IsNotNull(result.ShortInfo);
            Assert.AreNotEqual(0, result.Indices.Length);
            if (result.Indices.Length > 0)
            {
                foreach (var i in result.Indices)
                {
                    Assert.IsNotNull(i);
                    Assert.IsFalse(string.IsNullOrWhiteSpace(i.ID));
                }
            }
        }
    }
}

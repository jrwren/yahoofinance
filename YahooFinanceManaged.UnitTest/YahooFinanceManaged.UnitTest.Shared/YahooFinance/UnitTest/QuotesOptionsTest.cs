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
    public class QuotesOptionsTest : DownloadTest<QuotesOptionsResult>
    {
        public override Query<QuotesOptionsResult>[] CreateQueries()
        {
            return new YqlQuery<QuotesOptionsResult>[]{
                new QuotesOptionsQuery("YHOO"),
                new QuotesOptionsQuery("MSFT"),
                new QuotesOptionsQuery("GOOG")
            };
        }

        public override void CheckResult(QuotesOptionsResult result, Query<QuotesOptionsResult> responseQuery)
        {
            var query = (QuotesOptionsQuery)responseQuery;

            Assert.IsNotNull(result.ShortInfo);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ShortInfo.ID));
            Assert.AreNotEqual(0, result.PutChain.Length);
            Assert.AreNotEqual(0, result.CallChain.Length);
            if (result.PutChain.Length > 0)
            {
                foreach (var hi in result.PutChain)
                {
                    Assert.IsFalse(string.IsNullOrWhiteSpace(hi.ID));
                }
            }
        }
    }
}

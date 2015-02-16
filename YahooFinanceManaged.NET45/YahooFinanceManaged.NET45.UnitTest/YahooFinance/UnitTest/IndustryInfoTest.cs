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
    public class IndustryInfoTest : DownloadTest<IndustryInfoResult>
    {
        public override Query<IndustryInfoResult>[] CreateQueries()
        {
            return new Query<IndustryInfoResult>[]{
                new IndustryInfoQuery(3),
                new IndustryInfoQuery(6),
                new IndustryInfoQuery(112)
            };
        }

        public override void CheckResult(IndustryInfoResult result, Query<IndustryInfoResult> responseQuery)
        {
            Assert.AreNotEqual(0, result.Infos.Items.Length);
            foreach (var item in result.Infos.Items)
            {
                Assert.IsFalse(string.IsNullOrEmpty(item.Name), "Item invalid.");
            }
        }
    }
}

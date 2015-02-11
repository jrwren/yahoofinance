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
    public class IndustryListTest : DownloadTest<IndustryListResult>
    {
        public override Query<IndustryListResult>[] CreateQueries()
        {
            return new Query<IndustryListResult>[] { new IndustryListQuery() };
        }

        public override void CheckResult(IndustryListResult result, Query<IndustryListResult> responseQuery)
        {
 
            Assert.AreNotEqual(0, result.Sectors.Length);
            Assert.AreNotEqual(0, result.Industries.Length);

            var wm = WorldMarket.LoadInternalSource();

            Assert.AreEqual(wm.GetAllSectors().Length, result.Sectors.Length);
            Assert.AreEqual(wm.GetAllIndustries().Length, result.Industries.Length);
     
        }
    }
}

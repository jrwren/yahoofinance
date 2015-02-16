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
    public class AnalyseUpDowngradesTest : DownloadTest<AnalyseUpDowngradesResult>
    {
        public override Query<AnalyseUpDowngradesResult>[] CreateQueries()
        {
            return new Query<AnalyseUpDowngradesResult>[] { 
                new AnalyseUpDowngradesQuery("YHOO"),
                new AnalyseUpDowngradesQuery("MSFT"),
                new AnalyseUpDowngradesQuery("GOOG")
            };
        }

        public override void CheckResult(AnalyseUpDowngradesResult result, Query<AnalyseUpDowngradesResult> responseQuery)
        {
            var query = (AnalyseUpDowngradesQuery)responseQuery;
            
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ID));
            Assert.AreNotEqual(0, result.History.Length);
            if (result.History.Length > 0)
            {
                foreach (var hi in result.History)
                {
                    Assert.IsFalse(string.IsNullOrWhiteSpace(hi.ResearchFirm));
                }
            }
        }
    }
}

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
    public class StockScreenerTest : DownloadTest<ScreenerResult>
    {
        public override Query<ScreenerResult>[] CreateQueries()
        {
            StockScreenerQuery query = new StockScreenerQuery();

            var criterias = new List<ScreenerCriteria>();

            var c1 = new StockIndustryCriteria();
            c1.IndustryID = 112;
            criterias.Add(c1);

            query.Criterias = criterias.ToArray();
            query.View = StockScreenerView.All;

            return new Query<ScreenerResult>[] { query };
        }

        public override void CheckResult(ScreenerResult result, Query<ScreenerResult> responseQuery)
        {
            Assert.IsTrue(result.Items.Length > 0, "No Items");

            foreach (var item in result.Items)
            {
                Assert.IsFalse(string.IsNullOrEmpty(item.Name), "Item invalid.");
            }
        }
    }
}

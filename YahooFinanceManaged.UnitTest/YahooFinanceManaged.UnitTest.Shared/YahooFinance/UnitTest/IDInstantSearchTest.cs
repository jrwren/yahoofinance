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
    public class IDSearchInstantTest : DownloadTest<IDSearchInstantResult>
    {
        private string[] Queries = new string[] { "micro", "yaho", "googl", "bas", "germany" };
        private Random rnd = new Random(DateTime.Now.Millisecond);

        public override Query<IDSearchInstantResult>[] CreateQueries()
        {
            IDSearchInstantQuery query = new IDSearchInstantQuery();
            query.LookupText = "micros"; //Queries[rnd.Next(0, Queries.Length - 1)];
            return new Query<IDSearchInstantResult>[] { query };
        }

        public override void CheckResult(IDSearchInstantResult result, Query<IDSearchInstantResult> responseQuery)
        {
            Assert.AreNotEqual(result.Items.Length, 0);
            foreach (var item in result.Items)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(item.ID));
            }
        }
    }
}

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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaasOne.Net;
using MaasOne.YahooFinance.Web;

namespace MaasOne.UnitTest
{
    [TestClass]
    public abstract class DownloadTest<T> where T : ResultBase
    {
        public abstract Query<T>[] CreateQueries();

        public abstract void CheckResult(T result, Query<T> responseQuery);


        [TestMethod]
        public void DownloadSync()
        {
            Query<T>[] queries = this.CreateQueries();

            Assert.AreNotEqual(0, queries.Length);

            DownloadClient<T> dl = new DownloadClient<T>();

            foreach (Query<T> query in queries)
            {
                dl.DefaultQuery = query;
                this.Download(dl);
            }

            if (queries[0] is YqlQuery<T>)
            {
                YqlQuery<T>[] yqlQueries = queries.Cast<YqlQuery<T>>().ToArray();

                YqlDownloadClient<T> ydl = new YqlDownloadClient<T>();
                ydl.DefaultQueries = yqlQueries;

                this.Download(ydl);
            }
        }


        private void Download(DownloadClientBase dl)
        {
            IQueryResponse response = ((IQueryDownload)dl).Download();

            Assert.AreEqual(ConnectionState.Success, response.Connection.State);

            if (response is Response<T>)
            {
                Response<T> resp = (Response<T>)response;
                Assert.IsNotNull(resp.Result);
                this.CheckIntegrity(resp.Result.Integrity);
                this.CheckResult(resp.Result, resp.Query);

            }
            else if (response is YqlResponse<T>)
            {
                YqlResponse<T> resp = (YqlResponse<T>)response;
                Assert.AreEqual(resp.Queries.Length, resp.Results.Length);
                if (resp.Results.Length > 0)
                {
                    for (int i = 0; i < resp.Results.Length; i++)
                    {
                        Assert.IsNotNull(resp.Results[i]);
                        this.CheckIntegrity(resp.Results[i].Integrity);
                        this.CheckResult(resp.Results[i], resp.Queries[i]);
                    }
                }
            }
        }

        private void CheckIntegrity(DataIntegrityInfo integrity)
        {
            if (integrity.IsComplete == false)
            {
                foreach (string message in integrity.Messages)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Integrity Warning: {0}", message));
                }
            }
        }
    }
}

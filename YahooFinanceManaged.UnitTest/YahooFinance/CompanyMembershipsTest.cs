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

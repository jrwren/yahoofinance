using System;
using System.Collections.Generic;

namespace MaasOne.YahooFinance
{

    public class FundFamily
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public FundFamily(string id, string name) { this.ID = id; this.Name = name; }
        internal FundFamily(Resources.WorldMarketFamily family)
        {
            this.ID = family.ID;
            this.Name = family.Name;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is FundFamily && this.ID.Equals(((FundFamily)obj).ID) && this.Name.Equals(((FundFamily)obj).Name);
        }

    }

}

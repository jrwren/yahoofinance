using System;
using System.Collections.Generic;
using MaasOne.Finance.Yahoo.Screener;

namespace MaasOne.Finance.Yahoo.Data
{

    public class ScreenerItemData
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public Dictionary<ScreenerProperty, object> Values { get; private set; }
        public ScreenerItemData() { this.Values = new Dictionary<ScreenerProperty, object>(); }
    }
}

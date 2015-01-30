// **************************************************************************************************
// **  
// **  Yahoo! Finance Managed
// **  Written by Marius Häusler 2015
// **  It would be pleasant, if you could contact me when you are using this code.
// **  Contact: maasone@live.com
// **  Project Home: https://yahoofinance.codeplex.com/
// **  
// **************************************************************************************************
// **  
// **  Copyright @ Marius Häusler
// **  
// **  Licensed under GNU Lesser General Public License (LGPL) (Version 2.1, February 1999).
// **  
// **  License Text: https://yahoofinance.codeplex.com/license
// **  
// **  
// **************************************************************************************************
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

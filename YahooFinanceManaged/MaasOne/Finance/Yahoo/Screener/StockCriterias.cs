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

namespace MaasOne.Finance.Yahoo.Screener
{


    public class StockIndustryCriteria : ScreenerCriteria
    {
        public override string DisplayName { get { return "Industry"; } }
        public override ScreenerProperty Property { get { return ScreenerProperty.Industry; } }
        public int IndustryID { get; set; }

        public StockIndustryCriteria() { }

        protected override bool IsValid()
        {
            return this.IndustryID >= 100 && this.IndustryID < 1000;
        }
        protected override string TagParameter()
        {
            return string.Format("&{0}={1}", YFHelper.GetPropertyTag(this.Property), this.IndustryID);
        }

    }






}

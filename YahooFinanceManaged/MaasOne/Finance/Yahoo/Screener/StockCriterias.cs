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

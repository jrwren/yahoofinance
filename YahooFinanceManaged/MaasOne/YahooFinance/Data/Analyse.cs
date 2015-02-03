using System;
using System.Collections.Generic;

namespace MaasOne.YahooFinance.Data
{
    public class AnalyseUpDowngradesItem
    {
        public DateTime Date { get; set; }
        public string ResearchFirm { get; set; }
        public AnalyseUpDowngradeAction Action { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }

    public enum AnalyseUpDowngradeAction
    {
        Initiated,
        Upgrade,
        Downgrade
    }
}

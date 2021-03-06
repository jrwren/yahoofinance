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
using System.Collections.Generic;

namespace MaasOne.YahooFinance.Data
{
    public class AnalyseUpDowngrade
    {
        public DateTime Date { get; set; }

        public string ResearchFirm { get; set; }

        public AnalyseUpDowngradeAction Action { get; set; }

        public string From { get; set; }

        public string To { get; set; }


        public AnalyseUpDowngrade() { }
    }


    public enum AnalyseUpDowngradeAction
    {
        Initiated,
        Upgrade,
        Downgrade
    }
}

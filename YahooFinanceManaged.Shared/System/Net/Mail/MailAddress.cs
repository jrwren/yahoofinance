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
#if (SILVERLIGHT)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaasOne.Net.Mail
{
    public class MailAddress
    {
        public string Address { get; set; }
        public string DisplayName { get; set; }
        public MailAddress(string address) { this.Address = address; }
        public MailAddress(string address, string displayName) : this(address) { this.DisplayName = displayName; }
    }
}
#endif
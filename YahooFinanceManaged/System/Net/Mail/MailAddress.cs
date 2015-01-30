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
#if (PORTABLE40 || PORTABLE45)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net.Mail
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
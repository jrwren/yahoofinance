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
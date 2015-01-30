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

namespace System
{
    internal static class Extensions2
    {

#if (PORTABLE40 || PORTABLE45)


        public static string ToLongTimeString(this DateTime d) { return d.ToString("T"); }
        public static string ToLongDateString(this DateTime d) { return d.ToString("D"); }
#endif
        public static bool IsNullOrWhiteSpace(this string self)
        {
#if (NET20)
            if (self != null) { foreach (char c in self) { if (!Char.IsWhiteSpace(c)) return false; } }
            return true;
#else
            return string.IsNullOrWhiteSpace(self);
#endif
        }
    }

}

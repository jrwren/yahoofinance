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

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

namespace MaasOne.Net
{
    /// <summary>
    /// This exception is thrown when a parsing process fails.
    /// </summary>
#if !(SILVERLIGHT || NETFX_CORE)
    [Serializable]
#endif
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message) { }

        public ParseException(Exception innerException) : base("An Exception was thrown during parsing process. See InnerException for more details.", innerException) { }

        public ParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}

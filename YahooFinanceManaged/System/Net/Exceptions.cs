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

namespace System.Net
{

    public class ConvertException : Exception
    {
        public ConvertException(string message) : base(message) { }
        public ConvertException(Exception innerException) : base(innerException is ConvertException ? innerException.Message : "An Exception was thrown during result conversion process. See InnerException for more details.", innerException is ConvertException ? innerException.InnerException : innerException) { }
        public ConvertException(string message, Exception innerException) : base(innerException is ConvertException ? innerException.Message : message, innerException is ConvertException ? innerException.InnerException : innerException) { }

    }

}

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

    public class ValidationResult
    {

        public bool Success { get; set; }
        public List<KeyValuePair<string, string>> Info { get; set; }

        public ValidationResult()
        {
            this.Success = true;
            this.Info = new List<KeyValuePair<string,string>>();
        }
        
        internal Exception CreateException()
        {
            if (this.Success) { return null; }
            else
            {
                string message = string.Empty;
                foreach (var info in this.Info)
                {
                    message += string.Format("\n{0}: \"{1}\"", info.Key, info.Value);
                }
                return new ArgumentException("The query is not valid.\n" + message + "\n\n(Is the Clone() method copying all the data?)");
            }
        }

    }

}

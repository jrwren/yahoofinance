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

namespace MaasOne.Net
{

    public class ValidationResult
    {

        public bool Success { get; set; }
        public Dictionary<string, string> Info { get; private set; }

        public ValidationResult()
        {
            this.Success = true;
            this.Info = new Dictionary<string, string>();
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

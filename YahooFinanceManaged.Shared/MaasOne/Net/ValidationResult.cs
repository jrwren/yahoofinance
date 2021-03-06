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
    /// <summary>
    /// Provides information about a validity check for a query.
    /// </summary>
    public class ValidationResult
    {
        public ValidationResult()
        {
            this.Success = true;
            this.Info = new Dictionary<string, string>();
        }



        /// <summary>
        /// Gets the list of additional information for each issue which prevents a positive validity check.
        /// </summary>
        public Dictionary<string, string> Info { get; private set; }

        /// <summary>
        /// Gets or sets the value whether the checked query is valid.
        /// </summary>
        public bool Success { get; set; }



        internal ArgumentException CreateException()
        {
            if (this.Success) { return null; }
            else
            {
                string message = string.Empty;
                foreach (var info in this.Info)
                {
                    message += string.Format("\n\n{0}: \"{1}\"", info.Key, info.Value);
                }
                return new ArgumentException("The query is not valid.\n" + message + "\n\n(Is the Clone() method copying all data?)");
            }
        }
    }
}

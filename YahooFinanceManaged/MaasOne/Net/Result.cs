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
    public abstract class ResultBase
    {
        [System.Xml.Serialization.XmlIgnore()]
        public DataIntegrityInfo Integrity { get; internal set; }
    }


    public class DataIntegrityInfo
    {
        public bool IsComplete { get; private set; }

        public string[] Messages { get; private set; }

        internal DataIntegrityInfo(bool isComplete, string[] messages)
        {
            this.IsComplete = isComplete;
            this.Messages = messages != null ? messages : new string[0];
        }
    }
}

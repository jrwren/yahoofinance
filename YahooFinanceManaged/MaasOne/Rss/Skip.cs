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
using System.Xml.Serialization;

namespace MaasOne.Rss
{
    [XmlRootAttribute(ElementName = "day")] 
    public class Day {
        /// <summary>
        /// The name of the day.
        /// </summary>
        [XmlText()]
        public string Value { get; set; }
    }
    [XmlRootAttribute(ElementName = "hour")] 
    public class Hour
    {
        /// <summary>
        /// The Hour value.
        /// </summary>
        [XmlText()]
        public int Value { get; set; }
    }
}

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
using System.Xml.Serialization;

namespace MaasOne.Rss
{
    [XmlRootAttribute(ElementName = "day")]
    public class Day
    {
        public Day() { }



        /// <summary>
        /// The name of the day.
        /// </summary>
        [XmlText()]
        public string Value { get; set; }
    }
    [XmlRootAttribute(ElementName = "hour")]
    public class Hour
    {
        public Hour() { }



        /// <summary>
        /// The Hour value.
        /// </summary>
        [XmlText()]
        public int Value { get; set; }
    }
}

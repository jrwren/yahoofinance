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
    /// <summary>
    /// Provides information about the RSS channel where the item comes from.
    /// </summary>
    /// <remarks></remarks>
    [XmlRootAttribute(ElementName = "source")]
    public class Source
    {
        public Source() { }



        /// <summary>
        /// The title of the RSS channel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlText()]
        public string Title { get; set; }

        /// <summary>
        /// The url of the RSS channel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "url")]
        public string Url { get; set; }



        public override string ToString()
        {
            return this.Title;
        }

        public Source Clone()
        {
            var res = new Source();
            res.Title = this.Title;
            res.Url = this.Url;
            return res;
        }
    }

}
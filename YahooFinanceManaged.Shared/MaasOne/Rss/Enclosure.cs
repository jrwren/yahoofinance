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
    /// Enclosure describes a media object that is attached to the item. 
    /// </summary>
    /// <remarks></remarks>
    [XmlRootAttribute(ElementName = "enclosure")]
    public class Enclosure
    {
        public Enclosure()
        {
            this.Url = null;
            this.Type = string.Empty;
            this.Length = 0;
        }



        /// <summary>
        /// The location of the object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// The MIME type of the object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// The size in Bytes of the object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlAttribute(AttributeName = "length")]
        public long Length { get; set; }



        public override string ToString()
        {
            return this.Type;
        }

        public Enclosure Clone()
        {
            var res = new Enclosure();
            res.Length = this.Length;
            res.Type = this.Type;
            res.Url = this.Url;
            return res;
        }
    }

}
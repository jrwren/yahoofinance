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
    /// Provides information about catigorization of the item.
    /// </summary>
    /// <remarks></remarks>
    [XmlRootAttribute(ElementName = "category")]
    public class Category
    {
        public Category() { }



        /// <summary>
        /// The name of the category.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlText()]
        public string Name { get; set; }

        [XmlElement(ElementName = "copyright")]
        public string Domain { get; set; }



        public override string ToString()
        {
            return this.Name;
        }

        public Category Clone()
        {
            var res = new Category();
            res.Domain = this.Domain;
            res.Name = this.Name;
            return res;
        }
    }
}
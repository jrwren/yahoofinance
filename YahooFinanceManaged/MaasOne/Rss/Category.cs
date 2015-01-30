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

        public Category() { }
        public override string ToString()
        {
            return this.Name;
        }

        public Category Clone() {
            var res = new Category();
            res.Domain = this.Domain;
            res.Name = this.Name;
            return res;
        }
    }

}
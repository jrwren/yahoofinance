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
    /// Provides information about a globally unique identifier. 
    /// </summary>
    /// <remarks></remarks>
    public class GUID
    {
        /// <summary>
        /// It's a string that uniquely identifies the item. When present, an aggregator may choose to use this string to determine if an item is new.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>There are no rules for the syntax of a guid. Aggregators must view them as a string. It's up to the source of the feed to establish the uniqueness of the string.</remarks>
        [XmlText()]
        public string ID { get; set; }
        /// <summary>
        ///  If the GUID element has an attribute named isPermaLink with a value of true, the reader may assume that it is a permalink to the item, that is, a url that can be opened in a Web browser, that points to the full item described by the FeedItem element.
        /// </summary>
        /// <value>If its value is FALSE, the guid may not be assumed to be a url, or a url to anything in particular.</value>
        /// <returns></returns>
        /// <remarks>IsPermaLink is optional, its default value is TRUE.</remarks>
        [XmlElement(ElementName = "isPermaLink")]
        public bool IsPermaLink { get; set; }

        public GUID() { }

        public override string ToString()
        {
            return this.ID;
        }

        public GUID Clone()
        {
            var res = new GUID();
            res.ID = this.ID;
            res.IsPermaLink = this.IsPermaLink;
            return res;
        }
    }

}
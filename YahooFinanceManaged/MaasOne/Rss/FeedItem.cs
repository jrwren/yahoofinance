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


    /// <summary>
    /// Stores data of a rss feed item.
    /// </summary>
    /// <remarks></remarks>
    [XmlRootAttribute(ElementName = "channel")]
    public class FeedItem
    {

        /// <summary>
        /// Email address of the author of the item.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "author")]
        public string Author { get; set; }
        /// <summary>
        /// Includes the item in one or more categories.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "category", Type = typeof(Category))]
        public List<Category> Category { get; set; }
        /// <summary>
        /// URL of a page for comments relating to the item.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "comments")]
        public string Comments { get; set; }
        /// <summary>
        /// The item synopsis.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        /// <summary>
        /// Describes a media object that is attached to the item.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "enclosure")]
        public Enclosure Enclosure { get; set; }
        /// <summary>
        /// A string that uniquely identifies the item.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "guid")]
        public GUID GUID { get; set; }
        /// <summary>
        /// The URL of the item.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        /// <summary>
        /// Indicates when the item was published.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "pubDate")]
        public string PublishDate { get; set; }
        /// <summary>
        /// The RSS channel that the item came from.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "source")]
        public Source Source { get; set; }
        /// <summary>
        /// The title of the item.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        public FeedItem() { }

        public override string ToString()
        {
            return this.Title;
        }

        public FeedItem Clone()
        {
            var res = new FeedItem();
            res.Author = this.Author;
            if (this.Category != null)
            {
                res.Category = new List<Category>();
                foreach (var c in this.Category) { res.Category.Add(c.Clone()); }
            }
            res.Comments = this.Comments;
            res.Description = this.Description;
            if (this.Enclosure != null) res.Enclosure = this.Enclosure.Clone();
            if (this.GUID != null) res.GUID = this.GUID.Clone();
            res.Link = this.Link;
            res.PublishDate = this.PublishDate;
            if (this.Source != null) res.Source = this.Source.Clone();
            res.Title = this.Title;
            return res;
        }
    }


}
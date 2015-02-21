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
    [XmlRootAttribute(ElementName = "channel")]
    public class Channel
    {
        public Channel() { }



        /// <summary>
        /// Specify one or more categories that the channel belongs to.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "category", Type = typeof(Category))]
        public List<Category> Category { get; set; }

        /// <summary>
        /// Allows processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds. 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "cloud")]
        public Cloud Cloud { get; set; }

        /// <summary>
        /// Copyright notice for content in the channel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "copyright")]
        public string Copyright { get; set; }

        /// <summary>
        /// 	Phrase or sentence describing the channel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// A URL that points to the documentation (http://www.rssboard.org/rss-specification) for the format used in the RSS file. It's probably a pointer to this page. It's for people who might stumble across an RSS file on a Web server 25 years from now and wonder what it is.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "docs")]
        public string Docs { get; set; }

        /// <summary>
        /// A string indicating the program used to generate the channel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "generator")]
        public string Generator { get; set; }

        /// <summary>
        /// Specifies a GIF, JPEG or PNG image that can be displayed with the channel. 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "image")]
        public Image Image { get; set; }

        /// <summary>
        /// A channel may contain any number of items. An item may represent a "story" -- much like a story in a newspaper or magazine; if so its description is a synopsis of the story, and the link points to the full story.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "item", Type = typeof(FeedItem))]
        public List<FeedItem> Items { get; set; }

        /// <summary>
        /// The language the channel is written in. This allows aggregators to group all Italian language sites, for example, on a single page.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "language")]
        public string Language { get; set; }

        /// <summary>
        /// The last time the content of the channel changed.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "lastBuildDate")]
        public string LastBuildDate { get; set; }

        /// <summary>
        /// The URL to the HTML website corresponding to the channel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Email address for person responsible for editorial content.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "managingEditor")]
        public string ManagingEditor { get; set; }

        /// <summary>
        /// The publication date for the content in the channel. For example, the New York Times publishes on a daily basis, the publication date flips once every 24 hours. That's when the pubDate of the channel changes.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "pubDate")]
        public string PublishDate { get; set; }

        /// <summary>
        /// The PICS (http://www.w3.org/PICS/) rating for the channel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "rating")]
        public string Rating { get; set; }

        /// <summary>
        /// A hint for aggregators telling them which hours they can skip. This element contains up to 24 hour sub-elements whose value is a number between 0 and 23, representing a time in GMT, when aggregators, if they support the feature, may not read the channel on hours listed in the SkipHours element. The hour beginning at midnight is hour zero.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "skipHours")]
        public List<int> SkipHours { get; set; }

        /// <summary>
        /// A hint for aggregators telling them which days they can skip. This element contains up to seven day sub-elements whose value is Monday, Tuesday, Wednesday, Thursday, Friday, Saturday or Sunday. Aggregators may not read the channel during days listed in the SkipDays element.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "skipDays")]
        public List<string> SkipDays { get; set; }

        /// <summary>
        /// Specifies a text input box that can be displayed with the channel. The purpose of the TextInput element is something of a mystery. You can use it to specify a search engine box. Or to allow a reader to provide feedback. Most aggregators ignore it.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "textInput")]
        public TextInputBox TextInput { get; set; }

        /// <summary>
        /// The name of the channel. It's how people refer to your service. If you have an HTML website that contains the same information as your RSS file, the title of your channel should be the same as the title of your website.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// ttl stands for time to live. It's a number of minutes that indicates how long a channel can be cached before refreshing from the source.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "ttl")]
        public int Ttl { get; set; }

        /// <summary>
        /// Email address for person responsible for technical issues relating to channel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "webmaster")]
        public string Webmaster { get; set; }



        public Channel Clone()
        {
            var copy = new Channel();
            if (this.Category != null)
            {
                foreach (var c in this.Category)
                {
                    copy.Category.Add(c.Clone());
                }
            }
            copy.Description = this.Description;
            copy.Copyright = this.Copyright;
            copy.Docs = this.Docs;
            copy.Generator = this.Generator;
            if (this.Image != null) copy.Image = this.Image.Clone();
            if (this.Items != null)
            {
                copy.Items = new List<FeedItem>();
                foreach (FeedItem item in this.Items)
                {
                    copy.Items.Add(item.Clone());
                }
            }
            copy.Language = this.Language;
            copy.LastBuildDate = this.LastBuildDate;
            copy.Link = this.Link;
            copy.ManagingEditor = this.ManagingEditor;
            copy.PublishDate = this.PublishDate;
            copy.Rating = this.Rating;
            copy.SkipDays.AddRange(this.SkipDays);
            copy.SkipHours.AddRange(this.SkipHours);
            copy.Title = this.Title;
            copy.Ttl = this.Ttl;
            copy.Webmaster = this.Webmaster;
            return copy;
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}

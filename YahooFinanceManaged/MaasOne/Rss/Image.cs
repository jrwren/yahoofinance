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
    /// Provides RSS feed image data.
    /// </summary>
    /// <remarks></remarks>
    [XmlRootAttribute(ElementName = "image")]
    public class Image
    {
        /// <summary>
        /// Gets or sets the text that is included in the TITLE attribute of the link formed around the image in the HTML rendering.
        /// </summary>
        /// <value></value> 
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the height of the image in pixels.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Maximum value is 400, default value is 31.</remarks>
        [XmlElement(ElementName = "height")]
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets the URL of the site, when the channel is rendered, the image is a link to the site.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>In practice the image TITLE and LINK should have the same value as the channel's TITLE and LINK.</remarks>
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        /// <summary>
        /// Defines the text to display if the image could not be shown.
        /// </summary>
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the URL of a GIF, JPEG or PNG image that represents the channel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "url")]
        public string URL { get; set; }
        /// <summary>
        /// Gets or sets the width of the image in pixels.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Maximum value is 144, default value is 88.</remarks>
        [XmlElement(ElementName = "width")]
        public int Width { get; set; }

        public Image Clone()
        {
            var res = new Image();
            res.Description = this.Description;
            res.Height = this.Height;
            res.Link = this.Link;
            res.Title = this.Title;
            res.URL = this.URL;
            res.Width = this.Width;
            return res;
        }
    }

}
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
    /// Represents a text input box for doing something.
    /// </summary>
    /// <remarks>The purpose of the TextInputBox element is something of a mystery. You can use it to specify a search engine box. Or to allow a reader to provide feedback. Most aggregators ignore it.</remarks>
    [XmlRootAttribute(ElementName = "textInput")]
    public class TextInputBox
    {
        /// <summary>
        /// The label of the Submit button in the text input area.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Title { get; set; }
        /// <summary>
        /// Explains the text input area.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Description { get; set; }
        /// <summary>
        /// The name of the text object in the text input area.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name { get; set; }
        /// <summary>
        /// The URL of the CGI script that processes text input requests.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Link { get; set; }

        public TextInputBox Clone()
        {
            var res = new TextInputBox();
            res.Description = this.Description;
            res.Link = this.Link;
            res.Name = this.Name;
            res.Title = this.Title;
            return res;
        }
    }

}
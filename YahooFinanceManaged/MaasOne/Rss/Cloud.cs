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
    /// It's purpose is to allow processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds.
    /// </summary>
    /// <remarks></remarks>
    [XmlRootAttribute(ElementName = "cloud")] 
    public class Cloud
    {
        [XmlElement(ElementName = "domain")]
        public string Domain { get; set; }
        [XmlElement(ElementName = "path")]
        public string Path { get; set; }
        [XmlElement(ElementName = "registerProcedure")]
        public string RegisterProcedure { get; set; }
        [XmlElement(ElementName = "protocol")]
        public string Protocol { get; set; }
        [XmlElement(ElementName = "port")]
        public int Port { get; set; }
    }

}
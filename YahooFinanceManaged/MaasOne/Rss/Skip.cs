using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MaasOne.Rss
{
    [XmlRootAttribute(ElementName = "day")] 
    public class Day {
        /// <summary>
        /// The name of the day.
        /// </summary>
        [XmlText()]
        public string Value { get; set; }
    }
    [XmlRootAttribute(ElementName = "hour")] 
    public class Hour
    {
        /// <summary>
        /// The Hour value.
        /// </summary>
        [XmlText()]
        public int Value { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace MaasOne.Rss
{

    /// <summary>
    /// Provides information about the RSS channel where the item comes from.
    /// </summary>
    /// <remarks></remarks>
    [XmlRootAttribute(ElementName = "source")]
    public class Source
    {
        /// <summary>
        /// The title of the RSS channel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlText()]
        public string Title { get; set; }
        /// <summary>
        /// The url of the RSS channel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        public Source() { }

        public override string ToString()
        {
            return this.Title;
        }

        public Source Clone()
        {
            var res = new Source();
            res.Title = this.Title;
            res.Url = this.Url;
            return res;
        }
    }

}
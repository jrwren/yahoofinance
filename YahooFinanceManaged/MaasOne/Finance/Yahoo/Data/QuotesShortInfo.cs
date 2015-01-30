using System;
using System.Collections.Generic;

namespace MaasOne.Finance.Yahoo.Data
{

    public class QuotesShortInfo : IQuotePrice
    {
        /// <summary>
        /// Gets or sets the ID property value
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
        public string ID { get; set; }
        /// <summary>
        /// Gets or sets the Title property value
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the LastTradePriceOnly property value
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
        public double LastTradePriceOnly { get; set; }
        /// <summary>
        /// Gets or sets the Change property value
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
        public double Change { get; set; }
        /// <summary>
        /// Gets or sets the ChangeInPercent property value
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
        public double ChangeInPercent { get; set; }
    }

}

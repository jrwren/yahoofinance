#region "License"
// *********************************************************************************************
// **                                                                                         **
// **  Yahoo! Finance Managed                                                                 **
// **                                                                                         **
// **  Copyright (c) Marius Häusler 2009-2015                                                 **
// **                                                                                         **
// **  Licensed under GNU Lesser General Public License (LGPL) (Version 2.1, February 1999).  **
// **                                                                                         **
// **  License: https://www.gnu.org/licenses/old-licenses/lgpl-2.1.txt                        **
// **                                                                                         **
// **  Project: https://yahoofinance.codeplex.com/                                            **
// **                                                                                         **
// **  Contact: maasone@live.com                                                              **
// **                                                                                         **
// *********************************************************************************************
#endregion
using System;
using System.Collections.Generic;

namespace MaasOne.YahooFinance.Data
{
    /// <summary>
    /// Stores information about a search result. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class IDSearchInstantItem : IID
    {
        /// <summary>
        /// The ID of the stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        [System.Xml.Serialization.XmlAttribute()]
        public string ID { get; set; }

        /// <summary>
        /// The name of the stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute()]
        public string Name { get; set; }

        /// <summary>
        /// The type of the stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute(Type = typeof(int))]
        public SecurityType Type { get; set; }

        /// <summary>
        /// The stock exchange of the stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute()]
        public string Exchange { get; set; }


        public IDSearchInstantItem() { }
    }


    /// <summary>
    /// Stores information about a search result. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class IDSearchItem : IDSearchInstantItem, IQuotePrice
    {
        /// <summary>
        /// The industry name of the stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        [System.Xml.Serialization.XmlAttribute()]
        public Industry Industry { get; set; }

        /// <summary>
        /// ISIN
        /// </summary>
        [System.Xml.Serialization.XmlAttribute()]
        public ISIN ISIN { get; set; }

        /// <summary>
        /// LastTradePriceOnly
        /// </summary>
        [System.Xml.Serialization.XmlAttribute()]
        public double? LastTradePriceOnly { get; set; }


        public IDSearchItem() { }


        internal void SetType(string type)
        {
            switch (type.ToLower())
            {
                case "stock":
                case "aktien":
                case "titre":
                case "acción":
                    this.Type = SecurityType.Stock;
                    break;
                case "mutual fund":
                case "fund":
                case "fonds":
                case "Fonds commun de placement":
                case "fondo mutuo":
                    this.Type = SecurityType.Fund;
                    break;
                case "etf":
                case "trackers":
                case "fondos cotizados en bolsa":
                    this.Type = SecurityType.ETF;
                    break;
                case "index":
                case "indice":
                case "índice":
                    this.Type = SecurityType.Index;
                    break;
                case "futures":
                case "future":
                case "futuro":
                    this.Type = SecurityType.Future;
                    break;
                case "warrant":
                case "zertifikate & os":
                case "warrants":
                    this.Type = SecurityType.Warrant;
                    break;
                case "currency":
                case "währungen":
                case "devises":
                case "divisas":
                    this.Type = SecurityType.Currency;
                    break;
                default:
                    this.Type = SecurityType.Any;
                    if (type != string.Empty)
                        System.Diagnostics.Debug.WriteLine(type);
                    break;
            }
        }
    }
}

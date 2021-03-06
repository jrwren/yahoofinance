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
using System.Xml.Serialization;

namespace System.Resources
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yahoofinance.codeplex.com/1/market.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://yahoofinance.codeplex.com/1/market.xsd", IsNullable = false)]
    public partial class WorldMarket
    {
        
        private WorldMarketSector[] sectorsField;

        private WorldMarketIndustry[] industriesField;

        private WorldMarketCurrency[] currenciesField;
         
        private WorldMarketCountry[] countriesField;

        private WorldMarketStockExchange[] stockExchangesField;

        private WorldMarketIndex[] indicesField;

        private WorldMarketCategory[] fundCategoriesField;

        private WorldMarketFamily[] fundFamiliesField;

#if !(SILVERLIGHT || NETFX_CORE)
        private System.Xml.XmlElement[] anyField;
#else
        private System.Xml.Linq.XElement[] anyField;
#endif

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Sector", IsNullable = false)]
        public WorldMarketSector[] Sectors
        {
            get
            {
                return this.sectorsField;
            }
            set
            {
                this.sectorsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Industry", IsNullable = false)]
        public WorldMarketIndustry[] Industries
        {
            get
            {
                return this.industriesField;
            }
            set
            {
                this.industriesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Currency", IsNullable = false)]
        public WorldMarketCurrency[] Currencies
        {
            get
            {
                return this.currenciesField;
            }
            set
            {
                this.currenciesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Country", IsNullable = false)]
        public WorldMarketCountry[] Countries
        {
            get
            {
                return this.countriesField;
            }
            set
            {
                this.countriesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("StockExchange", IsNullable = false)]
        public WorldMarketStockExchange[] StockExchanges
        {
            get
            {
                return this.stockExchangesField;
            }
            set
            {
                this.stockExchangesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Index", IsNullable = false)]
        public WorldMarketIndex[] Indices
        {
            get
            {
                return this.indicesField;
            }
            set
            {
                this.indicesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FundCategories", IsNullable = false)]
        public WorldMarketCategory[] FundCategories
        {
            get
            {
                return this.fundCategoriesField;
            }
            set
            {
                this.fundCategoriesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FundFamilies", IsNullable = false)]
        public WorldMarketFamily[] FundFamilies
        {
            get
            {
                return this.fundFamiliesField;
            }
            set
            {
                this.fundFamiliesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
#if !(SILVERLIGHT || NETFX_CORE)
        public System.Xml.XmlElement[] Any
#else
        public System.Xml.Linq.XElement[] Any
#endif
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yahoofinance.codeplex.com/1/market.xsd")]
    public partial class WorldMarketSector
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yahoofinance.codeplex.com/1/market.xsd")]
    public partial class WorldMarketIndustry
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yahoofinance.codeplex.com/1/market.xsd")]
    public partial class WorldMarketCurrency
    {

        private string idField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yahoofinance.codeplex.com/1/market.xsd")]
    public partial class WorldMarketCountry
    {

        private string idField;

        private string currencyField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yahoofinance.codeplex.com/1/market.xsd")]
    public partial class WorldMarketStockExchange
    {

        private WorldMarketStockExchangeTradingTime tradingTimeField;

        private string idField;

        private string countryField;

        private string suffixField;

        private string nameField;

        private string locationField;

        public WorldMarketStockExchange()
        {
            this.suffixField = "";
            this.locationField = "";
        }

        /// <remarks/>
        public WorldMarketStockExchangeTradingTime TradingTime
        {
            get
            {
                return this.tradingTimeField;
            }
            set
            {
                this.tradingTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string Suffix
        {
            get
            {
                return this.suffixField;
            }
            set
            {
                this.suffixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yahoofinance.codeplex.com/1/market.xsd")]
    public partial class WorldMarketStockExchangeTradingTime
    {

        private string tradingDaysField;

        private WorldMarketStockExchangeTradingTimeNonTradingDay[] nonTradingDaysField;

        private System.DateTime openingTimeLocalField;

        private System.DateTime closingTimeLocalField;

        private int delayMinutesField;

        private string timeZoneField;

        public WorldMarketStockExchangeTradingTime()
        {
            this.tradingDaysField = "Mo Tu We Th Fr";
        }

        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("Mo Tu We Th Fr")]
        public string TradingDays
        {
            get
            {
                return this.tradingDaysField;
            }
            set
            {
                this.tradingDaysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("NonTradingDay", IsNullable = false)]
        public WorldMarketStockExchangeTradingTimeNonTradingDay[] NonTradingDays
        {
            get
            {
                return this.nonTradingDaysField;
            }
            set
            {
                this.nonTradingDaysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "time")]
        public System.DateTime OpeningTimeLocal
        {
            get
            {
                return this.openingTimeLocalField;
            }
            set
            {
                this.openingTimeLocalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "time")]
        public System.DateTime ClosingTimeLocal
        {
            get
            {
                return this.closingTimeLocalField;
            }
            set
            {
                this.closingTimeLocalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int DelayMinutes
        {
            get
            {
                return this.delayMinutesField;
            }
            set
            {
                this.delayMinutesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TimeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yahoofinance.codeplex.com/1/market.xsd")]
    public partial class WorldMarketStockExchangeTradingTimeNonTradingDay
    {

        private System.DateTime dateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yahoofinance.codeplex.com/1/market.xsd")]
    public partial class WorldMarketIndex
    {

        private string idField;

        private string stockExchangeField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string StockExchange
        {
            get
            {
                return this.stockExchangeField;
            }
            set
            {
                this.stockExchangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yahoofinance.codeplex.com/1/market.xsd")]
    public partial class WorldMarketCategory
    {

        private string idField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://yahoofinance.codeplex.com/1/market.xsd")]
    public partial class WorldMarketFamily
    {

        private string idField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }
}
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

namespace MaasOne.YahooFinance.Data
{

    public class CompanyProfileData : IID
    {
        [System.Xml.Serialization.XmlAttribute]
        public string ID { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string BusinessSummary { get; set; }
        public string CorporateGovernance { get; set; }
        public string[] CompanyWebsites { get; set; }
        [System.Xml.Serialization.XmlElement]
        public CompanyProfileDetails Details { get; set; }
        public CompanyProfileExecutivePerson[] KeyExecutives { get; set; }

        public CompanyProfileData()
        {
            this.CompanyWebsites = new string[] { };
            this.Details = new CompanyProfileDetails();
            this.KeyExecutives = new CompanyProfileExecutivePerson[] { };
        }

    }

    public class CompanyProfileDetails
    {
        [System.Xml.Serialization.XmlAttribute]
        public Sector Sector { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public Industry Industry { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public int? FullTimeEmployees { get; set; }

        public CompanyProfileDetails() { }
    }

    public class CompanyProfileExecutivePerson
    {
        [System.Xml.Serialization.XmlAttribute]
        public string Name { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public int? Age { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public string Position { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public long? Pay { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public long? Exercised { get; set; }
    }

}

using System;
using System.Collections.Generic;

namespace MaasOne.Finance.Yahoo.Data
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
        public int? Pay { get; set; }
        [System.Xml.Serialization.XmlAttribute]
        public int? Exercised { get; set; }
    }

}

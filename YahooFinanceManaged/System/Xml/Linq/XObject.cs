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
#if (NET20)
using System;
using System.Collections.Generic;
using System.Xml;

namespace System.Xml.Linq
{

    internal abstract class XObject
    {
        public abstract XObject Parent { get; }
        public abstract XName Name { get; }
    }

    internal abstract class XContainer : XObject
    {
        public abstract XElement[] Elements();
        public abstract void Add(string value);
        public abstract void Add(XElement element);
        public abstract void Add(XAttribute attribute);
        public abstract void RemoveElements();
    }

    internal class XDocument : XContainer
    {

        internal XmlDocument mDoc = null;

        public override XName Name { get { return new XName(mDoc.Name); } }

        public XElement Root { get; private set; }

        public override XElement[] Elements() { return this.Root.Elements(); }

        public void SetDeclaration(XDeclaration declaration)
        {
            if (mDoc.FirstChild is XmlDeclaration)
            {
                mDoc.RemoveChild(mDoc.FirstChild);
            }
            mDoc.InsertBefore(declaration.mDeclaration, mDoc.DocumentElement);
        }

        public override void Add(XElement element) { this.Root.Add(element); }
        public override void Add(XAttribute attribute) { this.Root.Add(attribute); }
        public override void Add(string value) { this.Root.Add(value); }

        public override void RemoveElements() { this.Root.RemoveElements(); }

        public override XObject Parent { get { return null; } }

        public XDocument()
        {
            mDoc = new XmlDocument();
            XmlDeclaration decl = mDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            mDoc.InsertBefore(decl, mDoc.DocumentElement);
            var root = mDoc.CreateElement("Root");
            mDoc.AppendChild(root);
            this.Root = new XElement(root);
        }
        private XDocument(XmlDocument doc)
        {
            mDoc = doc;
            this.Root = new XElement(doc.DocumentElement);
        }

        public override bool Equals(object obj) { return obj is XDocument && this.mDoc.Equals(((XDocument)obj).mDoc); }


        public static XDocument Load(System.IO.Stream input)
        {
            var doc = new XmlDocument();
            doc.Load(input);
            return new XDocument(doc);
        }
        public static XDocument Parse(string text)
        {
            var doc = new XmlDocument();
            doc.LoadXml(text);
            return new XDocument(doc);
        }
    }

    internal class XElement : XContainer
    {
        internal XmlNode mElement = null;


        public override XName Name { get { return new XName(mElement.Name); } }

        public string Value { get { return mElement.Value; } }


        public override XObject Parent { get { return this.mElement.ParentNode != null ? new XElement(this.mElement.ParentNode) : null; } }

        public override XElement[] Elements()
        {
            XElement[] lst = new XElement[mElement.ChildNodes.Count];
            for (int i = 0; i < mElement.ChildNodes.Count; i++) { lst[i] = new XElement(mElement.ChildNodes[i]); }
            return lst;
        }
        public XAttribute[] Attributes()
        {
            XAttribute[] lst = new XAttribute[mElement.Attributes.Count];
            for (int i = 0; i < mElement.ChildNodes.Count; i++) { lst[i] = new XAttribute(mElement.Attributes[i]); }
            return lst;
        }
        public XAttribute Attribute(XName name) { return this.Attribute(name.LocalName); }
        public XAttribute Attribute(string localName)
        {
            return new XAttribute(mElement.Attributes[localName]);
        }

        public override void Add(string value) { if (this.mElement.Value == null) this.mElement.Value = string.Empty; this.mElement.Value += value; }
        public override void Add(XAttribute attribute)
        {
            this.mElement.Attributes.Append(attribute.mAttribute);
        }
        public override void Add(XElement element)
        {
            this.mElement.AppendChild(element.mElement);
        }
        public override void RemoveElements()
        {
            this.mElement.RemoveAll();
        }



        public XElement(XName name, XDocument ownderDoc)
        {
            mElement = ownderDoc.mDoc.CreateElement(name.LocalName);
        }
        internal XElement(XmlNode element)
        {
            mElement = element;
        }
        public override bool Equals(object obj) { return obj is XElement && this.mElement.Equals(((XElement)obj).mElement); }

    }

    internal class XAttribute : XObject
    {
        internal XmlAttribute mAttribute = null;

        public override XName Name { get { return new XName(mAttribute.Name); } }

        public string Value { get { return mAttribute.Value; } }

        public override XObject Parent { get { return new XElement(this.mAttribute.ParentNode); } }

        public XAttribute(XName name, string value, XDocument ownerDoc)
        {
            mAttribute = ownerDoc.mDoc.CreateAttribute(name.LocalName);
            mAttribute.Value = value;
        }
        internal XAttribute(XmlAttribute attribute)
        {
            mAttribute = attribute;
        }

        public override bool Equals(object obj) { return obj is XAttribute && this.mAttribute.Equals(((XAttribute)obj).mAttribute); }
    }

    internal class XName
    {
        public string LocalName { get; set; }
        public string Namespace { get; set; }

        internal XName(string localName)
        {
            this.LocalName = localName;
        }
        internal XName(string localName, string ns)
        {
            this.LocalName = localName;
            this.Namespace = ns;

        }

        public override bool Equals(object obj) { return obj is XName && this.LocalName.Equals(((XName)obj).LocalName); }

        public override string ToString()
        {
            return this.LocalName;
        }

        public static XName Get(string localName) { return new XName(localName); }
        public static XName Get(string localName, string ns) { return new XName(localName, ns); }

    }

    internal class XDeclaration
    {
        internal XmlDeclaration mDeclaration = null;

        internal XDeclaration(string version, string encoding, string standalone, XDocument ownerDoc)
        {
            mDeclaration = ownerDoc.mDoc.CreateXmlDeclaration(version, encoding, standalone);
        }
        internal XDeclaration(XmlDeclaration declaration) { this.mDeclaration = declaration; }
    }

}
#endif
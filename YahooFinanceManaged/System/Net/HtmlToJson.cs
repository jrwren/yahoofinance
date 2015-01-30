using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace System.Net
{

    internal class HtmlToJsonParser
    {

        private const string CONTENT_PROPERTY_NAME = "content";

        public static JObject Parse(string html) { return new HtmlToJsonParser().ParseDocument(html); }
        public JObject ParseDocument(string html)
        {
            JObject obj = null;
            html = System.Text.RegularExpressions.Regex.Replace(html, "(<script.*?>(.|\n)*?</script>)|(<style.*?>(.|\n)*?</style>)|(<!--.*?-->)", "");
            TextNavigator sc = new TextNavigator(html);
            sc.TrimStart();
            if (sc.StartsWith("<"))
            {
                obj = new JObject();
                while (!sc.IsAtEnd)
                {
                    this.ParseNode(sc, obj);
                }
            }
            return obj;
        }

        private string ParseName(TextNavigator sc)
        {
            sc.TrimStart();
            string name = null;
            string n = string.Empty;
            string ns = string.Empty;
            for (int i = sc.Index; i < sc.Value.Length; i++)
            {
                if (IsValidTagChar(sc.Value[i]))
                {
                    n += sc.Value[i];
                }
                else if (n == string.Empty && sc.Value[i] == ' ')
                {
                }
                else if (i > 0)
                {
                    if (sc.Value[i] == ':' && n != string.Empty && ns == string.Empty)
                    {
                        ns = n;
                        n = string.Empty;
                    }
                    else if (sc.Value[i] == ' ' || sc.Value[i] == '=' || sc.Value[i] == '>' || sc.Value[i] == '/' || sc.Value[i] == '\r' || sc.Value[i] == '\n')
                    {
                        sc.Index = i;
                        break;
                    }
                    else
                    {
                        n = string.Empty;
                        sc.Index = i;
                        break;
                    }
                }
                else
                {
                    throw new Exception("Invalid Name.");
                }
            }
            if (n != string.Empty) name = (string.IsNullOrEmpty(ns) ? "" : ns + ":") + n;
            sc.TrimStart();
            return name;
        }
        private JProperty ParseAttribute(TextNavigator sc)
        {
            sc.TrimStart();
            JProperty att = null;
            string name = null;
            if (IsValidTagChar(sc.StartValue)) name = ParseName(sc);
            if (name != null && sc.StartValue == '=')
            {
                sc.Index++;
                sc.TrimStart();
                string val = string.Empty;
                Nullable<char> quotesSign = ((sc.StartValue == '\"' || sc.StartValue == '\'') ? (Nullable<char>)sc.StartValue : null);
                for (int i = (sc.Index + (quotesSign.HasValue ? 1 : 0)); i < sc.Value.Length; i++)
                {
                    if (quotesSign.HasValue)
                    {
                        if (sc.Value[i] == quotesSign.Value) { sc.Index = i + 1; break; }
                        else { val += sc.Value[i]; }
                    }
                    else
                    {
                        if (sc.Value[i] == ' ' || sc.Value[i] == '>' || sc.Value[i] == '\r' || sc.Value[i] == '\n') { sc.Index = i; break; }
                        else { val += sc.Value[i]; }
                    }
                }

                att = new JProperty(name, this.DecodeXml(val));
            }

            sc.TrimStart();
            return att;
        }
        private KeyValuePair<string, JToken> ParseNode(TextNavigator tn, JObject parent)
        {
            tn.TrimStart();
            if (tn.StartValue == '<')
            {
                string name = null;
                JToken node = null;
                bool isComment = false;
                bool isDeclaration = false;

                #region "Start Tag: Name, Declaration, Comment"
                tn.Index = tn.Index + 1;
                int breakOff = 0;
                if (tn.StartsWith("?xml"))
                {
                    //Declaration
                    isDeclaration = true;
                    tn.Index += 4;
                }
                else if (tn.StartValue == '!')
                {
                    //Comment
                    isComment = true;
                    if (tn.Value[tn.Index + 1] == '-') breakOff = 1;
                    if (tn.Value[tn.Index + 2] == '-') breakOff = 2;
                    tn.Index += breakOff + 1;
                    tn.TrimStart();
                }
                else if (IsValidTagChar(tn.StartValue))
                {
                    //Name
                    name = ParseName(tn);
                    if (name != null) { node = new JObject(); }
                    else { throw new Exception("Invalid Node Name."); }
                }
                else
                {
                    throw new Exception("Invalid Node Name.");
                }
                #endregion

                if (node != null || isComment || isDeclaration)
                {
                    var attNames = new List<string>();
                    bool elementAtEnd = false;

                    if (name == "br") { elementAtEnd = true; node = null; }

                    #region "Attributes, Declaration, Comment"
                    //Declaration, Attributes, Comment

                    string comment = string.Empty;
                    string declVer = string.Empty;
                    string declEnc = string.Empty;
                    string declSta = string.Empty;
                    for (int i = tn.Index; i < tn.Value.Length; i++)
                    {
                        if (!isComment && !isDeclaration)
                        {
                            #region "Attributes"
                            //Attributes
                            if (tn.Value[i] != ' ')
                            {
                                if (tn.Value[i] == '>')
                                {
                                    tn.Index = i + 1;
                                    break;
                                }
                                else if (tn.Value[i] == '/' && tn.Value[i + 1] == '>')
                                {
                                    elementAtEnd = true;
                                    tn.Index += 2;
                                    break;
                                }
                                else if (IsValidTagChar(tn.Value[i]))
                                {
                                    JProperty att = ParseAttribute(tn.NewIndex(i));
                                    i = tn.Index - 1;
                                    if (att != null) { ((JObject)node).Add(att); attNames.Add(att.Name); }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region "Comment"
                            if (isComment)
                            {
                                //Comment
                                if ((breakOff == 2 && tn.Value[i] == '-' && tn.Value[i + 1] == '-' && tn.Value[i + 2] == '>') || (breakOff == 0 && tn.Value[i] == '>') || (breakOff == 1 && tn.Value[i] == '-' && tn.Value[i + 1] == '>'))
                                {
                                    //if (parent != null) parent.Add(new XComment(comment));
                                    tn.Index = i + breakOff + 1;
                                    break;
                                }
                                else
                                {
                                    comment += tn.Value[i];
                                }/**/
                            }
                            #endregion
                            #region"Declaration"
                            else if (isDeclaration)
                            {
                                //Declaration
                                if (tn.Value[i] == '?' && tn.Value[i + 1] == '>')
                                {
                                    //if (parent != null && parent is XDocument) this.SetDeclaration(declVer, declEnc, declSta, (XDocument)parent);
                                    tn.Index = i + 2;
                                    break;
                                }
                                else if (IsValidTagChar(tn.Value[i]))
                                {
                                    JProperty att = ParseAttribute(tn.NewIndex(i));
                                    i = tn.Index - 1;
                                    /* if (att != null)
                                     {
                                         if (att.Name.ToLower() == "version") { declVer = att.Value; }
                                         else if (att.Name.ToLower() == "encoding") { declEnc = att.Value; }
                                         else if (att.Name.ToLower() == "standalone") { declSta = att.Value; }
                                     }*/
                                }
                            }
                            #endregion
                        }
                    }

                    #endregion

                    if (name == "link") { elementAtEnd = true; }

                    ///Add to parent
                    if (node != null && parent != null)
                    {
                        this.AddItemToParent(node, name, parent);
                    }


                    if (node != null && elementAtEnd == false)
                    {
                        #region "Content & End Tag"
                        //Content & End Tag

                        string innerText = string.Empty;
                        for (int i = tn.Index; i < tn.Value.Length; i++)
                        {
                            if (tn.Value[i] == '<')
                            {
                                if (tn.Value[i + 1] == '/')
                                {
                                    #region "InnerText"
                                    //InnerText --> JValue
                                    if (innerText.Trim(new char[] { ' ', '\n', '\r', '\t' }).IsNullOrWhiteSpace() == false)
                                    {
                                        if (((JObject)node).Count == 0)
                                        {
                                            var newValue = new JValue(this.DecodeXml(innerText));
                                            foreach (JProperty elem in parent.Children<JProperty>())
                                            {
                                                if (elem.Name == name)
                                                {
                                                    if (elem.Value is JObject)
                                                    {
                                                        elem.Value = newValue;
                                                    }
                                                    else if (elem.Value is JArray)
                                                    {
                                                        var arr = (JArray)elem.Value;
                                                        int index = arr.IndexOf(node);
                                                        if (index >= 0)
                                                        {
                                                            arr.Insert(index, newValue);
                                                            arr.RemoveAt(index + 1);
                                                        }
                                                    }
                                                }
                                            }
                                            node = newValue;
                                        }
                                        else
                                        {
                                            if (node[CONTENT_PROPERTY_NAME] == null)
                                            {
                                                ((JObject)node).Add(new JProperty(CONTENT_PROPERTY_NAME, DecodeXml(innerText)));
                                            }
                                            else
                                            {
                                                ((JValue)node[CONTENT_PROPERTY_NAME]).Value = ((JValue)node[CONTENT_PROPERTY_NAME]).Value.ToString() + "\n" + DecodeXml(innerText);
                                            }
                                        }
                                        innerText = string.Empty;
                                    }
                                    #endregion

                                    #region "End Tag"

                                    //End Tag
                                    string endName = ParseName(tn.NewIndex(i + 2));
                                    if (endName != null)
                                    {
                                        if (endName == name)
                                        {
                                            //Correct actual end name
                                            tn.Index = i + 3 + name.Length;
                                            break;
                                        }
                                        else
                                        {
                                            //Other end name
                                            JObject pare = this.FindParent(node, endName);
                                            if (pare != null)
                                            {
                                                //Other end name relies to one parent --> move all, except attributes, to direct parent.
                                                if (node is JObject)
                                                {
                                                    this.MoveAllChildElementsToParent((JObject)node, parent, attNames);
                                                }
                                                tn.Index = i;
                                                break;
                                            }
                                            else
                                            {
                                                //Unknown end name --> ignore
                                                tn.Index = i + endName.ToString().Length + 2;
                                                i = tn.Index - 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Invalid End Name.");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    //Child Start Tag
                                    var child = this.ParseNode(tn.NewIndex(i), (JObject)node);

                                    if (child.Key == "br") { innerText += "\n"; }
                                    else if (child.Key != "a" && child.Key != "small")
                                    {
                                        if (innerText.IsNullOrWhiteSpace() == false)
                                        {
                                            JObject p = new JObject();
                                            p.Add(new JProperty(CONTENT_PROPERTY_NAME, innerText));

                                            innerText = string.Empty;
                                            this.AddItemToParent(p, "p", (JObject)node);
                                        }
                                    }
                                    i = tn.Index - 1;
                                }
                            }
                            else if (!(tn.Value[i] == ' ' && innerText == string.Empty))
                            {
                                //Inner Text
                                innerText += tn.Value[i];
                            }
                        }

                        #endregion
                    }

                }
                tn.TrimStart();
                return new KeyValuePair<string, JToken>(name, node);
            }
            else
            {
                throw new Exception("Invalid Start Tag. A node has to start with [<].");
            }
        }

        private void MoveAllChildElementsToParent(JObject node, JObject parent, List<string> childAttributes)
        {
            if (node.Count > 0)
            {
                var nodeName = "";
                foreach (var e in parent)
                {
                    if (e.Value is JArray)
                    {
                        foreach (JToken jt in (JArray)e.Value)
                        {
                            if (jt == node) { nodeName = e.Key; break; }
                        }
                        if (string.IsNullOrEmpty(nodeName) == false) break;
                    }
                    else
                    {
                        if (e.Value == node) { nodeName = e.Key; break; }
                    }
                }

                for (var n = node.Count - 1; n >= 0; n--)
                {
                    try
                    {
                        JProperty p = (JProperty)node.GetItem(n);
                        if (childAttributes.Contains(p.Name) == false)
                        {
                            p.Remove();
                            this.AddItemToParent(p.Value, p.Name, parent);

                            JToken nod = parent[nodeName];
                            if (nod is JObject)
                            {
                                node = (JObject)nod;
                            }
                            else
                            {
                                var arr = (JArray)nod;
                                foreach (JObject itm in arr)
                                {
                                    if (itm == node)
                                    {
                                        node = itm;
                                        break;
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                    }
                }
            }
        }
        private void AddItemToParent(JToken item, string itemName, JObject parent)
        {
            JToken po = parent[itemName];
            if (po == null)
            {
                parent.Add(new JProperty(itemName, item));
            }
            else
            {
                if (po is JObject)
                {
                    if (item is JArray)
                    {
                        var arr = (JArray)item;
                        arr.Insert(0, po);
                        parent[itemName].Replace(arr);
                    }
                    else
                    {
                        parent[itemName].Replace(new JArray(po, item));
                    }
                }
                else if (po is JArray)
                {
                    if (item is JArray)
                    {
                        ((JArray)po).Merge(item);
                    }
                    else
                    {
                        ((JArray)po).Add(item);
                    }
                }
            }
        }

        private Dictionary<string, string> mXmlDecodings = new Dictionary<string, string>();

        private bool IsValidTagChar(char c) { return c == '_' || c == '-' || char.IsLetterOrDigit(c); }
        private string DecodeXml(string escapedTxt)
        {
            return escapedTxt.Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&apos;", "\'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&nbsp;", "");
            if (mXmlDecodings.Count == 0)
            {
                mXmlDecodings.Add("quot", "\"");
                mXmlDecodings.Add("apos", "\'");
                mXmlDecodings.Add("lt", "<");
                mXmlDecodings.Add("gt", ">");
                mXmlDecodings.Add("nbsp", "");
                mXmlDecodings.Add("amp", "&");
            }

            if (escapedTxt.Length > 4)
            {
                string newTxt = string.Empty;
                bool isInSequence = false;
                string sequenceTxt = string.Empty;
                for (int i = 0; i < escapedTxt.Length; i++)
                {
                    if (!isInSequence)
                    {
                        if (escapedTxt[i] == '&') { isInSequence = true; }
                        else { newTxt += escapedTxt[i]; }
                    }
                    else
                    {
                        if (escapedTxt[i] == ';')
                        {
                            isInSequence = false;
                            newTxt += mXmlDecodings[sequenceTxt];
                            sequenceTxt = string.Empty;
                        }
                        else
                        {
                            var found = false;
                            foreach (var k in mXmlDecodings.Keys) { if (k.StartsWith(sequenceTxt)) { found = true; break; } }
                            if (found)
                            { sequenceTxt += escapedTxt[i]; }
                            else
                            {
                                sequenceTxt += escapedTxt[i];
                                newTxt += "&" + sequenceTxt;
                                sequenceTxt = string.Empty;
                                isInSequence = false;
                            }
                        }
                    }
                }
                return newTxt;
            }
            else
            {
                return escapedTxt;
            }
        }
        private JObject FindParent(JToken child, string parentName)
        {
            var prp = this.GetJProperty(child);
            if (prp != null)
            {
                if (prp.Name.Equals(parentName))
                { return (JObject)child; }
                else
                { if (prp.Parent != null) return this.FindParent((JObject)prp.Parent, parentName); }
            }
            return null;
        }
        private JProperty GetJProperty(JToken jObj)
        {
            JProperty prp = null;
            if (jObj != null && jObj.Parent != null)
            {
                if (jObj.Parent is JProperty)
                { prp = (JProperty)jObj.Parent; }
                else if (jObj.Parent is JArray)
                { if (jObj.Parent.Parent is JProperty) prp = (JProperty)jObj.Parent.Parent; }
            }
            return prp;
        }

        private class TextNavigator
        {
            public int Index = 0;
            public string Value = string.Empty;
            public bool IsAtEnd { get { return this.Index >= this.Value.Length - 1; } }
            public char StartValue { get { return this.Value[this.Index]; } }

            public TextNavigator(string value) { this.Value = value; }

            public TextNavigator NewIndex(int index)
            {
                this.Index = index;
                return this;
            }
            public void TrimStart()
            {
                for (int i = this.Index; i < this.Value.Length; i++)
                {
                    if (this.Value[i] != ' ' && this.Value[i] != '\n' && this.Value[i] != '\r' && this.Value[i] != '\t' && this.Value[i] != '\0') break;
                    this.Index = i + 1;
                }
            }
            public bool StartsWith(string value)
            {
                if (value != string.Empty)
                {
                    for (int i = this.Index; i < this.Value.Length; i++)
                    {
                        if (i - this.Index < value.Length)
                        {
                            if (this.Value[i] != value[i - this.Index]) return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

    }

}

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
    /// Provides RSS feed data.
    /// </summary>
    /// <remarks></remarks>
    [XmlRootAttribute(ElementName = "rss")]
    public class Feed : MaasOne.Net.ResultBase
    {
        public Feed() { }



        [XmlElement(ElementName = "channel")]
        public Channel Channel { get; set; }



        public Feed Clone()
        {
            var copy = new Feed();
            if (this.Channel != null) copy.Channel = this.Channel.Clone();
            return copy;
        }

        public override string ToString()
        {
            return this.Channel != null ? Channel.Title : base.ToString();
        }


        public static System.DateTime RFC822DateFromString(string d)
        {
            System.DateTime result = default(System.DateTime);
            if (d != string.Empty)
            {
                int spacePos = d.LastIndexOf(" ");
                string timezone = d.Substring(spacePos + 1);

                if (System.DateTime.TryParse(d, out result))
                {
                    if (timezone == "Z" | timezone == "GMT")
                        result = result.ToUniversalTime();
                    return result;
                }
                else
                {
                    result = Convert.ToDateTime(d.Substring(0, spacePos));
                    if (d[spacePos + 1] == '+')
                    {
                        result = result.AddHours(-1 * Convert.ToInt32(d.Substring(spacePos + 2, 2)));
                        result = result.AddMinutes(-1 * Convert.ToInt32(d.Substring(spacePos + 4, 2)));
                    }
                    else if (d[spacePos + 1] == '-')
                    {
                        int h = Convert.ToInt32(d.Substring(spacePos + 2, 2));
                        result = result.AddHours(h);
                        int m = Convert.ToInt32(d.Substring(spacePos + 4, 2));
                        result = result.AddMinutes(m);
                    }
                    else
                    {
                        switch (timezone)
                        {
                            case "A":
                                result = result.AddHours(1);
                                break;
                            case "B":
                                result = result.AddHours(2);
                                break;
                            case "C":
                                result = result.AddHours(3);
                                break;
                            case "D":
                                result = result.AddHours(4);
                                break;
                            case "EDT":
                                result = result.AddHours(4);
                                break;
                            case "E":
                                result = result.AddHours(5);
                                break;
                            case "EST":
                                result = result.AddHours(5);
                                break;
                            case "CDT":
                                result = result.AddHours(5);
                                break;
                            case "F":
                                result = result.AddHours(6);
                                break;
                            case "CST":
                                result = result.AddHours(6);
                                break;
                            case "MDT":
                                result = result.AddHours(6);
                                break;
                            case "G":
                                result = result.AddHours(7);
                                break;
                            case "MST":
                                result = result.AddHours(7);
                                break;
                            case "PDT":
                                result = result.AddHours(7);
                                break;
                            case "H":
                                result = result.AddHours(8);
                                break;
                            case "PST":
                                result = result.AddHours(8);
                                break;
                            case "I":
                                result = result.AddHours(9);
                                break;
                            case "K":
                                result = result.AddHours(10);
                                break;
                            case "L":
                                result = result.AddHours(11);
                                break;
                            case "M":
                                result = result.AddHours(12);
                                break;
                            case "N":
                                result = result.AddHours(-1);
                                break;
                            case "O":
                                result = result.AddHours(-2);
                                break;
                            case "P":
                                result = result.AddHours(-3);
                                break;
                            case "Q":
                                result = result.AddHours(-4);
                                break;
                            case "R":
                                result = result.AddHours(-5);
                                break;
                            case "S":
                                result = result.AddHours(-6);
                                break;
                            case "T":
                                result = result.AddHours(-7);
                                break;
                            case "U":
                                result = result.AddHours(-8);
                                break;
                            case "V":
                                result = result.AddHours(-9);
                                break;
                            case "W":
                                result = result.AddHours(-10);
                                break;
                            case "X":
                                result = result.AddHours(-11);
                                break;
                            case "Y":
                                result = result.AddHours(-12);
                                break;
                        }
                    }
                }
            }
            return result;
        }
    }
}

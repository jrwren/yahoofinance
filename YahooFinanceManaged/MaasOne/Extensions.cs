using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#if  (NET20)
using Newtonsoft.Json.Utilities.LinqBridge;
#else
using System.Linq;
#endif

namespace MaasOne
{

    internal static class Extensions
    {

        public static JToken FindFirst(this JObject root, string nodeName) { return FindFirst(root, nodeName, null, null); }
        public static JToken FindFirst(this JObject root, string nodeName, string propertyName, object propertyValue)
        {
            var res = Find(root, nodeName, propertyName, propertyValue, true);
            if (res.Length > 0) return res[0];
            return null;
        }

        public static JToken[] Find(this JObject root, string nodeName) { return Find(root, nodeName, null, null); }
        public static JToken[] Find(this JObject root, string nodeName, string propertyName, object propertyValue) { return Find(root, nodeName, propertyName, propertyValue, false); }

        private static JToken[] Find(JObject root, string nodeName, string propertyName, object propertyValue, bool returnFirst)
        {
            List<JToken> lst = new List<JToken>();
            if (root != null)
            {
                IEnumerable<JToken> tokens = root.Descendants().OfType<JProperty>().Where(p => p.Name == nodeName).Select(p => p.Value);
                if (string.IsNullOrEmpty(propertyName))
                {
                    if (tokens.Count() > 0) lst.Add(tokens.First());
                    if (returnFirst) return lst.ToArray();
                }
                else
                {
                    foreach (var d in tokens)
                    {
                        if (d is JObject)
                        {
                            JToken dValObj = d[propertyName];
                            if (dValObj != null && dValObj is JValue && ((JValue)dValObj).Value.Equals(propertyValue))
                            {
                                lst.Add(d);
                                if (returnFirst) return lst.ToArray();
                            }
                        }
                        else if (d is JArray)
                        {
                            foreach (JToken dObj in (JArray)d)
                            {
                                if (!(dObj is JValue))
                                {
                                    JToken dValObj = dObj[propertyName];
                                    if (dValObj != null && dValObj is JValue && ((JValue)dValObj).Value.Equals(propertyValue))
                                    {
                                        lst.Add(dObj);
                                        if (returnFirst) return lst.ToArray();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return lst.ToArray();
        }


        internal static bool EnumContains<T>(this IEnumerable<T> values, T item)
        {
            foreach (T i in values) { if (i.Equals(item)) return true; }
            return false;
        }
        internal static T[] EnumToArray<T>(this IEnumerable<T> values)
        {
            if (values != null)
            {
                if (values is T[])
                {
                    return (T[])values;
                }
                else if (values is List<T>)
                {
                    return ((List<T>)values).ToArray();
                }
                else
                {
                    return new List<T>(values).ToArray();
                }
            }
            else
            {
                return new T[] { };
            }
        }

    }

}

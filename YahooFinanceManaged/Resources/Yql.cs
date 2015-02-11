using System;
using System.Collections.Generic;
using MaasOne.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace System.Resources
{
  

        internal class YqlResponse
        {
            [JsonProperty("query")]
            public YqlQuery Query { get; set; }
        }
        internal class YqlQuery
        {
            [JsonProperty("count")]
            public int Count { get; set; }
            [JsonProperty("created")]
            public DateTime Created { get; set; }
            [JsonProperty("lang")]
            public System.Globalization.CultureInfo Lang { get; set; }
            [JsonProperty("diagnostics")]
            public YqlDiagnostics Diagnostics { get; set; }
            [JsonProperty("results")]
            public JToken Results { get; set; }
        }



        internal class SingleOrArrayConverter<T> : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(IEnumerable<T>));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JToken token = JToken.Load(reader);
                if (token.Type == JTokenType.Array)
                {
                    return token.ToObject<T[]>();
                }
                return new T[] { token.ToObject<T>() };
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }


    
}

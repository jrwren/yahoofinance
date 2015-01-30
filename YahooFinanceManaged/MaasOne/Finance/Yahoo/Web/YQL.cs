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
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MaasOne.Finance.Yahoo.Web
{

    public interface IYqlQuery
    {
        bool UseDirectSource { get; set; }
        bool GetDiagnostics { get; set; }
    }

    public interface IYqlResult
    {
        YqlDiagnostics Diagnostics { get; }
    }


    
    public class YqlDiagnostics
    {
        [JsonProperty("publiclycallable")]
        public bool PubliclyCallable { get; set; }
        [JsonProperty("url"), JsonConverter(typeof(SingleOrArrayConverter<YqlDiagUrl>))]
        public YqlDiagUrl[] Url { get; set; }
        [JsonProperty("user-time")]
        public int UserTime { get; set; }
        [JsonProperty("service-time")]
        public int ServiceTime { get; set; }
        [JsonProperty("build-version")]
        public string BuildVersion { get; set; }

        public YqlDiagnostics() { }
    }
    public class YqlDiagUrl
    {
        [JsonProperty("execution-start-time")]
        public int ExecutionStartTime { get; set; }
        [JsonProperty("execution-stop-time")]
        public int ExecutionStopTime { get; set; }
        [JsonProperty("execution-time")]
        public int ExecutionTime { get; set; }
        [JsonProperty("content")]
        public Uri Content { get; set; }

        public YqlDiagUrl() { }
    }




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

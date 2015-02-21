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
using Newtonsoft.Json;

namespace MaasOne.Net
{
    public class YqlDiagnostics
    {
        internal YqlDiagnostics() { }



        [JsonProperty("publiclycallable")]
        public bool PubliclyCallable { get; set; }

        [JsonProperty("url"), JsonConverter(typeof(System.Resources.SingleOrArrayConverter<YqlDiagUrl>))]
        public YqlDiagUrl[] Url { get; set; }

        [JsonProperty("user-time")]
        public int UserTime { get; set; }

        [JsonProperty("service-time")]
        public int ServiceTime { get; set; }

        [JsonProperty("build-version")]
        public string BuildVersion { get; set; }
    }




    public class YqlDiagUrl
    {
        internal YqlDiagUrl() { }



        [JsonProperty("execution-start-time")]
        public int ExecutionStartTime { get; set; }

        [JsonProperty("execution-stop-time")]
        public int ExecutionStopTime { get; set; }

        [JsonProperty("execution-time")]
        public int ExecutionTime { get; set; }

        [JsonProperty("content")]
        public Uri Content { get; set; }
    }




    /// <summary>
    /// Provides properties to set the start index and count number for a YQL query in results queue.
    /// </summary>
    public interface IYqlIndexQuery
    {
        /// <summary>
        /// The results queue start index.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// The total number of results.
        /// </summary>
        int Count { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace MaasOne.Net
{
    public abstract class ResultBase
    {
        [System.Xml.Serialization.XmlIgnore()]
        public DataIntegrityInfo Integrity { get; internal set; }
    }


    public class DataIntegrityInfo
    {
        public bool IsComplete { get; private set; }

        public string[] Messages { get; private set; }

        internal DataIntegrityInfo(bool isComplete, string[] messages)
        {
            this.IsComplete = isComplete;
            this.Messages = messages != null ? messages : new string[0];
        }
    }
}

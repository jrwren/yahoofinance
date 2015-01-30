using System;
using System.Collections.Generic;

namespace System.Net
{

    public class ValidationResult
    {

        public bool Success { get; set; }
        public List<KeyValuePair<string, string>> Info { get; set; }

        public ValidationResult()
        {
            this.Success = true;
            this.Info = new List<KeyValuePair<string,string>>();
        }
        
        internal Exception CreateException()
        {
            if (this.Success) { return null; }
            else
            {
                string message = string.Empty;
                foreach (var info in this.Info)
                {
                    message += string.Format("\n{0}: \"{1}\"", info.Key, info.Value);
                }
                return new ArgumentException("The query is not valid.\n" + message + "\n\n(Is the Clone() method copying all the data?)");
            }
        }

    }

}

using System;
using System.Collections.Generic;

namespace System.Net
{

    public class ConvertException : Exception
    {
        public ConvertException(string message) : base(message) { }
        public ConvertException(Exception innerException) : base(innerException is ConvertException ? innerException.Message : "An Exception was thrown during result conversion process. See InnerException for more details.", innerException is ConvertException ? innerException.InnerException : innerException) { }
        public ConvertException(string message, Exception innerException) : base(innerException is ConvertException ? innerException.Message : message, innerException is ConvertException ? innerException.InnerException : innerException) { }

    }

}

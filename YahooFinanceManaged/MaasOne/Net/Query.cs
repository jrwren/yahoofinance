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


namespace MaasOne.Net
{

    public abstract class Query<T> : IQuery where T : class
    {
        protected abstract void ValidateQuery(ValidationResult result);
        protected abstract Uri CreateUrl();
        protected abstract T ConvertResult(System.IO.Stream stream);
        public abstract Query<T> Clone();
        internal void ValidateQueryInternal(ValidationResult result) { this.ValidateQuery(result); }
        internal Uri GetUrlInternal() { return this.CreateUrl(); }
        internal T ConvertResultInternal(System.IO.Stream stream) { return this.ConvertResult(stream); }
        IQuery IQuery.GetCloneDeep() { return this.Clone(); }
    }

}

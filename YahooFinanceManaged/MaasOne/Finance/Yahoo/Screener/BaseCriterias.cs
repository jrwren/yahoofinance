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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;


namespace MaasOne.Finance.Yahoo.Screener
{

    public abstract class ScreenerCriteria
    {

        public abstract string DisplayName { get; }
        public abstract ScreenerProperty Property { get; }

        protected ScreenerCriteria() { }

        protected abstract string TagParameter();
        protected abstract bool IsValid();

        internal string TagParameterInternal() { return this.TagParameter(); }
        internal bool IsValidInternal() { return this.IsValid(); }

    }

    public abstract class ScreenerMinMaxCriteria<T> : ScreenerCriteria where T : struct
    {
        public T? Minimum { get; set; }
        public T? Maximum { get; set; }
        protected ScreenerMinMaxCriteria() { }
    }



}

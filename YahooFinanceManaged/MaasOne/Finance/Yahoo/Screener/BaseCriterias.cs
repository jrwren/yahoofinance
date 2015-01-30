// ******************************************************************************
// ** 
// **  Yahoo! Managed
// **  Written by Marius H�usler 2012
// **  It would be pleasant, if you contact me when you are using this code.
// **  Contact: YahooFinanceManaged@gmail.com
// **  Project Home: http://code.google.com/p/yahoo-finance-managed/
// **  
// ******************************************************************************
// **  
// **  Copyright 2012 Marius H�usler
// **  
// **  Licensed under the Apache License, Version 2.0 (the "License");
// **  you may not use this file except in compliance with the License.
// **  You may obtain a copy of the License at
// **  
// **    http://www.apache.org/licenses/LICENSE-2.0
// **  
// **  Unless required by applicable law or agreed to in writing, software
// **  distributed under the License is distributed on an "AS IS" BASIS,
// **  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// **  See the License for the specific language governing permissions and
// **  limitations under the License.
// ** 
// ******************************************************************************
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

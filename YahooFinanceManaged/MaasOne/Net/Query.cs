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


namespace MaasOne.Net
{

    public abstract class Query<T> : IQuery where T : class
    {
        protected abstract void ValidateQuery(ValidationResult result);
        protected abstract Uri CreateUrl();
        protected abstract T ConvertResult(System.IO.Stream stream);
        public abstract Query<T> Clone();

        IQuery IQuery.GetClone() { return this.Clone(); }

        internal void ValidateQueryInternal(ValidationResult result) { this.ValidateQuery(result); }
        internal Uri GetUrlInternal() { return this.CreateUrl(); }
        internal T ConvertResultInternal(System.IO.Stream stream) { return this.ConvertResult(stream); }
    }

}

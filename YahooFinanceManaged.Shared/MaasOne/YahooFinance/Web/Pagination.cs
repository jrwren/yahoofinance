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
using System.Collections.Generic;

namespace MaasOne.YahooFinance.Web
{
    public abstract class Pagination
    {
        public virtual int Start { get; protected set; }

        public virtual int Count { get; protected set; }


        public Pagination() { }
    }


    public class LimitedPagination : Pagination
    {
        public int Max { get; protected set; }


        protected LimitedPagination() { }

        public LimitedPagination(int start, int count, int max) { 
            this.Start = start;
            this.Count = count;
            this.Max = max; 
        }
    }


    public class StartIndexPagination : Pagination
    {
        /// <summary>
        /// Gets or sets the start index of the pagination. The index is 0-based.
        /// </summary>
        public new int Start
        {
            get { return base.Start; }
            set
            {
                if (value < 0) throw new ArgumentException("The start index value cannot be negative.", "value");
                base.Start = value;
            }
        }


        public StartIndexPagination() : base() { }


        public virtual StartIndexPagination Clone()
        {
            return new StartIndexPagination() { Start = this.Start, Count = this.Count };
        }
    }
}

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

namespace MaasOne.YahooFinance.Web
{

    public abstract class Pagination
    {

        public virtual int Start { get; protected set; }
        public virtual int Count { get; protected set; }

        public Pagination() { this.Start = 0; this.Count = 0; }

    }

    public class LimitedPagination : Pagination
    {

        public int Max { get; protected set; }

        protected LimitedPagination() : this(0, 0, 0) { }
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

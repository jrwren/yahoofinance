using System;
using System.Collections.Generic;

namespace MaasOne.Finance.Yahoo.Web
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

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

namespace MaasOne.Net
{
    public class DownloadOperationCollection : IEnumerable<DownloadOperation>
    {
        private readonly List<DownloadOperation> mItems = new List<DownloadOperation>();



        internal DownloadOperationCollection() { }



        public DownloadOperation[] Items { get { return this.mItems.ToArray(); } }

        public DownloadOperation this[int index] { get { return this.mItems[index]; } }

        public int Count { get { return this.mItems.Count; } }



        public void CancelAll()
        {
            foreach (DownloadOperation ds in this.mItems) { ds.Cancel(); }
        }

        public IEnumerator<DownloadOperation> GetEnumerator()
        {
            return mItems.GetEnumerator();
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mItems.GetEnumerator();
        }


        internal void Add(DownloadOperation item)
        {
            this.mItems.Add(item);
        }

        internal bool Remove(DownloadOperation item)
        {
            if (this.mItems.Contains(item))
            {
                item.Cleanup();
                return this.mItems.Remove(item);
            }
            return false;
        }

        internal bool Remove(object clientObject)
        {
            foreach (DownloadOperation ds in this.mItems)
            {
                if (ds.ClientObject == clientObject)
                {
                    ds.Cleanup();
                    return this.Remove(ds);
                }
            }
            return false;
        }
    }
}

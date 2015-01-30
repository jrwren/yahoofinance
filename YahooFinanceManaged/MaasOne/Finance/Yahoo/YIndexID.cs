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
using System.Text;
using MaasOne.Finance.Yahoo;

namespace MaasOne.Finance.Yahoo
{
    /// <summary>
    /// Stores information of an stock index. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class YIndexID : YID
    {


        private bool mDownloadComponents = false;
        /// <summary>
        /// The Yahoo index ID
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ID
        {
            get
            {
                if (mDownloadComponents)
                {
                    return "@" + base.ID;
                }
                else
                {
                    return base.ID;
                }
            }
        }
        /// <summary>
        /// The overloaded type of the financial product 
        /// </summary>
        /// <value></value>
        /// <returns>Returns only [Index]. The type of an stock index is of course always [Index].</returns>
        /// <remarks></remarks>
        public override SecurityType Type
        {
            get { return SecurityType.Index; }
            set { base.Type = SecurityType.Index; }
        }
        /// <summary>
        /// Indicates whether the downloader will query all stocks of an index or not
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool DownloadComponents
        {
            get { return mDownloadComponents; }
            set { mDownloadComponents = value; this.OnPropertyChanged("DownloadComponents"); }
        }

        public StockExchange StockExchange { get; set; }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="id">The unmanaged ID</param>
        /// <remarks></remarks>
        public YIndexID(string id)
            : base(id)
        {
            base.Type = SecurityType.Index;
            mDownloadComponents = id.StartsWith("@");
        }
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="id">The unmanaged ID</param>
        /// <param name="downloadComponents">True, if you want to download all components of the index</param>
        /// <remarks></remarks>
        public YIndexID(string id, bool downloadComponents) : this(id) { mDownloadComponents = downloadComponents; }
        internal YIndexID(MaasOne.Resources.WorldMarketIndex orig, WorldMarket wm)
            : this(orig.ID)
        {
            this.Name = orig.Name;
            this.StockExchange = wm.GetStockExchangeFromID(orig.StockExchange);
            if (this.StockExchange == null) throw new ArgumentException("The stock exchange ID could not be found.", "StockExchangeID");
            this.StockExchange.Indices.Add(this);
        }

        /// <summary>
        /// Returns the full ID of the stock index.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.ID;
        }

    }
}

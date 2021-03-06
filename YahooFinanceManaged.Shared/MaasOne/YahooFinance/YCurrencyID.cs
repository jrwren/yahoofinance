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
using System.Text;
using System.ComponentModel;


namespace MaasOne.YahooFinance
{
    /// <summary>
    /// Stores informations about base and dependency currency. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class YCurrencyID : IID
    {
        private CurrencyInfo mBaseCurrency;
        private CurrencyInfo mDepCurrency;


        /// <summary>
        ///The Yahoo ID of the relation
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID
        {
            get
            {
                if (mBaseCurrency != null && mDepCurrency != null)
                {
                    return string.Format("{0}{1}=X", mBaseCurrency.ID, mDepCurrency.ID);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// The currency with the ratio value of 1.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CurrencyInfo BaseCurrency
        {
            get { return mBaseCurrency; }
            set { mBaseCurrency = value; }
        }

        /// <summary>
        /// The currency of the dependent value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CurrencyInfo DepCurrency
        {
            get { return mDepCurrency; }
            set { mDepCurrency = value; }
        }
        
        /// <summary>
        /// The display name of the relation.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DisplayName
        {
            get
            {
                if (mBaseCurrency != null && mDepCurrency != null)
                {
                    return string.Format("{0} to {1}", mBaseCurrency.Name, mDepCurrency.Name);
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        public YCurrencyID()
        {
            mBaseCurrency = null;
            mDepCurrency = null;
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="baseCur"></param>
        /// <param name="depCur"></param>
        /// <remarks></remarks>
        public YCurrencyID(CurrencyInfo baseCur, CurrencyInfo depCur)
        {
            this.BaseCurrency = baseCur;
            this.DepCurrency = depCur;
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="id"></param>
        /// <remarks></remarks>
     /*   public YCurrencyID(string id, WorldMarket market)
        {
            YCurrencyID newRel = market.YCurrencyIDFromString(id);
            if (newRel != null)
            {
                this.BaseCurrency = newRel.BaseCurrency;
                this.DepCurrency = newRel.DepCurrency;
            }
            else
            {
                throw new ArgumentException("The id is not valid", "id");
            }
        }*/


        /// <summary>
        /// Returns the ID of the currency relation
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.ID;
        }
    }
}

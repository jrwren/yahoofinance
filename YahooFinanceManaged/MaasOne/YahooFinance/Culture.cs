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


namespace MaasOne.YahooFinance
{
    public class Culture : System.Globalization.CultureInfo
    {

        private Language mLanguage = Language.en;

        private Country mCountry = Country.US;
        public Language Language
        {
            get { return mLanguage; }
        }
        public Country Country
        {
            get { return mCountry; }
        }

        public Culture(Language lang, Country cnt)
            : base(lang.ToString().Replace("no", "nn").Replace("tzh", "zh") + "-" + cnt.ToString().Replace("CT", "ES").Replace("UK", "GB"))
        {
            mLanguage = lang;
            mCountry = cnt;
        }

        public override object Clone()
        {
            return new Culture(this.Language, this.Country);
        }
        public Culture CloneStrict()
        {
            return (Culture)this.Clone();
        }
        public override string ToString()
        {
            return this.DisplayName;
        }
        

    }
}

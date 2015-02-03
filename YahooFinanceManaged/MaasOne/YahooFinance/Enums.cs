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

    /// <summary>
    /// Provides the financial type of the security in general like "Stock" or "Index".
    /// </summary>
    /// <remarks></remarks>
    public enum SecurityType
    {
        /// <summary>
        /// All
        /// </summary>
        /// <remarks></remarks>
        Any,
        /// <summary>
        /// Stock
        /// </summary>
        /// <remarks></remarks>
        Stock,
        /// <summary>
        /// Fund
        /// </summary>
        /// <remarks></remarks>
        Fund,
        /// <summary>
        /// Exchange Traded Fund
        /// </summary>
        /// <remarks></remarks>
        ETF,
        /// <summary>
        /// Index
        /// </summary>
        /// <remarks></remarks>
        Index,
        /// <summary>
        /// Future
        /// </summary>
        /// <remarks></remarks>
        Future,
        /// <summary>
        /// Warrant
        /// </summary>
        /// <remarks></remarks>
        Warrant,
        /// <summary>
        /// Currency
        /// </summary>
        /// <remarks></remarks>
        Currency
    }

    /// <summary>
    /// Provides the Yahoo! supported countries.
    /// </summary>
    /// <remarks></remarks>
    public enum Country
    {
        /// <summary>
        /// Argentina
        /// </summary>
        /// <remarks></remarks>
        AR = 0,

        /// <summary>
        /// Austria
        /// </summary>
        /// <remarks></remarks>
        AT = 1,

        /// <summary>
        /// Australia
        /// </summary>
        /// <remarks></remarks>
        AU = 2,

        /// <summary>
        /// Belgium
        /// </summary>
        /// <remarks></remarks>
        BE = 3,

        /// <summary>
        /// Brazil
        /// </summary>
        /// <remarks></remarks>
        BR = 4,

        /// <summary>
        /// Canada
        /// </summary>
        /// <remarks></remarks>
        CA = 5,

        /// <summary>
        /// Switzerland
        /// </summary>
        /// <remarks></remarks>
        CH = 6,

        /// <summary>
        /// Chile
        /// </summary>
        /// <remarks></remarks>
        CL = 7,

        /// <summary>
        /// China
        /// </summary>
        /// <remarks></remarks>
        CN = 8,

        /// <summary>
        /// Columbia
        /// </summary>
        /// <remarks></remarks>
        CO = 9,

        /// <summary>
        /// Catalan
        /// </summary>
        /// <remarks></remarks>
        CT = 10,

        /// <summary>
        /// Czech Republic
        /// </summary>
        /// <remarks></remarks>
        CZ = 11,

        /// <summary>
        /// Germany
        /// </summary>
        /// <remarks></remarks>
        DE = 12,

        /// <summary>
        /// Denmark
        /// </summary>
        /// <remarks></remarks>
        DK = 13,

        /// <summary>
        /// Spain
        /// </summary>
        /// <remarks></remarks>
        ES = 14,

        /// <summary>
        /// Finland
        /// </summary>
        /// <remarks></remarks>
        FI = 15,

        /// <summary>
        /// France
        /// </summary>
        /// <remarks></remarks>
        FR = 16,

        /// <summary>
        /// Hong Kong
        /// </summary>
        /// <remarks></remarks>
        HK = 17,

        /// <summary>
        /// Hungary
        /// </summary>
        /// <remarks></remarks>
        HU = 18,

        /// <summary>
        /// Indonesia
        /// </summary>
        /// <remarks></remarks>
        ID = 19,

        /// <summary>
        /// Ireland
        /// </summary>
        /// <remarks></remarks>
        IE = 20,

        /// <summary>
        /// Israel
        /// </summary>
        /// <remarks></remarks>
        IL = 21,

        /// <summary>
        /// India
        /// </summary>
        /// <remarks></remarks>
        IN = 22,

        /// <summary>
        /// Italy
        /// </summary>
        /// <remarks></remarks>
        IT = 23,

        /// <summary>
        /// Japan
        /// </summary>
        /// <remarks></remarks>
        JP = 24,

        /// <summary>
        /// Korea
        /// </summary>
        /// <remarks></remarks>
        KR = 25,

        /// <summary>
        /// Mexico
        /// </summary>
        /// <remarks></remarks>
        MX = 26,

        /// <summary>
        /// Malaysia
        /// </summary>
        /// <remarks></remarks>
        MY = 27,

        /// <summary>
        /// Netherlands
        /// </summary>
        /// <remarks></remarks>
        NL = 28,

        /// <summary>
        /// Norway
        /// </summary>
        /// <remarks></remarks>
        NO = 29,

        /// <summary>
        /// New Zealand
        /// </summary>
        /// <remarks></remarks>
        NZ = 30,

        /// <summary>
        /// Peru
        /// </summary>
        /// <remarks></remarks>
        PE = 31,

        /// <summary>
        /// Philippines
        /// </summary>
        /// <remarks></remarks>
        PH = 32,

        /// <summary>
        /// Portugal
        /// </summary>
        /// <remarks></remarks>
        PT = 33,

        /// <summary>
        /// Romania
        /// </summary>
        /// <remarks></remarks>
        RO = 34,

        /// <summary>
        /// Russia
        /// </summary>
        /// <remarks></remarks>
        RU = 35,

        /// <summary>
        /// Sweden
        /// </summary>
        /// <remarks></remarks>
        SE = 36,

        /// <summary>
        /// Singapore
        /// </summary>
        /// <remarks></remarks>
        SG = 37,

        /// <summary>
        /// Thailand
        /// </summary>
        /// <remarks></remarks>
        TH = 38,

        /// <summary>
        /// Turkey
        /// </summary>
        /// <remarks></remarks>
        TR = 39,

        /// <summary>
        /// Taiwan
        /// </summary>
        /// <remarks></remarks>
        TW = 40,

        /// <summary>
        /// United Kingdom
        /// </summary>
        /// <remarks></remarks>
        UK = 41,

        /// <summary>
        /// United States of America
        /// </summary>
        /// <remarks></remarks>
        US = 42,

        /// <summary>
        /// Venezuela
        /// </summary>
        /// <remarks></remarks>
        VE = 43,

        /// <summary>
        /// Vietnam
        /// </summary>
        /// <remarks></remarks>
        VN = 44

    }

    /// <summary>
    /// Provides the Yahoo! supported languages.
    /// </summary>
    /// <remarks></remarks>
    public enum Language
    {
        /// <summary>
        /// Arabic
        /// </summary>
        /// <remarks></remarks>
        ar = 0,

        /// <summary>
        /// Bulgarian
        /// </summary>
        /// <remarks></remarks>
        bg = 1,

        /// <summary>
        /// Catalan
        /// </summary>
        /// <remarks></remarks>
        ca = 2,

        /// <summary>
        /// Czech
        /// </summary>
        /// <remarks></remarks>
        cs = 3,

        /// <summary>
        /// Danish
        /// </summary>
        /// <remarks></remarks>
        da = 4,

        /// <summary>
        /// German
        /// </summary>
        /// <remarks></remarks>
        de = 5,

        /// <summary>
        /// Greek
        /// </summary>
        /// <remarks></remarks>
        el = 6,

        /// <summary>
        /// English
        /// </summary>
        /// <remarks></remarks>
        en = 7,

        /// <summary>
        /// Spanish
        /// </summary>
        /// <remarks></remarks>
        es = 8,

        /// <summary>
        /// Estonian
        /// </summary>
        /// <remarks></remarks>
        et = 9,

        /// <summary>
        /// Persian
        /// </summary>
        /// <remarks></remarks>
        fa = 10,

        /// <summary>
        /// Finnish
        /// </summary>
        /// <remarks></remarks>
        fi = 11,

        /// <summary>
        /// French
        /// </summary>
        /// <remarks></remarks>
        fr = 12,

        /// <summary>
        /// Hebrew
        /// </summary>
        /// <remarks></remarks>
        he = 13,

        /// <summary>
        /// Croatian
        /// </summary>
        /// <remarks></remarks>
        hr = 14,

        /// <summary>
        /// Hungarian
        /// </summary>
        /// <remarks></remarks>
        hu = 15,

        /// <summary>
        /// Indonesian
        /// </summary>
        /// <remarks></remarks>
        id = 16,

        /// <summary>
        /// Indian
        /// </summary>
        /// <remarks></remarks>
        @in = 17,

        /// <summary>
        /// Icelandic
        /// </summary>
        /// <remarks></remarks>
        @is = 18,

        /// <summary>
        /// Italian
        /// </summary>
        /// <remarks></remarks>
        it = 19,

        /// <summary>
        /// Japanese
        /// </summary>
        /// <remarks></remarks>
        ja = 20,

        /// <summary>
        /// Korean
        /// </summary>
        /// <remarks></remarks>
        ko = 21,

        /// <summary>
        /// Lithuanian
        /// </summary>
        /// <remarks></remarks>
        lt = 22,

        /// <summary>
        /// Latvian
        /// </summary>
        /// <remarks></remarks>
        lv = 23,

        /// <summary>
        /// Malaysian
        /// </summary>
        /// <remarks></remarks>
        ms = 24,

        /// <summary>
        /// Dutch
        /// </summary>
        /// <remarks></remarks>
        nl = 25,

        /// <summary>
        /// Norwegian
        /// </summary>
        /// <remarks></remarks>
        no = 26,

        /// <summary>
        /// Polish
        /// </summary>
        /// <remarks></remarks>
        pl = 27,

        /// <summary>
        /// Portuguese
        /// </summary>
        /// <remarks></remarks>
        pt = 28,

        /// <summary>
        /// Romanian
        /// </summary>
        /// <remarks></remarks>
        ro = 29,

        /// <summary>
        /// Russian
        /// </summary>
        /// <remarks></remarks>
        ru = 30,

        /// <summary>
        /// Slovak
        /// </summary>
        /// <remarks></remarks>
        sk = 31,

        /// <summary>
        /// Slovenian
        /// </summary>
        /// <remarks></remarks>
        sl = 32,

        /// <summary>
        /// Serbian
        /// </summary>
        /// <remarks></remarks>
        sr = 33,

        /// <summary>
        /// Swedish
        /// </summary>
        /// <remarks></remarks>
        sv = 34,

        /// <summary>
        /// Chinese_Simplified
        /// </summary>
        /// <remarks></remarks>
        szh = 35,

        /// <summary>
        /// Thai
        /// </summary>
        /// <remarks></remarks>
        th = 36,

        /// <summary>
        /// Filipino
        /// </summary>
        /// <remarks></remarks>
        tl = 37,

        /// <summary>
        /// Turkish
        /// </summary>
        /// <remarks></remarks>
        tr = 38,

        /// <summary>
        /// Chinese_Traditional
        /// </summary>
        /// <remarks></remarks>
        tzh = 39,

        /// <summary>
        /// Vietnamese
        /// </summary>
        /// <remarks></remarks>
        vi = 40


    }

    /// <summary>
    /// Provides the Yahoo! supported server locations.
    /// </summary>
    /// <remarks></remarks>
    public enum YahooServer
    {
        /// <summary>
        /// Argentina
        /// </summary>
        /// <remarks></remarks>
        Argentina = 0,

        /// <summary>
        /// Australia
        /// </summary>
        /// <remarks></remarks>
        Australia = 1,

        /// <summary>
        /// Brazil
        /// </summary>
        /// <remarks></remarks>
        Brazil = 2,

        /// <summary>
        /// Canada
        /// </summary>
        /// <remarks></remarks>
        Canada = 3,

        /// <summary>
        /// France
        /// </summary>
        /// <remarks></remarks>
        France = 4,

        /// <summary>
        /// Germany
        /// </summary>
        /// <remarks></remarks>
        Germany = 5,

        /// <summary>
        /// Hong Kong
        /// </summary>
        /// <remarks></remarks>
        HongKong = 6,

        /// <summary>
        /// India
        /// </summary>
        /// <remarks></remarks>
        India = 7,

        /// <summary>
        /// Italy
        /// </summary>
        /// <remarks></remarks>
        Italy = 8,

        /// <summary>
        /// Korea
        /// </summary>
        /// <remarks></remarks>
        Korea = 9,

        /// <summary>
        /// Mexico
        /// </summary>
        /// <remarks></remarks>
        Mexico = 10,

        /// <summary>
        /// New Zealand
        /// </summary>
        /// <remarks></remarks>
        NewZealand = 11,

        /// <summary>
        /// Singapore
        /// </summary>
        /// <remarks></remarks>
        Singapore = 12,

        /// <summary>
        /// Spain
        /// </summary>
        /// <remarks></remarks>
        Spain = 13,

        /// <summary>
        /// UK
        /// </summary>
        /// <remarks></remarks>
        UK = 14,

        /// <summary>
        /// USA
        /// </summary>
        /// <remarks></remarks>
        USA = 15,

    }

}

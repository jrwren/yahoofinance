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

    public class Sector
    {

        public int ID { get; private set; }
        public string Name { get; private set; }

        public Sector(int id, string name)
        {
            if (id < 0) throw new ArgumentException("ID is invalid.", "id");
            this.ID = id;
            this.Name = name;
        }
        internal Sector(MaasOne.Resources.WorldMarketSector orig) : this(orig.ID, orig.Name) { }

        public override bool Equals(object obj)
        {
            return obj != null && obj is Sector && ((Sector)obj).ID.Equals(this.ID) && ((Sector)obj).Name.Equals(this.Name);
        }
        public override string ToString()
        {
            return this.Name;
        }

    }


    public class Industry
    {

        public int ID { get; private set; }
        public string Name { get; private set; }

        public Industry(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
        internal Industry(MaasOne.Resources.WorldMarketIndustry orig, WorldMarket wm) : this(orig.ID, orig.Name) { }

        public override bool Equals(object obj)
        {
            return obj != null && obj is Industry && ((Industry)obj).ID.Equals(this.ID) && ((Industry)obj).Name.Equals(this.Name);
        }
        public override string ToString()
        {
            return this.Name;
        }

    }

}

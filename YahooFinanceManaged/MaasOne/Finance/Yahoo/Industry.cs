using System;
using System.Collections.Generic;
using System.Text;

namespace MaasOne.Finance.Yahoo
{

    public class Sector
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public List<Industry> Industries { get; set; }

        private Sector()
        {
            this.Industries = new List<Industry>();
        }
        public Sector(int id, string name)
            : this()
        {
            this.ID = id;
            this.Name = name;
        }
        internal Sector(MaasOne.Resources.WorldMarketSector orig)
            : this()
        {
            this.ID = orig.ID;
            this.Name = orig.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Sector)
            {
                return ((Sector)obj).ID.Equals(this.ID) && ((Sector)obj).Name.Equals(this.Name);
            }
            return false;
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

        public Sector Sector { get; private set; }

        public Industry(int id, string name, Sector sector)
        {
            this.ID = id;
            this.Name = name;
            this.Sector = sector;
            if (this.Sector == null) throw new ArgumentException("The sector is null.", "sector");
            if (this.Sector.Industries.Contains(this) == false) this.Sector.Industries.Add(this);
        }
        internal Industry(MaasOne.Resources.WorldMarketIndustry orig, WorldMarket wm)
        {
            this.ID = orig.ID;
            this.Name = orig.Name;
            this.Sector = wm.GetSectorFromID((int)Math.Floor(this.ID / 100.0));
            if (this.Sector == null) throw new ArgumentException("The sector ID could not be found.", "IndustryID-->SectorID");
            if (this.Sector.Industries.Contains(this) == false) this.Sector.Industries.Add(this);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Industry)
            {
                return ((Industry)obj).ID.Equals(this.ID) && ((Industry)obj).Name.Equals(this.Name);
            }
            return false;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

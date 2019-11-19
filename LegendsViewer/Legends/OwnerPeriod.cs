using System;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class OwnerPeriod
    {
        public readonly Site Site;
        public DwarfObject Owner;
        public DwarfObject Ender;
        public readonly int StartYear;
        public int EndYear;
        public string StartCause;
        public string EndCause;

        public OwnerPeriod(Site site, DwarfObject owner, int startYear, string startCause)
        {
            Site = site;
            Owner = owner;
            StartYear = startYear;
            StartCause = startCause;
            EndYear = -1;

            if (Owner is Entity entity)
            {
                entity.AddOwnedSite(this);
            }
            else
            {
                Console.WriteLine();
            }
        }
    }
}

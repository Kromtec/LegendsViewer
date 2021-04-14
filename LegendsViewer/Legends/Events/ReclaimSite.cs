using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class ReclaimSite : WorldEvent
    {
        public Entity Civ, SiteEntity;
        public Site Site;
        public bool Unretired;

        public ReclaimSite(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "civ_id": Civ = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_civ_id": SiteEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "unretire": Unretired = true; property.Known = true; break;
                }
            }

            SiteEntity?.SetParent(Civ);

            //Make sure period was lost by an event, otherwise unknown loss
            if (Site.OwnerHistory.Count == 0)
            {
                Site.OwnerHistory.Add(new OwnerPeriod(Site, null, -1, "founded"));
            }
            if (Site.OwnerHistory.Last().EndYear == -1)
            {
                Site.OwnerHistory.Last().EndCause = "abandoned";
                Site.OwnerHistory.Last().EndYear = Year - 1 == 0 ? -1 : Year - 1;
            }
            if (Unretired)
            {
                Site.OwnerHistory.Add(new OwnerPeriod(Site, SiteEntity, Year, "unretired"));
            }
            else
            {
                Site.OwnerHistory.Add(new OwnerPeriod(Site, SiteEntity, Year, "reclaimed"));
            }

            Civ.AddEvent(this);
            if (SiteEntity != Civ)
            {
                SiteEntity.AddEvent(this);
            }

            Site.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (SiteEntity != null && SiteEntity != Civ)
            {
                eventString += SiteEntity.ToLink(link, pov, this) + " of ";
            }
            eventString += Civ != null ? Civ.ToLink(link, pov, this) : "UNKNOWN CIVILISATION";
            if (Unretired)
            {
                eventString += " were taken by a mood to act against their better judgment at ";
            }
            else
            {
                eventString += " launched an expedition to reclaim ";
            }
            eventString += Site != null ? Site.ToLink(link, pov, this) : "UNKNOWN SITE";
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
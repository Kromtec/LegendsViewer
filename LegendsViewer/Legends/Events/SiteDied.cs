using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class SiteDied : WorldEvent
    {
        public Entity Civ, SiteEntity;
        public Site Site;
        public Boolean Abandoned;
        public SiteDied(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "civ_id": Civ = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_civ_id": SiteEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "abandoned":
                        property.Known = true;
                        Abandoned = true;
                        break;
                }
            }

            string endCause = "withered";
            if (Abandoned)
            {
                endCause = "abandoned";
            }

            Site.OwnerHistory.Last().EndYear = Year;
            Site.OwnerHistory.Last().EndCause = endCause;
            if (SiteEntity != null)
            {
                SiteEntity.SiteHistory.Last(s => s.Site == Site).EndYear = Year;
                SiteEntity.SiteHistory.Last(s => s.Site == Site).EndCause = endCause;
            }
            Civ.SiteHistory.Last(s => s.Site == Site).EndYear = Year;
            Civ.SiteHistory.Last(s => s.Site == Site).EndCause = endCause;

            Civ.AddEvent(this);
            SiteEntity.AddEvent(this);
            Site.AddEvent(this);

            world.AddPlayerRelatedDwarfObjects(SiteEntity);
            world.AddPlayerRelatedDwarfObjects(Site);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + SiteEntity.PrintEntity(link, pov);
            if (Abandoned)
            {
                eventString += " abandoned the settlement of " + Site.ToLink(link, pov, this);
            }
            else
            {
                eventString += " settlement of " + Site.ToLink(link, pov, this) + " withered";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class CreatedSite : WorldEvent
    {
        public Entity Civ, SiteEntity;
        public Site Site;
        public HistoricalFigure Builder;
        public CreatedSite(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "civ_id": Civ = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_civ_id": SiteEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "builder_hfid": Builder = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                }
            }

            if (SiteEntity != null)
            {
                SiteEntity.Parent = Civ;
                Site.OwnerHistory.Add(new OwnerPeriod(Site, SiteEntity, Year, "founded"));
            }
            else if (Civ != null)
            {
                Site.OwnerHistory.Add(new OwnerPeriod(Site, Civ, Year, "founded"));
            }
            else if (Builder != null)
            {
                Site.OwnerHistory.Add(new OwnerPeriod(Site, Builder, Year, "created"));
            }
            Site.AddEvent(this);
            SiteEntity.AddEvent(this);
            Civ.AddEvent(this);
            Builder.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (Builder != null)
            {
                eventString += Builder.ToLink(link, pov) + " created " + Site.ToLink(link, pov) ;
            }
            else
            {
                if (SiteEntity != null)
                {
                    eventString += SiteEntity.ToLink(link, pov) + " of ";
                }

                eventString += Civ.ToLink(link, pov) + " founded " + Site.ToLink(link, pov);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
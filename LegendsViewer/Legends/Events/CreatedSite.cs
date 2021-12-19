using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class CreatedSite : WorldEvent
    {
        public Entity Civ { get; set; }
        public Entity ResidentCiv { get; set; }
        public Entity SiteEntity { get; set; }
        public Site Site { get; set; }
        public HistoricalFigure Builder { get; set; }

        public CreatedSite(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "civ_id": Civ = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "resident_civ_id": ResidentCiv = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_civ_id": SiteEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "builder_hfid": Builder = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                }
            }

            if (ResidentCiv != null)
            {
                Site.OwnerHistory.Add(new OwnerPeriod(Site, ResidentCiv, Year, "constructed", Builder));
            }
            else if (SiteEntity != null)
            {
                SiteEntity.SetParent(Civ);
                Site.OwnerHistory.Add(new OwnerPeriod(Site, SiteEntity, Year, "founded", Builder));
            }
            else if (Civ != null)
            {
                Site.OwnerHistory.Add(new OwnerPeriod(Site, Civ, Year, "founded", Builder));
            }
            ResidentCiv.AddEvent(this);
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
                eventString += Builder.ToLink(link, pov, this);
                eventString += " constructed ";
                eventString += Site.ToLink(link, pov, this);
                if (ResidentCiv != null)
                {
                    eventString += " for ";
                    eventString += ResidentCiv.ToLink(link, pov, this);
                }
            }
            else
            {
                if (SiteEntity != null)
                {
                    eventString += SiteEntity.ToLink(link, pov, this) + " of ";
                }

                eventString += Civ.ToLink(link, pov, this);
                eventString += " founded ";
                eventString += Site.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
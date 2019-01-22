using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.Events
{
    public class SpottedLeavingSite : WorldEvent
    {
        public HistoricalFigure Spotter { get; set; }
        public Entity LeaverCiv { get; set; }
        public Entity SiteCiv { get; set; }
        public Site Site { get; set; }

        public SpottedLeavingSite(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "spotter_hfid": Spotter = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "leaver_civ_id": LeaverCiv = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_civ_id": SiteCiv = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                }
            }

            Spotter.AddEvent(this);
            LeaverCiv.AddEvent(this);
            SiteCiv.AddEvent(this);
            Site.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Spotter?.ToLink(true, pov) ?? "An unknown creature";
            if (SiteCiv != null)
            {
                eventString += " of ";
                eventString += SiteCiv.ToLink(true, pov);
            }
            eventString += " spotted the forces";
            if (LeaverCiv != null)
            {
                eventString += " of ";
                eventString += LeaverCiv.ToLink(true, pov);
            }
            eventString += " slipping out";
            if (Site != null)
            {
                eventString += " of ";
                eventString += Site.ToLink(true, pov);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

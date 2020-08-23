using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfFreed : WorldEvent
    {
        public List<HistoricalFigure> RescuedHistoricalFigures { get; set; }
        public HistoricalFigure FreeingHf { get; set; }
        public Entity FreeingCiv { get; set; }
        public Entity SiteCiv { get; set; }
        public Entity HoldingCiv { get; set; }
        public Site Site { get; set; }

        public HfFreed(List<Property> properties, World world)
            : base(properties, world)
        {
            RescuedHistoricalFigures = new List<HistoricalFigure>();
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "freeing_civ_id": FreeingCiv = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "freeing_hfid": FreeingHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "rescued_hfid": RescuedHistoricalFigures.Add(world.GetHistoricalFigure(Convert.ToInt32(property.Value))); break;
                    case "site_civ_id": SiteCiv = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "holding_civ_id": HoldingCiv = world.GetEntity(Convert.ToInt32(property.Value)); break;
                }
            }
            foreach (var rescuedHistoricalFigure in RescuedHistoricalFigures)
            {
                rescuedHistoricalFigure.AddEvent(this);
            }
            FreeingCiv.AddEvent(this);
            FreeingHf.AddEvent(this);
            Site.AddEvent(this);
            SiteCiv.AddEvent(this);
            HoldingCiv.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (FreeingHf != null)
            {
                eventString += FreeingHf?.ToLink(link, pov, this) ?? "an unknown creature";
            }
            else
            {
                eventString += "the forces of ";
                eventString += FreeingCiv?.ToLink(link, pov, this) ?? "an unknown civilization";
            }
            eventString += " freed ";
            for (int i = 0; i < RescuedHistoricalFigures.Count; i++)
            {
                if (i > 0)
                {
                    eventString += " and ";
                }
                eventString += RescuedHistoricalFigures[i]?.ToLink(link, pov, this) ?? "an unknown creature";
            }
            if (Site != null)
            {
                eventString += " from " + Site.ToLink(link, pov, this);
            }
            if (SiteCiv != null)
            {
                eventString += " and " + SiteCiv.ToLink(link, pov, this);
            }
            if (HoldingCiv != null)
            {
                eventString += " of " + HoldingCiv.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
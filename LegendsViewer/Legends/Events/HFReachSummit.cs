using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfReachSummit : WorldEvent
    {
        public HistoricalFigure HistoricalFigure { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public Site Site { get; set; }
        public Location Coordinates;

        public HfReachSummit(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "group_hfid": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "site": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                    case "group": if (HistoricalFigure == null) { HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                }
            }
            HistoricalFigure.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
            Site.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
            eventString += " reached the summit";
            if (Region != null)
            {
                eventString += ", which rises above ";
                eventString += Region.ToLink(link, pov, this);
            }
            else if (UndergroundRegion != null)
            {
                eventString += ", in the depths of ";
                eventString += UndergroundRegion.ToLink(link, pov, this);
            }
            if (Site != null)
            {
                eventString += " in ";
                eventString += Site.ToLink(link, pov, this);
            }
            eventString += ".";
            return eventString;
        }
    }
}
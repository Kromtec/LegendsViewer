using System;
using System.Collections.Generic;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfAbducted : WorldEvent
    {
        public HistoricalFigure Target { get; set; }
        public HistoricalFigure Snatcher { get; set; }
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public HfAbducted(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "target_hfid": Target = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "snatcher_hfid": Snatcher = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                }
            }

            Target.AddEvent(this);
            Snatcher.AddEvent(this);
            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (Snatcher != null)
            {
                eventString += Snatcher.ToLink(link, pov, this);
            }
            else
            {
                eventString += "UNKNOWN HISTORICAL FIGURE";
            }

            eventString += " abducted " + Target.ToLink(link, pov, this) + " from " + Site.ToLink(link, pov, this);
            if (!(ParentCollection is Abduction))
            {
                eventString += PrintParentCollection(link, pov);
            }
            eventString += ".";
            return eventString;
        }
    }
}
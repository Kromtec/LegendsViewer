using System;
using System.Collections.Generic;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfNewPet : WorldEvent
    {
        public string Pet { get; set; }
        public HistoricalFigure HistoricalFigure;
        public Site Site;
        public WorldRegion Region;
        public UndergroundRegion UndergroundRegion;
        public Location Coordinates;
        public HfNewPet(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                    case "group_hfid": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "group": if (HistoricalFigure == null) { HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "pets": Pet = property.Value.Replace("_", " ").Replace("2", "two"); break;
                }
            }

            HistoricalFigure.AddEvent(this);
            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + HistoricalFigure.ToLink(link, pov) + " tamed the creatures named ";
            if (!string.IsNullOrWhiteSpace(Pet))
            {
                eventString += Pet;
            }
            else
            {
                eventString += "UNKNOWN";
            }
            if (Site != null)
            {
                eventString += " in " + Site.ToLink(link, pov);
            }
            else if (Region != null)
            {
                eventString += " in " + Region.ToLink(link, pov);
            }
            else if (UndergroundRegion != null)
            {
                eventString += " in " + UndergroundRegion.ToLink(link, pov);
            }
            if (!(ParentCollection is Journey))
            {
                eventString += PrintParentCollection(link, pov);
            }
            eventString += ".";
            return eventString;
        }
    }
}
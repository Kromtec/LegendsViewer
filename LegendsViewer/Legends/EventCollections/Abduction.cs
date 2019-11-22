using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.EventCollections
{
    public class Abduction : EventCollection
    {
        public string Ordinal;
        public Location Coordinates;

        public HistoricalFigure Abductee;
        public WorldRegion Region;
        public UndergroundRegion UndergroundRegion;
        public Site Site;
        public Entity Attacker;
        public Entity Defender;

        public List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return AllEvents.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }
        public Abduction(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "ordinal": Ordinal = String.Intern(property.Value); break;
                    case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                    case "parent_eventcol": ParentCollection = world.GetEventCollection(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "attacking_enid": Attacker = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "defending_enid": Defender = world.GetEntity(Convert.ToInt32(property.Value)); break;
                }
            }

            Abductee.AddEventCollection(this);
            Region.AddEventCollection(this);
            UndergroundRegion.AddEventCollection(this);
            Site.AddEventCollection(this);
            Attacker.AddEventCollection(this);
            Defender.AddEventCollection(this);
        }
        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            return "an abduction";
            /*string colString = this.GetYearTime(true) + "The " + ordinals[numeral] + " abduction of ";
            if (abductee != null) colString += abductee.ToLink(path, pov);
            else colString += "UNKNOWN FIGURE";
                             return colString + " in " + period.ToLink(path, pov) + " ocurred";*/
        }
    }
}

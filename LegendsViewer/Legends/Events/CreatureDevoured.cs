using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class CreatureDevoured : WorldEvent
    {
        public string Race { get; set; }
        public string Caste { get; set; }

        public HistoricalFigure Eater, Victim;
        public Entity Entity { get; set; }
        public Site Site;
        public WorldRegion Region;
        public UndergroundRegion UndergroundRegion;
        public CreatureDevoured(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "victim": Victim = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "eater": Eater = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "entity": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "race": Race = property.Value.Replace("_", " "); break;
                    case "caste": Caste = property.Value; break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                }
            }

            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
            Eater.AddEvent(this);
            Victim.AddEvent(this);
            Entity.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (Eater != null)
            {
                eventString += Eater.ToLink(link, pov, this);
            }
            else
            {
                eventString += "UNKNOWN HISTORICAL FIGURE";
            }
            eventString += " devoured ";
            if (Victim != null)
            {
                eventString += Victim.ToLink(link, pov, this);
            }
            else if (!string.IsNullOrWhiteSpace(Race))
            {
                eventString += " a ";
                if (!string.IsNullOrWhiteSpace(Caste))
                {
                    eventString += Caste + " ";
                }
                eventString += Race;
            }
            else
            {
                eventString += "UNKNOWN HISTORICAL FIGURE";
            }
            eventString += " in ";
            if (Site != null)
            {
                eventString += Site.ToLink(link, pov, this);
            }
            else if (Region != null)
            {
                eventString += Region.ToLink(link, pov, this);
            }
            else if (UndergroundRegion != null)
            {
                eventString += UndergroundRegion.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
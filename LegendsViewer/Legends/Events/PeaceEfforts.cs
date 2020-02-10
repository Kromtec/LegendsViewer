using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class PeaceEfforts : WorldEvent
    {
        public string Decision { get; set; }
        public string Topic { get; set; }
        public Entity Source { get; set; }
        public Entity Destination { get; set; }
        public Site Site;
        public PeaceEfforts(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "topic": Topic = property.Value; break;
                    case "source": Source = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "destination": Destination = world.GetEntity(Convert.ToInt32(property.Value)); break;
                }
            }

            Site.AddEvent(this);
            Source.AddEvent(this);
            Destination.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (Source != null && Destination != null)
            {
                eventString += Destination.ToLink(link, pov, this) + " " + Decision + " an offer of peace from " + Source.ToLink(link, pov, this) + " in " + ParentCollection.ToLink(link, pov, this) + ".";
            }
            else
            {
                eventString += "Peace " + Decision + " in " + ParentCollection.ToLink(link, pov, this) + ".";
            }
            return eventString;
        }
    }
}
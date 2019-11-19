using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.EventCollections
{
    public class Purge : EventCollection
    {
        public string Ordinal;
        public string Adjective { get; set; }
        public Site Site;

        public List<string> Filters;

        public override List<WorldEvent> FilteredEvents
        {
            get { return AllEvents.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }

        public Purge(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "ordinal": Ordinal = String.Intern(property.Value); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "adjective": Adjective = property.Value; break;
                }
            }
            Site.AddEventCollection(this);
        }

        public override string ToLink(bool link = true, DwarfObject pov = null)
        {
            return "a "+(!string.IsNullOrWhiteSpace(Adjective) ? Adjective.ToLower()+" " : "")+"purge";
        }
    }
}

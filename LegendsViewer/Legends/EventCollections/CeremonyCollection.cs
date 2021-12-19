using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.EventCollections
{
    public class CeremonyCollection : EventCollection
    {
        public string Ordinal;

        public List<string> Filters;
        public override List<WorldEvent> FilteredEvents => AllEvents.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList();
        public CeremonyCollection(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "ordinal": Ordinal = string.Intern(property.Value); break;
                }
            }
        }
        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            return "a ceremony";
        }
    }
}

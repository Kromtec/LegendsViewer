using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls.Query.Attributes;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class Feature : WorldObject
    {
        public string Type { get; set; } // legends_plus.xml
        public int Reference { get; set; } // legends_plus.xml

        public static List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }

        public Feature(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "type": Type = string.Intern(property.Value); break;
                    case "reference": Reference = Convert.ToInt32(property.Value); break;
                }
            }
        }

        public override string ToString() { return Type; }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            return Type;
        }
    }
}

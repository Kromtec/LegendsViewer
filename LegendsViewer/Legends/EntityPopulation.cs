using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class EntityPopulation : WorldObject
    {
        public CreatureInfo Race { get; set; } // legends_plus.xml
        public int Count { get; set; } // legends_plus.xml
        public int EntityId { get; set; } // legends_plus.xml
        public Entity Entity { get; set; } // legends_plus.xml
        public List<HistoricalFigure> Member { get; set; }

        public static List<string> Filters;
        public override List<WorldEvent> FilteredEvents => Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList();
        public EntityPopulation(List<Property> properties, World world)
            : base(properties, world)
        {
            EntityId = -1;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "race":
                        var raceCount = property.Value.Split(':');
                        Race = world.GetCreatureInfo(raceCount[0]);
                        Count = Convert.ToInt32(raceCount[1]);
                        break;
                    case "civ_id":
                        EntityId = property.ValueAsInt();
                        break;
                }
            }
        }
    }
}

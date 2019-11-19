using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.EventCollections
{
    public class Occasion : EventCollection
    {
        public Entity Civ { get; set; }
        public string Ordinal { get; set; }
        public int OccasionId { get; set; }
        public EntityOccasion EntityOccasion { get; set; }

        public List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return AllEvents.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }

        public Occasion(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "civ_id": Civ = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "ordinal": Ordinal = string.Intern(property.Value); break;
                    case "occasion_id": OccasionId = Convert.ToInt32(property.Value); break;
                }
            }
            if (Civ != null && Civ.Occassions.Any())
            {
                EntityOccasion = Civ.Occassions.ElementAt(OccasionId);
            }
            Civ.AddEventCollection(this);
        }

        public override string ToLink(bool link = true, DwarfObject pov = null)
        {
            return "an occasion";
        }
    }
}

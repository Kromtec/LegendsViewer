using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class RazedStructure : WorldEvent
    {
        public Entity Entity { get; set; }
        public Site Site { get; set; }
        public int StructureId { get; set; }
        public Structure Structure { get; set; }

        public RazedStructure(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "civ_id": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "structure_id": StructureId = Convert.ToInt32(property.Value); break;
                }
            }
            if (Site != null)
            {
                Structure = Site.Structures.FirstOrDefault(structure => structure.Id == StructureId);
            }

            Entity.AddEvent(this);
            Site.AddEvent(this);
            Structure.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + Entity.ToLink(link, pov, this) + " razed ";
            eventString += Structure != null ? Structure.ToLink(link, pov, this) : "UNKNOWN STRUCTURE";
            eventString += " in " + Site.ToLink(link, pov, this);
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
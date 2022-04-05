using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class EntityCreated : WorldEvent
    {
        public Entity Entity { get; set; }
        public Site Site { get; set; }
        public int StructureId { get; set; }
        public Structure Structure { get; set; }
        public HistoricalFigure Creator { get; set; }

        public EntityCreated(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "entity_id": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "structure_id": StructureId = Convert.ToInt32(property.Value); break;
                    case "creator_hfid": Creator = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                }
            }
            if (Site != null)
            {
                Structure = Site.Structures.Find(structure => structure.Id == StructureId);
                if (Entity != null && Structure != null)
                {
                    Entity.OriginStructure = Structure;
                }
            }
            Entity.AddEvent(this);
            Site.AddEvent(this);
            Structure.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (Creator != null)
            {
                eventString += Creator.ToLink(link, pov, this);
                eventString += " formed ";
                eventString += Entity.ToLink(link, pov, this);
            }
            else
            {
                eventString += Entity.ToLink(link, pov, this);
                eventString += " formed";
            }
            if (Structure != null)
            {
                eventString += " in ";
                eventString += Structure.ToLink(link, pov, this);
            }
            if (Site != null)
            {
                eventString += " in ";
                eventString += Site.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
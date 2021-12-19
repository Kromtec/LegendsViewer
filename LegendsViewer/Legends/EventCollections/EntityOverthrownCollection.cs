using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.EventCollections
{
    public class EntityOverthrownCollection : EventCollection
    {
        public string Ordinal;
        public Location Coordinates;

        public WorldRegion Region;
        public UndergroundRegion UndergroundRegion;
        public Site Site;
        public Entity TargetEntity;

        public List<string> Filters;
        public override List<WorldEvent> FilteredEvents => AllEvents.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList();
        public EntityOverthrownCollection(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "ordinal": Ordinal = string.Intern(property.Value); break;
                    case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                    case "parent_eventcol": ParentCollection = world.GetEventCollection(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "target_entity_id": TargetEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                }
            }

            Region.AddEventCollection(this);
            UndergroundRegion.AddEventCollection(this);
            Site.AddEventCollection(this);
            TargetEntity.AddEventCollection(this);
        }
        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            return "a coup";
        }
    }
}

using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class EntityBreachFeatureLayer : WorldEvent
    {
        public Entity SiteEntity { get; set; }
        public Entity CivEntity { get; set; }
        public Site Site { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }

        // http://www.bay12games.com/dwarves/mantisbt/view.php?id=11335
        // 0011335: <site_entity_id> and <civ_entity_id> of "entity breach feature layer" event point both to same entity
        public EntityBreachFeatureLayer(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "site_entity_id": SiteEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "civ_entity_id": CivEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                }
            }

            if (SiteEntity != CivEntity)
            {
                SiteEntity.AddEvent(this);
            }
            CivEntity.AddEvent(this);
            Site.AddEvent(this);
            UndergroundRegion.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += SiteEntity.ToLink(link, pov, this);
            eventString += " of ";
            eventString += CivEntity.ToLink(link, pov, this);
            eventString += " breached ";
            eventString += UndergroundRegion.ToLink(link, pov, this);
            if (Site != null)
            {
                eventString += " at ";
                eventString += Site.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class EntityIncorporated : WorldEvent
    {
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public Entity JoinerEntity { get; set; }
        public Entity JoinedEntity { get; set; }
        public HistoricalFigure Leader { get; set; }
        public bool PartialIncorporation { get; set; }

        public EntityIncorporated(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "joiner_entity_id": JoinerEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "joined_entity_id": JoinedEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "leader_hfid": Leader = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "partial_incorporation":
                        property.Known = true; 
                        PartialIncorporation = true; 
                        break;
                }
            }

            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
            Leader.AddEvent(this);
            JoinerEntity.AddEvent(this);
            JoinedEntity.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += JoinerEntity.ToLink(link, pov, this);
            if (PartialIncorporation)
            {
                eventString += " began operating at the direction of ";
            }
            else
            {
                eventString += " fully incorporated into ";
            }
            eventString += JoinedEntity.ToLink(link, pov, this);
            if (Leader != null)
            {
                eventString += " under the leadership of ";
                eventString += Leader.ToLink(link, pov, this);
            }
            if (Site != null)
            {
                eventString += " in ";
                eventString += Site.ToLink(link, pov, this);
            }
            else if (Region != null)
            {
                eventString += " in ";
                eventString += Region.ToLink(link, pov, this);
            }
            else if (UndergroundRegion != null)
            {
                eventString += " in ";
                eventString += UndergroundRegion.ToLink(link, pov, this);
            }

            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

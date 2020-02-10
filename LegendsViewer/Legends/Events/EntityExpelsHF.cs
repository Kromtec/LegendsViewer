using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class EntityExpelsHf : WorldEvent
    {
        public Entity Entity { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public Site Site { get; set; }

        public EntityExpelsHf(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "entity_id":
                        Entity = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "hfid":
                        HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "site_id":
                        Site = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                }
            }

            Entity.AddEvent(this);
            HistoricalFigure.AddEvent(this);
            Site.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Entity?.ToLink(true, pov) ?? "an unknown entity";
            eventString += " expelled ";
            eventString += HistoricalFigure?.ToLink(true, pov) ?? "an unknown creature";
            eventString += " from ";
            eventString += Site?.ToLink(true, pov) ?? "an unknown site";
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

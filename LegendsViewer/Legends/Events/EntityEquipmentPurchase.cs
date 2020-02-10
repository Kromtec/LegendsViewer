using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class EntityEquipmentPurchase : WorldEvent
    {
        public Entity Entity { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public int Quality { get; set; }

        public EntityEquipmentPurchase(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "entity_id": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "hfid": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "new_equipment_level": Quality = Convert.ToInt32(property.Value); break;
                }
            }

            Entity.AddEvent(this);
            HistoricalFigure.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Entity.ToLink(link, pov, this);
            eventString += " purchased ";
            if (Quality == 1)
            {
                eventString += "well-crafted ";
            }
            else if (Quality == 2)
            {
                eventString += "finely-crafted ";
            }
            else if (Quality == 3)
            {
                eventString += "superior quality ";
            }
            else if (Quality == 4)
            {
                eventString += "exceptional ";
            }
            else if (Quality == 5)
            {
                eventString += "masterwork ";
            }
            eventString += "equipment";
            if (HistoricalFigure != null)
            {
                eventString += ", which ";
                eventString += HistoricalFigure.ToLink(link, pov, this);
                eventString += " received";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
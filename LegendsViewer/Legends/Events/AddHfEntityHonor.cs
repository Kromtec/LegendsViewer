using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class AddHfEntityHonor : WorldEvent
    {
        public Entity Entity { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public Honor Honor { get; set; }
        public int HonorId { get; set; }

        public AddHfEntityHonor(List<Property> properties, World world) : base(properties, world)
        {
            HonorId = -1;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "entity_id": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "hfid": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "honor_id": HonorId = Convert.ToInt32(property.Value); break;
                }
            }

            if (HonorId >= 0)
            {
                Honor = Entity.Honors.Find(h => h.Id == HonorId);
            }
            Entity.AddEvent(this);
            HistoricalFigure.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += HistoricalFigure.ToLink(link, pov, this);
            eventString += $" received the title {Honor.Name} in ";
            eventString += Entity.ToLink(link, pov, this);
            string requirementsString = Honor.PrintRequirementsAsString();
            if (!string.IsNullOrWhiteSpace(requirementsString))
            {
                eventString += $" after {requirementsString}";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
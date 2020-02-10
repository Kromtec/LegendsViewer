using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfEquipmentPurchase : WorldEvent
    {
        public HistoricalFigure GroupHistoricalFigure { get; set; }
        public Site Site { get; set; }
        public int StructureId { get; set; }
        public Structure Structure { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public int Quality { get; set; }

        public HfEquipmentPurchase(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "quality": Quality = Convert.ToInt32(property.Value); break;
                    case "group_hfid": GroupHistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "structure_id": StructureId = Convert.ToInt32(property.Value); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                }
            }

            if (Site != null)
            {
                Structure = Site.Structures.FirstOrDefault(structure => structure.Id == StructureId);
            }

            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
            GroupHistoricalFigure.AddEvent(this);
            Structure.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += GroupHistoricalFigure.ToLink(link, pov, this);
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
            if (Structure != null)
            {
                eventString += " at ";
                eventString += Structure.ToLink(link, pov, this);
            }

            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

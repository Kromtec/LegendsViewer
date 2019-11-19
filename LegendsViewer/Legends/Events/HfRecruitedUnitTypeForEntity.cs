using System;
using System.Collections.Generic;
using LegendsViewer.Controls;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfRecruitedUnitTypeForEntity : WorldEvent
    {
        public HistoricalFigure HistoricalFigure { get; set; }
        public Entity Entity { get; set; }
        public UnitType UnitType { get; set; }
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }

        public HfRecruitedUnitTypeForEntity(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "hfid": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "entity_id": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "unit_type":
                        switch (property.Value)
                        {
                            case "monk":
                                UnitType = UnitType.Monk;
                                break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                }
            }
            HistoricalFigure.AddEvent(this);
            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += HistoricalFigure.ToLink(link, pov);
            eventString += " recruited ";
            switch (UnitType)
            {
                case UnitType.Monk:
                    eventString += "monks";
                    break;
                default:
                    eventString += UnitType.GetDescription();
                    break;
            }
            if (Entity != null)
            {
                eventString += " into ";
                eventString += Entity.ToLink(link, pov);
            }

            eventString += " in ";
            if (Site != null)
            {
                eventString += Site.ToLink(link, pov);
            }
            else if (Region != null)
            {
                eventString += Region.ToLink(link, pov);
            }
            else if (UndergroundRegion != null)
            {
                eventString += UndergroundRegion.ToLink(link, pov);
            }
            else
            {
                eventString += "UNKNOWN LOCATION";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

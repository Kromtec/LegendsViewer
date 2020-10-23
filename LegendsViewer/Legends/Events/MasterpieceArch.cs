using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class MasterpieceArch : WorldEvent
    {
        private string SkillAtTime { get; set; }
        public HistoricalFigure Maker { get; set; }
        public Entity MakerEntity { get; set; }
        public Site Site { get; set; }
        public string BuildingType { get; set; }
        public string BuildingSubType { get; set; }
        public int BuildingCustom { get; set; }

        public string Process { get; set; }

        public MasterpieceArch(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "skill_at_time": SkillAtTime = property.Value; break;
                    case "hfid": Maker = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "entity_id": MakerEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "maker": if (Maker == null) { Maker = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "maker_entity": if (MakerEntity == null) { MakerEntity = world.GetEntity(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "building_type": BuildingType = property.Value; break;
                    case "building_subtype": BuildingSubType = property.Value; break;
                    case "building_custom": BuildingCustom = Convert.ToInt32(property.Value); break;
                }
            }
            Maker.AddEvent(this);
            MakerEntity.AddEvent(this);
            Site.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Maker != null ? Maker.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
            eventString += " ";
            eventString += Process;
            eventString += " a masterful ";
            if (!string.IsNullOrWhiteSpace(BuildingSubType) && BuildingSubType != "-1")
            {
                eventString += BuildingSubType;
            }
            else
            {
                eventString += !string.IsNullOrWhiteSpace(BuildingType) ? BuildingType : "UNKNOWN BUILDING";
            }
            eventString += " for ";
            eventString += MakerEntity != null ? MakerEntity.ToLink(link, pov, this) : "UNKNOWN ENTITY";
            eventString += " in ";
            eventString += Site != null ? Site.ToLink(link, pov, this) : "UNKNOWN SITE";
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
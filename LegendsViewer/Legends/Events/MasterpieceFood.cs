using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class MasterpieceFood : WorldEvent
    {
        private int SkillAtTime { get; set; }
        public HistoricalFigure Maker { get; set; }
        public Entity MakerEntity { get; set; }
        public Site Site { get; set; }
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public string ItemSubType { get; set; }

        public MasterpieceFood(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "skill_at_time": SkillAtTime = Convert.ToInt32(property.Value); break;
                    case "hfid": Maker = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "entity_id": MakerEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "maker": if (Maker == null) { Maker = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "maker_entity": if (MakerEntity == null) { MakerEntity = world.GetEntity(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "item_type": ItemType = property.Value; break;
                    case "item_subtype": ItemSubType = property.Value; break;
                    case "item_id": ItemId = Convert.ToInt32(property.Value); break;
                }
            }
            Maker.AddEvent(this);
            MakerEntity.AddEvent(this);
            Site.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Maker != null ? Maker.ToLink(link, pov) : "UNKNOWN HISTORICAL FIGURE";
            eventString += " prepared a masterful ";
            switch (ItemSubType)
            {
                case "0":
                    eventString += "biscuits";
                    break;
                case "1":
                    eventString += "stew";
                    break;
                case "2":
                    eventString += "roasts";
                    break;
                default:
                    eventString += "meal";
                    break;
            }
            eventString += " for ";
            eventString += MakerEntity != null ? MakerEntity.ToLink(link, pov) : "UNKNOWN ENTITY";
            eventString += " in ";
            eventString += Site != null ? Site.ToLink(link, pov) : "UNKNOWN SITE";
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
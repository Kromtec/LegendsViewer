using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class MasterpieceItemImprovement : WorldEvent
    {
        private int SkillAtTime { get; set; }
        public HistoricalFigure Improver { get; set; }
        public Entity ImproverEntity { get; set; }
        public Site Site { get; set; }
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public string ItemSubType { get; set; }
        public string Material { get; set; }
        public int MaterialType { get; set; }
        public int MaterialIndex { get; set; }
        public string ImprovementType { get; set; }
        public string ImprovementSubType { get; set; }
        public string ImprovementMaterial { get; set; }
        public int ImprovementMaterialType { get; set; }
        public int ImprovementMaterialIndex { get; set; }
        public int ArtId { get; set; }
        public int ArtSubId { get; set; }

        public MasterpieceItemImprovement(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "skill_at_time": SkillAtTime = Convert.ToInt32(property.Value); break;
                    case "hfid": Improver = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "entity_id": ImproverEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "maker": if (Improver == null) { Improver = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "maker_entity": if (ImproverEntity == null) { ImproverEntity = world.GetEntity(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "skill_used": SkillAtTime = Convert.ToInt32(property.Value); break;
                    case "item_type": ItemType = property.Value.Replace("_", " "); break;
                    case "item_subtype": ItemSubType = property.Value.Replace("_", " "); break;
                    case "mat": Material = property.Value.Replace("_", " "); break;
                    case "mat_type": MaterialType = Convert.ToInt32(property.Value); break;
                    case "mat_index": MaterialIndex = Convert.ToInt32(property.Value); break;
                    case "improvement_type": ImprovementType = property.Value.Replace("_", " "); break;
                    case "improvement_subtype": ImprovementSubType = property.Value.Replace("_", " "); break;
                    case "imp_mat": ImprovementMaterial = property.Value.Replace("_", " "); break;
                    case "imp_mat_type": ImprovementMaterialType = Convert.ToInt32(property.Value); break;
                    case "imp_mat_index": ImprovementMaterialIndex = Convert.ToInt32(property.Value); break;
                    case "art_id": ArtId = Convert.ToInt32(property.Value); break;
                    case "art_subid": ArtSubId = Convert.ToInt32(property.Value); break;
                }
            }
            Improver.AddEvent(this);
            ImproverEntity.AddEvent(this);
            Site.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Improver != null ? Improver.ToLink(link, pov) : "UNKNOWN HISTORICAL FIGURE";
            switch (ImprovementType)
            {
                case "art image":
                    eventString += " added a masterful image";
                    break;
                case "covered":
                    eventString += " added a masterful covering";
                    break;
                default:
                    eventString += " added masterful ";
                    if (!string.IsNullOrWhiteSpace(ImprovementSubType) && ImprovementSubType != "-1")
                    {
                        eventString += ImprovementSubType;
                    }
                    else
                    {
                        eventString += !string.IsNullOrWhiteSpace(ImprovementType) ? ImprovementType : "UNKNOWN ITEM";
                    }
                    break;
            }
            eventString += " in ";
            eventString += !string.IsNullOrWhiteSpace(ImprovementMaterial) ? ImprovementMaterial + " " : "";
            eventString += " to a ";
            eventString += !string.IsNullOrWhiteSpace(Material) ? Material + " " : "";
            if (!string.IsNullOrWhiteSpace(ItemSubType) && ItemSubType != "-1")
            {
                eventString += ItemSubType;
            }
            else
            {
                eventString += !string.IsNullOrWhiteSpace(ItemType) ? ItemType : "UNKNOWN ITEM";
            }
            eventString += " for ";
            eventString += ImproverEntity != null ? ImproverEntity.ToLink(link, pov) : "UNKNOWN ENTITY";
            eventString += " at ";
            eventString += Site != null ? Site.ToLink(link, pov) : "UNKNOWN SITE";
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
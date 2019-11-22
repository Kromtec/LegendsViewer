using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class CreatedStructure : WorldEvent
    {
        public int StructureId { get; set; }
        public Structure Structure { get; set; }
        public Entity Civ { get; set; }
        public Entity SiteEntity { get; set; }
        public Site Site { get; set; }
        public HistoricalFigure Builder { get; set; }

        public CreatedStructure(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "structure_id": StructureId = Convert.ToInt32(property.Value); break;
                    case "civ_id": Civ = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_civ_id": SiteEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "builder_hfid": Builder = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "structure": StructureId = Convert.ToInt32(property.Value); break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "civ": if (Civ == null) { Civ = world.GetEntity(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "site_civ": if (SiteEntity == null) { SiteEntity = world.GetEntity(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "builder_hf": if (Builder == null) { Builder = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                }
            }

            if (Site != null)
            {
                Structure = Site.Structures.FirstOrDefault(structure => structure.Id == StructureId);
            }
            Civ.AddEvent(this);
            SiteEntity.AddEvent(this);
            Site.AddEvent(this);
            Builder.AddEvent(this);
            Structure.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (Builder != null)
            {
                eventString += Builder != null ? Builder.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                eventString += ", thrust a spire of slade up from the underworld, naming it ";
                eventString += Structure != null ? Structure.ToLink(link, pov, this) : "UNKNOWN STRUCTURE";
                eventString += ", and established a gateway between worlds in ";
                eventString += Site != null ? Site.ToLink(link, pov, this) : "UNKNOWN SITE";
            }
            else
            {
                if (SiteEntity != null)
                {
                    eventString += SiteEntity.ToLink(link, pov, this);
                    eventString += " of ";
                }
                eventString += Civ != null ? Civ.ToLink(link, pov, this) : "UNKNOWN CIV";
                eventString += " constructed ";
                eventString += Structure != null ? Structure.ToLink(link, pov, this) : "UNKNOWN STRUCTURE";
                eventString += " in ";
                eventString += Site != null ? Site.ToLink(link, pov, this) : "UNKNOWN SITE";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
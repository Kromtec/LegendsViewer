using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.WorldObjects
{
    public class Structure : WorldObject
    {
        public string Name { get; set; } // legends_plus.xml
        public string AltName { get; set; } // legends_plus.xml
        public StructureType Type { get; set; } // legends_plus.xml
        public List<int> InhabitantIDs { get; set; } // legends_plus.xml
        public List<HistoricalFigure> Inhabitants { get; set; } // legends_plus.xml
        public int DeityId { get; set; } // legends_plus.xml
        public HistoricalFigure Deity { get; set; } // legends_plus.xml
        public int ReligionId { get; set; } // legends_plus.xml
        public Entity Religion { get; set; } // legends_plus.xml
        public StructureSubType StructureSubType { get; set; } // legends_plus.xml

        public string TypeAsString
        {
            get
            {
                if (StructureSubType != StructureSubType.Unknown)
                {
                    return StructureSubType.GetDescription();
                }
                return Type.GetDescription();
            }
        }

        public string Icon { get; set; }
        public int EntityId { get; set; }
        public Entity Entity { get; set; }

        public Site Site { get; set; }

        public int GlobalId { get; set; }
        public List<int> CopiedArtifactIds { get; set; }
        public List<Artifact> CopiedArtifacts { get; set; }
        public static List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }

        public Structure(List<Property> properties, World world, Site site)
            : base(properties, world)
        {
            Name = "UNKNOWN STRUCTURE";
            InhabitantIDs = new List<int>();
            CopiedArtifactIds = new List<int>();
            DeityId = -1;
            ReligionId = -1;
            EntityId = -1;

            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "local_id": Id = Convert.ToInt32(property.Value); break;
                    case "name": Name = Formatting.InitCaps(property.Value); break;
                    case "name2": AltName = Formatting.InitCaps(property.Value); break;
                    case "inhabitant":
                        InhabitantIDs.Add(Convert.ToInt32(property.Value));
                        break;
                    case "deity":
                    case "worship_hfid":
                        DeityId = Convert.ToInt32(property.Value);
                        break;
                    case "entity_id": EntityId = Convert.ToInt32(property.Value); break;
                    case "religion": ReligionId = Convert.ToInt32(property.Value); break;
                    case "copied_artifact_id": CopiedArtifactIds.Add(Convert.ToInt32(property.Value)); break;
                    case "dungeon_type":
                        switch (property.Value)
                        {
                            case "0": StructureSubType = StructureSubType.Dungeon; break;
                            case "1": StructureSubType = StructureSubType.Sewers; break;
                            case "2": StructureSubType = StructureSubType.Catacombs; break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "subtype":
                        switch (property.Value)
                        {
                            case "standard": break;
                            case "catacombs": StructureSubType = StructureSubType.Catacombs; break;
                            case "sewers": StructureSubType = StructureSubType.Sewers; break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "type":
                        switch (property.Value)
                        {
                            case "mead_hall":
                            case "mead hall":
                                Type = StructureType.MeadHall; break;
                            case "market": Type = StructureType.Market; break;
                            case "keep": Type = StructureType.Keep; break;
                            case "temple": Type = StructureType.Temple; break;
                            case "dungeon": Type = StructureType.Dungeon; break;
                            case "tomb": Type = StructureType.Tomb; break;
                            case "inn_tavern":
                            case "inn tavern":
                                Type = StructureType.InnTavern; break;
                            case "underworld_spire":
                            case "underworld spire":
                                Type = StructureType.UnderworldSpire; break;
                            case "library": Type = StructureType.Library; break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                }
            }
            string icon = "";
            switch (Type)
            {
                case StructureType.MeadHall:
                    icon = "<i class=\"fa fa-fw fa-beer\"></i>";
                    break;
                case StructureType.Market:
                    icon = "<i class=\"fa fa-fw fa-balance-scale\"></i>";
                    break;
                case StructureType.Keep:
                    icon = "<i class=\"fa fa-fw fa-fort-awesome\"></i>";
                    break;
                case StructureType.Temple:
                    icon = "<i class=\"fa fa-fw fa-university\"></i>";
                    break;
                case StructureType.Dungeon:
                    icon = "<i class=\"fa fa-fw fa-magnet fa-flip-vertical\"></i>";
                    break;
                case StructureType.InnTavern:
                    icon = "<i class=\"fa fa-fw fa-cutlery\"></i>";
                    break;
                case StructureType.Tomb:
                    icon = "<i class=\"fa fa-fw fa-youtube-play fa-rotate-270\"></i>";
                    break;
                case StructureType.UnderworldSpire:
                    icon = "<i class=\"fa fa-fw fa-indent fa-rotate-270\"></i>";
                    break;
                case StructureType.Library:
                    icon = "<i class=\"fa fa-fw fa-graduation-cap\"></i>";
                    break;
                default:
                    icon = "";
                    break;
            }
            Icon = icon;
            Site = site;

            GlobalId = world.Structures.Count;
            world.Structures.Add(this);
        }

        public void Resolve(World world)
        {
            Inhabitants = new List<HistoricalFigure>();
            if (InhabitantIDs.Any())
            {
                foreach (int inhabitantId in InhabitantIDs)
                {
                    Inhabitants.Add(world.GetHistoricalFigure(inhabitantId));
                }
            }
            if (DeityId != -1)
            {
                Deity = world.GetHistoricalFigure(DeityId);
            }
            if (ReligionId != -1)
            {
                Religion = world.GetEntity(ReligionId);
            }
            if (EntityId != -1)
            {
                Entity = world.GetEntity(EntityId);
            }
            if (Deity != null && Religion != null)
            {
                if (!Religion.Worshipped.Contains(Deity))
                {
                    Religion.Worshipped.Add(Deity);
                    Deity.DedicatedStructures.Add(this);
                }
            }
            CopiedArtifacts = new List<Artifact>();
            if (CopiedArtifactIds.Any())
            {
                foreach (int copiedArtifactId in CopiedArtifactIds)
                {
                    CopiedArtifacts.Add(world.GetArtifact(copiedArtifactId));
                }
            }
        }


        public override string ToString() { return Name; }

        public override string ToLink(bool link = true, DwarfObject pov = null)
        {
            if (link)
            {
                string title = "";
                if (StructureSubType != StructureSubType.Unknown)
                {
                    title += StructureSubType.GetDescription();
                }
                else
                {
                    title += Type.GetDescription();
                }
                title += "&#13";
                title += "Events: " + Events.Count;

                string linkedString = "";
                if (pov != this)
                {
                    linkedString = Icon + "<a href = \"structure#" + GlobalId + "\" title=\"" + title + "\">" + Name + "</a>";
                }
                else
                {
                    linkedString = Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(Name) + "</a>";
                }
                return linkedString;
            }
            return Icon + Name;
        }

        public override string GetIcon()
        {
            return Icon;
        }
    }
}

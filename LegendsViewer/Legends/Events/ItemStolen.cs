using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class ItemStolen : WorldEvent
    {
        public int StructureId { get; set; }
        public Structure Structure { get; set; }
        public Artifact Artifact { get; set; }
        public string ItemType { get; set; }
        public string ItemSubType { get; set; }
        public string Material { get; set; }
        public int MaterialType { get; set; }
        public int MaterialIndex { get; set; }
        public HistoricalFigure Thief { get; set; }
        public Entity Entity { get; set; }
        public Site Site { get; set; }
        public Site ReturnSite { get; set; }
        public Circumstance Circumstance { get; set; }
        public int CircumstanceId { get; set; }

        public ItemStolen(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "histfig": Thief = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "entity": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "item":
                        Artifact = world.GetArtifact(Convert.ToInt32(property.Value));
                        break;
                    case "item_type": ItemType = property.Value.Replace("_", " "); break;
                    case "item_subtype": ItemSubType = property.Value; break;
                    case "mat": Material = property.Value; break;
                    case "mattype": MaterialType = Convert.ToInt32(property.Value); break;
                    case "matindex": MaterialIndex = Convert.ToInt32(property.Value); break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "structure": StructureId = Convert.ToInt32(property.Value); break;
                    case "circumstance":
                        switch (property.Value)
                        {
                            case "historical event collection":
                                Circumstance = Circumstance.HistoricalEventCollection;
                                break;
                            case "defeated hf":
                                Circumstance = Circumstance.DefeatedHf;
                                break;
                            case "murdered hf":
                                Circumstance = Circumstance.MurderedHf;
                                break;
                            default:
                                if (property.Value != "-1")
                                {
                                    property.Known = false;
                                }
                                break;
                        }
                        break;
                    case "circumstance_id":
                        CircumstanceId = Convert.ToInt32(property.Value);
                        break;
                    case "reason":
                        if (property.Value != "-1")
                        {
                            property.Known = false;
                        }
                        break;
                    case "reason_id":
                        if (property.Value != "-1")
                        {
                            property.Known = false;
                        }
                        break;
                }
            }
            if (Site != null)
            {
                Structure = Site.Structures.FirstOrDefault(structure => structure.Id == StructureId);
            }
            Thief.AddEvent(this);
            Site.AddEvent(this);
            Entity.AddEvent(this);
            Structure.AddEvent(this);
            Artifact.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (Artifact != null)
            {
                eventString += Artifact.ToLink(link, pov, this);
            }
            else
            {
                if (string.IsNullOrEmpty(ItemType))
                {
                    eventString += " an unknown item ";
                }
                else
                {
                    eventString += " a ";
                    if (!string.IsNullOrWhiteSpace(Material))
                    {
                        eventString += Material + " ";
                    }
                    eventString += ItemType;
                }
            }
            eventString += " was stolen ";
            if (Structure != null)
            {
                eventString += "from ";
                eventString += Structure.ToLink(link, pov, this);
                eventString += " ";
            }
            eventString += "in ";
            if (Site != null)
            {
                eventString += Site.ToLink(link, pov, this);
            }
            else
            {
                eventString += "UNKNOWN SITE";
            }
            eventString += " by ";
            if (Thief != null)
            {
                eventString += Thief.ToLink(link, pov, this);
            }
            else
            {
                eventString += "an unknown creature";
            }

            if (ReturnSite != null)
            {
                eventString += " and brought to " + ReturnSite.ToLink();
            }
            if (!(ParentCollection is Theft))
            {
                eventString += PrintParentCollection(link, pov);
            }
            eventString += ".";
            return eventString;
        }
    }
}
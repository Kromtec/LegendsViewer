using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class BodyAbused : WorldEvent
    {
        // TODO
        public string ItemType { get; set; } // legends_plus.xml
        public string ItemSubType { get; set; } // legends_plus.xml
        public string Material { get; set; } // legends_plus.xml
        public int PileTypeId { get; set; } // legends_plus.xml
        public int MaterialTypeId { get; set; } // legends_plus.xml
        public int MaterialIndex { get; set; } // legends_plus.xml

        public AbuseType AbuseType { get; set; } // legends_plus.xml
        public Entity Abuser { get; set; } // legends_plus.xml
        public List<HistoricalFigure> Bodies { get; set; } // legends_plus.xml
        public HistoricalFigure HistoricalFigure { get; set; } // legends_plus.xml
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public Location Coordinates { get; set; }
        public BodyAbused(List<Property> properties, World world)
            : base(properties, world)
        {
            Bodies = new List<HistoricalFigure>();
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "civ": Abuser = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "bodies": Bodies.Add(world.GetHistoricalFigure(Convert.ToInt32(property.Value))); break;
                    case "histfig": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "props_item_type": ItemType = property.Value; break;
                    case "props_item_subtype": ItemSubType = property.Value; break;
                    case "props_item_mat": Material = property.Value; break;
                    case "abuse_type":
                        switch (property.Value)
                        {
                            case "0":
                                AbuseType = AbuseType.Impaled;
                                break;
                            case "1":
                                AbuseType = AbuseType.Piled;
                                break;
                            case "2":
                                AbuseType = AbuseType.Flayed;
                                break;
                            case "3":
                                AbuseType = AbuseType.Hung;
                                break;
                            case "4":
                                AbuseType = AbuseType.Mutilated;
                                break;
                            case "5":
                                AbuseType = AbuseType.Animated;
                                break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "props_pile_type": PileTypeId = Convert.ToInt32(property.Value); break;
                    case "props_item_mat_type": MaterialTypeId = Convert.ToInt32(property.Value); break;
                    case "props_item_mat_index": MaterialIndex = Convert.ToInt32(property.Value); break;
                }
            }

            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
            Bodies.ForEach(body =>
            {
                if (body != HistoricalFigure.Unknown)
                {
                    body.AddEvent(this);
                    if (AbuseType == AbuseType.Animated)
                    {
                        body.CreatureTypes.Add(new HistoricalFigure.CreatureType("animated corpse", this));
                    }
                }
            });
            HistoricalFigure.AddEvent(this);
            Abuser.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (Bodies.Count > 1)
            {
                eventString += "the bodies of ";
                for (int i = 0; i < Bodies.Count; i++)
                {
                    eventString += Bodies[i].ToLink(link, pov, this) ?? "UNKNOWN HISTORICAL FIGURE";
                    if (i != Bodies.Count - 1)
                    {
                        if (i == Bodies.Count - 2)
                        {
                            eventString += " and ";
                        }
                        else
                        {
                            eventString += ", ";
                        }
                    }
                }
                eventString += " were ";
            }
            else
            {
                eventString += "the body of ";
                eventString += Bodies.FirstOrDefault()?.ToLink(link, pov, this) ?? "UNKNOWN HISTORICAL FIGURE";
                eventString += " was ";
            }
            switch (AbuseType)
            {
                case AbuseType.Impaled:
                    eventString += "impaled on a ";
                    eventString += !string.IsNullOrWhiteSpace(Material) ? Material + " " : "";
                    if (!string.IsNullOrWhiteSpace(ItemSubType) && ItemSubType != "-1")
                    {
                        eventString += ItemSubType;
                    }
                    else
                    {
                        eventString += !string.IsNullOrWhiteSpace(ItemType) ? ItemType : "UNKNOWN ITEM";
                    }
                    break;
                case AbuseType.Piled:
                    eventString += "added to a grisly mound";
                    break;
                case AbuseType.Flayed:
                    eventString += "flayed";
                    break;
                case AbuseType.Hung:
                    eventString += "hung";
                    break;
                case AbuseType.Mutilated:
                    eventString += "horribly mutilated";
                    break;
                case AbuseType.Animated:
                    eventString += "animated";
                    break;
                default:
                    eventString += "abused";
                    break;
            }
            eventString += " by ";

            if (HistoricalFigure != null)
            {
                eventString += HistoricalFigure.ToLink(link, pov, this);
                if (Abuser != null)
                {
                    eventString += " of ";
                }
            }
            if (Abuser != null)
            {
                eventString += Abuser.ToLink(link, pov, this);
            }
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
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
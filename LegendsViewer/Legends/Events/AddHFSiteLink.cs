using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class AddHfSiteLink : WorldEvent
    {
        public int StructureId { get; set; }
        public Structure Structure { get; set; } // TODO
        public Entity Civ { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public Site Site { get; set; }
        public SiteLinkType LinkType { get; set; }

        public AddHfSiteLink(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                LinkType = SiteLinkType.Unknown;
                switch (property.Name)
                {
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "structure": StructureId = Convert.ToInt32(property.Value); break;
                    case "civ": Civ = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "histfig": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "link_type":
                        switch (property.Value.Replace("_", " "))
                        {
                            case "lair": LinkType = SiteLinkType.Lair; break;
                            case "hangout": LinkType = SiteLinkType.Hangout; break;
                            case "home site building": LinkType = SiteLinkType.HomeSiteBuilding; break;
                            case "home site underground": LinkType = SiteLinkType.HomeSiteUnderground; break;
                            case "home structure": LinkType = SiteLinkType.HomeStructure; break;
                            case "seat of power": LinkType = SiteLinkType.SeatOfPower; break;
                            case "occupation": LinkType = SiteLinkType.Occupation; break;
                            case "home site realization building": LinkType = SiteLinkType.HomeSiteRealizationBuilding; break;
                            case "home site abstract building": LinkType = SiteLinkType.HomeSiteAbstractBuilding; break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                }
            }
            if (Site != null)
            {
                Structure = Site.Structures.FirstOrDefault(structure => structure.Id == StructureId);
            }
            HistoricalFigure.AddEvent(this);
            Civ.AddEvent(this);
            Site.AddEvent(this);
            Structure.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
            switch (LinkType)
            {
                case SiteLinkType.HomeSiteAbstractBuilding:
                case SiteLinkType.HomeSiteRealizationBuilding:
                    eventString += " took up residence in ";
                    break;
                case SiteLinkType.Hangout:
                    eventString += " ruled from ";
                    break;
                case SiteLinkType.SeatOfPower:
                    eventString += " started working from ";
                    break;
                case SiteLinkType.Occupation:
                    eventString += " started working at ";
                    break;
                default:
                    eventString += " UNKNOWN LINKTYPE (" + LinkType + ") ";
                    break;
            }
            eventString += Structure != null ? Structure.ToLink(link, pov, this) : "UNKNOWN STRUCTURE";
            if (Civ != null)
            {
                eventString += " of " + Civ.ToLink(link, pov, this);
            }
            if (Site != null)
            {
                eventString += " in " + Site.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
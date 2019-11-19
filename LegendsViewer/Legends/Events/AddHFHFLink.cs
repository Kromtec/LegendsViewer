using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class AddHfhfLink : WorldEvent
    {
        public HistoricalFigure HistoricalFigure, HistoricalFigureTarget;
        public HistoricalFigureLinkType LinkType;
        public AddHfhfLink(List<Property> properties, World world)
            : base(properties, world)
        {
            LinkType = HistoricalFigureLinkType.Unknown;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "hfid": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "hfid_target": HistoricalFigureTarget = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "link_type":
                        HistoricalFigureLinkType linkType;
                        if (Enum.TryParse(Formatting.InitCaps(property.Value.Replace("_", " ")).Replace(" ", ""), out linkType))
                        {
                            LinkType = linkType;
                        }
                        else
                        {
                            world.ParsingErrors.Report("Unknown HF HF Link Type: " + property.Value);
                        }
                        break;
                    case "histfig1":
                    case "histfig2":
                        property.Known = true;
                        break;
                    case "hf": if (HistoricalFigure == null) { HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "hf_target": if (HistoricalFigureTarget == null) { HistoricalFigureTarget = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                }
            }

            //Fill in LinkType by looking at related historical figures.
            if (LinkType == HistoricalFigureLinkType.Unknown && HistoricalFigure != HistoricalFigure.Unknown && HistoricalFigureTarget != HistoricalFigure.Unknown)
            {
                List<HistoricalFigureLink> historicalFigureToTargetLinks = HistoricalFigure.RelatedHistoricalFigures.Where(link => link.Type != HistoricalFigureLinkType.Child).Where(link => link.HistoricalFigure == HistoricalFigureTarget).ToList();
                HistoricalFigureLink historicalFigureToTargetLink = null;
                if (historicalFigureToTargetLinks.Count <= 1)
                {
                    historicalFigureToTargetLink = historicalFigureToTargetLinks.FirstOrDefault();
                }

                HfAbducted abduction = HistoricalFigureTarget.Events.OfType<HfAbducted>().SingleOrDefault(abduction1 => abduction1.Snatcher == HistoricalFigure);
                if (historicalFigureToTargetLink != null && abduction == null)
                {
                    LinkType = historicalFigureToTargetLink.Type;
                }
                else if (abduction != null)
                {
                    LinkType = HistoricalFigureLinkType.Prisoner;
                }
            }

            HistoricalFigure.AddEvent(this);
            HistoricalFigureTarget.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();

            if (pov == HistoricalFigureTarget)
            {
                eventString += HistoricalFigureTarget?.ToLink(link, pov) ?? "an unknown creature";
            }
            else
            {
                eventString += HistoricalFigure.ToLink(link, pov);
            }

            switch (LinkType)
            {
                case HistoricalFigureLinkType.Apprentice:
                    if (pov == HistoricalFigureTarget)
                    {
                        eventString += " began an apprenticeship under ";
                    }
                    else
                    {
                        eventString += " became the master of ";
                    }

                    break;
                case HistoricalFigureLinkType.Master:
                    if (pov == HistoricalFigureTarget)
                    {
                        eventString += " became the master of ";
                    }
                    else
                    {
                        eventString += " began an apprenticeship under ";
                    }

                    break;
                case HistoricalFigureLinkType.FormerApprentice:
                    if (pov == HistoricalFigure)
                    {
                        eventString += " ceased being the apprentice of ";
                    }
                    else
                    {
                        eventString += " ceased being the master of ";
                    }

                    break;
                case HistoricalFigureLinkType.FormerMaster:
                    if (pov == HistoricalFigure)
                    {
                        eventString += " ceased being the master of ";
                    }
                    else
                    {
                        eventString += " ceased being the apprentice of ";
                    }

                    break;
                case HistoricalFigureLinkType.Deity:
                    if (pov == HistoricalFigureTarget)
                    {
                        eventString += " received the worship of ";
                    }
                    else
                    {
                        eventString += " began worshipping ";
                    }

                    break;
                case HistoricalFigureLinkType.Lover:
                    eventString += " became romantically involved with ";
                    break;
                case HistoricalFigureLinkType.Spouse:
                    eventString += " married ";
                    break;
                case HistoricalFigureLinkType.Prisoner:
                    if (pov == HistoricalFigureTarget)
                    {
                        eventString += " was imprisoned by ";
                    }
                    else
                    {
                        eventString += " imprisoned ";
                    }

                    break;
                case HistoricalFigureLinkType.PetOwner:
                    eventString += " became the owner of ";
                    break;
                default:
                    eventString += " linked ("+ LinkType + ") to ";
                    break;
            }

            if (pov == HistoricalFigureTarget)
            {
                eventString += HistoricalFigure?.ToLink(link, pov) ?? "an unknown creature";
            }
            else
            {
                eventString += HistoricalFigureTarget.ToLink(link, pov);
            }

            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
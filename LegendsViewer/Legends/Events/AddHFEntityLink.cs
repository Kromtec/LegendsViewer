using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Interfaces;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class AddHfEntityLink : WorldEvent, IFeatured
    {
        public Entity Entity;
        public HistoricalFigure HistoricalFigure;
        public HfEntityLinkType LinkType;
        public string Position;
        public AddHfEntityLink(List<Property> properties, World world)
            : base(properties, world)
        {
            LinkType = HfEntityLinkType.Unknown;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "civ":
                    case "civ_id":
                        Entity = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "histfig":
                        HistoricalFigure = world.GetHistoricalFigure(property.ValueAsInt());
                        break;
                    case "link_type":
                        switch (property.Value.Replace("_"," "))
                        {
                            case "position":
                                LinkType = HfEntityLinkType.Position;
                                break;
                            case "prisoner":
                                LinkType = HfEntityLinkType.Prisoner;
                                break;
                            case "enemy":
                                LinkType = HfEntityLinkType.Enemy;
                                break;
                            case "member":
                                LinkType = HfEntityLinkType.Member;
                                break;
                            case "slave":
                                LinkType = HfEntityLinkType.Slave;
                                break;
                            case "squad":
                                LinkType = HfEntityLinkType.Squad;
                                break;
                            case "former member":
                                LinkType = HfEntityLinkType.FormerMember;
                                break;
                            default:
                                world.ParsingErrors.Report("Unknown HfEntityLinkType: " + property.Value);
                                break;
                        }
                        break;
                    case "position":
                        Position = property.Value;
                        break;
                }
            }

            HistoricalFigure.AddEvent(this);
            Entity.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (HistoricalFigure != null)
            {
                eventString += HistoricalFigure.ToLink(link, pov);
            }
            else
            {
                eventString += "UNKNOWN HISTORICAL FIGURE";
            }

            switch (LinkType)
            {
                case HfEntityLinkType.Prisoner:
                    eventString += " was imprisoned by ";
                    break;
                case HfEntityLinkType.Slave:
                    eventString += " was enslaved by ";
                    break;
                case HfEntityLinkType.Enemy:
                    eventString += " became an enemy of ";
                    break;
                case HfEntityLinkType.Member:
                    eventString += " became a member of ";
                    break;
                case HfEntityLinkType.FormerMember:
                    eventString += " became a former member of ";
                    break;
                case HfEntityLinkType.Squad:
                case HfEntityLinkType.Position:
                    EntityPosition position = Entity.EntityPositions.FirstOrDefault(pos => pos.Name.ToLower() == Position.ToLower());
                    if (position != null)
                    {
                        string positionName = position.GetTitleByCaste(HistoricalFigure.Caste);
                        eventString += " became the " + positionName + " of ";
                    }
                    else
                    {
                        eventString += " became the " + Position + " of ";
                    }
                    break;
                default:
                    eventString += " linked to ";
                    break;
            }

            eventString += Entity.ToLink(link, pov);
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }

        public string PrintFeature(bool link = true, DwarfObject pov = null)
        {
            string eventString = "";
            eventString += "the ascension of ";
            if (HistoricalFigure != null)
            {
                eventString += HistoricalFigure.ToLink(link, pov);
            }
            else
            {
                eventString += "UNKNOWN HISTORICAL FIGURE";
            }

            eventString += " to the position of ";
            if (Position != null)
            {
                EntityPosition position = Entity.EntityPositions.FirstOrDefault(pos => pos.Name.ToLower() == Position.ToLower());
                if (position != null)
                {
                    string positionName = position.GetTitleByCaste(HistoricalFigure?.Caste);
                    eventString += positionName;
                }
                else
                {
                    eventString += Position;
                }
            }
            else
            {
                eventString += "UNKNOWN POSITION";
            }
            eventString += " of ";
            eventString += Entity?.ToLink(link, pov) ?? "UNKNOWN ENTITY";
            eventString += " in ";
            eventString += Year;
            return eventString;
        }
    }
}
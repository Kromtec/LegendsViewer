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
        public Entity Entity { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public HfEntityLinkType LinkType { get; set; }
        public string Position { get; set; }
        public int PositionId { get; set; }

        public AddHfEntityLink(List<Property> properties, World world)
            : base(properties, world)
        {
            LinkType = HfEntityLinkType.Unknown;
            PositionId = -1;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "civ":
                    case "civ_id":
                        Entity = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "hfid":
                    case "histfig":
                        if (HistoricalFigure == null)
                        {
                            HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        }
                        break;
                    case "link":
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
                    case "position_id":
                        PositionId = Convert.ToInt32(property.Value);
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
                eventString += HistoricalFigure.ToLink(link, pov, this);
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
                    EntityPosition position = Entity.EntityPositions.FirstOrDefault(pos => pos.Name.ToLower() == Position.ToLower() || pos.Id == PositionId);
                    if (position != null)
                    {
                        string positionName = position.GetTitleByCaste(HistoricalFigure?.Caste);
                        eventString += " became the " + positionName + " of ";
                    }
                    else if(!string.IsNullOrWhiteSpace(Position))
                    {
                        eventString += " became the " + Position + " of ";
                    }
                    else
                    {
                        eventString += " became an unspecified position of ";
                    }
                    break;
                default:
                    eventString += " linked to ";
                    break;
            }

            eventString += Entity.ToLink(link, pov, this);
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
                eventString += HistoricalFigure.ToLink(link, pov, this);
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
            eventString += Entity?.ToLink(link, pov, this) ?? "UNKNOWN ENTITY";
            eventString += " in ";
            eventString += Year;
            return eventString;
        }
    }
}
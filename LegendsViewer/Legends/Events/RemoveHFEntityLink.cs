using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class RemoveHfEntityLink : WorldEvent
    {
        public Entity Entity { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public HfEntityLinkType LinkType { get; set; }
        public string Position { get; set; }
        public int PositionId { get; set; }

        public RemoveHfEntityLink(List<Property> properties, World world)
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
                    case "hfid":
                    case "histfig":
                        HistoricalFigure = world.GetHistoricalFigure(property.ValueAsInt());
                        break;
                    case "link":
                    case "link_type":
                        switch (property.Value.Replace("_", " "))
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
                    eventString += " escaped from the prisons of ";
                    break;
                case HfEntityLinkType.Slave:
                    eventString += " fled from ";
                    break;
                case HfEntityLinkType.Enemy:
                    eventString += " stopped being an enemy of ";
                    break;
                case HfEntityLinkType.Member:
                    eventString += " left ";
                    break;
                case HfEntityLinkType.Squad:
                case HfEntityLinkType.Position:
                    EntityPosition position = Entity.EntityPositions.Find(pos => string.Equals(pos.Name, Position, StringComparison.OrdinalIgnoreCase) || pos.Id == PositionId);
                    if (position != null)
                    {
                        string positionName = position.GetTitleByCaste(HistoricalFigure?.Caste);
                        eventString += " stopped being the " + positionName + " of ";
                    }
                    else if (!string.IsNullOrWhiteSpace(Position))
                    {
                        eventString += " stopped being the " + Position + " of ";
                    }
                    else
                    {
                        eventString += " stopped being an unspecified position of ";
                    }
                    break;
                default:
                    eventString += " stopped being linked to ";
                    break;
            }

            eventString += Entity.ToLink(link, pov, this);
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
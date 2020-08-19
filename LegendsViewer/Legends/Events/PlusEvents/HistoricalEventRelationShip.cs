using System;
using System.Collections.Generic;
using LegendsViewer.Controls;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events.PlusEvents
{
    public class HistoricalEventRelationShip : WorldEvent
    {
        public HistoricalFigure SourceHf { get; set; }
        public HistoricalFigure TargetHf { get; set; }
        public VagueRelationshipType RelationshipType { get; set; }

        public HistoricalEventRelationShip(List<Property> properties, World world) : base(properties, world)
        {
            Type = "historical event relationship";
            Seconds72 = -1;
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "event":
                        Id = Convert.ToInt32(property.Value);
                        break;
                    case "source_hf":
                        SourceHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "target_hf":
                        TargetHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "year":
                        Year = Convert.ToInt32(property.Value);
                        break;
                    case "relationship":
                        RelationshipType = VagueRelationship.GetVagueRelationshipTypeByProperty(property, property.Value);
                        break;
                }
            }

            SourceHf.AddEvent(this);
            TargetHf.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            switch (RelationshipType)
            {
                case VagueRelationshipType.JealousObsession:
                    eventString += SourceHf != null ? SourceHf.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " became infatuated with ";
                    eventString += TargetHf != null ? TargetHf.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    break;
                case VagueRelationshipType.Lieutenant:
                    eventString += SourceHf != null ? SourceHf.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " recognized ";
                    eventString += TargetHf != null ? TargetHf.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " as a capable ";
                    eventString += RelationshipType.GetDescription().ToLower();
                    break;
                case VagueRelationshipType.FormerLover:
                    eventString += SourceHf != null ? SourceHf.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " and ";
                    eventString += TargetHf != null ? TargetHf.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " broke up";
                    break;
                default:
                    eventString += SourceHf != null ? SourceHf.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " and ";
                    eventString += TargetHf != null ? TargetHf.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " became ";
                    eventString += RelationshipType.GetDescription().ToLower() + "s";
                    break;
            }
            eventString += ".";
            return eventString;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class ArtifactClaimFormed : WorldEvent
    {
        public Artifact Artifact { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public Entity Entity { get; set; }
        public Claim Claim { get; set; }
        public int PositionProfileId { get; set; }

        public ArtifactClaimFormed(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "artifact_id":
                        Artifact = world.GetArtifact(Convert.ToInt32(property.Value));
                        break;
                    case "hist_figure_id":
                        HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "entity_id":
                        Entity = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "claim":
                        switch (property.Value)
                        {
                            case "symbol":
                                Claim = Claim.Symbol;
                                break;
                            case "heirloom":
                                Claim = Claim.Heirloom;
                                break;
                            case "treasure":
                                Claim = Claim.Heirloom;
                                break;
                            default:
                                property.Known = false;
                                break;
                        }

                        break;
                    case "position_profile_id":
                        PositionProfileId = Convert.ToInt32(property.Value);
                        break;
                }
            }

            Artifact.AddEvent(this);
            HistoricalFigure.AddEvent(this);
            Entity.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Artifact.ToLink(link, pov);
            if (Claim == Claim.Symbol ||
                Claim == Claim.Heirloom && HistoricalFigure != null)
            {
                eventString += " was made a ";
                eventString += Claim.GetDescription();
                if (PositionProfileId > -1 && Entity != null)
                {
                    eventString += " of the ";
                    bool foundPosition = false;
                    foreach (EntityPositionAssignment assignment in Entity.EntityPositionAssignments)
                    {
                        EntityPosition position =
                            Entity.EntityPositions.FirstOrDefault(pos => pos.Id == assignment.PositionId);
                        if (position != null && assignment.HistoricalFigure != null)
                        {
                            string positionName = position.GetTitleByCaste(assignment.HistoricalFigure.Caste);
                            eventString += positionName;
                            foundPosition = true;
                            break;
                        }
                    }
                    if (!foundPosition)
                    {
                        eventString += "Position Title '" + PositionProfileId + "'";
                    }
                }
            }
            else
            {
                eventString += " was claimed";
            }
            if (Entity != null)
            {
                eventString += " by ";
                eventString += Entity.ToLink(link, pov);
            }
            if (HistoricalFigure != null)
            {
                eventString += " by ";
                eventString += HistoricalFigure.ToLink(link, pov);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
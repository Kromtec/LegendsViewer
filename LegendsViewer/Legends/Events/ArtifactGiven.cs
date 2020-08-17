using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class ArtifactGiven : WorldEvent
    {
        public Artifact Artifact { get; set; }
        public HistoricalFigure HistoricalFigureGiver { get; set; }
        public HistoricalFigure HistoricalFigureReceiver { get; set; }
        public Entity EntityGiver { get; set; }
        public Entity EntityReceiver { get; set; }
        public ArtifactReason ArtifactReason { get; set; }

        // http://www.bay12games.com/dwarves/mantisbt/view.php?id=11350
        // 0011350: "artifact given" event sometimes has the same <giver_hist_figure_id> and <receiver_hist_figure_id>
        public ArtifactGiven(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "artifact_id": Artifact = world.GetArtifact(Convert.ToInt32(property.Value)); break;
                    case "giver_hist_figure_id": HistoricalFigureGiver = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "receiver_hist_figure_id": HistoricalFigureReceiver = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "giver_entity_id": EntityGiver = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "receiver_entity_id": EntityReceiver = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "reason":
                        switch (property.Value)
                        {
                            case "cement bonds of friendship":
                                ArtifactReason = ArtifactReason.CementBondsOfFriendship;
                                break;
                            case "part of trade negotiation":
                                ArtifactReason = ArtifactReason.PartOfTradeNegotiation;
                                break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                }
            }

            Artifact.AddEvent(this);
            HistoricalFigureGiver.AddEvent(this);
            if (HistoricalFigureGiver != HistoricalFigureReceiver)
            {
                HistoricalFigureReceiver.AddEvent(this);
            }
            EntityGiver.AddEvent(this);
            EntityReceiver.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Artifact.ToLink(link, pov, this);
            eventString += " was offered to ";
            if (HistoricalFigureReceiver != null)
            {
                eventString += HistoricalFigureReceiver.ToLink(link, pov, this);
                if (EntityReceiver != null)
                {
                    eventString += " of ";
                }
            }
            if (EntityReceiver != null)
            {
                eventString += EntityReceiver.ToLink(link, pov, this);
            }

            eventString += " by ";
            if (HistoricalFigureGiver != null)
            {
                eventString += HistoricalFigureGiver.ToLink(link, pov, this);
                if (EntityGiver != null)
                {
                    eventString += " of ";
                }
            }
            if (EntityGiver != null)
            {
                eventString += EntityGiver.ToLink(link, pov, this);
            }
            if (ArtifactReason != ArtifactReason.Unknown)
            {
                switch (ArtifactReason)
                {
                    case ArtifactReason.CementBondsOfFriendship:
                        eventString += " in order to cement the bonds of friendship";
                        break;
                    case ArtifactReason.PartOfTradeNegotiation:
                        eventString += " as part of a trade negotiation";
                        break;
                }
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

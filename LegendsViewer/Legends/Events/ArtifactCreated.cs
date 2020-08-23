using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class ArtifactCreated : WorldEvent
    {
        public Artifact Artifact { get; set; }
        public bool ReceivedName { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public Site Site { get; set; }
        public HistoricalFigure SanctifyFigure { get; set; }
        public ArtifactReason Reason { get; set; }
        public Circumstance Circumstance { get; set; }
        public HistoricalFigure DefeatedFigure { get; set; }

        public ArtifactCreated(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "artifact_id": Artifact = world.GetArtifact(Convert.ToInt32(property.Value)); break;
                    case "hist_figure_id":
                    case "creator_hfid": 
                        HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); 
                        break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "name_only": ReceivedName = true; property.Known = true; break;
                    case "hfid": if (HistoricalFigure == null) { HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "unit_id":
                    case "creator_unit_id":
                        if (property.Value != "-1")
                        {
                            property.Known = true;
                        }
                        break;
                    case "anon_3":
                        if (property.Value != "-1")
                        {
                            property.Known = true;
                        }
                        break;
                    case "anon_4":
                    case "sanctify_hf":
                        SanctifyFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "reason":
                        switch (property.Value)
                        {
                            case "sanctify_hf":
                                Reason = ArtifactReason.SanctifyHistoricalFigure;
                                break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "circumstance":
                        foreach (var subProperty in property.SubProperties)
                        {
                            switch (subProperty.Name)
                            {
                                case "type":
                                    switch (subProperty.Value)
                                    {
                                        case "defeated":
                                            Circumstance = Circumstance.DefeatedHf;
                                            break;
                                        default:
                                            property.Known = false;
                                            break;
                                    }
                                    break;
                                case "defeated":
                                    DefeatedFigure = world.GetHistoricalFigure(Convert.ToInt32(subProperty.Value));
                                    break;
                            }
                        }
                        property.Known = true;
                        break;
                }
            }

            if (Artifact != null && HistoricalFigure != null)
            {
                Artifact.Creator = HistoricalFigure;
            }
            Artifact.AddEvent(this);
            HistoricalFigure.AddEvent(this);
            Site.AddEvent(this);
            SanctifyFigure.AddEvent(this);
            DefeatedFigure.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + Artifact.ToLink(link, pov, this);
            if (ReceivedName)
            {
                eventString += " received its name";
            }
            else
            {
                eventString += " was created";
            }

            if (Site != null)
            {
                eventString += " in " + Site.ToLink(link, pov, this);
            }

            if (ReceivedName)
            {
                eventString += " from ";
            }
            else
            {
                eventString += " by ";
            }

            eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
            if (SanctifyFigure != null)
            {
                eventString += " in order to sanctify ";
                eventString += SanctifyFigure.ToLink(link, pov, this);
                eventString += " by preserving a part of the body";
            }

            if (DefeatedFigure != null)
            {
                eventString += " after defeating ";
                eventString += DefeatedFigure.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
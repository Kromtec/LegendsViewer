using System;
using System.Collections.Generic;
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

        public ArtifactCreated(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "artifact_id": Artifact = world.GetArtifact(Convert.ToInt32(property.Value)); break;
                    case "hist_figure_id": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "name_only": ReceivedName = true; property.Known = true; break;
                    case "hfid": if (HistoricalFigure == null) { HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "unit_id":
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
                        if (property.Value != "-1")
                        {
                            SanctifyFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        }
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
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + Artifact.ToLink(link, pov);
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
                eventString += " in " + Site.ToLink(link, pov);
            }

            if (ReceivedName)
            {
                eventString += " from ";
            }
            else
            {
                eventString += " by ";
            }

            eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov) : "UNKNOWN HISTORICAL FIGURE";
            if (SanctifyFigure != null)
            {
                eventString += " in order to sanctify ";
                eventString += SanctifyFigure.ToLink(link, pov);
                eventString += " by preserving a part of the body";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
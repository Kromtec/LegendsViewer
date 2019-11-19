using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class ArtifactTransformed : WorldEvent
    {
        public int UnitId { get; set; }
        public Artifact NewArtifact { get; set; }
        public Artifact OldArtifact { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public Site Site { get; set; }

        public ArtifactTransformed(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "unit_id": UnitId = Convert.ToInt32(property.Value); break;
                    case "new_artifact_id": NewArtifact = world.GetArtifact(Convert.ToInt32(property.Value)); break;
                    case "old_artifact_id": OldArtifact = world.GetArtifact(Convert.ToInt32(property.Value)); break;
                    case "hist_figure_id": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                }
            }

            NewArtifact.AddEvent(this);
            OldArtifact.AddEvent(this);
            HistoricalFigure.AddEvent(this);
            Site.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += NewArtifact.ToLink(link, pov);
            eventString += ", ";
            if (!string.IsNullOrWhiteSpace(NewArtifact.Material))
            {
                eventString += NewArtifact.Material;
            }
            if (!string.IsNullOrWhiteSpace(NewArtifact.SubType))
            {
                eventString += " ";
                eventString += NewArtifact.SubType;
            }
            else
            {
                eventString += " ";
                eventString += !string.IsNullOrWhiteSpace(NewArtifact.Type) ? NewArtifact.Type.ToLower() : "UNKNOWN TYPE";
            }
            eventString += ", was made from ";
            eventString += OldArtifact.ToLink(link, pov);
            eventString += ", ";
            if (!string.IsNullOrWhiteSpace(OldArtifact.Material))
            {
                eventString += OldArtifact.Material;
            }
            if (!string.IsNullOrWhiteSpace(OldArtifact.SubType))
            {
                eventString += " ";
                eventString += OldArtifact.SubType;
            }
            else
            {
                eventString += " ";
                eventString += !string.IsNullOrWhiteSpace(OldArtifact.Type) ? OldArtifact.Type.ToLower() : "UNKNOWN TYPE";
            }
            if (Site != null)
            {
                eventString += " in " + Site.ToLink(link, pov);
            }

            eventString += " by ";
            eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov) : "UNKNOWN HISTORICAL FIGURE";
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
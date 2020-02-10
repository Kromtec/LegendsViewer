using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class ArtifactDestroyed : WorldEvent
    {
        public Artifact Artifact { get; set; }
        public Site Site { get; set; }
        public HistoricalFigure Destroyer { get; set; }

        public ArtifactDestroyed(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "artifact_id": Artifact = world.GetArtifact(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "destroyer_enid": Destroyer = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                }
            }

            Site.AddEvent(this);
            Artifact.AddEvent(this);
            Destroyer.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Artifact.ToLink(link, pov, this);
            eventString += " was destroyed";
            if (Destroyer != null)
            {
                eventString += " by ";
                eventString += Destroyer.ToLink(link, pov, this);
            }
            eventString += " in ";
            eventString += Site.ToLink(link, pov, this);
            eventString += ".";
            return eventString;
        }
    }
}
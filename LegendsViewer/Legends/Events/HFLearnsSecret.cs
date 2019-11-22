using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfLearnsSecret : WorldEvent
    {
        public HistoricalFigure Student { get; set; }
        public HistoricalFigure Teacher { get; set; }
        public Artifact Artifact { get; set; }
        public string Interaction { get; set; }
        public string SecretText { get; set; }

        public HfLearnsSecret(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "student_hfid": Student = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "teacher_hfid": Teacher = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "artifact_id": Artifact = world.GetArtifact(Convert.ToInt32(property.Value)); break;
                    case "interaction": Interaction = property.Value; break;
                    case "student": if (Student == null) { Student = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "teacher": if (Teacher == null) { Teacher = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "artifact": if (Artifact == null) { Artifact = world.GetArtifact(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "secret_text": SecretText = property.Value.Replace("[IS_NAME:", "").Replace("]", ""); break;
                }
            }

            Student.AddEvent(this);
            Teacher.AddEvent(this);
            Artifact.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();

            if (Teacher != null)
            {
                eventString += Teacher.ToLink(link, pov, this);
                eventString += " taught ";
                eventString += Student != null ? Student.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                eventString += " ";
                eventString += !string.IsNullOrWhiteSpace(SecretText) ? SecretText : "(" + Interaction + ")";
            }
            else
            {
                eventString += Student != null ? Student.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                eventString += " learned ";
                eventString += !string.IsNullOrWhiteSpace(SecretText) ? SecretText : "(" + Interaction + ")";
                eventString += " from ";
                eventString += Artifact != null ? Artifact.ToLink(link, pov, this) : "UNKNOWN ARTIFACT";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.Events
{
    public class ChangedCreatureType : WorldEvent
    {
        public HistoricalFigure Changee { get; set; }
        public HistoricalFigure Changer { get; set; }
        public string OldRace { get; set; }
        public string NewRace { get; set; }

        // TODO Handle caste changes
        public string OldCaste { get; set; }
        public string NewCaste { get; set; }

        public ChangedCreatureType(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "old_race": OldRace = Formatting.FormatRace(property.Value); break;
                    case "old_caste": OldCaste = property.Value; break;
                    case "new_race": NewRace = Formatting.FormatRace(property.Value); break;
                    case "new_caste": NewCaste = property.Value; break;
                    case "changee_hfid": Changee = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "changer_hfid": Changer = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "changee": if (Changee == null) { Changee = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "changer": if (Changer == null) { Changer = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                }
            }

            Changee.PreviousRace = OldRace;
            Changee.AddEvent(this);
            Changer.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Changer?.ToLink(link, pov) ?? "An unknown creature";
            eventString += " changed ";
            eventString += Changee?.ToLink(link, pov) ?? "an unknown creature";
            eventString += " from ";
            eventString += Formatting.AddArticle(OldRace).ToLower();
            eventString += " into ";
            eventString += Formatting.AddArticle(NewRace).ToLower();
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class Schedule : WorldObject
    {
        public List<Feature> Features { get; set; } // legends_plus.xml
        public ScheduleType Type { get; set; } // legends_plus.xml
        public int Reference { get; set; }
        public int Reference2 { get; set; }
        public string ItemType { get; set; }
        public string ItemSubType { get; set; }

        public static List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }

        public Schedule(List<Property> properties, World world)
            : base(properties, world)
        {
            Features = new List<Feature>();
            Reference = -1;
            Reference2 = -1;

            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "type":
                        switch (property.Value)
                        {
                            case "procession": Type = ScheduleType.Procession; break;
                            case "ceremony": Type = ScheduleType.Ceremony; break;
                            case "foot_race": Type = ScheduleType.FootRace; break;
                            case "throwing_competition": Type = ScheduleType.ThrowingCompetition; break;
                            case "dance_performance": Type = ScheduleType.DancePerformance; break;
                            case "storytelling": Type = ScheduleType.Storytelling; break;
                            case "poetry_recital": Type = ScheduleType.PoetryRecital; break;
                            case "musical_performance": Type = ScheduleType.MusicalPerformance; break;
                            case "wrestling_competition": Type = ScheduleType.WrestlingCompetition; break;
                            case "gladiatory_competition": Type = ScheduleType.GladiatoryCompetition; break;
                            case "poetry_competition": Type = ScheduleType.PoetryCompetition; break;
                            case "dance_competition": Type = ScheduleType.DanceCompetition; break;
                            case "musical_competition": Type = ScheduleType.MusicalCompetition; break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "feature":
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            Features.Add(new Feature(property.SubProperties, world));
                        }

                        break;
                    case "reference": Reference = Convert.ToInt32(property.Value); break;
                    case "reference2": Reference2 = Convert.ToInt32(property.Value); break;
                    case "item_type": ItemType = string.Intern(property.Value); break;
                    case "item_subtype": ItemSubType = string.Intern(property.Value); break;
                }
            }
        }

        public override string ToString() { return Type.GetDescription(); }

        public override string ToLink(bool link = true, DwarfObject pov = null)
        {
            return Type.GetDescription();
        }
    }
}

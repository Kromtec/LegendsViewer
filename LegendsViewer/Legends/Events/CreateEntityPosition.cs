using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class CreateEntityPosition : WorldEvent
    {
        public HistoricalFigure HistoricalFigure { get; set; }
        public Entity Civ { get; set; }
        public Entity SiteCiv { get; set; }
        public string Position { get; set; }
        public ReasonForCreatingEntity Reason { get; set; }

        public CreateEntityPosition(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "histfig": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "civ": Civ = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_civ": SiteCiv = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "position": Position = property.Value; break;
                    case "reason":
                        switch (property.Value)
                        {
                            case "0":
                            case "force_of_argument":
                                Reason = ReasonForCreatingEntity.ForceOfArgument;
                                break;
                            case "1":
                            case "threat_of_violence":
                                Reason = ReasonForCreatingEntity.ThreatOfViolence;
                                break;
                            case "2":
                            case "collaboration":
                                Reason = ReasonForCreatingEntity.Collaboration;
                                break;
                            case "3":
                            case "wave_of_popular_support":
                                Reason = ReasonForCreatingEntity.WaveOfPopularSupport;
                                break;
                            case "4":
                            case "as_a_matter_of_course":
                                Reason = ReasonForCreatingEntity.AsAMatterOfCourse;
                                break;
                        }
                        break;
                }
            }
            HistoricalFigure.AddEvent(this);
            Civ.AddEvent(this);
            if (SiteCiv != Civ)
            {
                SiteCiv.AddEvent(this);
            }
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            switch (Reason)
            {
                case ReasonForCreatingEntity.ForceOfArgument:
                    eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " of ";
                    eventString += Civ != null ? Civ.ToLink(link, pov, this) : "UNKNOWN CIV";
                    eventString += " created the position of ";
                    eventString += !string.IsNullOrWhiteSpace(Position) ? Position : "UNKNOWN POSITION";
                    eventString += " through force of argument";
                    break;
                case ReasonForCreatingEntity.ThreatOfViolence:
                    eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " of ";
                    eventString += Civ != null ? Civ.ToLink(link, pov, this) : "UNKNOWN CIV";
                    eventString += " compelled the creation of the position of ";
                    eventString += !string.IsNullOrWhiteSpace(Position) ? Position : "UNKNOWN POSITION";
                    eventString += " with threats of violence";
                    break;
                case ReasonForCreatingEntity.Collaboration:
                    eventString += SiteCiv != null ? SiteCiv.ToLink(link, pov, this) : "UNKNOWN ENTITY";
                    eventString += " collaborated to create the position of ";
                    eventString += !string.IsNullOrWhiteSpace(Position) ? Position : "UNKNOWN POSITION";
                    break;
                case ReasonForCreatingEntity.WaveOfPopularSupport:
                    eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " of ";
                    eventString += Civ != null ? Civ.ToLink(link, pov, this) : "UNKNOWN CIV";
                    eventString += " created the position of ";
                    eventString += !string.IsNullOrWhiteSpace(Position) ? Position : "UNKNOWN POSITION";
                    eventString += ", pushed by a wave of popular support";
                    break;
                case ReasonForCreatingEntity.AsAMatterOfCourse:
                    eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " of ";
                    eventString += Civ != null ? Civ.ToLink(link, pov, this) : "UNKNOWN CIV";
                    eventString += " created the position of ";
                    eventString += !string.IsNullOrWhiteSpace(Position) ? Position : "UNKNOWN POSITION";
                    eventString += " as a matter of course";
                    break;
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
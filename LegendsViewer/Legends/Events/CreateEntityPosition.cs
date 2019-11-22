using System;
using System.Collections.Generic;
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
        public int Reason { get; set; } // TODO // legends_plus.xml

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
                    case "reason": Reason = Convert.ToInt32(property.Value); break;
                }
            }
            HistoricalFigure.AddEvent(this);
            Civ.AddEvent(this);
            SiteCiv.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            switch (Reason)
            {
                case 0:
                    eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " of ";
                    eventString += Civ != null ? Civ.ToLink(link, pov, this) : "UNKNOWN CIV";
                    eventString += " created the position of ";
                    eventString += !string.IsNullOrWhiteSpace(Position) ? Position : "UNKNOWN POSITION";
                    eventString += " through force of argument";
                    break;
                case 1:
                    eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " of ";
                    eventString += Civ != null ? Civ.ToLink(link, pov, this) : "UNKNOWN CIV";
                    eventString += " compelled the creation of the position of ";
                    eventString += !string.IsNullOrWhiteSpace(Position) ? Position : "UNKNOWN POSITION";
                    eventString += " with threats of violence";
                    break;
                case 2:
                    eventString += SiteCiv != null ? SiteCiv.ToLink(link, pov, this) : "UNKNOWN ENTITY";
                    eventString += " collaborated to create the position of ";
                    eventString += !string.IsNullOrWhiteSpace(Position) ? Position : "UNKNOWN POSITION";
                    break;
                case 3:
                    eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                    eventString += " of ";
                    eventString += Civ != null ? Civ.ToLink(link, pov, this) : "UNKNOWN CIV";
                    eventString += " created the position of ";
                    eventString += !string.IsNullOrWhiteSpace(Position) ? Position : "UNKNOWN POSITION";
                    eventString += ", pushed by a wave of popular support";
                    break;
                case 4:
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
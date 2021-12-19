using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class MasterpieceLost : WorldEvent
    {
        public HistoricalFigure HistoricalFigure { get; set; }
        public Site Site { get; set; }
        public string Method { get; set; } // TODO destroy method
        public MasterpieceItem CreationEvent { get; set; }

        public MasterpieceLost(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "histfig": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "creation_event": CreationEvent = world.GetEvent(Convert.ToInt32(property.Value)) as MasterpieceItem; break;
                    case "method":
                        Method = property.Value;
                        break;
                }
            }
            HistoricalFigure.AddEvent(this);
            Site.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += "the masterful ";
            if (CreationEvent != null)
            {
                eventString += !string.IsNullOrWhiteSpace(CreationEvent.Material) ? CreationEvent.Material + " " : "";
                if (!string.IsNullOrWhiteSpace(CreationEvent.ItemSubType) && CreationEvent.ItemSubType != "-1")
                {
                    eventString += CreationEvent.ItemSubType;
                }
                else
                {
                    eventString += !string.IsNullOrWhiteSpace(CreationEvent.ItemType) ? CreationEvent.ItemType : "UNKNOWN ITEM";
                }
                eventString += " created by ";
                eventString += CreationEvent.Maker != null ? CreationEvent.Maker.ToLink(link, pov, this) : "UNKNOWN HISTORICAL FIGURE";
                eventString += " for ";
                eventString += CreationEvent.MakerEntity != null ? CreationEvent.MakerEntity.ToLink(link, pov, this) : "UNKNOWN ENTITY";
                eventString += " at ";
                eventString += CreationEvent.Site != null ? CreationEvent.Site.ToLink(link, pov, this) : "UNKNOWN SITE";
                eventString += " ";
                eventString += CreationEvent.GetYearTime();
            }
            else
            {
                eventString += "UNKNOWN ITEM";
            }
            eventString += " was destroyed by ";
            eventString += HistoricalFigure != null ? HistoricalFigure.ToLink(link, pov, this) : "an unknown creature";
            eventString += " in ";
            eventString += Site != null ? Site.ToLink(link, pov, this) : "UNKNOWN SITE";
            eventString += ".";
            return eventString;
        }
    }
}
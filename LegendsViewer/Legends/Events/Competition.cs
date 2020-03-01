using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class Competition : OccasionEvent
    {
        private HistoricalFigure Winner { get; set; }
        private List<HistoricalFigure> Competitors { get; set; }

        public Competition(List<Property> properties, World world) : base(properties, world)
        {
            OccasionType = OccasionType.Competition;
            Competitors = new List<HistoricalFigure>();
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "winner_hfid":
                        Winner = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "competitor_hfid":
                        Competitors.Add(world.GetHistoricalFigure(Convert.ToInt32(property.Value)));
                        break;
                }
            }

            Winner.AddEvent(this);
            Competitors.ForEach(competitor =>
            {
                if (competitor != Winner && competitor != HistoricalFigure.Unknown)
                {
                    competitor.AddEvent(this);
                }
            });
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = base.Print(link, pov);
            if (Competitors.Any())
            {
                eventString += "</br>";
                eventString += "Competing were ";
                for (int i = 0; i < Competitors.Count; i++)
                {
                    HistoricalFigure competitor = Competitors.ElementAt(i);
                    if (i == 0)
                    {
                        eventString += competitor.ToLink(link, pov, this);
                    }
                    else if (i == Competitors.Count - 1)
                    {
                        eventString += " and " + competitor.ToLink(link, pov, this);
                    }
                    else
                    {
                        eventString += ", " + competitor.ToLink(link, pov, this);
                    }
                }
                eventString += ". ";
            }
            if (Winner != null)
            {
                eventString += "The winner was ";
                eventString += Winner.ToLink(link, pov, this);
                eventString += ".";
            }
            return eventString;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfConvicted : WorldEvent
    {
        public HistoricalFigure ConvictedHf { get; set; }
        public Entity ConvicterEntity { get; set; }
        public string Crime { get; set; }
        public HistoricalFigure FooledHf { get; set; }
        public HistoricalFigure FramerHf { get; set; }
        public int PrisonMonth { get; set; }
        public bool DeathPenalty { get; set; }
        public bool WrongfulConviction { get; set; }
        public HistoricalFigure CorruptConvictorHf { get; set; }
        public HistoricalFigure PlotterHf { get; set; }
        public bool Exiled { get; set; }

        public HfConvicted(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "convicted_hfid": ConvictedHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "convicter_enid": ConvicterEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "crime": Crime = property.Value; break;
                    case "prison_months": PrisonMonth = Convert.ToInt32(property.Value); break;
                    case "fooled_hfid": FooledHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "framer_hfid": FramerHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "death_penalty": property.Known = true; DeathPenalty = true; break;
                    case "wrongful_conviction": property.Known = true; WrongfulConviction = true; break;
                    case "corrupt_convicter_hfid": CorruptConvictorHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "plotter_hfid": PlotterHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "exiled": property.Known = true; Exiled = true; break;
                }
            }
            ConvictedHf.AddEvent(this);
            ConvicterEntity.AddEvent(this);
            FooledHf.AddEvent(this);
            FramerHf.AddEvent(this);
            CorruptConvictorHf.AddEvent(this);
            PlotterHf.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += ConvictedHf.ToLink(link, pov, this);
            eventString += $" was {(WrongfulConviction ? "wrongfully " : "")}convicted of {Crime} by ";
            eventString += ConvicterEntity.ToLink(link, pov, this);
            if (CorruptConvictorHf != null)
            {
                eventString += " and ";
                eventString += CorruptConvictorHf.ToLink(link, pov, this);
            }
            if (PlotterHf != null && PlotterHf != CorruptConvictorHf)
            {
                eventString += " plotted by ";
                eventString += PlotterHf.ToLink(link, pov, this);
            }
            if (FooledHf != null && FramerHf != null)
            {
                eventString += " after ";
                eventString += FramerHf.ToLink(link, pov, this);
                eventString += " fooled ";
                eventString += FooledHf.ToLink(link, pov, this);
                eventString += " with fabricated evidence";
            }
            if (PrisonMonth > 0)
            {
                eventString += $" and imprisoned for a term of {(PrisonMonth > 12 ? (PrisonMonth / 12) + " years" : PrisonMonth + " month")}";
            }
            else if (DeathPenalty)
            {
                eventString += $" and sentenced to death";
            }
            if (Exiled)
            {
                eventString += " and exiled";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";

            return eventString;
        }
    }
}

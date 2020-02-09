using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class FailedFrameAttempt : WorldEvent
    {
        public HistoricalFigure TargetHf { get; set; }
        public Entity ConvicterEntity { get; set; }
        public HistoricalFigure FooledHf { get; set; }
        public HistoricalFigure FramerHf { get; set; }
        public HistoricalFigure PlotterHf { get; set; }
        public string Crime { get; set; }

        public FailedFrameAttempt(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "target_hfid": TargetHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "convicter_enid": ConvicterEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "fooled_hfid": FooledHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "framer_hfid": FramerHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "plotter_hfid": PlotterHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "crime": Crime = property.Value; break;
                }
            }

            TargetHf.AddEvent(this);
            ConvicterEntity.AddEvent(this);
            if (FooledHf != TargetHf)
            {
                FooledHf.AddEvent(this);
            }
            if (FramerHf != FooledHf)
            {
                FramerHf.AddEvent(this);
            }
            PlotterHf.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += FramerHf.ToLink(link, pov, this);
            eventString += " attempted to frame ";
            eventString += TargetHf.ToLink(link, pov, this);
            eventString += $" for {Crime}";
            if (PlotterHf != null)
            {
                eventString += " at the behest of ";
                eventString += PlotterHf.ToLink(link, pov, this);
            }
            eventString += " by fooling ";
            eventString += FooledHf.ToLink(link, pov, this);
            if (ConvicterEntity != null)
            {
                eventString += " and ";
                eventString += ConvicterEntity.ToLink(link, pov, this);
            }
            eventString += " with fabricated evidence, but nothing came from it";
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

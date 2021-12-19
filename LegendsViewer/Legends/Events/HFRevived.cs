using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfRevived : WorldEvent
    {
        private readonly string _ghost;
        public HistoricalFigure HistoricalFigure { get; set; }
        public HistoricalFigure Actor { get; set; }
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public bool RaisedBefore { get; set; }
        public bool Disturbance { get; set; }

        public HfRevived(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "ghost": _ghost = property.Value; break;
                    case "hfid": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "raised_before": RaisedBefore = true; property.Known = true; break;
                    case "actor_hfid": Actor = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "disturbance": Disturbance = true; property.Known = true; break;
                }
            }

            HistoricalFigure.AddEvent(this);
            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += HistoricalFigure.ToLink(link, pov, this);
            if (Disturbance)
            {
                eventString += " was disturbed from eternal rest";
            }
            else
            {
                eventString += Actor != null ? " was brought" : " came";
                eventString += " back from the dead";
            }

            if (RaisedBefore)
            {
                eventString += " once more";
            }

            if (Actor != null)
            {
                eventString += " by ";
                eventString += Actor.ToLink(link, pov, this);
            }

            if (!string.IsNullOrWhiteSpace(_ghost))
            {
                if (RaisedBefore)
                {
                    eventString += ", this time";
                }
                eventString += " as a " + _ghost;
            }

            if (Site != null)
            {
                eventString += " in ";
                eventString += Site.ToLink(link, pov, this);
            }
            else if (Region != null)
            {
                eventString += " in ";
                eventString += Region.ToLink(link, pov, this);
            }
            else if (UndergroundRegion != null)
            {
                eventString += " in ";
                eventString += UndergroundRegion.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
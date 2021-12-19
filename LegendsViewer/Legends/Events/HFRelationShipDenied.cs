using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfRelationShipDenied : WorldEvent
    {
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public HistoricalFigure Seeker { get; set; }
        public HistoricalFigure Target { get; set; }
        public string Relationship { get; set; }
        public string Reason { get; set; }
        public HistoricalFigure ReasonHf { get; set; }

        public HfRelationShipDenied(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "site_id":
                        Site = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                    case "subregion_id":
                        Region = world.GetRegion(Convert.ToInt32(property.Value));
                        break;
                    case "feature_layer_id":
                        UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value));
                        break;
                    case "seeker_hfid":
                        Seeker = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "target_hfid":
                        Target = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "relationship":
                        Relationship = property.Value;
                        break;
                    case "reason":
                        Reason = property.Value;
                        break;
                    case "reason_id":
                        ReasonHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                }
            }

            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
            Seeker.AddEvent(this);
            Target.AddEvent(this);
            if (ReasonHf?.Equals(Seeker) == false && !ReasonHf.Equals(Target))
            {
                ReasonHf.AddEvent(this);
            }
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Seeker.ToLink(link, pov, this);
            eventString += " was denied ";
            switch (Relationship)
            {
                case "apprentice":
                    eventString += "an apprenticeship under ";
                    break;
                default:
                    break;
            }
            eventString += Target.ToLink(link, pov, this);
            if (Site != null)
            {
                eventString += " in ";
                eventString += Site.ToLink(link, pov, this);
            }
            if (ReasonHf != null)
            {
                switch (Reason)
                {
                    case "jealousy":
                        eventString += " due to jealousy of " + ReasonHf.ToLink(link, pov, this);
                        break;
                    case "prefers working alone":
                        eventString += " as " + ReasonHf.ToLink(link, pov, this) + " prefers to work alone";
                        break;
                    default:
                        break;
                }
            }
            eventString += ".";
            return eventString;
        }
    }
}
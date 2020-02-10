using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class WrittenContentComposed : WorldEvent
    {
        public Entity Civ { get; set; }
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public string WrittenContentId { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public string Reason { get; set; }
        public int ReasonId { get; set; }
        public HistoricalFigure GlorifiedHf { get; set; }
        public HistoricalFigure CircumstanceHf { get; set; }
        public string Circumstance { get; set; }
        public int CircumstanceId { get; set; }
        public WrittenContent WrittenContent { get; set; }

        public WrittenContentComposed(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "civ_id":
                        Civ = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "site_id":
                        Site = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                    case "hist_figure_id":
                        HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "wc_id":
                        WrittenContentId = property.Value;
                        break;
                    case "reason":
                        Reason = property.Value;
                        break;
                    case "reason_id":
                        ReasonId = Convert.ToInt32(property.Value);
                        break;
                    case "circumstance":
                        Circumstance = property.Value;
                        break;
                    case "circumstance_id":
                        CircumstanceId = Convert.ToInt32(property.Value);
                        break;
                    case "subregion_id":
                        Region = world.GetRegion(Convert.ToInt32(property.Value));
                        break;
                }
            }

            Civ.AddEvent(this);
            Site.AddEvent(this);
            Region.AddEvent(this);
            HistoricalFigure.AddEvent(this);
            if (Reason == "glorify hf")
            {
                GlorifiedHf = world.GetHistoricalFigure(ReasonId);
                GlorifiedHf.AddEvent(this);
            }
            if (Circumstance == "pray to hf" || Circumstance == "dream about hf")
            {
                CircumstanceHf = world.GetHistoricalFigure(CircumstanceId);
                if (GlorifiedHf != null && GlorifiedHf != CircumstanceHf)
                {
                    CircumstanceHf.AddEvent(this);
                }
            }
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += WrittenContent != null ? WrittenContent.ToLink(link, pov, this) : "UNKNOWN WRITTEN CONTENT";
            eventString += " was authored by ";
            eventString += HistoricalFigure.ToLink(link, pov, this);
            if (Site != null)
            {
                eventString += " in ";
                eventString += Site.ToLink(link, pov, this);
            }
            if (GlorifiedHf != null)
            {
                eventString += " in order to glorify " + GlorifiedHf.ToLink(link, pov, this);
            }
            if (!string.IsNullOrWhiteSpace(Circumstance))
            {
                if (CircumstanceHf != null)
                {
                    switch (Circumstance)
                    {
                        case "pray to hf":
                            eventString += " after praying to " + CircumstanceHf.ToLink(link, pov, this);
                            break;
                        case "dream about hf":
                            eventString += " after dreaming of " + CircumstanceHf.ToLink(link, pov, this);
                            break;
                    }
                }
                else
                {
                    eventString += " after a " + Circumstance;
                }
            }
            eventString += ".";
            return eventString;
        }
    }
}
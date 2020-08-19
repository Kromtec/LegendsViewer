using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class FormCreatedEvent : WorldEvent
    {
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public string FormId { get; set; }
        public string Reason { get; set; }
        public int ReasonId { get; set; }
        public HistoricalFigure GlorifiedHf { get; set; }
        public HistoricalFigure PrayToHf { get; set; }
        public string Circumstance { get; set; }
        public int CircumstanceId { get; set; }
        public FormType FormType { get; set; }
        public ArtForm ArtForm { get; set; }

        public FormCreatedEvent(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "hist_figure_id":
                        HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "site_id":
                        Site = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                    case "form_id":
                        FormId = property.Value;
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

            Site.AddEvent(this);
            Region.AddEvent(this);
            HistoricalFigure.AddEvent(this);
            if (Reason == "glorify hf")
            {
                GlorifiedHf = world.GetHistoricalFigure(ReasonId);
                if (GlorifiedHf != HistoricalFigure)
                {
                    GlorifiedHf.AddEvent(this);
                }
            }
            if (Circumstance == "pray to hf")
            {
                PrayToHf = world.GetHistoricalFigure(CircumstanceId);
                if (PrayToHf != GlorifiedHf)
                {
                    PrayToHf.AddEvent(this);
                }
            }
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            switch (FormType)
            {
                case FormType.Musical:
                    eventString += ArtForm != null ? ArtForm.ToLink(link, pov, this) : "UNKNOWN MUSICAL FORM ";
                    break;
                case FormType.Poetic:
                    eventString += ArtForm != null ? ArtForm.ToLink(link, pov, this) : "UNKNOWN POETIC FORM ";
                    break;
                case FormType.Dance:
                    eventString += ArtForm != null ? ArtForm.ToLink(link, pov, this) : "UNKNOWN DANCE FORM ";
                    break;
                default:
                    eventString += "UNKNOWN FORM ";
                    break;
            }
            eventString += " was created by ";
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
                if (PrayToHf != null)
                {
                    eventString += " after praying to " + PrayToHf.ToLink(link, pov, this);
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
using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfRansomed : WorldEvent
    {
        public HistoricalFigure RansomedHf { get; set; }
        public HistoricalFigure RansomerHf { get; set; }
        public Entity PayerEntity { get; set; }
        public Site MovedToSite { get; set; }

        public HfRansomed(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "ransomed_hfid": RansomedHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "ransomer_hfid": RansomerHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "payer_entity_id": PayerEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "moved_to_site_id": MovedToSite = world.GetSite(Convert.ToInt32(property.Value)); break;
                }
            }


            PayerEntity.AddEvent(this);
            RansomedHf.AddEvent(this);
            RansomerHf.AddEvent(this);
            MovedToSite.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += RansomerHf.ToLink(link, pov, this);
            eventString += " ransomed ";
            eventString += RansomedHf.ToLink(link, pov, this);
            if (PayerEntity != null)
            {
                eventString += " to ";
                eventString += PayerEntity.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ". ";
            if (MovedToSite != null)
            {
                eventString += RansomedHf.ToLink(link, pov, this).ToUpperFirstLetter();
                eventString += " was sent to ";
                eventString += MovedToSite.ToLink(link, pov, this);
            }
            return eventString;
        }
    }
}
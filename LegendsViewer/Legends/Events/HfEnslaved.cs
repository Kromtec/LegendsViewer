using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfEnslaved : WorldEvent
    {
        public HistoricalFigure EnslavedHf { get; set; }
        public HistoricalFigure SellerHf { get; set; }
        public HistoricalFigure PayerHf { get; set; }
        public Entity PayerEntity { get; set; }
        public Site MovedToSite { get; set; }

        public HfEnslaved(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "enslaved_hfid": EnslavedHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "seller_hfid": SellerHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "payer_hfid": PayerHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "payer_entity_id": PayerEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "moved_to_site_id": MovedToSite = world.GetSite(Convert.ToInt32(property.Value)); break;
                }
            }

            PayerHf.AddEvent(this);
            PayerEntity.AddEvent(this);
            EnslavedHf.AddEvent(this);
            SellerHf.AddEvent(this);
            MovedToSite.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += SellerHf.ToLink(link, pov, this);
            eventString += " sold ";
            eventString += EnslavedHf.ToLink(link, pov, this);
            if (PayerHf != null)
            {
                eventString += " to ";
                eventString += PayerHf.ToLink(link, pov, this);
                if (PayerEntity != null)
                {
                    eventString += " of ";
                    eventString += PayerEntity.ToLink(link, pov, this);
                }
            }
            else if (PayerEntity != null)
            {
                eventString += " to ";
                eventString += PayerEntity.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ". ";
            if (MovedToSite != null)
            {
                eventString += EnslavedHf.ToLink(link, pov, this).ToUpperFirstLetter();
                eventString += " was sent to ";
                eventString += MovedToSite.ToLink(link, pov, this);
            }
            return eventString;
        }
    }
}
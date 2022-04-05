using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class Trade : WorldEvent
    {
        public HistoricalFigure Trader { get; set; }
        public Entity TraderEntity { get; set; }
        public Site SourceSite { get; set; }
        public Site DestSite { get; set; }
        public int AccountShift { get; set; }

        // TODO find out what these properties do
        public int ProductionZoneId { get; set; }
        public int Allotment { get; set; }
        public int AllotmentIndex { get; set; }

        public Trade(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "trader_hfid": Trader = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "trader_entity_id": TraderEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "source_site_id": SourceSite = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "dest_site_id": DestSite = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "production_zone_id": ProductionZoneId = Convert.ToInt32(property.Value); break;
                    case "allotment": Allotment = Convert.ToInt32(property.Value); break;
                    case "allotment_index": AllotmentIndex = Convert.ToInt32(property.Value); break;
                    case "account_shift": AccountShift = Convert.ToInt32(property.Value); break;
                }
            }


            SourceSite.AddEvent(this);
            DestSite.AddEvent(this);
            Trader.AddEvent(this);
            TraderEntity.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Trader.ToLink(link, pov, this);
            if (TraderEntity != null)
            {
                eventString += " of ";
                eventString += TraderEntity.ToLink(link, pov, this);
            }
            // same ranges like in "gamble" event
            var balance = AccountShift;
            if (balance >= 5000)
            {
                eventString += " made a fortune";
            }
            else if (balance >= 1000)
            {
                eventString += " did well";
            }
            else if (balance <= -1000)
            {
                eventString += " did poorly";
            }
            else if (balance <= -5000)
            {
                eventString += " lost a fortune";
            }
            else
            {
                eventString += " broke even";
            }
            eventString += " trading goods";
            if (SourceSite != null)
            {
                eventString += " from ";
                eventString += SourceSite.ToLink(link, pov, this);
            }

            if (DestSite != null)
            {
                eventString += " to ";
                eventString += DestSite.ToLink(link, pov, this);
            }

            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

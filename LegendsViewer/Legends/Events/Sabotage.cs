using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class Sabotage : WorldEvent
    {
        public HistoricalFigure SaboteurHf { get; set; }
        public HistoricalFigure TargetHf { get; set; }
        public Site Site { get; set; }

        public Sabotage(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "saboteur_hfid":
                        SaboteurHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "target_hfid":
                        TargetHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "site_id":
                        Site = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                }
            }

            SaboteurHf.AddEvent(this);
            TargetHf.AddEvent(this);
            Site.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += SaboteurHf.ToLink(link, pov, this);
            eventString += " sabotaged the activities of ";
            eventString += TargetHf.ToLink(link, pov, this);
            if (Site != null)
            {
                eventString += " at ";
                eventString += Site.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

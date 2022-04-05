using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfPrayedInsideStructure : WorldEvent
    {
        public HistoricalFigure HistoricalFigure { get; set; }
        public Site Site { get; set; }
        public int StructureId { get; set; }
        public Structure Structure { get; set; }
        public string Action { get; set; }

        public HfPrayedInsideStructure(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "histfig":
                    case "hist_fig_id":
                        if (HistoricalFigure == null)
                        {
                            HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        }
                        else
                        {
                            property.Known = true;
                        }
                        break;
                    case "site":
                    case "site_id":
                        if (Site == null)
                        {
                            Site = world.GetSite(Convert.ToInt32(property.Value));
                        }
                        else
                        {
                            property.Known = true;
                        }
                        break;
                    case "structure":
                    case "structure_id":
                        StructureId = Convert.ToInt32(property.Value);
                        break;
                    case "action":
                        Action = property.Value;
                        break;
                }
            }

            if (Site != null)
            {
                Structure = Site.Structures.Find(structure => structure.Id == StructureId);
            }
            HistoricalFigure.AddEvent(this);
            Site.AddEvent(this);
            Structure.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += HistoricalFigure.ToLink(link, pov, this);
            eventString += " prayed";
            if (Structure != null)
            {
                eventString += " inside ";
                eventString += Structure.ToLink(link, pov, this);
            }
            if (Site != null)
            {
                eventString += " in ";
                eventString += Site.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

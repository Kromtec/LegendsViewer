using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfInterrogated : WorldEvent
    {
        public HistoricalFigure TargetHf { get; set; }
        public bool WantedAndRecognized { get; set; }
        public bool HeldFirmInInterrogation { get; set; }
        public HistoricalFigure InterrogatorHf { get; set; }
        public Entity ArrestingEntity { get; set; }

        public HfInterrogated(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "target_hfid": TargetHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "interrogator_hfid": InterrogatorHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "arresting_enid": ArrestingEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "held_firm_in_interrogation": property.Known = true; HeldFirmInInterrogation = true; break;
                    case "wanted_and_recognized": property.Known = true; WantedAndRecognized = true; break;
                }
            }

            TargetHf.AddEvent(this);
            InterrogatorHf.AddEvent(this);
            ArrestingEntity.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (WantedAndRecognized && HeldFirmInInterrogation)
            {
                eventString += TargetHf.ToLink(link, pov, this);
                eventString += " was recognized and arrested by ";
                eventString += ArrestingEntity.ToLink(link, pov, this);
                eventString += ". Despite the interrogation by ";
                eventString += InterrogatorHf.ToLink(link, pov, this);
                eventString += ", ";
                eventString += TargetHf.ToLink(link, pov, this);
                eventString += " refused to reveal anything and was released";
            }
            else
            {
                eventString += TargetHf.ToLink(link, pov, this);
                eventString += " was interrogated";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
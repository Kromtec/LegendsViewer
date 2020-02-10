using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class EntityLaw : WorldEvent
    {
        public Entity Entity { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public EntityLawType Law { get; set; }
        public bool LawLaid { get; set; }
        private string _unknownLawType;

        public EntityLaw(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "entity_id": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "hist_figure_id": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "law_add":
                    case "law_remove":
                        switch (property.Value)
                        {
                            case "harsh": Law = EntityLawType.Harsh; break;
                            default:
                                Law = EntityLawType.Unknown;
                                _unknownLawType = property.Value;
                                world.ParsingErrors.Report("Unknown Law Type: " + _unknownLawType);
                                break;
                        }
                        LawLaid = property.Name == "law_add";
                        break;
                }
            }

            Entity.AddEvent(this);
            HistoricalFigure.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + HistoricalFigure.ToLink(link, pov, this);
            if (LawLaid)
            {
                eventString += " laid a series of ";
            }
            else
            {
                eventString += " lifted numerous ";
            }

            switch (Law)
            {
                case EntityLawType.Harsh: eventString += "oppressive"; break;
                case EntityLawType.Unknown: eventString += "(" + _unknownLawType + ")"; break;
            }
            if (LawLaid)
            {
                eventString += " edicts upon ";
            }
            else
            {
                eventString += " laws from ";
            }

            eventString += Entity.ToLink(link, pov, this);
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfWounded : WorldEvent
    {
        public int WoundeeRace { get; set; }
        public int WoundeeCaste { get; set; }

        // TODO
        public int BodyPart { get; set; } // legends_plus.xml
        public int InjuryType { get; set; } // legends_plus.xml
        public bool PartLost { get; set; } // legends_plus.xml

        public HistoricalFigure Woundee { get; set; }
        public HistoricalFigure Wounder { get; set; }
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public bool WasTorture { get; set; }


        public HfWounded(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "woundee_hfid": Woundee = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "wounder_hfid": Wounder = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "woundee": if (Woundee == null) { Woundee = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "wounder": if (Wounder == null) { Wounder = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "woundee_race": WoundeeRace = Convert.ToInt32(property.Value); break;
                    case "woundee_caste": WoundeeCaste = Convert.ToInt32(property.Value); break;
                    case "body_part": BodyPart = Convert.ToInt32(property.Value); break;
                    case "injury_type": InjuryType = Convert.ToInt32(property.Value); break;
                    case "part_lost":
                        if (int.TryParse(property.Value, out int partLost))
                        {
                            PartLost = partLost != 0;
                        }
                        else if(bool.TryParse(property.Value, out bool partLostBool))
                        {
                            PartLost = partLostBool;
                        }
                        else
                        {
                            property.Known = false;
                        }
                        break;
                    case "was_torture":
                        property.Known = true; 
                        WasTorture = true; 
                        break;
                }
            }

            Woundee.AddEvent(this);
            if (Woundee != Wounder)
            {
                Wounder.AddEvent(this);
            }
            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (Woundee != null)
            {
                eventString += Woundee.ToLink(link, pov, this);
            }
            else
            {
                eventString += "UNKNOWN HISTORICAL FIGURE";
            }

            eventString += " was wounded by ";
            if (Wounder != null)
            {
                eventString += Wounder.ToLink(link, pov, this);
            }
            else
            {
                eventString += "UNKNOWN HISTORICAL FIGURE";
            }

            if (Site != null)
            {
                eventString += " in " + Site.ToLink(link, pov, this);
            }
            else if (Region != null)
            {
                eventString += " in " + Region.ToLink(link, pov, this);
            }
            else if (UndergroundRegion != null)
            {
                eventString += " in " + UndergroundRegion.ToLink(link, pov, this);
            }

            if (WasTorture)
            {
                eventString += " as a means of torture";
            }

            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
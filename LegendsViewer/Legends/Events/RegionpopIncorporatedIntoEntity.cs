using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    //dwarf mode eventsList


    // new 0.42.XX events


    public class RegionpopIncorporatedIntoEntity : WorldEvent
    {
        public Site Site { get; set; }
        public Entity JoinEntity { get; set; }
        public string PopRace { get; set; }
        public int PopNumberMoved { get; set; }
        public WorldRegion PopSourceRegion { get; set; }
        public string PopFlId { get; set; }

        public RegionpopIncorporatedIntoEntity(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "join_entity_id":
                        JoinEntity = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "site_id":
                        Site = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                    case "pop_race":
                        PopRace = property.Value;
                        break;
                    case "pop_number_moved":
                        PopNumberMoved = Convert.ToInt32(property.Value);
                        break;
                    case "pop_srid":
                        PopSourceRegion = world.GetRegion(Convert.ToInt32(property.Value));
                        break;
                    case "pop_flid":
                        PopFlId = property.Value;
                        break;
                }
            }

            Site.AddEvent(this);
            JoinEntity.AddEvent(this);
            PopSourceRegion.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (PopNumberMoved > 200)
            {
                eventString += " hundreds of ";
            }
            else if (PopNumberMoved > 24)
            {
                eventString += " dozens of ";
            }
            else
            {
                eventString += " several ";
            }
            eventString += "UNKNOWN RACE";
            eventString += " from ";
            eventString += PopSourceRegion != null ? PopSourceRegion.ToLink(link, pov) : "UNKNOWN REGION";
            eventString += " joined with ";
            eventString += JoinEntity != null ? JoinEntity.ToLink(link, pov) : "UNKNOWN ENTITY";
            eventString += " at ";
            eventString += Site != null ? Site.ToLink(link, pov) : "UNKNOWN SITE";
            eventString += ".";
            return eventString;
        }
    }

}

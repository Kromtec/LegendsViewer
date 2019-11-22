using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class SiteTributeForced : WorldEvent
    {
        public Entity Attacker { get; set; }
        public Entity Defender { get; set; }
        public Entity SiteEntity { get; set; }
        public Site Site { get; set; }
        public string Season { get; set; }

        public SiteTributeForced(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "attacker_civ_id":
                        Attacker = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "defender_civ_id":
                        Defender = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "site_civ_id":
                        SiteEntity = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "site_id":
                        Site = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                    case "season":
                        Season = property.Value;
                        break;
                }
            }

            Attacker.AddEvent(this);
            Defender.AddEvent(this);
            SiteEntity.AddEvent(this);
            Site.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + Attacker.ToLink(link, pov, this) + " secured tribute from " + SiteEntity.ToLink(link, pov, this);
            if (Defender != null)
            {
                eventString += " of " + Defender.ToLink(link, pov, this);
            }
            eventString += ", to be delivered from " + Site.ToLink(link, pov, this);
            if (!string.IsNullOrWhiteSpace(Season))
            {
                eventString += " every " + Season.Trim();
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
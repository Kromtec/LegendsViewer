using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class SiteSurrendered : WorldEvent
    {
        public Entity Attacker { get; set; }
        public Entity Defender { get; set; }
        public Entity SiteEntity { get; set; }
        public Site Site { get; set; }

        public SiteSurrendered(List<Property> properties, World world) : base(properties, world)
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
                }
            }

            Attacker.AddEvent(this);
            Defender.AddEvent(this);
            if (SiteEntity != Defender)
            {
                SiteEntity.AddEvent(this);
            }
            Site.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += SiteEntity?.ToLink(link, pov, this);
            eventString += " of ";
            eventString += Defender?.ToLink(link, pov, this);
            eventString += " surrendered ";
            eventString += Site?.ToLink(link, pov, this);
            eventString += " to ";
            eventString += Attacker?.ToLink(link, pov, this);
            eventString += ".";
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

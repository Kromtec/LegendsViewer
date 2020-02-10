using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfAttackedSite : WorldEvent
    {
        public HistoricalFigure Attacker { get; set; }
        public Entity DefenderCiv { get; set; }
        public Entity SiteCiv { get; set; }
        public Site Site { get; set; }

        public HfAttackedSite(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "attacker_hfid":
                        Attacker = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        break;
                    case "defender_civ_id":
                        DefenderCiv = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "site_civ_id":
                        SiteCiv = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "site_id":
                        Site = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                }
            }

            Attacker.AddEvent(this);
            DefenderCiv.AddEvent(this);
            if (SiteCiv != DefenderCiv)
            {
                SiteCiv.AddEvent(this);
            }
            Site.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + Attacker.ToLink(link, pov, this) + " attacked " + SiteCiv.ToLink(link, pov, this);
            if (DefenderCiv != null && DefenderCiv != SiteCiv)
            {
                eventString += " of " + DefenderCiv.ToLink(link, pov, this);
            }
            eventString += " at " + Site.ToLink(link, pov, this);
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
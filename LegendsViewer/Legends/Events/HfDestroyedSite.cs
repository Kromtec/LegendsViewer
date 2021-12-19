using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfDestroyedSite : WorldEvent
    {
        public HistoricalFigure Attacker { get; set; }
        public Entity DefenderCiv { get; set; }
        public Entity SiteCiv { get; set; }
        public Site Site { get; set; }

        public HfDestroyedSite(List<Property> properties, World world) : base(properties, world)
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
            SiteCiv.AddEvent(this);
            Site.AddEvent(this);

            OwnerPeriod lastSiteOwnerPeriod = Site.OwnerHistory.LastOrDefault();
            if (lastSiteOwnerPeriod != null)
            {
                lastSiteOwnerPeriod.EndYear = Year;
                lastSiteOwnerPeriod.EndCause = "destroyed";
                lastSiteOwnerPeriod.Destroyer = Attacker;
            }
            OwnerPeriod lastDefenderCivOwnerPeriod = DefenderCiv?.SiteHistory.LastOrDefault(s => s.Site == Site);
            if (lastDefenderCivOwnerPeriod != null)
            {
                lastDefenderCivOwnerPeriod.EndYear = Year;
                lastDefenderCivOwnerPeriod.EndCause = "destroyed";
                lastDefenderCivOwnerPeriod.Destroyer = Attacker;
            }
            OwnerPeriod lastSiteCivOwnerPeriod = SiteCiv.SiteHistory.LastOrDefault(s => s.Site == Site);
            if (lastSiteCivOwnerPeriod != null)
            {
                lastSiteCivOwnerPeriod.EndYear = Year;
                lastSiteCivOwnerPeriod.EndCause = "destroyed";
                lastSiteCivOwnerPeriod.Destroyer = Attacker;
            }
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + Attacker.ToLink(link, pov, this) + " routed " + SiteCiv.ToLink(link, pov, this);
            if (DefenderCiv != null)
            {
                eventString += " of " + DefenderCiv.ToLink(link, pov, this);
            }
            eventString += " and destroyed " + Site.ToLink(link, pov, this);
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
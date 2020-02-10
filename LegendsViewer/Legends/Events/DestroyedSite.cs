using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class DestroyedSite : WorldEvent
    {
        public Site Site { get; set; }
        public Entity SiteEntity { get; set; }
        public Entity Attacker { get; set; }
        public Entity Defender { get; set; }
        public bool NoDefeatMention { get; set; }

        public DestroyedSite(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "site_civ_id": SiteEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "attacker_civ_id": Attacker = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "defender_civ_id": Defender = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "no_defeat_mention":
                        NoDefeatMention = true;
                        property.Known = true;
                        break;
                }
            }

            if (Site.OwnerHistory.Count == 0)
            {
                if (SiteEntity != null && SiteEntity != Defender)
                {
                    SiteEntity.Parent = Defender;
                    Site.OwnerHistory.Add(new OwnerPeriod(Site, SiteEntity, -1, "founded"));
                }
                else
                {
                    Site.OwnerHistory.Add(new OwnerPeriod(Site, Defender, -1, "founded"));
                }
            }

            Site.OwnerHistory.Last().EndCause = "destroyed";
            Site.OwnerHistory.Last().EndYear = Year;
            Site.OwnerHistory.Last().Ender = Attacker;

            Site.AddEvent(this);
            if (SiteEntity != Defender)
            {
                SiteEntity.AddEvent(this);
            }

            Attacker.AddEvent(this);
            Defender.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Attacker?.ToLink(link, pov, this) ?? "an unknown entity";
            if (!NoDefeatMention)
            {
                eventString += " defeated ";
                if (SiteEntity != null && SiteEntity != Defender)
                {
                    eventString += SiteEntity.ToLink(link, pov, this) + " of ";
                }

                eventString += Defender?.ToLink(link, pov, this) ?? "an unknown entity";
                eventString += " and";
            }
            eventString += " destroyed ";
            eventString += Site?.ToLink(link, pov, this) ?? "an unknown site";
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
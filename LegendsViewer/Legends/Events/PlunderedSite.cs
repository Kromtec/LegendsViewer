using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class PlunderedSite : WorldEvent
    {
        public readonly Entity Attacker;
        public readonly Entity Defender;
        public readonly Entity SiteEntity;
        public readonly Site Site;
        public bool Detected { get; set; }
        public bool NoDefeatMention { get; set; }
        public bool WasRaid { get; set; }
        public bool TookLiveStock { get; set; }
        public bool TookItems { get; set; }

        public PlunderedSite(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "attacker_civ_id": Attacker = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "defender_civ_id": Defender = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_civ_id": SiteEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "detected":
                        if (string.IsNullOrEmpty(property.Value))
                        {
                            property.Known = true;
                            Detected = true;
                        }
                        break;
                    case "no_defeat_mention":
                        if (string.IsNullOrEmpty(property.Value))
                        {
                            property.Known = true;
                            NoDefeatMention = true;
                        }
                        break;
                    case "was_raid":
                        if (string.IsNullOrEmpty(property.Value))
                        {
                            property.Known = true;
                            WasRaid = true;
                        }
                        break;
                    case "took_livestock":
                        if (string.IsNullOrEmpty(property.Value))
                        {
                            property.Known = true;
                            TookLiveStock = true;
                        }
                        break;
                    case "took_items":
                        if (string.IsNullOrEmpty(property.Value))
                        {
                            property.Known = true;
                            TookItems = true;
                        }
                        break;
                }
            }

            Attacker.AddEvent(this);
            Defender.AddEvent(this);
            if (Defender != SiteEntity)
            {
                SiteEntity.AddEvent(this);
            }

            Site.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Attacker.ToLink(link, pov, this);
            if (TookLiveStock || TookItems)
            {
                eventString += " stole ";
                if (TookLiveStock)
                {
                    eventString += "livestock ";
                }
                else if (TookItems)
                {
                    eventString += "treasure ";
                }

                if (SiteEntity != null || Defender != null)
                {
                    eventString += "from ";
                }
                if (SiteEntity != null)
                {
                    eventString += SiteEntity.ToLink(link, pov, this);
                    if (Defender != SiteEntity && Defender != null)
                    {
                        eventString += " of ";
                    }
                }
                if (Defender != null)
                {
                    eventString += Defender.ToLink(link, pov, this);
                }
                eventString += " in ";
                eventString += Site.ToLink(link, pov, this);
            }
            else
            {
                eventString += " defeated ";
                if (SiteEntity != null && Defender != SiteEntity)
                {
                    eventString += SiteEntity.ToLink(link, pov, this);
                }
                if (Defender != null)
                {
                    eventString += " of ";
                    eventString += Defender.ToLink(link, pov, this);
                }
                eventString += " and pillaged ";
                eventString += Site.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
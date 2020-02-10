using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class AttackedSite : WorldEvent
    {
        public Entity Attacker { get; set; }
        public Entity Defender { get; set; }
        public Entity SiteEntity { get; set; }
        public Entity AttackerMercenaries { get; set; }
        public Entity DefenderMercenaries { get; set; }
        public Entity AttackerSupportMercenaries { get; set; }
        public Entity DefenderSupportMercenaries { get; set; }
        public Site Site { get; set; }
        public HistoricalFigure AttackerGeneral { get; set; }
        public HistoricalFigure DefenderGeneral { get; set; }

        public AttackedSite(List<Property> properties, World world)
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
                    case "attacker_general_hfid": AttackerGeneral = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "defender_general_hfid": DefenderGeneral = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "attacker_merc_enid": AttackerMercenaries = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "defender_merc_enid": DefenderMercenaries = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "a_support_merc_enid": AttackerSupportMercenaries = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "d_support_merc_enid": DefenderSupportMercenaries = world.GetEntity(Convert.ToInt32(property.Value)); break;
                }
            }

            Attacker.AddEvent(this);
            Defender.AddEvent(this);
            if (SiteEntity != Defender)
            {
                SiteEntity.AddEvent(this);
            }
            Site.AddEvent(this);
            AttackerGeneral.AddEvent(this);
            DefenderGeneral.AddEvent(this);
            if (AttackerMercenaries != Defender && AttackerMercenaries != Attacker)
            {
                AttackerMercenaries.AddEvent(this);
            }
            if (DefenderMercenaries != Defender && DefenderMercenaries != Attacker)
            {
                DefenderMercenaries.AddEvent(this);
            }
            AttackerSupportMercenaries.AddEvent(this);
            DefenderSupportMercenaries.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Attacker?.PrintEntity(true, pov) ?? "an unknown civilization";
            eventString += " attacked ";
            if (SiteEntity != null)
            {
                eventString += SiteEntity.PrintEntity(true, pov);
            }
            else
            {
                eventString += Defender?.PrintEntity(true, pov) ?? "an unknown civilization";
            }
            eventString += " at " + Site.ToLink(link, pov, this) + ". ";
            if (AttackerGeneral != null)
            {
                eventString += "Leader of the attack was ";
                eventString += AttackerGeneral.ToLink(link, pov, this);
            }
            if (DefenderGeneral != null)
            {
                eventString += ", and the defenders were led by ";
                eventString += DefenderGeneral.ToLink(link, pov, this);
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            if (AttackerMercenaries != null)
            {
                eventString += " ";
                eventString += AttackerMercenaries.ToLink(true, pov);
                eventString += " were hired by the attackers.";
            }
            if (DefenderMercenaries != null)
            {
                eventString += " ";
                eventString += DefenderMercenaries.ToLink(true, pov);
                eventString += " were hired by the defenders.";
            }
            if (AttackerSupportMercenaries != null)
            {
                eventString += " ";
                eventString += AttackerSupportMercenaries.ToLink(true, pov);
                eventString += " were hired as scouts by the attackers.";
            }
            if (DefenderSupportMercenaries != null)
            {
                eventString += " ";
                eventString += DefenderSupportMercenaries.ToLink(true, pov);
                eventString += " were hired as scouts by the defenders.";
            }
            return eventString;
        }
    }
}
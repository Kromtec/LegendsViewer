using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class FieldBattle : WorldEvent
    {
        public Entity Attacker { get; set; }
        public Entity Defender { get; set; }
        public Entity AttackerMercenaries { get; set; }
        public Entity DefenderMercenaries { get; set; }
        public Entity AttackerSupportMercenaries { get; set; }
        public Entity DefenderSupportMercenaries { get; set; }
        public WorldRegion Region { get; set; }
        public HistoricalFigure AttackerGeneral { get; set; }
        public HistoricalFigure DefenderGeneral { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public Location Coordinates { get; set; }

        public FieldBattle(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                    case "attacker_civ_id": Attacker = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "defender_civ_id": Defender = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "attacker_general_hfid": AttackerGeneral = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "defender_general_hfid": DefenderGeneral = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "attacker_merc_enid": AttackerMercenaries = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "defender_merc_enid": DefenderMercenaries = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "a_support_merc_enid": AttackerSupportMercenaries = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "d_support_merc_enid": DefenderSupportMercenaries = world.GetEntity(Convert.ToInt32(property.Value)); break;
                }
            }

            Attacker.AddEvent(this);
            Defender.AddEvent(this);
            AttackerGeneral.AddEvent(this);
            DefenderGeneral.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
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
            eventString += Attacker.ToLink(true, pov);
            eventString += " attacked ";
            eventString += Defender.ToLink(true, pov);
            eventString += " in " + Region.ToLink(link, pov, this) + ". ";
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
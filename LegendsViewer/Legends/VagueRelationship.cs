using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends
{
    public class VagueRelationship
    {
        public int HfId { get; set; }
        public VagueRelationshipType Type { get; set; }

        public VagueRelationship(List<Property> properties)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "hfid":
                        HfId = Convert.ToInt32(property.Value);
                        break;
                    case "war_buddy":
                        property.Known = true;
                        Type = VagueRelationshipType.WarBuddy;
                        break;
                    case "athlete_buddy":
                        property.Known = true;
                        Type = VagueRelationshipType.AthleteBuddy;
                        break;
                    case "childhood_friend":
                        property.Known = true;
                        Type = VagueRelationshipType.ChildhoodFriend;
                        break;
                    case "persecution_grudge":
                        property.Known = true;
                        Type = VagueRelationshipType.PersecutionGrudge;
                        break;
                    case "supernatural_grudge":
                        property.Known = true;
                        Type = VagueRelationshipType.SupernaturalGrudge;
                        break;
                    case "religious_persecution_grudge":
                        property.Known = true;
                        Type = VagueRelationshipType.ReligiousPersecutionGrudge;
                        break;
                    case "artistic_buddy":
                        property.Known = true;
                        Type = VagueRelationshipType.ArtisticBuddy;
                        break;
                    case "jealous_obsession":
                        property.Known = true;
                        Type = VagueRelationshipType.JealousObsession;
                        break;
                    case "grudge":
                        property.Known = true;
                        Type = VagueRelationshipType.Grudge;
                        break;
                }
            }
        }
    }
}

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
                    case "athlete_buddy":
                    case "childhood_friend":
                    case "persecution_grudge":
                    case "supernatural_grudge":
                    case "religious_persecution_grudge":
                    case "artistic_buddy":
                    case "jealous_obsession":
                    case "grudge":
                    case "jealous_relationship_grudge":
                    case "scholar_buddy":
                    case "business_rival":
                    case "atheletic_rival":
                        Type = GetVagueRelationshipTypeByProperty(property, property.Name); 
                        break;
                }
            }
        }

        public static VagueRelationshipType GetVagueRelationshipTypeByProperty(Property property, string key)
        {
            VagueRelationshipType type = VagueRelationshipType.Unknown;
            switch (key)
            {
                case "war_buddy":
                    type = VagueRelationshipType.WarBuddy;
                    break;
                case "athlete_buddy":
                    type = VagueRelationshipType.AthleteBuddy;
                    break;
                case "childhood_friend":
                    type = VagueRelationshipType.ChildhoodFriend;
                    break;
                case "persecution_grudge":
                    type = VagueRelationshipType.PersecutionGrudge;
                    break;
                case "supernatural_grudge":
                    type = VagueRelationshipType.SupernaturalGrudge;
                    break;
                case "religious_persecution_grudge":
                    type = VagueRelationshipType.ReligiousPersecutionGrudge;
                    break;
                case "artistic_buddy":
                    type = VagueRelationshipType.ArtisticBuddy;
                    break;
                case "jealous_obsession":
                    type = VagueRelationshipType.JealousObsession;
                    break;
                case "grudge":
                    type = VagueRelationshipType.Grudge;
                    break;
                case "jealous_relationship_grudge":
                    type = VagueRelationshipType.JealousRelationshipGrudge;
                    break;
                case "scholar_buddy":
                    type = VagueRelationshipType.ScholarBuddy;
                    break;
                case "business_rival":
                    type = VagueRelationshipType.BusinessRival;
                    break;
                case "atheletic_rival":
                    type = VagueRelationshipType.AthleticRival;
                    break;
                case "lover":
                    type = VagueRelationshipType.Lover;
                    break;
                case "former_lover":
                    type = VagueRelationshipType.FormerLover;
                    break;
                case "lieutenant":
                    type = VagueRelationshipType.Lieutenant;
                    break;
                default:
                    property.Known = false;
                    break;
            }

            if (type != VagueRelationshipType.Unknown)
            {
                property.Known = true;
            }
            return type;
        }
    }
}

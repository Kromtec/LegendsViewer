using System;
using System.Collections.Generic;
using LegendsViewer.Controls;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends
{
    public class Reputation
    {
        public static readonly List<string> KnownReputationSubProperties = new List<string>
        {
            "entity_id",
            "unsolved_murders",
            "first_ageless_year",
            "first_ageless_season_count",
            "rep_enemy_fighter",
            "rep_trade_partner",
            "rep_killer",
            "rep_poet",
            "rep_bard",
            "rep_storyteller",
            "rep_dancer",
            "rep_loyal_soldier",
            "rep_hero",
            "rep_hunter",
            "rep_treasure_hunter",
            "rep_knowledge_preserver",
            "rep_protector_of_weak"
        };
        public ReputationType Type { get; set; }
        public int Strength { get; set; }

        public Reputation(Property property)
        {
            switch (property.Name)
            {
                case "rep_friendly": Type = ReputationType.Friendly; Strength = Convert.ToInt32(property.Value); break;
                case "rep_buddy": Type = ReputationType.Buddy; Strength = Convert.ToInt32(property.Value); break;
                case "rep_grudge": Type = ReputationType.Grudge; Strength = Convert.ToInt32(property.Value); break;
                case "rep_bonded": Type = ReputationType.Bonded; Strength = Convert.ToInt32(property.Value); break;
                case "rep_quarreler": Type = ReputationType.Quarreler; Strength = Convert.ToInt32(property.Value); break;
                case "rep_trade_partner": Type = ReputationType.TradePartner; Strength = Convert.ToInt32(property.Value); break;
                case "rep_psychopath": Type = ReputationType.Psychopath; Strength = Convert.ToInt32(property.Value); break;
                case "rep_storyteller": Type = ReputationType.Storyteller; Strength = Convert.ToInt32(property.Value); break;
                case "rep_loyal_soldier": Type = ReputationType.LoyalSoldier; Strength = Convert.ToInt32(property.Value); break;
                case "rep_bully": Type = ReputationType.Bully; Strength = Convert.ToInt32(property.Value); break;
                case "rep_information_source": Type = ReputationType.InformationSource; Strength = Convert.ToInt32(property.Value); break;
                case "love": Type = ReputationType.Love; Strength = Convert.ToInt32(property.Value); break;
                case "respect": Type = ReputationType.Respect; Strength = Convert.ToInt32(property.Value); break;
                case "trust": Type = ReputationType.Trust; Strength = Convert.ToInt32(property.Value); break;
                case "loyalty": Type = ReputationType.Loyalty; Strength = Convert.ToInt32(property.Value); break;
                case "fear": Type = ReputationType.Fear; Strength = Convert.ToInt32(property.Value); break;
            }
        }

        public string Print()
        {
            return Type.GetDescription() + " (" + Strength + ")";
        }
    }
}

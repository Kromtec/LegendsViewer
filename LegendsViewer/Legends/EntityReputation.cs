using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends
{
    public class EntityReputation
    {
        public Entity Entity { get; set; }
        public int UnsolvedMurders { get; set; }
        public int FirstSuspectedAgelessYear { get; set; }
        public string FirstSuspectedAgelessSeason { get; set; }
        public Dictionary<ReputationType, int> Reputations = new Dictionary<ReputationType, int>();

        public EntityReputation(List<Property> properties, World world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "entity_id": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "unsolved_murders": UnsolvedMurders = Convert.ToInt32(property.Value); break;
                    case "first_ageless_year": FirstSuspectedAgelessYear = Convert.ToInt32(property.Value); break;
                    case "first_ageless_season_count": FirstSuspectedAgelessSeason = Formatting.TimeCountToSeason(Convert.ToInt32(property.Value)); break;
                    case "rep_enemy_fighter": Reputations.Add(ReputationType.EnemyFighter, Convert.ToInt32(property.Value)); break;
                    case "rep_trade_partner": Reputations.Add(ReputationType.TradePartner, Convert.ToInt32(property.Value)); break;
                    case "rep_killer": Reputations.Add(ReputationType.Killer, Convert.ToInt32(property.Value)); break;
                    case "rep_poet": Reputations.Add(ReputationType.Poet, Convert.ToInt32(property.Value)); break;
                    case "rep_bard": Reputations.Add(ReputationType.Bard, Convert.ToInt32(property.Value)); break;
                    case "rep_storyteller": Reputations.Add(ReputationType.Storyteller, Convert.ToInt32(property.Value)); break;
                    case "rep_dancer": Reputations.Add(ReputationType.Dancer, Convert.ToInt32(property.Value)); break;
                    case "rep_hero": Reputations.Add(ReputationType.Hero, Convert.ToInt32(property.Value)); break;
                    case "rep_hunter": Reputations.Add(ReputationType.Hunter, Convert.ToInt32(property.Value)); break;
                    case "rep_treasure_hunter": Reputations.Add(ReputationType.TreasureHunter, Convert.ToInt32(property.Value)); break;
                    case "rep_knowledge_preserver": Reputations.Add(ReputationType.KnowledgePreserver, Convert.ToInt32(property.Value)); break;
                    case "rep_protector_of_weak": Reputations.Add(ReputationType.ProtectorOfWeak, Convert.ToInt32(property.Value)); break;
                }
            }
        }
    }
}

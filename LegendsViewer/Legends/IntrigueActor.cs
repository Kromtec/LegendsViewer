using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends
{
    public class IntrigueActor
    {
        public int LocalId { get; set; }
        public string Role { get; set; }
        public string Strategy { get; set; }
        public int EntityId { get; }
        public int HfId { get; set; }
        public int HandleActorId { get; set; }
        public int StrategyEnId { get; set; }
        public int StrategyEppId { get; set; }
        public bool PromisedActorImmortality { get; set; }
        public bool PromisedMeImmortality { get; set; }

        public IntrigueActor(List<Property> properties)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "local_id": LocalId = Convert.ToInt32(property.Value); break;
                    case "entity_id": EntityId = Convert.ToInt32(property.Value); break;
                    case "role": Role = property.Value; break;
                    case "strategy": Strategy = property.Value; break;
                    case "hfid": HfId = Convert.ToInt32(property.Value); break;
                    case "handle_actor_id": HandleActorId = Convert.ToInt32(property.Value); break;
                    case "strategy_enid": StrategyEnId = Convert.ToInt32(property.Value); break;
                    case "strategy_eppid": StrategyEppId = Convert.ToInt32(property.Value); break;
                    case "promised_actor_immortality":
                        property.Known = true;
                        PromisedActorImmortality = true;
                        break;
                    case "promised_me_immortality":
                        property.Known = true;
                        PromisedMeImmortality = true;
                        break;
                }
            }
        }
    }
}

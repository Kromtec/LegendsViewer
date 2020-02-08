using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class EntityDissolved : WorldEvent
    {
        public Entity Entity { get; set; }
        public DissolveReason Reason { get; set; }
        public string ReasonString { get; set; }


        public EntityDissolved(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "entity_id": Entity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "reason":
                        ReasonString = property.Value;
                        switch (property.Value)
                        {
                            case "heavy losses in battle": Reason = DissolveReason.HeavyLossesInBattle; break;
                            case "lack of funds": Reason = DissolveReason.LackOfFunds; break;
                            default: 
                                property.Known = false; 
                                break;
                        }
                        break;
                }
            }

            Entity.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += Entity.ToLink(link, pov, this);
            eventString += " dissolved";
            switch (Reason)
            {
                case DissolveReason.HeavyLossesInBattle:
                    eventString += " taking ";
                    break;
                case DissolveReason.LackOfFunds:
                    eventString += " due to ";
                    break;
                default:
                    eventString += " because of ";
                    break;
            }
            eventString += ReasonString;

            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
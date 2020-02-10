using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class EntityAllianceFormed : WorldEvent
    {
        public Entity InitiatingEntity { get; set; }
        public Entity JoiningEntity { get; set; }

        public EntityAllianceFormed(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "initiating_enid": InitiatingEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "joining_enid": JoiningEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                }
            }


            InitiatingEntity.AddEvent(this);
            JoiningEntity.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += JoiningEntity.ToLink(link, pov, this);
            eventString += " swore to support ";
            eventString += InitiatingEntity.ToLink(link, pov, this);
            eventString += " in war if the latter did likewise";

            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HolyCityDeclaration : WorldEvent
    {
        public Site Site { get; set; }
        public Entity ReligionEntity { get; set; }

        public HolyCityDeclaration(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "religion_id": ReligionEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                }
            }
            Site.AddEvent(this);
            ReligionEntity.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += ReligionEntity.ToLink(link, pov, this);
            eventString += " declared ";
            eventString += Site.ToLink(link, pov, this);
            eventString += " to be a holy city";

            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

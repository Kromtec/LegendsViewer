using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class EntitySearchedSite : WorldEvent
    {
        public Entity SearcherCiv { get; set; }
        public Site Site { get; set; }
        public string Result { get; set; }

        public EntitySearchedSite(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "searcher_civ_id": SearcherCiv = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "result": Result = property.Value; break;
                }
            }

            SearcherCiv.AddEvent(this);
            Site.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += " the forces of ";
            eventString += SearcherCiv?.ToLink(true, pov) ?? "an unknown civilization";
            eventString += " searched ";
            eventString += Site?.ToLink(true, pov) ?? "an unknown site";
            eventString += PrintParentCollection(link, pov);
            if (!string.IsNullOrEmpty(Result))
            {
                eventString += " and " + Result;
            }
            eventString += ".";
            return eventString;
        }
    }
}

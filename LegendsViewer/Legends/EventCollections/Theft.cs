using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.EventCollections
{
    public class Theft : EventCollection
    {
        public string Ordinal;
        Location _coordinates;
        public WorldRegion Region;
        public UndergroundRegion UndergroundRegion;
        public Site Site;
        public Entity Attacker;
        public Entity Defender;

        public List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return AllEvents.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }
        public Theft(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "ordinal": Ordinal = string.Intern(property.Value); break;
                    case "coords": _coordinates = Formatting.ConvertToLocation(property.Value); break;
                    case "parent_eventcol": ParentCollection = world.GetEventCollection(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "attacking_enid": Attacker = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "defending_enid": Defender = world.GetEntity(Convert.ToInt32(property.Value)); break;
                }
            }

            foreach (ItemStolen theft in Collection.OfType<ItemStolen>())
            {
                if (theft.Site == null)
                {
                    theft.Site = Site;
                }
                if (!Site.Events.Contains(theft))
                {
                    Site.AddEvent(theft);
                    Site.Events = Site.Events.OrderBy(ev => ev.Id).ToList();
                }
                if (Attacker.SiteHistory.Count == 1)
                {
                    if (theft.ReturnSite == null)
                    {
                        theft.ReturnSite = Attacker.SiteHistory.First().Site;
                    }
                    if (!theft.ReturnSite.Events.Contains(theft))
                    {
                        theft.ReturnSite.AddEvent(theft);
                        theft.ReturnSite.Events = theft.ReturnSite.Events.OrderBy(ev => ev.Id).ToList();
                    }
                }

            }
            Attacker.AddEventCollection(this);
            Defender.AddEventCollection(this);
            Region.AddEventCollection(this);
            UndergroundRegion.AddEventCollection(this);
            Site.AddEventCollection(this);
        }

        public override string ToLink(bool link = true, DwarfObject pov = null)
        {
            return "a theft";
        }
    }
}

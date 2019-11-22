using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.EventCollections
{
    public class Duel : EventCollection
    {
        public string Ordinal;
        public Location Coordinates;
        public WorldRegion Region;
        public UndergroundRegion UndergroundRegion;
        public Site Site;
        public HistoricalFigure Attacker;
        public HistoricalFigure Defender;
        public List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return AllEvents.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }
        public Duel(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "ordinal": Ordinal = String.Intern(property.Value); break;
                    case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                    case "parent_eventcol": ParentCollection = world.GetEventCollection(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "attacking_hfid": Attacker = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "defending_hfid": Defender = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                }
            }
            //foreach (WorldEvent collectionEvent in Collection) this.AddEvent(collectionEvent);
            if (ParentCollection != null && ParentCollection.GetType() == typeof(Battle))
            {
                foreach (HfDied death in Collection.OfType<HfDied>())
                {
                    Battle battle = ParentCollection as Battle;
                    War parentWar = battle.ParentCollection as War;
                    if (battle.NotableAttackers.Contains(death.HistoricalFigure))
                    {
                        battle.AttackerDeathCount++;
                        battle.Attackers.Single(squad => squad.Race == death.HistoricalFigure.Race).Deaths++;
                        
                        if (parentWar != null)
                        {
                            parentWar.AttackerDeathCount++;
                        }
                    }
                    else if (battle.NotableDefenders.Contains(death.HistoricalFigure))
                    {
                        battle.DefenderDeathCount++;
                        battle.Defenders.Single(squad => squad.Race == death.HistoricalFigure.Race).Deaths++;
                        if (parentWar != null)
                        {
                            parentWar.DefenderDeathCount++;
                        }
                    }

                    if (parentWar != null)
                    {
                        (ParentCollection.ParentCollection as War).DeathCount++;
                    }
                }
            }
            Attacker.AddEventCollection(this);
            Defender.AddEventCollection(this);
            Region.AddEventCollection(this);
            UndergroundRegion.AddEventCollection(this);
            Site.AddEventCollection(this);
        }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            return "a duel";
        }
    }
}

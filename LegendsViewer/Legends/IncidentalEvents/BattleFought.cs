using System.Collections.Generic;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.IncidentalEvents
{
    public class BattleFought : WorldEvent
    {
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public HistoricalFigure HistoricalFigure { get; set; }
        public Battle Battle { get; }
        public bool WasHired { get; }
        public bool AsScout { get; }

        public BattleFought(HistoricalFigure hf, Battle battle, World world, bool wasHired = false, bool asScout = false) : base(new List<Property>(), world)
        {
            Id = -1;
            Type = "battle fought";
            Year = battle.StartYear;
            Seconds72 = battle.StartSeconds72;

            HistoricalFigure = hf;
            Battle = battle;
            WasHired = wasHired;
            AsScout = asScout;
            Site = battle.Site;
            Region = battle.Region;
            UndergroundRegion = battle.UndergroundRegion;
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += HistoricalFigure.ToLink(link, pov, this);
            if (WasHired)
            {
                eventString += " was hired";
                if (AsScout)
                {
                    eventString += " as a scout";
                }
                eventString += " to fight in ";
            }
            else
            {
                eventString += " fought in ";
            }
            eventString += Battle.ToLink(link, pov, this);
            if (Site != null)
            {
                eventString += " an assault on ";
                eventString += Site.ToLink(link, pov, this);
            }
            else if (Region != null)
            {
                eventString += " in ";
                eventString += Region.ToLink(link, pov, this);
            }
            else if (UndergroundRegion != null)
            {
                eventString += " in ";
                eventString += UndergroundRegion.ToLink(link, pov, this);
            }
            eventString += ".";
            return eventString;
        }
    }
}

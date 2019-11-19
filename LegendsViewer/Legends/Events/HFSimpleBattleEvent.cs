using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfSimpleBattleEvent : WorldEvent
    {
        public HfSimpleBattleType SubType;
        public string UnknownSubType;
        public HistoricalFigure HistoricalFigure1, HistoricalFigure2;
        public Site Site;
        public WorldRegion Region;
        public UndergroundRegion UndergroundRegion;
        public HfSimpleBattleEvent(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "subtype":
                        switch (property.Value)
                        {
                            case "attacked": SubType = HfSimpleBattleType.Attacked; break;
                            case "scuffle": SubType = HfSimpleBattleType.Scuffle; break;
                            case "confront": SubType = HfSimpleBattleType.Confronted; break;
                            case "2 lost after receiving wounds": SubType = HfSimpleBattleType.Hf2LostAfterReceivingWounds; break;
                            case "2 lost after giving wounds": SubType = HfSimpleBattleType.Hf2LostAfterGivingWounds; break;
                            case "2 lost after mutual wounds": SubType = HfSimpleBattleType.Hf2LostAfterMutualWounds; break;
                            case "happen upon": SubType = HfSimpleBattleType.HappenedUpon; break;
                            case "ambushed": SubType = HfSimpleBattleType.Ambushed; break;
                            case "corner": SubType = HfSimpleBattleType.Cornered; break;
                            case "surprised": SubType = HfSimpleBattleType.Surprised; break;
                            default: SubType = HfSimpleBattleType.Unknown; UnknownSubType = property.Value; property.Known = false; break;
                        }
                        break;
                    case "group_1_hfid": HistoricalFigure1 = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "group_2_hfid": HistoricalFigure2 = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                }
            }

            HistoricalFigure1.AddEvent(this);
            HistoricalFigure2.AddEvent(this);
            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
        }
        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + HistoricalFigure1.ToLink(link, pov);
            if (SubType == HfSimpleBattleType.Hf2LostAfterGivingWounds)
            {
                eventString = GetYearTime() + HistoricalFigure2.ToLink(link, pov) + " was forced to retreat from "
                                                                                      + HistoricalFigure1.ToLink(link, pov) + " despite the latter's wounds";
            }
            else if (SubType == HfSimpleBattleType.Hf2LostAfterMutualWounds)
            {
                eventString += " eventually prevailed and " + HistoricalFigure2.ToLink(link, pov)
                                                                                            + " was forced to make a hasty escape";
            }
            else if (SubType == HfSimpleBattleType.Hf2LostAfterReceivingWounds)
            {
                eventString = GetYearTime() + HistoricalFigure2.ToLink(link, pov) + " managed to escape from "
                                                                                              + HistoricalFigure1.ToLink(link, pov) + "'s onslaught";
            }
            else if (SubType == HfSimpleBattleType.Scuffle)
            {
                eventString += " fought with " + HistoricalFigure2.ToLink(link, pov) + ". While defeated, the latter escaped unscathed";
            }
            else if (SubType == HfSimpleBattleType.Attacked)
            {
                eventString += " attacked " + HistoricalFigure2.ToLink(link, pov);
            }
            else if (SubType == HfSimpleBattleType.Confronted)
            {
                eventString += " confronted " + HistoricalFigure2.ToLink(link, pov);
            }
            else if (SubType == HfSimpleBattleType.HappenedUpon)
            {
                eventString += " happened upon " + HistoricalFigure2.ToLink(link, pov);
            }
            else if (SubType == HfSimpleBattleType.Ambushed)
            {
                eventString += " ambushed " + HistoricalFigure2.ToLink(link, pov);
            }
            else if (SubType == HfSimpleBattleType.Cornered)
            {
                eventString += " cornered " + HistoricalFigure2.ToLink(link, pov);
            }
            else if (SubType == HfSimpleBattleType.Surprised)
            {
                eventString += " suprised " + HistoricalFigure2.ToLink(link, pov);
            }
            else
            {
                eventString += " fought (" + UnknownSubType + ") " + HistoricalFigure2.ToLink(link, pov);
            }

            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }

    }
}
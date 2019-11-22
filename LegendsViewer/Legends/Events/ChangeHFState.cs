using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class ChangeHfState : WorldEvent
    {
        public HistoricalFigure HistoricalFigure { get; set; }
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public Location Coordinates { get; set; }
        public HfState State { get; set; }
        public int SubState { get; set; }
        public Mood Mood { get; set; }
        public ChangeHfStateReason Reason { get; set; }

        public ChangeHfState(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "state":
                        switch (property.Value)
                        {
                            case "settled": State = HfState.Settled; break;
                            case "wandering": State = HfState.Wandering; break;
                            case "scouting": State = HfState.Scouting; break;
                            case "snatcher": State = HfState.Snatcher; break;
                            case "refugee": State = HfState.Refugee; break;
                            case "thief": State = HfState.Thief; break;
                            case "hunting": State = HfState.Hunting; break;
                            case "visiting": State = HfState.Visiting; break;
                            default: State = HfState.Unknown; property.Known = false; break;
                        }
                        break;
                    case "substate": SubState = Convert.ToInt32(property.Value); break;
                    case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                    case "hfid": HistoricalFigure = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "site": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "mood":
                        switch (property.Value)
                        {
                            case "macabre":
                                Mood = Mood.Macabre;
                                break;
                            case "secretive":
                                Mood = Mood.Secretive;
                                break;
                            case "insane":
                                Mood = Mood.Insane;
                                break;
                            case "possessed":
                                Mood = Mood.Possessed;
                                break;
                            case "berserk":
                                Mood = Mood.Berserk;
                                break;
                            case "fey":
                                Mood = Mood.Fey;
                                break;
                            case "melancholy":
                                Mood = Mood.Melancholy;
                                break;
                            case "fell":
                                Mood = Mood.Fell;
                                break;
                            case "catatonic":
                                Mood = Mood.Catatonic;
                                break;
                            default:
                                Mood = Mood.Unknown;
                                property.Known = false;
                                break;
                        }
                        break;
                    case "reason":
                        switch (property.Value)
                        {
                            case "failed mood":
                                Reason = ChangeHfStateReason.FailedMood;
                                break;
                            case "gather information":
                                Reason = ChangeHfStateReason.GatherInformation;
                                break;
                            case "be with master":
                                Reason = ChangeHfStateReason.BeWithMaster;
                                break;
                            case "flight":
                                Reason = ChangeHfStateReason.Flight;
                                break;
                            case "scholarship":
                                Reason = ChangeHfStateReason.Scholarship;
                                break;
                            case "on a pilgrimage":
                                Reason = ChangeHfStateReason.Pilgrimage;
                                break;
                            case "lack of sleep":
                                Reason = ChangeHfStateReason.LackOfSleep;
                                break;
                            case "great deal of stress":
                                Reason = ChangeHfStateReason.GreatDealOfStress;
                                break;
                            default:
                                if (property.Value != "-1")
                                {
                                    property.Known = false;
                                }
                                break;
                        }
                        break;
                }
            }
            if (HistoricalFigure != null)
            {
                HistoricalFigure.AddEvent(this);
                HistoricalFigure.States.Add(new HistoricalFigure.State(State, Year));
                HistoricalFigure.State lastState = HistoricalFigure.States.LastOrDefault();
                if (lastState != null)
                {
                    lastState.EndYear = Year;
                }

                HistoricalFigure.CurrentState = State;
            }
            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime() + HistoricalFigure.ToLink(link, pov, this);
            if (State == HfState.Visiting)
            {
                eventString += " visited ";
            }
            else if(State == HfState.Settled)
            {
                switch (SubState)
                {
                    case 45:
                        eventString += " fled to ";
                        break;
                    case 46:
                    case 47:
                        eventString += " moved to study in ";
                        break;
                    default:
                        eventString += " settled in ";
                        break;
                }
            }
            else if (State == HfState.Wandering)
            {
                eventString += " began wandering ";
            }
            else if (State == HfState.Refugee || State == HfState.Snatcher || State == HfState.Thief)
            {
                eventString += " became a " + State.ToString().ToLower() + " in ";
            }
            else if (State == HfState.Scouting)
            {
                eventString += " began scouting the area around ";
            }
            else if (State == HfState.Hunting)
            {
                eventString += " began hunting great beasts in ";
            }
            else if (Mood != Mood.Unknown)
            {
                switch (Mood)
                {
                    case Mood.Macabre:
                        eventString += " began to skulk and brood in ";
                        break;
                    case Mood.Secretive:
                        eventString += " withdrew from society in ";
                        break;
                    case Mood.Insane:
                        eventString += " became crazed in ";
                        break;
                    case Mood.Possessed:
                        eventString += " was possessed in ";
                        break;
                    case Mood.Berserk:
                        eventString += " went berserk in ";
                        break;
                    case Mood.Fey:
                        eventString += " was taken by a fey mood in ";
                        break;
                    case Mood.Melancholy:
                        eventString += " was striken by melancholy in ";
                        break;
                    case Mood.Fell:
                        eventString += " was taken by a fell mood in ";
                        break;
                    case Mood.Catatonic:
                        eventString += " stopped responding to the outside world in ";
                        break;
                }
            }
            else 
            {
                eventString += " changed state in ";
            }

            if (Site != null)
            {
                eventString += Site.ToLink(link, pov, this);
            }
            else if (Region != null)
            {
                eventString += Region.ToLink(link, pov, this);
            }
            else if (UndergroundRegion != null)
            {
                eventString += UndergroundRegion.ToLink(link, pov, this);
            }
            else
            {
                eventString += "the wilds";
            }

            if (Reason != ChangeHfStateReason.Unknown)
            {
                switch (Reason)
                {
                    case ChangeHfStateReason.FailedMood:
                        eventString += " after failing to create an artifact";
                        break;
                    case ChangeHfStateReason.GatherInformation:
                        eventString += " to gather information";
                        break;
                    case ChangeHfStateReason.BeWithMaster:
                        eventString += " in order to be with the master";
                        break;
                    case ChangeHfStateReason.Flight:
                        eventString += " in order to flee";
                        break;
                    case ChangeHfStateReason.Scholarship:
                        eventString += " in order to pursue scholarship";
                        break;
                    case ChangeHfStateReason.Pilgrimage:
                        eventString += " on a pilgrimage";
                        break;
                    case ChangeHfStateReason.LackOfSleep:
                        eventString += " due to lack of sleep";
                        break;
                    case ChangeHfStateReason.GreatDealOfStress:
                        eventString += " after a great deal of stress";
                        break;
                }
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
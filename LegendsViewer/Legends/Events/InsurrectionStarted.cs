using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class InsurrectionStarted : WorldEvent
    {
        public Entity Civ { get; set; }
        public Site Site { get; set; }
        public InsurrectionOutcome Outcome { get; set; }
        public bool ActualStart { get; set; }
        private readonly string _unknownOutcome;

        public InsurrectionStarted(List<Property> properties, World world) : base(properties, world)
        {
            ActualStart = false;

            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "target_civ_id":
                        Civ = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "site_id":
                        Site = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                    case "outcome":
                        switch (property.Value)
                        {
                            case "leadership overthrown":
                                Outcome = InsurrectionOutcome.LeadershipOverthrown;
                                break;
                            case "population gone":
                                Outcome = InsurrectionOutcome.PopulationGone;
                                break;
                            default:
                                Outcome = InsurrectionOutcome.Unknown;
                                _unknownOutcome = property.Value;
                                world.ParsingErrors.Report("Unknown Insurrection Outcome: " + _unknownOutcome);
                                break;
                        }
                        break;
                }
            }

            Civ.AddEvent(this);
            Site.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (ActualStart)
            {
                eventString += "an insurrection against " + Civ.ToLink(link, pov, this) + " began in " + Site.ToLink(link, pov, this);
            }
            else
            {
                eventString += "the insurrection in " + Site.ToLink(link, pov, this);
                switch (Outcome)
                {
                    case InsurrectionOutcome.LeadershipOverthrown:
                        eventString += " concluded with " + Civ.ToLink(link, pov, this) + " overthrown";
                        break;
                    case InsurrectionOutcome.PopulationGone:
                        eventString += " ended with the disappearance of the rebelling population";
                        break;
                    default:
                        eventString += " against " + Civ.ToLink(link, pov, this) + " concluded with (" + _unknownOutcome + ")";
                        break;
                }
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class AgreementConcluded : WorldEvent
    {
        public Entity Source { get; set; }
        public Entity Destination { get; set; }
        public Site Site { get; set; }
        public AgreementTopic Topic { get; set; }
        public int Result { get; set; }

        public AgreementConcluded(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "topic":
                        switch (property.Value)
                        {
                            case "treequota": Topic = AgreementTopic.TreeQuota; break;
                            case "becomelandholder": Topic = AgreementTopic.BecomeLandHolder; break;
                            case "promotelandholder": Topic = AgreementTopic.PromoteLandHolder; break;
                            case "tributeagreement":
                            case "unknown 9": Topic = AgreementTopic.Tribute; break;
                            default:
                                Topic = AgreementTopic.Unknown;
                                property.Known = false;
                                break;
                        }
                        break;
                    case "source": Source = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "destination": Destination = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "site": if (Site == null) { Site = world.GetSite(Convert.ToInt32(property.Value)); } else { property.Known = true; } break;
                    case "result": Result = Convert.ToInt32(property.Value); break;
                }
            }

            Site.AddEvent(this);
            Source.AddEvent(this);
            Destination.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            switch (Topic)
            {
                case AgreementTopic.TreeQuota:
                    eventString += "a lumber agreement between ";
                    break;
                case AgreementTopic.BecomeLandHolder:
                    eventString += "the establishment of landed nobility agreement between ";
                    break;
                case AgreementTopic.PromoteLandHolder:
                    eventString += "the elevation of the landed nobility agreement between ";
                    break;
                case AgreementTopic.Tribute:
                    eventString += "a tribute agreement between ";
                    break;
                default:
                    eventString += "UNKNOWN AGREEMENT";
                    break;
            }
            eventString += Source != null ? Source.ToLink(link, pov, this) : "UNKNOWN ENTITY";
            eventString += " and ";
            eventString += Destination != null ? Destination.ToLink(link, pov, this) : "UNKNOWN ENTITY";
            eventString += " at ";
            eventString += Site != null ? Site.ToLink(link, pov, this) : "UNKNOWN SITE";
            eventString += " concluded";
            switch (Result)
            {
                case -3:
                    eventString += "  with miserable outcome";
                    break;
                case -2:
                    eventString += " with a strong negative outcome";
                    break;
                case -1:
                    eventString += " in an unsatisfactory fashion";
                    break;
                case 0:
                    eventString += " fairly";
                    break;
                case 1:
                    eventString += " with a positive outcome";
                    break;
                case 2:
                    eventString += ", cementing bonds of mutual trust";
                    break;
                case 3:
                    eventString += " with a very strong positive outcome";
                    break;
                default:
                    eventString += " with an unknown outcome";
                    break;
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
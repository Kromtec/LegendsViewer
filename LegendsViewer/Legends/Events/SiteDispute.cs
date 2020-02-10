using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class SiteDispute : WorldEvent
    {
        public Dispute Dispute { get; set; }
        public Entity Entity1 { get; set; }
        public Entity Entity2 { get; set; }
        public Site Site1 { get; set; }
        public Site Site2 { get; set; }
        private string _unknownDispute;

        public SiteDispute(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "dispute":
                        switch (property.Value)
                        {
                            case "fishing rights":
                                Dispute = Dispute.FishingRights;
                                break;
                            case "grazing rights":
                                Dispute = Dispute.GrazingRights;
                                break;
                            case "livestock ownership":
                                Dispute = Dispute.LivestockOwnership;
                                break;
                            case "territory":
                                Dispute = Dispute.Territory;
                                break;
                            case "water rights":
                                Dispute = Dispute.WaterRights;
                                break;
                            case "rights-of-way":
                                Dispute = Dispute.RightsOfWay;
                                break;
                            default:
                                Dispute = Dispute.Unknown;
                                _unknownDispute = property.Value;
                                world.ParsingErrors.Report("Unknown Site Dispute: " + _unknownDispute);
                                break;
                        }
                        break;
                    case "entity_id_1":
                        Entity1 = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "entity_id_2":
                        Entity2 = world.GetEntity(Convert.ToInt32(property.Value));
                        break;
                    case "site_id_1":
                        Site1 = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                    case "site_id_2":
                        Site2 = world.GetSite(Convert.ToInt32(property.Value));
                        break;
                }
            }

            Entity1.AddEvent(this);
            Entity2.AddEvent(this);
            Site1.AddEvent(this);
            Site2.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string dispute = _unknownDispute;
            switch (Dispute)
            {
                case Dispute.FishingRights:
                    dispute = "fishing rights";
                    break;
                case Dispute.GrazingRights:
                    dispute = "grazing rights";
                    break;
                case Dispute.LivestockOwnership:
                    dispute = "livestock ownership";
                    break;
                case Dispute.Territory:
                    dispute = "territory";
                    break;
                case Dispute.WaterRights:
                    dispute = "water rights";
                    break;
                case Dispute.RightsOfWay:
                    dispute = "rights of way";
                    break;
            }

            string eventString = GetYearTime();
            eventString += Entity1 != null ? Entity1.ToLink(link, pov, this) : "UNKNOWN ENTITY";
            eventString += " of ";
            eventString += Site1 != null ? Site1.ToLink(link, pov, this) : "UNKNOWN SITE";
            eventString += " and ";
            eventString += Entity2 != null ? Entity2.ToLink(link, pov, this) : "UNKNOWN ENTITY";
            eventString += " of ";
            eventString += Site2 != null ? Site2.ToLink(link, pov, this) : "UNKNOWN SITE";
            eventString += " became embroiled in a dispute over " + dispute;
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
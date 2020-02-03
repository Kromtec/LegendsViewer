using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfsFormedReputationRelationship : WorldEvent
    {
        public HistoricalFigure HistoricalFigure1 { get; set; }
        public HistoricalFigure HistoricalFigure2 { get; set; }
        public int IdentityId1 { get; set; }
        public int IdentityId2 { get; set; }
        public ReputationType HfRep1Of2 { get; set; }
        public ReputationType HfRep2Of1 { get; set; }
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }

        public HfsFormedReputationRelationship(List<Property> properties, World world)
            : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "hfid1": HistoricalFigure1 = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "hfid2": HistoricalFigure2 = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "identity_id1": IdentityId1 = Convert.ToInt32(property.Value); break;
                    case "identity_id2": IdentityId2 = Convert.ToInt32(property.Value); break;
                    case "hf_rep_1_of_2":
                        switch (property.Value)
                        {
                            case "information source":
                                HfRep1Of2 = ReputationType.InformationSource;
                                break;
                            default:
                                property.Known = false;
                                break;
                        }

                        break;
                    case "hf_rep_2_of_1":
                        switch (property.Value)
                        {
                            case "information source":
                                HfRep2Of1 = ReputationType.InformationSource;
                                break;
                            case "buddy":
                                HfRep2Of1 = ReputationType.Buddy;
                                break;
                            case "friendly":
                                HfRep2Of1 = ReputationType.Friendly;
                                break;
                            default:
                                property.Known = false;
                                break;
                        }

                        break;
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
            string eventString = GetYearTime();
            eventString += HistoricalFigure1.ToLink(link, pov, this);
            if (IdentityId1 > 0)
            {
                eventString += " as '" + IdentityId1 + "'";
            }
            eventString += ", formed a false friendship with ";
            eventString += HistoricalFigure2.ToLink(link, pov, this);
            if (IdentityId2 > 0)
            {
                eventString += " as '" + IdentityId2 + "'";
            }
            if (HfRep2Of1 == ReputationType.Buddy || HfRep2Of1 == ReputationType.Friendly)
            {
                eventString += " in order to extract information";
            }
            else if (HfRep2Of1 == ReputationType.InformationSource)
            {
                eventString += " where each used the other for information";
            }
            eventString += " in ";
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
                eventString += "UNKNOWN LOCATION";
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}

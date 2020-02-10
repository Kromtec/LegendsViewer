using System;
using System.Collections.Generic;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class AgreementFormed : WorldEvent
    {
        public int AgreementId { get; set; }
        public HistoricalFigure Concluder { get; set; }
        private int AgreementSubjectId { get; set; }
        private AgreementReason Reason { get; set; }

        // TODO lots of new properties, but can not find an entry in DF legends mode
        public bool Successful { get; set; }
        public bool FailedJudgmentTest { get; set; }
        public bool Delegated { get; set; }
        
        public string Action { get; set; }
        public string Method { get; set; }

        public string TopFacet { get; set; }
        public int TopFacetRating { get; set; }
        public int TopFacetModifier { get; set; }

        public string TopValue { get; set; }
        public int TopValueRating { get; set; }
        public int TopValueModifier { get; set; }

        public string TopRelationshipFactor { get; set; }
        public int TopRelationshipRating { get; set; }
        public int TopRelationshipModifier { get; set; }

        public Entity RelevantEntity { get; set; }
        public int RelevantPositionProfileId { get; set; }
        public int RelevantIdForMethod { get; set; }

        public int AllyDefenseBonus { get; set; }
        public int CoconspiratorBonus { get; set; }

        public AgreementFormed(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "agreement_id": AgreementId = Convert.ToInt32(property.Value); break;
                    case "successful": property.Known = true; Successful = true; break;
                    case "failed_judgment_test": property.Known = true; FailedJudgmentTest = true; break;
                    case "delegated": property.Known = true; Delegated = true; break;
                    case "action": Action = property.Value; break;
                    case "method": Method = property.Value; break;
                    case "relevant_entity_id": RelevantEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "relevant_position_profile_id": RelevantPositionProfileId = Convert.ToInt32(property.Value); break;
                    case "relevant_id_for_method": RelevantIdForMethod = Convert.ToInt32(property.Value); break;
                    case "ally_defense_bonus": AllyDefenseBonus = Convert.ToInt32(property.Value); break;
                    case "coconspirator_bonus": CoconspiratorBonus = Convert.ToInt32(property.Value); break;
                    case "top_facet": TopFacet = property.Value; break;
                    case "top_facet_rating": TopFacetRating = Convert.ToInt32(property.Value); break;
                    case "top_facet_modifier": TopFacetModifier = Convert.ToInt32(property.Value); break;
                    case "top_value": TopValue = property.Value; break;
                    case "top_value_rating": TopValueRating = Convert.ToInt32(property.Value); break;
                    case "top_value_modifier": TopValueModifier = Convert.ToInt32(property.Value); break;
                    case "top_relationship_factor": TopRelationshipFactor = property.Value; break;
                    case "top_relationship_rating": TopRelationshipRating = Convert.ToInt32(property.Value); break;
                    case "top_relationship_modifier": TopRelationshipModifier = Convert.ToInt32(property.Value); break;

                    case "concluder_hfid": Concluder = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "agreement_subject_id": AgreementSubjectId = Convert.ToInt32(property.Value); break;
                    case "reason":
                        switch (property.Value)
                        {
                            case "arrived at location": Reason = AgreementReason.ArrivedAtLocation; break;
                            case "violent disagreement": Reason = AgreementReason.ViolentDisagreement; break;
                            case "whim": Reason = AgreementReason.Whim; break;
                            default:
                                Reason = AgreementReason.Unknown;
                                world.ParsingErrors.Report("Unknown Agreement Reason: " + property.Value);
                                break;
                        }
                        break;
                }
            }
            Concluder.AddEvent(this);
            RelevantEntity.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            if (Concluder != null)
            {
                eventString += Concluder.ToLink(link, pov, this);
                eventString += " formed an agreement";
            }
            else
            {
                eventString += " an agreement has been formed";
            }

            switch (Reason)
            {
                case AgreementReason.Whim:
                    eventString += " on a whim";
                    break;
                case AgreementReason.ViolentDisagreement:
                    eventString += " after a violent disagreement";
                    break;
                case AgreementReason.ArrivedAtLocation:
                    eventString += " after arriving at the location";
                    break;
            }
            eventString += PrintParentCollection(link, pov);
            eventString += ".";
            return eventString;
        }
    }
}
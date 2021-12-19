using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.Events
{
    public class HfsFormedIntrigueRelationship : WorldEvent
    {
        public HistoricalFigure CorruptorHf { get; set; }
        public HistoricalFigure TargetHf { get; set; }
        public Site Site { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }

        public bool FailedJudgmentTest { get; set; }
        public bool Successful { get; set; }

        public IntrigueAction Action { get; set; }
        public IntrigueMethod Method { get; set; }

        public string TopFacet { get; set; }
        public int TopFacetRating { get; set; }
        public int TopFacetModifier { get; set; }

        public string TopValue { get; set; }
        public int TopValueRating { get; set; }
        public int TopValueModifier { get; set; }

        public string TopRelationshipFactor { get; set; }
        public int TopRelationshipRating { get; set; }
        public int TopRelationshipModifier { get; set; }

        public int AllyDefenseBonus { get; set; }
        public int CoConspiratorBonus { get; set; }

        public HistoricalFigure LureHf { get; set; }
        public int CorruptorIdentityId { get; set; }
        public int TargetIdentityId { get; set; }

        public string CorruptorSeenAs { get; set; }
        public string TargetSeenAs { get; set; }
        public string Circumstance { get; set; }
        public int CircumstanceId { get; set; }

        public Entity RelevantEntity { get; set; }
        public int RelevantPositionProfileId { get; set; }
        public int RelevantIdForMethod { get; set; }

        // Similar to failed intrigue corruption
        public HfsFormedIntrigueRelationship(List<Property> properties, World world) : base(properties, world)
        {
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "corruptor_hfid": CorruptorHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "target_hfid": TargetHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "failed_judgment_test": property.Known = true; FailedJudgmentTest = true; break;
                    case "action":
                        switch (property.Value.Replace("_", " "))
                        {
                            case "bribe official": Action = IntrigueAction.BribeOfficial; break;
                            case "induce to embezzle": Action = IntrigueAction.InduceToEmbezzle; break;
                            case "corrupt in place": Action = IntrigueAction.CorruptInPlace; break;
                            case "bring into network": Action = IntrigueAction.BringIntoNetwork; break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "method":
                        switch (property.Value.Replace("_", " "))
                        {
                            case "intimidate": Method = IntrigueMethod.Intimidate; break;
                            case "flatter": Method = IntrigueMethod.Flatter; break;
                            case "bribe": Method = IntrigueMethod.Bribe; break;
                            case "precedence": Method = IntrigueMethod.Precedence; break;
                            case "offer immortality": Method = IntrigueMethod.OfferImmortality; break;
                            case "religious sympathy": Method = IntrigueMethod.ReligiousSympathy; break;
                            case "blackmail over embezzlement": Method = IntrigueMethod.BlackmailOverEmbezzlement; break;
                            case "revenge on grudge": Method = IntrigueMethod.RevengeOnGrudge; break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "top_facet": TopFacet = property.Value; break;
                    case "top_facet_rating": TopFacetRating = Convert.ToInt32(property.Value); break;
                    case "top_facet_modifier": TopFacetModifier = Convert.ToInt32(property.Value); break;
                    case "top_value": TopValue = property.Value; break;
                    case "top_value_rating": TopValueRating = Convert.ToInt32(property.Value); break;
                    case "top_value_modifier": TopValueModifier = Convert.ToInt32(property.Value); break;
                    case "top_relationship_factor": TopRelationshipFactor = property.Value; break;
                    case "top_relationship_rating": TopRelationshipRating = Convert.ToInt32(property.Value); break;
                    case "top_relationship_modifier": TopRelationshipModifier = Convert.ToInt32(property.Value); break;
                    case "ally_defense_bonus": AllyDefenseBonus = Convert.ToInt32(property.Value); break;
                    case "coconspirator_bonus": CoConspiratorBonus = Convert.ToInt32(property.Value); break;
                    case "lure_hfid": LureHf = world.GetHistoricalFigure(Convert.ToInt32(property.Value)); break;
                    case "corruptor_identity": CorruptorIdentityId = Convert.ToInt32(property.Value); break;
                    case "target_identity": TargetIdentityId = Convert.ToInt32(property.Value); break;
                    case "successful": property.Known = true; Successful = true; break;
                    case "corruptor_seen_as": CorruptorSeenAs = property.Value; break;
                    case "target_seen_as": TargetSeenAs = property.Value; break;
                    case "circumstance": Circumstance = property.Value; break;
                    case "circumstance_id": CircumstanceId = Convert.ToInt32(property.Value); break;
                    case "relevant_entity_id": RelevantEntity = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "relevant_position_profile_id": RelevantPositionProfileId = Convert.ToInt32(property.Value); break;
                    case "relevant_id_for_method": RelevantIdForMethod = Convert.ToInt32(property.Value); break;
                }
            }

            CorruptorHf.AddEvent(this);
            TargetHf.AddEvent(this);
            Site.AddEvent(this);
            Region.AddEvent(this);
            UndergroundRegion.AddEvent(this);
            LureHf.AddEvent(this);
            RelevantEntity.AddEvent(this);
        }

        public override string Print(bool link = true, DwarfObject pov = null)
        {
            string eventString = GetYearTime();
            eventString += TargetHf.ToLink(link, pov, this);
            switch (Action)
            {
                case IntrigueAction.BribeOfficial:
                    eventString += " began accepting bribes";
                    break;
                case IntrigueAction.InduceToEmbezzle:
                    eventString += " began embezzling funds";
                    break;
                case IntrigueAction.CorruptInPlace:
                    eventString += " was corrupted to be an agent";
                    break;
                case IntrigueAction.BringIntoNetwork:
                    eventString += " was corrupted to act on plots and schemes";
                    break;
            }
            if (Site != null)
            {
                eventString += " in ";
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
            else
            {
                eventString += " in the wilds";
            }
            eventString += " under the influence of ";
            eventString += CorruptorHf.ToLink(link, pov, this);
            eventString += PrintParentCollection(link, pov);
            eventString += ". ";
            if (LureHf != null)
            {
                eventString += LureHf.ToLink(link, pov, this).ToUpperFirstLetter();
                eventString += " lured ";
                eventString += TargetHf.ToLink(link, pov, this);
                eventString += " into a meeting";
            }
            else
            {
                eventString += CorruptorHf.ToLink(link, pov, this).ToUpperFirstLetter();
                eventString += " met with ";
                eventString += TargetHf.ToLink(link, pov, this);
            }
            eventString += " and ";
            switch (Method)
            {
                case IntrigueMethod.Intimidate:
                    eventString += "made a threat. ";
                    break;
                case IntrigueMethod.Flatter:
                    eventString += "made flattering remarks. ";
                    break;
                case IntrigueMethod.Bribe:
                    eventString += "offered a bribe. ";
                    break;
                case IntrigueMethod.Precedence:
                    eventString += "pulled ranks. ";
                    eventString += TargetHf.ToLink(link, pov, this).ToUpperFirstLetter();
                    eventString += " accepted in admiration of power.";
                    break;
                case IntrigueMethod.OfferImmortality:
                    eventString += "offered immortality. ";
                    break;
                case IntrigueMethod.ReligiousSympathy:
                    eventString += $"played on sympathy by appealing to a shared worship of {World.GetHistoricalFigure(RelevantIdForMethod)?.ToLink(link, pov, this)}. ";
                    break;
                case IntrigueMethod.BlackmailOverEmbezzlement:
                    var position = RelevantEntity?.EntityPositions.Find(p => p.Id == RelevantPositionProfileId);
                    eventString += $"made a blackmail threat, due to embezzlement using the position {position?.Name} of {RelevantEntity?.ToLink(link, pov, this)}. ";
                    break;
                case IntrigueMethod.RevengeOnGrudge:
                    eventString += $"offered revenge upon  {World.GetHistoricalFigure(RelevantIdForMethod)?.ToLink(link, pov, this)}. ";
                    break;
            }
            eventString += "The plan worked.";
            // TODO create the right sentences for facet, value and relationship factors
            //eventString += "<br/>";
            //if (TopFacet != null)
            //{
            //    eventString += $" TopFacet: {TopFacet} ({TopFacetRating}/{TopFacetModifier}) ";
            //}
            //if (TopValue != null)
            //{
            //    eventString += $" TopValue: {TopValue} ({TopValueRating}/{TopValueModifier}) ";
            //}
            //if (TopRelationshipFactor != null)
            //{
            //    eventString += $" TopRelationshipFactor: {TopRelationshipFactor} ({TopRelationshipRating}/{TopRelationshipModifier}) ";
            //}
            //if (FailedJudgmentTest)
            //{
            //    eventString += " FailedJudgmentTest ";
            //}
            //if (AllyDefenseBonus != 0)
            //{
            //    eventString += $" AllyDefenseBonus: {AllyDefenseBonus}";
            //}
            //if (CoConspiratorBonus != 0)
            //{
            //    eventString += $" CoConspiratorBonus: {CoConspiratorBonus}";
            //}
            return eventString;
        }
    }
}

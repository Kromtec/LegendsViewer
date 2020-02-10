using System;
using System.Collections.Generic;
using LegendsViewer.Legends;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.Query
{
    public class SearchProperty
    {
        public string Name;
        public string Description;
        public bool IsList;
        public bool IsSelectable;
        public Type Type;
        public List<SearchProperty> SubProperties = new List<SearchProperty>();
        public SearchProperty(string name, Type type)
            : this(name, name, type)
        {
        }

        public SearchProperty(string name, string description, Type type, bool selectable = false)
        {
            Name = name;
            Description = description;
            Type = type;
            if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(List<>))
            {
                IsList = true;

                //SubProperties = SearchObject.GetProperties(type.GetGenericArguments()[0]);
            }
            IsSelectable = selectable;
        }

        public override string ToString()
        {
            return Description;
        }

        public void SetSubProperties()
        {
            SubProperties = GetProperties(Type, true);
        }

        public static List<SearchProperty> GetProperties(Type searchType, bool noSubProperties = false)
        {
            Type nonGenericSearchType;
            if (searchType.IsGenericType && searchType != typeof(List<int>) && searchType != typeof(List<string>))
            {
                nonGenericSearchType = searchType.GetGenericArguments()[0];
            }
            else
            {
                nonGenericSearchType = searchType;
            }

            List<SearchProperty> searchProperties = new List<SearchProperty>();
            if (nonGenericSearchType == typeof(HistoricalFigure))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(HistoricalFigure.Name), typeof(string)),
                    new SearchProperty(nameof(HistoricalFigure.Race), typeof(string)),
                    new SearchProperty(nameof(HistoricalFigure.AssociatedType), "Associated Type", typeof(string)),
                    new SearchProperty(nameof(HistoricalFigure.Caste), typeof(string)),
                    new SearchProperty(nameof(HistoricalFigure.CurrentState), "Current State", typeof(HfState)),
                    new SearchProperty(nameof(HistoricalFigure.States), "All States", typeof(List<HistoricalFigure.State>)),
                    new SearchProperty(nameof(HistoricalFigure.Age), typeof(int)),
                    new SearchProperty(nameof(HistoricalFigure.Appeared), typeof(int)),
                    new SearchProperty(nameof(HistoricalFigure.BirthYear), "Birth Year", typeof(int)),
                    new SearchProperty(nameof(HistoricalFigure.DeathYear), "Death Year", typeof(int)),
                    new SearchProperty(nameof(HistoricalFigure.DeathCause), "Death Cause", typeof(DeathCause)),
                    new SearchProperty(nameof(HistoricalFigure.Alive), "Is Alive", typeof(bool)),
                    new SearchProperty(nameof(HistoricalFigure.Deity), "Is Deity", typeof(bool)),
                    new SearchProperty(nameof(HistoricalFigure.Force), "Is Force", typeof(bool)),
                    new SearchProperty(nameof(HistoricalFigure.Skeleton), "Is Skeleton", typeof(bool)),
                    new SearchProperty(nameof(HistoricalFigure.Zombie), "Is Zombie", typeof(bool)),
                    new SearchProperty(nameof(HistoricalFigure.Ghost), "Is Ghost", typeof(bool)),
                    new SearchProperty(nameof(HistoricalFigure.Animated), "Is Animated", typeof(bool)),
                    new SearchProperty(nameof(HistoricalFigure.AnimatedType), "Animated Type", typeof(string)),
                    new SearchProperty(nameof(HistoricalFigure.ActiveInteractions), "Active Interactions", typeof(List<string>)),
                    new SearchProperty(nameof(HistoricalFigure.InteractionKnowledge), "Interaction Knowledge", typeof(List<string>)),
                    new SearchProperty(nameof(HistoricalFigure.Goal), typeof(string)),
                    new SearchProperty(nameof(HistoricalFigure.JourneyPets), "Journey Pets", typeof(List<string>)),
                    new SearchProperty(nameof(HistoricalFigure.Positions), "Positions", typeof(List<HistoricalFigure.Position>)),
                    new SearchProperty(nameof(HistoricalFigure.RelatedHistoricalFigures), "Related Historical Figures", typeof(List<HistoricalFigureLink>), true),
                    new SearchProperty(nameof(HistoricalFigure.RelatedEntities), "Related Entities", typeof(List<EntityLink>), true),
                    new SearchProperty(nameof(HistoricalFigure.RelatedSites), "Related Sites", typeof(List<SiteLink>), true),
                    new SearchProperty(nameof(HistoricalFigure.Skills), typeof(List<Skill>)),
                    new SearchProperty(nameof(HistoricalFigure.HFKills), "Notable Kills", typeof(List<HistoricalFigure>), true),
                    new SearchProperty(nameof(HistoricalFigure.Abductions), typeof(List<HistoricalFigure>)),
                    new SearchProperty(nameof(HistoricalFigure.Abducted), typeof(int)),
                    new SearchProperty(nameof(HistoricalFigure.Battles), "Battles", typeof(List<Battle>), true),
                    new SearchProperty(nameof(HistoricalFigure.BattlesAttacking), "Battles (Attacking)", typeof(List<Battle>), true),
                    new SearchProperty(nameof(HistoricalFigure.BattlesDefending), "Battles (Defending)", typeof(List<Battle>), true),
                    new SearchProperty(nameof(HistoricalFigure.BattlesNonCombatant), "Battles (Non-Combatant)", typeof(List<Battle>), true),
                    new SearchProperty(nameof(HistoricalFigure.BeastAttacks), "Beast Attacks", typeof(List<BeastAttack>), true),
                    new SearchProperty(nameof(HistoricalFigure.Spheres), "Associated Spheres", typeof(List<string>))
                };
            }
            else if (nonGenericSearchType == typeof(HistoricalFigureLink))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(HistoricalFigureLink.HistoricalFigure), "Historical Figure", typeof(HistoricalFigure)),
                    new SearchProperty(nameof(HistoricalFigureLink.Type), typeof(HistoricalFigureLinkType)),
                    new SearchProperty(nameof(HistoricalFigureLink.Strength), typeof(int))
                };
            }
            else if (nonGenericSearchType == typeof(EntityLink))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(EntityLink.Entity), typeof(Entity)),
                    new SearchProperty(nameof(EntityLink.Type), typeof(EntityLinkType)),
                    new SearchProperty(nameof(EntityLink.Strength), typeof(int)),
                    new SearchProperty(nameof(EntityLink.PositionId), typeof(int)),
                    new SearchProperty(nameof(EntityLink.StartYear), typeof(int)),
                    new SearchProperty(nameof(EntityLink.EndYear), typeof(int))
                };
            }
            else if (nonGenericSearchType == typeof(SiteLink))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(SiteLink.Site), typeof(Site)),
                    new SearchProperty(nameof(SiteLink.Type), typeof(SiteLinkType)),
                    new SearchProperty(nameof(SiteLink.Entity), typeof(Entity))
                };
            }
            else if (nonGenericSearchType == typeof(Skill))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(Skill.Name), typeof(string)),
                    new SearchProperty(nameof(Skill.Points), typeof(int)),
                    new SearchProperty(nameof(Skill.Rank), typeof(string))
                };
            }
            else if (nonGenericSearchType == typeof(Entity))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(Entity.Name), typeof(string)),
                    new SearchProperty(nameof(Entity.Race), typeof(string)),
                    new SearchProperty(nameof(Entity.IsCiv), "Is Civilization", typeof(bool)),
                    new SearchProperty(nameof(Entity.Sites), "All Sites", typeof(List<Site>), true),
                    new SearchProperty(nameof(Entity.CurrentSites), "Current Sites", typeof(List<Site>), true),
                    new SearchProperty(nameof(Entity.LostSites), "Lost Sites", typeof(List<Site>), true),
                    new SearchProperty(nameof(Entity.Groups), "Groups", typeof(List<Entity>), true),
                    new SearchProperty(nameof(Entity.AllLeaders), "Leaders", typeof(List<HistoricalFigure>), true),
                    new SearchProperty(nameof(Entity.Worshipped), "Worshipped", typeof(List<HistoricalFigure>), true),
                    new SearchProperty(nameof(Entity.Wars), "Wars", typeof(List<War>), true),
                    new SearchProperty(nameof(Entity.WarsAttacking), "Wars (Attacking)", typeof(List<War>), true),
                    new SearchProperty(nameof(Entity.WarsDefending), "Wars (Defending)", typeof(List<War>), true),
                    new SearchProperty(nameof(Entity.WarKillDeathRatio), "Kills : Deaths", typeof(double)),
                    new SearchProperty(nameof(Entity.PopulationsAsList), "Populations", typeof(List<string>))
                };
            }
            else if (nonGenericSearchType == typeof(Site))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(Site.Name), typeof(string)),
                    new SearchProperty(nameof(Site.Type), typeof(string)),
                    new SearchProperty(nameof(Site.UntranslatedName), "Untranslated Name", typeof(string)),
                    new SearchProperty(nameof(Site.Deaths), "Deaths", typeof(List<string>)),
                    new SearchProperty(nameof(Site.NotableDeaths), "Notable Deaths", typeof(List<HistoricalFigure>), true),
                    new SearchProperty(nameof(Site.Battles), "Battles", typeof(List<Battle>), true),
                    new SearchProperty(nameof(Site.Conquerings), "Conquerings", typeof(List<SiteConquered>), true),
                    new SearchProperty(nameof(Site.CurrentOwner), "Current Owner", typeof(Entity)),
                    new SearchProperty(nameof(Site.PreviousOwners), "Previous Owners", typeof(List<Entity>), true),
                    new SearchProperty(nameof(Site.Connections), typeof(List<Site>)),
                    new SearchProperty(nameof(Site.PopulationsAsList), "Populations", typeof(List<string>)),
                    new SearchProperty(nameof(Site.BeastAttacks), "Beast Attacks", typeof(List<BeastAttack>), true)
                };
            }
            else if (nonGenericSearchType == typeof(WorldRegion))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(WorldRegion.Name), typeof(string)),
                    new SearchProperty(nameof(WorldRegion.Type), typeof(string)),
                    new SearchProperty(nameof(WorldRegion.Battles), "Battles", typeof(List<Battle>), true),
                    new SearchProperty(nameof(WorldRegion.Deaths), "Deaths", typeof(List<string>)),
                    new SearchProperty(nameof(WorldRegion.NotableDeaths), "Notable Deaths", typeof(List<HistoricalFigure>), true),
                    new SearchProperty(nameof(WorldRegion.SquareTiles), typeof(int)),
                };
            }
            else if (nonGenericSearchType == typeof(UndergroundRegion))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(UndergroundRegion.Type), typeof(string)),
                    new SearchProperty(nameof(UndergroundRegion.Depth), typeof(int)),
                    new SearchProperty(nameof(UndergroundRegion.SquareTiles), typeof(int))
                };
            }
            else if (nonGenericSearchType == typeof(War))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(War.Name), typeof(string)),
                    new SearchProperty(nameof(War.StartYear), "Start Year", typeof(int)),
                    new SearchProperty(nameof(War.EndYear), "End Year", typeof(int)),
                    new SearchProperty(nameof(War.Length), typeof(int)),
                    new SearchProperty(nameof(War.Attacker), typeof(Entity)),
                    new SearchProperty(nameof(War.Defender), typeof(Entity)),
                    new SearchProperty(nameof(War.Battles), "Battles", typeof(List<Battle>), true),
                    new SearchProperty(nameof(War.SitesLost), "Sites Lost", typeof(List<Site>), true),
                    new SearchProperty(nameof(War.Deaths), "Deaths", typeof(Dictionary<string, int>)),
                    new SearchProperty(nameof(War.AttackerBattleVictories), "Attacker Victories", typeof(List<Battle>), true),
                    new SearchProperty(nameof(War.DefenderBattleVictories), "Defender Victories", typeof(List<Battle>), true),
                    new SearchProperty(nameof(War.AttackerConquerings), "Attacker Conquerings", typeof(List<SiteConquered>), true),
                    new SearchProperty(nameof(War.DefenderConquerings), "Defender Conquerings", typeof(List<SiteConquered>), true),
                    new SearchProperty(nameof(War.AttackerSitesLost), "Attacker Sites Lost", typeof(List<Site>), true),
                    new SearchProperty(nameof(War.DefenderSitesLost), "Defender Sites Lost", typeof(List<Site>), true),
                    new SearchProperty(nameof(War.AttackerToDefenderVictories), "Attacker : Defender (Victories)", typeof(double)),
                    new SearchProperty(nameof(War.AttackerToDefenderKills), "Attacker : Defender (Kills)", typeof(double))
                };
            }
            else if (nonGenericSearchType == typeof(Battle))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(Battle.Name), typeof(string)),
                    new SearchProperty(nameof(Battle.Site), typeof(Site)),
                    new SearchProperty(nameof(Battle.Region), typeof(WorldRegion)),
                    new SearchProperty(nameof(Battle.StartYear), "Year", typeof(int)),
                    new SearchProperty(nameof(Battle.Attacker), typeof(Entity)),
                    new SearchProperty(nameof(Battle.Defender), typeof(Entity)),
                    new SearchProperty(nameof(Battle.AttackersAsList), "Attackers", typeof(List<string>)),
                    new SearchProperty(nameof(Battle.DefendersAsList), "Defenders", typeof(List<string>)),
                    new SearchProperty(nameof(Battle.NotableAttackers), "Notable Attackers", typeof(List<HistoricalFigure>), true),
                    new SearchProperty(nameof(Battle.NotableDefenders), "Notable Defenders", typeof(List<HistoricalFigure>), true),
                    new SearchProperty(nameof(Battle.AttackersToDefenders), "Attackers : Defenders", typeof(double)),
                    new SearchProperty(nameof(Battle.AttackersToDefendersRemaining), "Attackers : Defenders (Remaining)", typeof(double)),
                    new SearchProperty(nameof(Battle.Deaths),"Deaths", typeof(Dictionary<string, int>)),
                    new SearchProperty(nameof(Battle.NotableDeaths), "Notable Deaths", typeof(List<HistoricalFigure>), true),
                    new SearchProperty(nameof(Battle.NonCombatants), "Non-Combatants", typeof(List<HistoricalFigure>), true),
                    new SearchProperty(nameof(Battle.Victor), typeof(Entity)),
                    new SearchProperty(nameof(Battle.Outcome), typeof(BattleOutcome)),
                    new SearchProperty(nameof(Battle.Conquering), typeof(SiteConquered))
                };
            }
            else if (nonGenericSearchType == typeof(SiteConquered))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(SiteConquered.Ordinal), typeof(int)),
                    new SearchProperty(nameof(SiteConquered.ConquerType), "Conquered By", typeof(SiteConqueredType)),
                    new SearchProperty(nameof(SiteConquered.Site), typeof(Site)),
                    new SearchProperty(nameof(SiteConquered.StartYear), "Year", typeof(int)),
                    new SearchProperty(nameof(SiteConquered.Attacker), typeof(Entity)),
                    new SearchProperty(nameof(SiteConquered.Defender), typeof(Entity)),
                    new SearchProperty(nameof(SiteConquered.Battle), typeof(Battle)),
                    new SearchProperty(nameof(SiteConquered.Deaths), "Deaths", typeof(List<HistoricalFigure>), true)
                };

            }
            else if (nonGenericSearchType == typeof(BeastAttack))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(BeastAttack.Ordinal), typeof(int)),
                    new SearchProperty(nameof(BeastAttack.Site), "Site", typeof(Site), true),
                    new SearchProperty(nameof(BeastAttack.Defender), typeof(Entity)),
                    new SearchProperty(nameof(BeastAttack.Beast), typeof(HistoricalFigure)),
                    new SearchProperty(nameof(BeastAttack.StartYear), "Year", typeof(int)),
                    new SearchProperty(nameof(BeastAttack.Deaths), "Deaths", typeof(List<HistoricalFigure>), true)
                };
            }
            else if (nonGenericSearchType == typeof(Artifact))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(Artifact.Name), typeof(string)),
                    new SearchProperty(nameof(Artifact.Item), typeof(string)),
                    new SearchProperty(nameof(Artifact.Type), typeof(string)),
                    new SearchProperty(nameof(Artifact.SubType), "Sub Type", typeof(string)),
                    new SearchProperty(nameof(Artifact.Material), typeof(string)),
                    new SearchProperty(nameof(Artifact.Creator), typeof(HistoricalFigure)),
                    new SearchProperty(nameof(Artifact.Holder), typeof(HistoricalFigure)),
                    new SearchProperty(nameof(Artifact.Region), typeof(WorldRegion))
                };
            }
            else if (nonGenericSearchType == typeof(HfDied))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(HfDied.Slayer), typeof(HistoricalFigure)),
                    new SearchProperty(nameof(HfDied.HistoricalFigure), "Historical Figure", typeof(HistoricalFigure)),
                    new SearchProperty(nameof(HfDied.Cause), typeof(DeathCause)),
                    new SearchProperty(nameof(HfDied.Site), typeof(Site)),
                    new SearchProperty(nameof(HfDied.Region), typeof(WorldRegion))
                };
            }
            else if (nonGenericSearchType == typeof(HfAbducted))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(HfAbducted.Snatcher), typeof(HistoricalFigure)),
                    new SearchProperty(nameof(HfAbducted.Target), typeof(HistoricalFigure)),
                    new SearchProperty(nameof(HfAbducted.Site), typeof(Site))
                };
            }
            else if (nonGenericSearchType == typeof(Battle.Squad))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(Battle.Squad.Race), typeof(string)),
                    new SearchProperty(nameof(Battle.Squad.Numbers), typeof(int)),
                    new SearchProperty(nameof(Battle.Squad.Deaths), typeof(int))
                };
            }
            else if (nonGenericSearchType == typeof(Population))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(Population.Race), typeof(string)),
                    new SearchProperty(nameof(Population.Count), typeof(int))
                };
            }
            else if (nonGenericSearchType == typeof(HistoricalFigure.Position))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(HistoricalFigure.Position.Entity), typeof(Entity)),
                    new SearchProperty(nameof(HistoricalFigure.Position.Title), typeof(string)),
                    new SearchProperty(nameof(HistoricalFigure.Position.Began), typeof(int)),
                    new SearchProperty(nameof(HistoricalFigure.Position.Ended), typeof(int)),
                    new SearchProperty(nameof(HistoricalFigure.Position.Length), typeof(int))
                };
            }
            else if (nonGenericSearchType == typeof(HistoricalFigure.State))
            {
                searchProperties = new List<SearchProperty>
                {
                    new SearchProperty(nameof(HistoricalFigure.State.HfState), "State", typeof(HfState)),
                    new SearchProperty(nameof(HistoricalFigure.State.StartYear), "Start Year", typeof(int)),
                    new SearchProperty(nameof(HistoricalFigure.State.EndYear), "End Year", typeof(int))
                };
            }
            else if (searchType == typeof(List<int>))
            {
                searchProperties = new List<SearchProperty> { new SearchProperty("Value", typeof(int)) };
            }
            else if (searchType == typeof(List<string>))
            {
                searchProperties = new List<SearchProperty> { new SearchProperty("Value", typeof(string)) };
            }

            if (nonGenericSearchType.BaseType == typeof(WorldObject))
            {
                searchProperties.Add(new SearchProperty(nameof(WorldObject.Events), typeof(List<WorldEvent>)));
                searchProperties.Add(new SearchProperty(nameof(WorldObject.FilteredEvents), "Events (Filtered)", typeof(List<WorldEvent>)));
            }

            if (nonGenericSearchType.BaseType == typeof(EventCollection))
            {
                searchProperties.Add(new SearchProperty(nameof(EventCollection.AllEvents), "Events", typeof(List<WorldEvent>)));
                searchProperties.Add(new SearchProperty(nameof(EventCollection.FilteredEvents), "Events (Filtered)", typeof(List<WorldEvent>)));
            }

            if (nonGenericSearchType.BaseType == typeof(WorldEvent))
            {
                searchProperties.Add(new SearchProperty(nameof(WorldEvent.Year), typeof(int)));
            }

            foreach (SearchProperty property in searchProperties)
            {
                if (!noSubProperties)
                {
                    property.SetSubProperties();
                }
            }

            return searchProperties;
        }

        public static List<QueryComparer> GetComparers(Type type)
        {
            List<QueryComparer> comparers = new List<QueryComparer>();
            if (type == null)
            {
                return comparers;
            }

            if (type == typeof(string))
            {
                comparers = new List<QueryComparer> { QueryComparer.Equals, QueryComparer.Contains, QueryComparer.StartsWith, QueryComparer.EndsWith, QueryComparer.NotEqual, QueryComparer.NotContains, QueryComparer.NotStartsWith, QueryComparer.NotEndsWith };
            }
            else if (type == typeof(int) || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) || type == typeof(double))
            {
                comparers = new List<QueryComparer> { QueryComparer.GreaterThan, QueryComparer.LessThan, QueryComparer.Equals };
            }
            else if (type == typeof(bool) || type.IsEnum)
            {
                comparers = new List<QueryComparer> { QueryComparer.Equals, QueryComparer.NotEqual };
            }

            return comparers;
        }

        public static string ComparerToString(QueryComparer comparer)
        {
            switch (comparer)
            {
                case QueryComparer.Contains: return "Contains";
                case QueryComparer.EndsWith: return "Ends With";
                case QueryComparer.Equals: return "Equals";
                case QueryComparer.NotEqual: return "Doesn't Equal";
                case QueryComparer.GreaterThan: return "Greater Than";
                case QueryComparer.Is: return "Is";
                case QueryComparer.LessThan: return "Less Than";
                case QueryComparer.StartsWith: return "Starts With";
                case QueryComparer.NotStartsWith: return "Doesn't Start With";
                case QueryComparer.NotEndsWith: return "Doesn't End With";
                case QueryComparer.NotContains: return "Doesn't Contain";
                default: return comparer.ToString();
            }
        }

        public static QueryComparer StringToComparer(string comparer)
        {
            switch (comparer)
            {
                case "Contains": return QueryComparer.Contains;
                case "Ends With": return QueryComparer.EndsWith;
                case "Equals": return QueryComparer.Equals;
                case "Greater Than": return QueryComparer.GreaterThan;
                case "Is": return QueryComparer.Is;
                case "Less Than": return QueryComparer.LessThan;
                case "Starts With": return QueryComparer.StartsWith;
                case "Doesn't Equal": return QueryComparer.NotEqual;
                default: return QueryComparer.All;
            }
        }
    }
}

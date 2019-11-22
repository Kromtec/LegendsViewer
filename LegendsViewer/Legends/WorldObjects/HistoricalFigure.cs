using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.WorldObjects
{
    public class HistoricalFigure : WorldObject
    {
        private static readonly List<string> KnownEntitySubProperties = new List<string> { "entity_id", "link_strength", "link_type", "position_profile_id", "start_year", "end_year" };
        private static readonly List<string> KnownSiteLinkSubProperties = new List<string> { "link_type", "site_id", "sub_id", "entity_id", "occupation_id" };
        private static readonly List<string> KnownEntitySquadLinkProperties = new List<string> { "squad_id", "squad_position", "entity_id", "start_year", "end_year" };

        public static readonly string ForceNatureIcon = "<i class=\"glyphicon fa-fw glyphicon-leaf\"></i>";
        public static readonly string DeityIcon = "<i class=\"fa fa-fw fa-sun-o\"></i>";
        public static readonly string NeuterIcon = "<i class=\"fa fa-fw fa-neuter\"></i>";
        public static readonly string FemaleIcon = "<i class=\"fa fa-fw fa-venus\"></i>";
        public static readonly string MaleIcon = "<i class=\"fa fa-fw fa-mars\"></i>";

        public static HistoricalFigure Unknown;
        public string Name { get; set; }

        private string ShortName
        {
            get
            {
                if (string.IsNullOrEmpty(_shortName))
                {
                    if (Name.IndexOf(" ", StringComparison.Ordinal) >= 2 && !Name.StartsWith("The "))
                    {
                        _shortName = Name.Substring(0, Name.IndexOf(" ", StringComparison.Ordinal));
                    }
                    else
                    {
                        _shortName = Name;
                    }
                }
                return _shortName;
            }
        }

        private string RaceString
        {
            get
            {
                if (string.IsNullOrEmpty(_raceString))
                {
                    _raceString = GetRaceString();
                }
                return _raceString;
            }
        }

        private string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    _title = GetAnchorTitle();
                }
                return _title;
            }
        }

        public string Race { get; set; }
        public string PreviousRace { get; set; }
        public string Caste { get; set; }
        public string AssociatedType { get; set; }
        public int EntityPopulationId { get; set; }
        public EntityPopulation EntityPopulation { get; set; }
        public HfState CurrentState { get; set; }
        public List<int> UsedIdentityIds { get; set; }
        public int CurrentIdentityId { get; set; }
        public List<Artifact> HoldingArtifacts { get; set; }
        public List<State> States { get; set; }
        public List<CreatureType> CreatureTypes { get; set; }
        public List<HistoricalFigureLink> RelatedHistoricalFigures { get; set; }
        public List<EntityLink> RelatedEntities { get; set; }
        public List<EntityReputation> Reputations { get; set; }
        public List<RelationshipProfileHf> RelationshipProfiles { get; set; }
        public List<SiteLink> RelatedSites { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Structure> DedicatedStructures { get; set; }
        public int Age { get; set; }
        public int Appeared { get; set; }
        public int BirthYear { get; set; }
        public int BirthSeconds72 { get; set; }
        public int DeathYear { get; set; }
        public int DeathSeconds72 { get; set; }
        public DeathCause DeathCause { get; set; }
        public List<string> ActiveInteractions { get; set; }
        public List<string> InteractionKnowledge { get; set; }
        public string Goal { get; set; }
        public string Interaction { get; set; }

        public HistoricalFigure LineageCurseParent { get; set; }
        public List<HistoricalFigure> LineageCurseChilds { get; set; }

        public List<string> JourneyPets { get; set; }
        public List<HfDied> NotableKills { get; set; }
        public List<HistoricalFigure> HFKills => NotableKills.Select(kill => kill.HistoricalFigure).ToList();
        public List<HistoricalFigure> Abductions { get { return Events.OfType<HfAbducted>().Where(abduction => abduction.Snatcher == this).Select(abduction => abduction.Target).ToList(); } set { } }
        public int Abducted => Events.OfType<HfAbducted>().Count(abduction => abduction.Target == this);
        public List<string> Spheres { get; set; }
        public List<Battle> Battles { get; set; }
        public List<Battle> BattlesAttacking => Battles.Where(battle => battle.NotableAttackers.Contains(this)).ToList();
        public List<Battle> BattlesDefending => Battles.Where(battle => battle.NotableDefenders.Contains(this)).ToList();
        public List<Battle> BattlesNonCombatant => Battles.Where(battle => battle.NonCombatants.Contains(this)).ToList();
        public List<Position> Positions { get; set; }
        public Entity WorshippedBy { get; set; }
        public List<BeastAttack> BeastAttacks { get; set; }
        public bool Alive
        {
            get
            {
                if (DeathYear == -1)
                {
                    return true;
                }

                return false;
            }
            set { }
        }
        public bool Deity { get; set; }
        public bool Skeleton { get; set; }
        public bool Force { get; set; }
        public bool Zombie { get; set; }
        public bool Ghost { get; set; }
        public bool Animated { get; set; }
        public string AnimatedType { get; set; }
        public bool Adventurer { get; set; }
        public string BreedId { get; set; }

        public static List<string> Filters;
        private string _shortName;
        private string _raceString;
        private string _title;

        public override List<WorldEvent> FilteredEvents
        {
            get { return Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }
        public HistoricalFigure()
        {
            Initialize();
            Name = "an unknown creature";
            Race = "UNKNOWN";
            Caste = "UNKNOWN";
            AssociatedType = "UNKNOWN";
        }
        public override string ToString() { return Name; }
        public HistoricalFigure(List<Property> properties, World world)
            : base(properties, world)
        {
            Initialize();
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "appeared": Appeared = Convert.ToInt32(property.Value); break;
                    case "birth_year": BirthYear = Convert.ToInt32(property.Value); break;
                    case "birth_seconds72": BirthSeconds72 = Convert.ToInt32(property.Value); break;
                    case "death_year": DeathYear = Convert.ToInt32(property.Value); break;
                    case "death_seconds72": DeathSeconds72 = Convert.ToInt32(property.Value); break;
                    case "name": Name = Formatting.InitCaps(property.Value.Replace("'", "`")); break;
                    case "race": Race = string.Intern(Formatting.FormatRace(property.Value)); break;
                    case "caste": Caste = string.Intern(Formatting.InitCaps(property.Value.ToLower().Replace('_', ' '))); break;
                    case "associated_type": AssociatedType = string.Intern(Formatting.InitCaps(property.Value.ToLower().Replace('_', ' '))); break;
                    case "deity": Deity = true; property.Known = true; break;
                    case "skeleton": Skeleton = true; property.Known = true; break;
                    case "force": Force = true; property.Known = true; Race = "Force"; break;
                    case "zombie": Zombie = true; property.Known = true; break;
                    case "ghost": Ghost = true; property.Known = true; break;
                    case "hf_link": //Will be processed after all HFs have been loaded
                        world.AddHFtoHfLink(this, property);
                        property.Known = true;
                        List<string> knownSubProperties = new List<string> { "hfid", "link_strength", "link_type" };
                        if (property.SubProperties != null)
                        {
                            foreach (string subPropertyName in knownSubProperties)
                            {
                                Property subProperty = property.SubProperties.FirstOrDefault(property1 => property1.Name == subPropertyName);
                                if (subProperty != null)
                                {
                                    subProperty.Known = true;
                                }
                            }
                        }

                        break;
                    case "entity_link":
                    case "entity_former_position_link":
                    case "entity_position_link":
                        world.AddHFtoEntityLink(this, property);
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            foreach (string subPropertyName in KnownEntitySubProperties)
                            {
                                Property subProperty = property.SubProperties.FirstOrDefault(property1 => property1.Name == subPropertyName);
                                if (subProperty != null)
                                {
                                    subProperty.Known = true;
                                }
                            }
                        }

                        break;
                    case "entity_reputation":
                        world.AddReputation(this, property);
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            foreach (string subPropertyName in Reputation.KnownReputationSubProperties)
                            {
                                Property subProperty = property.SubProperties.FirstOrDefault(property1 => property1.Name == subPropertyName);
                                if (subProperty != null)
                                {
                                    subProperty.Known = true;
                                }
                            }
                        }

                        break;
                    case "entity_squad_link":
                    case "entity_former_squad_link":
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            foreach (string subPropertyName in KnownEntitySquadLinkProperties)
                            {
                                Property subProperty = property.SubProperties.FirstOrDefault(property1 => property1.Name == subPropertyName);
                                if (subProperty != null)
                                {
                                    subProperty.Known = true;
                                }
                            }
                        }

                        break;
                    case "relationship_profile_hf":
                    case "relationship_profile_hf_visual":
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            RelationshipProfiles.Add(new RelationshipProfileHf(property.SubProperties));
                        }
                        break;
                    case "site_link":
                        world.AddHFtoSiteLink(this, property);
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            foreach (string subPropertyName in KnownSiteLinkSubProperties)
                            {
                                Property subProperty = property.SubProperties.FirstOrDefault(property1 => property1.Name == subPropertyName);
                                if (subProperty != null)
                                {
                                    subProperty.Known = true;
                                }
                            }
                        }

                        break;
                    case "hf_skill":
                        if (property.SubProperties != null)
                        {
                            var skill = new Skill(property.SubProperties);
                            property.Known = SkillDictionary.IsKnownSkill(skill);
                            Skills.Add(skill);
                        }
                        break;
                    case "active_interaction": ActiveInteractions.Add(string.Intern(property.Value)); break;
                    case "interaction_knowledge": InteractionKnowledge.Add(string.Intern(property.Value)); break;
                    case "animated": Animated = true; property.Known = true; break;
                    case "animated_string": if (AnimatedType != "") { throw new Exception("Animated Type already exists."); } AnimatedType = Formatting.InitCaps(property.Value); break;
                    case "journey_pet": JourneyPets.Add(Formatting.FormatRace(property.Value)); break;
                    case "goal": Goal = Formatting.InitCaps(property.Value); break;
                    case "sphere": Spheres.Add(property.Value); break;
                    case "current_identity_id": CurrentIdentityId = Convert.ToInt32(property.Value); break;
                    case "used_identity_id": UsedIdentityIds.Add(Convert.ToInt32(property.Value)); break;
                    case "ent_pop_id": EntityPopulationId = Convert.ToInt32(property.Value); break;
                    case "holds_artifact":
                        var artifact = world.GetArtifact(Convert.ToInt32(property.Value));
                        HoldingArtifacts.Add(artifact);
                        artifact.Holder = this;
                        break;
                    case "adventurer":
                        Adventurer = true;
                        property.Known = true;
                        break;
                    case "breed_id":
                        BreedId = property.Value;
                        if (!string.IsNullOrWhiteSpace(BreedId))
                        {
                            if (world.Breeds.ContainsKey(BreedId))
                            {
                                world.Breeds[BreedId].Add(this);
                            }
                            else
                            {
                                world.Breeds.Add(BreedId, new List<HistoricalFigure> { this });
                            }
                        }
                        break;
                    case "sex": property.Known = true; break;
                }
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = !string.IsNullOrWhiteSpace(AnimatedType) ? Formatting.InitCaps(AnimatedType) : "(Unnamed)";
            }
            if (Adventurer)
            {
                world.AddPlayerRelatedDwarfObjects(this);
            }
        }

        private void Initialize()
        {
            Appeared = BirthYear = BirthSeconds72 = DeathYear = DeathSeconds72 = EntityPopulationId = -1;
            Name = Race = Caste = AssociatedType = "";
            DeathCause = DeathCause.None;
            CurrentState = HfState.None;
            NotableKills = new List<HfDied>();
            Battles = new List<Battle>();
            Positions = new List<Position>();
            Spheres = new List<string>();
            BeastAttacks = new List<BeastAttack>();
            States = new List<State>();
            CreatureTypes = new List<CreatureType>();
            RelatedHistoricalFigures = new List<HistoricalFigureLink>();
            RelatedEntities = new List<EntityLink>();
            Reputations = new List<EntityReputation>();
            RelationshipProfiles = new List<RelationshipProfileHf>();
            RelatedSites = new List<SiteLink>();
            Skills = new List<Skill>();
            AnimatedType = "";
            Goal = "";
            ActiveInteractions = new List<string>();
            PreviousRace = "";
            InteractionKnowledge = new List<string>();
            JourneyPets = new List<string>();
            HoldingArtifacts = new List<Artifact>();
            LineageCurseChilds = new List<HistoricalFigure>();
            DedicatedStructures = new List<Structure>();
            UsedIdentityIds = new List<int>();
        }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            if (this == Unknown)
            {
                return Name;
            }

            if (link)
            {
                string icon = GetIcon();
                if (pov == null || pov != this)
                {
                    if (pov != null && pov.GetType() == typeof(BeastAttack) && (pov as BeastAttack)?.Beast == this) //Highlight Beast when printing Beast Attack Log
                    {
                        return icon + "<a href=\"hf#" + Id + "\" title=\"" + Title + "\"><font color=#339900>" + ShortName + "</font></a>";
                    }

                    if (worldEvent != null)
                    {
                        return "the " + GetRaceStringByWorldEvent(worldEvent) + " " + icon + "<a href=\"hf#" + Id + "\" title=\"" + Title + "\">" + Name + "</a>";
                    }
                    return "the " + RaceString + " " + icon + "<a href=\"hf#" + Id + "\" title=\"" + Title + "\">" + Name + "</a>";
                }
                return "<a href=\"hf#" + Id + "\" title=\"" + Title + "\">" + HtmlStyleUtil.CurrentDwarfObject(ShortName) + "</a>";
            }
            if (pov == null || pov != this)
            {
                if (worldEvent != null)
                {
                    return GetRaceStringByWorldEvent(worldEvent) + " " + Name;
                }
                return RaceString + " " + Name;
            }
            return ShortName;
        }

        public override string GetIcon()
        {
            if (Force)
            {
                return ForceNatureIcon;
            }
            if (Deity)
            {
                return DeityIcon;
            }
            if (Caste == "Female")
            {
                return FemaleIcon;
            }
            if (Caste == "Male")
            {
                return MaleIcon;
            }
            if (Caste == "Default")
            {
                return NeuterIcon;
            }
            return "";
        }

        private string GetAnchorTitle()
        {
            string title = "";
            if (Positions.Any())
            {
                title += GetLastNoblePosition();
                title += "&#13";
            }
            if (!string.IsNullOrWhiteSpace(AssociatedType) && AssociatedType != "Standard")
            {
                title += AssociatedType;
                title += "&#13";
            }
            title += !string.IsNullOrWhiteSpace(Caste) && Caste != "Default" ? Caste + " " : "";
            title += Formatting.InitCaps(RaceString);
            if (!Deity && !Force)
            {
                title += " (" + BirthYear + " - " + (DeathYear == -1 ? "Present" : DeathYear.ToString()) + ")";
            }
            title += "&#13";
            title += "Events: " + Events.Count;
            return title;
        }

        public string GetLastNoblePosition()
        {
            string title = "";
            if (Positions.Any())
            {
                string positionName = "";
                var hfposition = Positions.Last();
                EntityPosition position = hfposition.Entity.EntityPositions.FirstOrDefault(pos => pos.Name.ToLower() == hfposition.Title.ToLower());
                if (position != null)
                {
                    positionName = position.GetTitleByCaste(Caste);
                }
                else
                {
                    positionName = hfposition.Title;
                }
                title += (hfposition.Ended == -1 ? "" : "Former ") + positionName + " of " + hfposition.Entity.Name;
            }
            return title;
        }

        public string ToTreeLeafLink(DwarfObject pov = null)
        {
            string dead = DeathYear != -1 ? "<br/>" + HtmlStyleUtil.SymbolDead : "";
            if (pov == null || pov != this)
            {
                return "<a " + (Deity ? "class=\"hf_deity\"" : "") + " href=\"hf#" + Id + "\" title=\"" + Title + "\">" + Race + (Deity ? " Deity" : "") + "<br/>" + Name + dead + "</a>";
            }
            return "<a " + (Deity ? "class=\"hf_deity\"" : "") + " title=\"" + Title + "\">" + Race + (Deity ? " Deity" : "") + "<br/>" + HtmlStyleUtil.CurrentDwarfObject(Name) + dead + "</a>";
        }

        public class Position
        {
            public Entity Entity { get; set; }
            public int Began { get; set; }
            public int Ended { get; set; }
            public string Title { get; set; }
            public int Length { get; set; }
            public Position(Entity civ, int began, int ended, string title) { Entity = civ; Began = began; Ended = ended; Title = title; }
        }

        public class State
        {
            public HfState HfState { get; set; }
            public int StartYear { get; set; }
            public int EndYear { get; set; }

            public State(HfState state, int start)
            {
                HfState = state;
                StartYear = start;
                EndYear = -1;
            }
        }

        public class CreatureType
        {
            public string Type { get; set; }
            public int StartYear { get; set; }
            public int StartMonth { get; set; }
            public int StartDay { get; set; }

            public CreatureType(string type, int startYear, int startMonth, int startDay)
            {
                Type = type;
                StartYear = startYear;
                StartMonth = startMonth;
                StartDay = startDay;
            }

            public CreatureType(string type, WorldEvent worldEvent) : this(type, worldEvent.Year, worldEvent.Month, worldEvent.Day)
            {
            }
        }

        public string CasteNoun(bool owner = false)
        {
            if (Caste.ToLower() == "male")
            {
                if (owner)
                {
                    return "his";
                }
                else
                {
                    return "he";
                }
            }

            if (Caste.ToLower() == "female")
            {
                if (owner)
                {
                    return "her";
                }
                else
                {
                    return "she";
                }
            }

            if (owner)
            {
                return "its";
            }

            return "it";
        }

        public string GetRaceTitleString()
        {
            string hfraceString = "";

            if (Ghost)
            {
                hfraceString += "ghostly ";
            }

            if (Skeleton)
            {
                hfraceString += "skeletal ";
            }

            if (Zombie)
            {
                hfraceString += "zombie ";
            }

            if (Caste.ToUpper() == "MALE")
            {
                hfraceString += "male ";
            }
            else if (Caste.ToUpper() == "FEMALE")
            {
                hfraceString += "female ";
            }

            hfraceString += GetRaceString();

            return Formatting.AddArticle(hfraceString);
        }

        public string GetRaceString()
        {
            if (Deity)
            {
                return Race.ToLower() + " deity";
            }

            if (Force)
            {
                return "force";
            }

            string raceString = "";
            if (!string.IsNullOrWhiteSpace(PreviousRace))
            {
                raceString += PreviousRace.ToLower() + " turned ";
            }
            else if (!string.IsNullOrWhiteSpace(AnimatedType) && !Name.Contains("Corpse"))
            {
                raceString += AnimatedType.ToLower();
            }
            else
            {
                raceString += Race.ToLower();
            }

            foreach (var creatureType in CreatureTypes)
            {
                raceString += " " + creatureType.Type;
            }

            return raceString;
        }

        private string GetRaceStringByWorldEvent(WorldEvent worldEvent)
        {
            return GetRaceStringForTimeStamp(worldEvent.Year, worldEvent.Month, worldEvent.Day);
        }

        private string GetRaceStringForTimeStamp(int year, int month, int day)
        {
            if (!CreatureTypes.Any())
            {
                return RaceString;
            }

            List<CreatureType> relevantCreatureTypes = GetRelevantCreatureTypesByTimeStamp(year, month, day);
            string raceString = "";
            if (!string.IsNullOrWhiteSpace(PreviousRace))
            {
                raceString += PreviousRace.ToLower();
            }
            else if (!string.IsNullOrWhiteSpace(AnimatedType))
            {
                raceString += AnimatedType.ToLower();
            }
            else
            {
                raceString += Race.ToLower();
            }

            foreach (var creatureType in relevantCreatureTypes)
            {
                raceString += " " + creatureType.Type;
            }

            return raceString;
        }

        private List<CreatureType> GetRelevantCreatureTypesByTimeStamp(int year, int month, int day)
        {
            List<CreatureType> relevantCreatureTypes = new List<CreatureType>();
            foreach (var creatureType in CreatureTypes)
            {
                if (creatureType.StartYear < year)
                {
                    relevantCreatureTypes.Add(creatureType);
                }
                else if (creatureType.StartYear == year)
                {
                    if (creatureType.StartMonth < month)
                    {
                        relevantCreatureTypes.Add(creatureType);
                    }
                    else if (creatureType.StartMonth == month && creatureType.StartDay < day)
                    {
                        relevantCreatureTypes.Add(creatureType);
                    }
                }
            }
            return relevantCreatureTypes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.IncidentalEvents;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.EventCollections
{
    public class Battle : EventCollection
    {
        public static readonly string Icon = "<i class=\"glyphicon fa-fw glyphicon-bishop\"></i>";

        public string Name { get; set; }
        public BattleOutcome Outcome { get; set; }
        public Location Coordinates { get; set; }
        public WorldRegion Region { get; set; }
        public UndergroundRegion UndergroundRegion { get; set; }
        public Site Site { get; set; }
        public SiteConquered Conquering { get; set; }
        public Entity Attacker { get; set; }
        public Entity Defender { get; set; }
        public Entity Victor { get; set; }
        public List<Squad> Attackers { get; set; }
        public List<Squad> Defenders { get; set; }
        public List<HistoricalFigure> NotableAttackers { get; set; }
        public List<HistoricalFigure> NotableDefenders { get; set; }
        public List<HistoricalFigure> NonCombatants { get; set; }
        public List<Squad> AttackerSquads { get; set; }
        public List<Squad> DefenderSquads { get; set; }
        public int AttackerCount { get { return NotableAttackers.Count + AttackerSquads.Sum(squad => squad.Numbers); } set { } }
        public int DefenderCount { get { return NotableDefenders.Count + DefenderSquads.Sum(squad => squad.Numbers); } set { } }
        public int AttackersRemainingCount { get { return Attackers.Sum(squad => squad.Numbers - squad.Deaths); } set { } }
        public int DefendersRemainingCount { get { return Defenders.Sum(squad => squad.Numbers - squad.Deaths); } set { } }
        public int DeathCount { get { return AttackerDeathCount + DefenderDeathCount; } set { } }
        public Dictionary<CreatureInfo, int> Deaths { get; set; }
        public List<HistoricalFigure> NotableDeaths {
            get
            {
                return NotableAttackers.Where(attacker => GetSubEvents().OfType<HfDied>()
                    .Count(death => death.HistoricalFigure == attacker) > 0)
                    .Concat(NotableDefenders.Where(defender => GetSubEvents().OfType<HfDied>().Count(death => death.HistoricalFigure == defender) > 0))
                    .ToList(); 
            } 
        }
        public int AttackerDeathCount { get; set; }
        public int DefenderDeathCount { get; set; }
        public double AttackersToDefenders
        {
            get
            {
                if (AttackerCount == 0 && DefenderCount == 0) return 0;
                if (DefenderCount == 0) return double.MaxValue;
                return Math.Round(AttackerCount / Convert.ToDouble(DefenderCount), 2);
            }
            set { }
        }
        public double AttackersToDefendersRemaining
        {
            get
            {
                if (AttackersRemainingCount == 0 && DefendersRemainingCount == 0) return 0;
                if (DefendersRemainingCount == 0) return double.MaxValue;
                return Math.Round(AttackersRemainingCount / Convert.ToDouble(DefendersRemainingCount), 2);
            }
            set { }
        }

        public bool IndividualMercenaries { get; set; }
        public bool CompanyMercenaries { get; set; }
        public Entity AttackingMercenaryEntity { get; set; }
        public Entity DefendingMercenaryEntity { get; set; }
        public bool AttackingSquadAnimated { get; set; }
        public bool DefendingSquadAnimated { get; set; }
        public List<Entity> AttackerSupportMercenaryEntities { get; set; }
        public List<Entity> DefenderSupportMercenaryEntities { get; set; }
        public List<HistoricalFigure> AttackerSupportMercenaryHfs { get; set; }
        public List<HistoricalFigure> DefenderSupportMercenaryHfs { get; set; }

        public static List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return AllEvents.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }

        public Battle(List<Property> properties, World world)
            : base(properties, world)
        {

            Initialize();

            var attackerSquadRaces = new List<CreatureInfo>();
            var attackerSquadEntityPopulation = new List<int>();
            var attackerSquadNumbers = new List<int>();
            var attackerSquadDeaths = new List<int>();
            var attackerSquadSite = new List<int>();
            var defenderSquadRaces = new List<CreatureInfo>();
            var defenderSquadEntityPopulation = new List<int>();
            var defenderSquadNumbers = new List<int>();
            var defenderSquadDeaths = new List<int>();
            var defenderSquadSite = new List<int>();
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "outcome":
                        switch (property.Value)
                        {
                            case "attacker won": Outcome = BattleOutcome.AttackerWon; break;
                            case "defender won": Outcome = BattleOutcome.DefenderWon; break;
                            default: Outcome = BattleOutcome.Unknown; world.ParsingErrors.Report("Unknown Battle Outcome: " + property.Value); break;
                        }
                        break;
                    case "name": Name = Formatting.InitCaps(property.Value); break;
                    case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                    case "war_eventcol": ParentCollection = world.GetEventCollection(Convert.ToInt32(property.Value)); break;
                    case "subregion_id": Region = world.GetRegion(Convert.ToInt32(property.Value)); break;
                    case "feature_layer_id": UndergroundRegion = world.GetUndergroundRegion(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "attacking_hfid": NotableAttackers.Add(world.GetHistoricalFigure(Convert.ToInt32(property.Value))); break;
                    case "defending_hfid": NotableDefenders.Add(world.GetHistoricalFigure(Convert.ToInt32(property.Value))); break;
                    case "attacking_squad_race": attackerSquadRaces.Add(world.GetCreatureInfo(property.Value)); break;
                    case "attacking_squad_entity_pop": attackerSquadEntityPopulation.Add(Convert.ToInt32(property.Value)); break;
                    case "attacking_squad_number":
                        int attackerSquadNumber = Convert.ToInt32(property.Value);
                        attackerSquadNumbers.Add(attackerSquadNumber < 0 || attackerSquadNumber > Squad.MAX_SIZE ? Squad.MAX_SIZE : attackerSquadNumber);
                        break;
                    case "attacking_squad_deaths":
                        int attackerSquadDeath = Convert.ToInt32(property.Value);
                        attackerSquadDeaths.Add(attackerSquadDeath < 0 || attackerSquadDeath > Squad.MAX_SIZE ? Squad.MAX_SIZE : attackerSquadDeath);
                        break;
                    case "attacking_squad_site": attackerSquadSite.Add(Convert.ToInt32(property.Value)); break;
                    case "defending_squad_race": defenderSquadRaces.Add(world.GetCreatureInfo(property.Value)); break;
                    case "defending_squad_entity_pop": defenderSquadEntityPopulation.Add(Convert.ToInt32(property.Value)); break;
                    case "defending_squad_number":
                        int defenderSquadNumber = Convert.ToInt32(property.Value);
                        defenderSquadNumbers.Add(defenderSquadNumber < 0 || defenderSquadNumber > Squad.MAX_SIZE ? Squad.MAX_SIZE : defenderSquadNumber);
                        break;
                    case "defending_squad_deaths":
                        int defenderSquadDeath = Convert.ToInt32(property.Value);
                        defenderSquadDeaths.Add(defenderSquadDeath < 0 || defenderSquadDeath > Squad.MAX_SIZE ? Squad.MAX_SIZE : defenderSquadDeath);
                        break;
                    case "defending_squad_site": defenderSquadSite.Add(Convert.ToInt32(property.Value)); break;
                    case "noncom_hfid": NonCombatants.Add(world.GetHistoricalFigure(Convert.ToInt32(property.Value))); break;
                    case "individual_merc": property.Known = true; IndividualMercenaries = true; break;
                    case "company_merc": property.Known = true; CompanyMercenaries = true; break;
                    case "attacking_merc_enid": 
                        AttackingMercenaryEntity = world.GetEntity(Convert.ToInt32(property.Value));
                        if (AttackingMercenaryEntity != null)
                        {
                            AttackingMercenaryEntity.Type = EntityType.MercenaryCompany;
                        }
                        break;
                    case "defending_merc_enid": 
                        DefendingMercenaryEntity = world.GetEntity(Convert.ToInt32(property.Value));
                        if (DefendingMercenaryEntity != null)
                        {
                            DefendingMercenaryEntity.Type = EntityType.MercenaryCompany;
                        }
                        break;
                    case "attacking_squad_animated": property.Known = true; AttackingSquadAnimated = true; break;
                    case "defending_squad_animated": property.Known = true; DefendingSquadAnimated = true; break;
                    case "a_support_merc_enid":
                        var attackerSupportMercenaryEntity = world.GetEntity(Convert.ToInt32(property.Value));
                        if (attackerSupportMercenaryEntity != null)
                        {
                            AttackerSupportMercenaryEntities.Add(attackerSupportMercenaryEntity);
                            attackerSupportMercenaryEntity.Type = EntityType.MercenaryCompany;
                        }
                        break;
                    case "d_support_merc_enid": 
                        var defenderSupportMercenaryEntity = world.GetEntity(Convert.ToInt32(property.Value));
                        if (defenderSupportMercenaryEntity != null)
                        {
                            DefenderSupportMercenaryEntities.Add(defenderSupportMercenaryEntity);
                            defenderSupportMercenaryEntity.Type = EntityType.MercenaryCompany;
                        }
                        break;
                    case "a_support_merc_hfid": AttackerSupportMercenaryHfs.Add(world.GetHistoricalFigure(Convert.ToInt32(property.Value))); break;
                    case "d_support_merc_hfid": DefenderSupportMercenaryHfs.Add(world.GetHistoricalFigure(Convert.ToInt32(property.Value))); break;
                }
            }

            if (Collection.OfType<AttackedSite>().Any())
            {
                Attacker = Collection.OfType<AttackedSite>().First().Attacker;
                Defender = Collection.OfType<AttackedSite>().First().Defender;
            }
            else if (Collection.OfType<FieldBattle>().Any())
            {
                Attacker = Collection.OfType<FieldBattle>().First().Attacker;
                Defender = Collection.OfType<FieldBattle>().First().Defender;
            }

            foreach (HistoricalFigure involvedHf in NotableAttackers.Union(NotableDefenders).Where(hf => hf != HistoricalFigure.Unknown))
            {
                involvedHf.Battles.Add(this);
                involvedHf.AddEventCollection(this);
                involvedHf.AddEvent(new BattleFought(involvedHf, this, World));
            }

            foreach (HistoricalFigure involvedSupportMercenaries in AttackerSupportMercenaryHfs.Union(DefenderSupportMercenaryHfs).Where(hf => hf != HistoricalFigure.Unknown))
            {
                involvedSupportMercenaries.Battles.Add(this);
                involvedSupportMercenaries.AddEventCollection(this);
                involvedSupportMercenaries.AddEvent(new BattleFought(involvedSupportMercenaries, this, World, true, true));
            }

            for (int i = 0; i < attackerSquadRaces.Count; i++)
            {
                AttackerSquads.Add(new Squad(attackerSquadRaces[i], attackerSquadNumbers[i], attackerSquadDeaths[i], attackerSquadSite[i], attackerSquadEntityPopulation[i]));
            }

            for (int i = 0; i < defenderSquadRaces.Count; i++)
            {
                DefenderSquads.Add(new Squad(defenderSquadRaces[i], defenderSquadNumbers[i], defenderSquadDeaths[i], defenderSquadSite[i], defenderSquadEntityPopulation[i]));
            }

            var groupedAttackerSquads = from squad in AttackerSquads
                                        group squad by squad.Race into squadRace
                                        select new { Race = squadRace.Key, Count = squadRace.Sum(squad => squad.Numbers), Deaths = squadRace.Sum(squad => squad.Deaths) };
            foreach (var squad in groupedAttackerSquads)
            {
                int attackerSquadNumber = squad.Count + NotableAttackers.Count(attacker => attacker?.Race?.Id == squad.Race.Id);
                int attackerSquadDeath = squad.Deaths + Collection.OfType<HfDied>().Count(death => death.HistoricalFigure?.Race == squad.Race && NotableAttackers.Contains(death.HistoricalFigure));
                Squad attackerSquad = new Squad(squad.Race, attackerSquadNumber, attackerSquadDeath, -1, -1);
                Attackers.Add(attackerSquad);
            }

            foreach (var attacker in NotableAttackers.Where(hf => Attackers.Count(squad => squad.Race == hf.Race) == 0).GroupBy(hf => hf.Race).Select(race => new { Race = race.Key, Count = race.Count() }))
            {
                var attackerDeath = Collection.OfType<HfDied>().Count(death => NotableAttackers.Contains(death.HistoricalFigure) && death.HistoricalFigure?.Race == attacker.Race);
                Attackers.Add(new Squad(attacker.Race, attacker.Count, attackerDeath, -1, -1));
            }

            var groupedDefenderSquads = from squad in DefenderSquads
                                        group squad by squad.Race into squadRace
                                        select new { Race = squadRace.Key, Count = squadRace.Sum(squad => squad.Numbers), Deaths = squadRace.Sum(squad => squad.Deaths) };
            foreach (var squad in groupedDefenderSquads)
            {
                int defenderSquadNumber = squad.Count + NotableDefenders.Count(defender => defender?.Race?.Id == squad.Race.Id);
                int defenderSquadDeath = squad.Deaths + Collection.OfType<HfDied>().Count(death => death.HistoricalFigure?.Race == squad.Race && NotableDefenders.Contains(death.HistoricalFigure));
                Defenders.Add(new Squad(squad.Race, defenderSquadNumber, defenderSquadDeath, -1, -1));
            }

            foreach (var defender in NotableDefenders.Where(hf => Defenders.Count(squad => squad.Race == hf.Race) == 0).GroupBy(hf => hf.Race).Select(race => new { Race = race.Key, Count = race.Count() }))
            {
                int defenderDeath = Collection.OfType<HfDied>().Count(death => NotableDefenders.Contains(death.HistoricalFigure) && death.HistoricalFigure.Race == defender.Race);
                Defenders.Add(new Squad(defender.Race, defender.Count, defenderDeath, -1, -1));
            }

            Deaths = new Dictionary<CreatureInfo, int>();
            foreach (Squad squad in Attackers.Concat(Defenders).Where(a => a.Race != null && a.Race != CreatureInfo.Unknown))
            {
                if (Deaths.ContainsKey(squad.Race))
                {
                    Deaths[squad.Race] += squad.Deaths;
                }
                else
                {
                    Deaths[squad.Race] = squad.Deaths;
                }
            }

            AttackerDeathCount = Attackers.Sum(attacker => attacker.Deaths);
            DefenderDeathCount = Defenders.Sum(defender => defender.Deaths);

            if (Outcome == BattleOutcome.AttackerWon)
            {
                Victor = Attacker;
            }
            else if (Outcome == BattleOutcome.DefenderWon)
            {
                Victor = Defender;
            }

            if (ParentCollection is War parentWar)
            {
                if (parentWar.Attacker == Attacker)
                {
                    parentWar.AttackerDeathCount += AttackerDeathCount;
                    parentWar.DefenderDeathCount += DefenderDeathCount;
                }
                else
                {
                    parentWar.AttackerDeathCount += DefenderDeathCount;
                    parentWar.DefenderDeathCount += AttackerDeathCount;
                }
                parentWar.DeathCount += attackerSquadDeaths.Sum() + defenderSquadDeaths.Sum() + Collection.OfType<HfDied>().Count();

                if (Attacker == parentWar.Attacker && Victor == Attacker)
                {
                    parentWar.AttackerVictories.Add(this);
                }
                else
                {
                    parentWar.DefenderVictories.Add(this);
                }
            }

            Site?.Warfare.Add(this);
            Region?.Battles.Add(this);
            UndergroundRegion?.Battles.Add(this);

            if (attackerSquadDeaths.Sum() + defenderSquadDeaths.Sum() + Collection.OfType<HfDied>().Count() == 0)
            {
                Notable = false;
            }

            if (attackerSquadNumbers.Sum() + NotableAttackers.Count > (defenderSquadNumbers.Sum() + NotableDefenders.Count) * 10 //NotableDefenders outnumbered 10 to 1
                && Victor == Attacker
                && AttackerDeathCount < (NotableAttackers.Count + attackerSquadNumbers.Sum()) * 0.1) //NotableAttackers losses < 10%
            {
                Notable = false;
            }
            Attacker.AddEventCollection(this);
            if (Defender != Attacker)
            {
                Defender.AddEventCollection(this);
            }
            Region.AddEventCollection(this);
            UndergroundRegion.AddEventCollection(this);
            Site.AddEventCollection(this);
        }

        private void Initialize()
        {
            Name = "";
            Outcome = BattleOutcome.Unknown;
            Coordinates = new Location(0, 0);
            AttackerDeathCount = 0;
            DefenderDeathCount = 0;

            NotableAttackers = new List<HistoricalFigure>();
            NotableDefenders = new List<HistoricalFigure>();
            AttackerSquads = new List<Squad>();
            DefenderSquads = new List<Squad>();
            Attackers = new List<Squad>();
            Defenders = new List<Squad>();
            NonCombatants = new List<HistoricalFigure>();
            AttackerSupportMercenaryEntities = new List<Entity>();
            DefenderSupportMercenaryEntities = new List<Entity>();
            AttackerSupportMercenaryHfs = new List<HistoricalFigure>();
            DefenderSupportMercenaryHfs = new List<HistoricalFigure>();
        }

        public class Squad
        {
            public const int MAX_SIZE = 100;
            public CreatureInfo Race { get; set; }
            public int Numbers { get; set; }
            public int Deaths { get; set; }
            public int Site { get; set; }
            public int Population { get; set; }
            public Squad(CreatureInfo race, int numbers, int deaths, int site, int population)
            {
                Race = race;
                Numbers = numbers;
                Deaths = deaths;
                Site = site;
                Population = population;
            }
        }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            if (link)
            {
                string title = Type;
                title += "&#13";
                title += Attacker != null ? Attacker.PrintEntity(false) : "UNKNOWN";
                title += " (Attacker)";
                if (Victor == Attacker)
                {
                    title += "(V)";
                }

                title += "&#13";
                title += "Kills: " + DefenderDeathCount;
                title += "&#13";
                title += Defender != null ? Defender.PrintEntity(false) : "UNKNOWN";
                title += " (Defender)";
                if (Victor == Defender)
                {
                    title += "(V)";
                }

                title += "&#13";
                title += "Kills: " + AttackerDeathCount;

                string linkedString;
                if (pov != this)
                {
                    linkedString = Icon + "<a href = \"collection#" + Id + "\" title=\"" + title + "\"><font color=\"#6E5007\">" + Name + "</font></a>";
                }
                else
                {
                    linkedString = Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(Name) + "</a>";
                }
                return linkedString;
            }
            return Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override string GetIcon()
        {
            return Icon;
        }
    }
}

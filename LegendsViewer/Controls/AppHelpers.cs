using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls
{
    public static class AppHelpers
    {
        public static readonly string[][] EventInfo = {

            new[] {"add hf entity link",           "Historical Figure linked to Entity",       "Enemy / Prisoner / Member / General / King / Queen / Etc."},
            new[] {"add hf hf link",               "Historical Figures linked",                "Marriage / Imprisonment / Worship"},
            new[] {"attacked site",                "Site Attacked",                            ""},
            new[] {"body abused",                  "Historical Figure Body Abused",            "Mutilation / Impalement / Hanging"},
            new[] {"change hf job",                "Historical Figure Change Job",             ""},
            new[] {"change hf state",              "Historical Figure Change State",           "Scouting / Wandering / Snatcher / Thief / Refugee"},
            new[] {"changed creature type",        "Historical Figure Transformed",            "Transformed into race / caste of abductor"},
            new[] {"create entity position",       "Entity Position Created",                  ""},
            new[] {"created site",                 "Site Founded",                             ""},
            new[] {"created world construction",   "Entity Construction Created",              "Road / Bridges / Tunnels connecting two sites"},
            new[] {"creature devoured",            "Historical Figure Eaten",                  ""},
            new[] {"destroyed site",               "Site Destroyed",                           "Site Attacked and Destroyed"},
            new[] {"field battle",                 "Entity Battle",                            "Battle between two entities"},
            new[] {"hf abducted",                  "Historical Figure Abduction",              ""},
            new[] {"hf died",                      "Historical Figure Death",                  ""},
            new[] {"hf new pet",                   "Historical Figure Tamed Creatures",        "Tamed creatures in region"},
            new[] {"hf reunion",                   "Historical Figure Reunion",                ""},
            new[] {"hf simple battle event",       "Historical Figure Fight",                  "Multiple Outcomes / Subtypes"},
            new[] {"hf travel",                    "Historical Figure Travel",                 ""},
            new[] {"hf wounded",                   "Historical Figure Wounded",                ""},
            new[] {"impersonate hf",               "Historical Figure Impersonation",          "Deity is impersonated, fooling Deity's associated civilization"},
            new[] {"item stolen",                  "Historical Figure Theft",                  ""},
            new[] {"new site leader",              "Site Taken Over / New Leader",             "Site Attacked and taken over. New Government and Leader installed."},
            new[] {"peace accepted",               "Entity Accepted Peace",                    ""},
            new[] {"peace rejected",               "Entity Rejected Peace",                    ""},
            new[] {"plundered site",               "Site Pillaged",                            "Site attacked and plundered"},
            new[] {"reclaim site",                 "Site Reclaimed",                           ""},
            new[] {"remove hf entity link",        "Historical Figure detached from Entity",   "No longer in leader position / escaped prisons"},
            new[] {"artifact created",             "Artifact Created",       ""},
            new[] {"artifact destroyed",           "Artifact Destroyed",     ""},
            new[] {"diplomat lost",                "DF Mode - Diplomat Lost",                  ""},
            new[] {"entity created",               "Entity Created",                           ""},
            new[] {"hf revived",                   "DF Mode - Historical Figure Became Ghost", ""},
            new[] {"masterpiece arch design",      "DF Mode - Masterpiece Arch. Designed",     ""},
            new[] {"masterpiece arch constructed", "DF Mode - Masterpiece Arch. Constructed",  ""},
            new[] {"masterpiece engraving",        "DF Mode - Masterpiece Engraving",          ""},
            new[] {"masterpiece food",             "DF Mode - Masterpiece Food Cooked",        ""},
            new[] {"masterpiece dye",              "DF Mode - Masterpiece Dye Made",           ""},
            new[] {"masterpiece item",             "DF Mode - Masterpiece Item Made",          ""},
            new[] {"masterpiece item improvement", "DF Mode - Masterpiece Item Improvement",   ""},
            new[] {"masterpiece lost",             "DF Mode - Masterpiece Item Lost",          ""},
            new[] {"merchant",                     "DF Mode - Merchants Arrived",              ""},
            new[] {"first contact",                "DF Mode - First Contact",                  ""},
            new[] {"site abandoned",               "DF Mode - Site Abandoned",                 ""},
            new[] {"site died",                    "DF Mode - Site Withered",                  ""},
            new[] {"site retired",                 "DF Mode - Site Retired",                   ""},
            new[] {"add hf site link",             "Historical Figure linked to Site",         "Historical Figure started living at site"},
            new[] {"created structure",            "Site Structure Created",                   "Some sort of structure created"},
            new[] {"hf razed structure",           "Site Structure Razed",                     ""},
            new[] {"remove hf site link",          "Historical Figure detached from Site",     "Historical Figure moved out of site"},
            new[] {"replaced structure",           "Site Structure Replaced",                  "Housing replaced with bigger housing"},
            new[] {"site taken over",              "Site Taken Over",                          ""},
            new[] {"entity relocate",              "Entity Relocated",                         ""},
            new[] {"hf gains secret goal",         "Historical Figure Gained Secret Goal",     ""},
            new[] {"hf profaned structure",        "Historical Figure Profaned structure",     ""},
            new[] {"hf disturbed structure",       "Historical Figure Disturbed structure",    ""},
            new[] {"hf does interaction",          "Historical Figure Did Interaction",        ""},
            new[] {"entity primary criminals",     "Entity Became Primary Criminals",          ""},
            new[] {"hf confronted",                "Historical Figure Confronted",             ""},
            new[] {"assume identity",              "Historical Figure Assumed Identity",       ""},
            new[] {"entity law",                   "Entity Law Change",                        ""},
            new[] {"change hf body state",         "Historical Figure Body State Changed",     ""},
            new[] {"razed structure",              "Entity Razed Structure",                   ""},
            new[] {"hf learns secret",             "Historical Figure Learned Secret",         ""},
            new[] {"artifact stored",              "Artifact Stored",        ""},
            new[] {"artifact possessed",           "Artifact Possessed",      ""},
            new[] {"artifact transformed",         "Artifact Transformed",   ""},
            new[] {"agreement made",               "Entity Agreement Made",                    ""},
            new[] {"agreement rejected",           "Entity Agreement Rejected",                ""},
            new[] {"artifact lost",                "Artifact Lost",                            ""},
            new[] {"site dispute",                 "Site Dispute",                             ""},
            new[] {"hf attacked site",             "Historical Figure Attacked Site",          ""},
            new[] {"hf destroyed site",            "Historical Figure Destroyed Site",         ""},
            new[] {"agreement formed",             "Agreement Formed",                         ""},
            new[] {"agreement concluded",          "Agreement Concluded",                      ""},
            new[] {"site tribute forced",          "Site Tribute Forced",                      ""},
            new[] {"insurrection started",         "Insurrection Started",                     ""},
            new[] {"hf reach summit",              "Historical Figure Reach Summit",           ""},

            // new 0.42.XX events
            new[] { "procession",                         "Procession",                               ""},
            new[] { "ceremony",                           "Ceremony",                                 ""},
            new[] { "performance",                        "Performance",                              ""},
            new[] { "competition",                        "Competition",                              ""},
            new[] { "written content composed",           "Written Content Composed",                 ""},
            new[] { "knowledge discovered",               "Knowledge Discovered",                     ""},
            new[] { "hf relationship denied",             "Historical Figure Relationship Denied",    ""},
            new[] { "poetic form created",                "Poetic Form Created",                      ""},
            new[] { "musical form created",               "Musical Form Created",                     ""},
            new[] { "dance form created",                 "Dance Form Created",                       ""},
            new[] { "regionpop incorporated into entity", "Regionpop Incorporated Into Entity",       ""},

            // new 0.44.XX events
            new[] { "hfs formed reputation relationship", "Reputation Relationship Formed", ""},
            new[] { "hf recruited unit type for entity",  "Recruited Unit Type For Entity", ""},
            new[] { "hf prayed inside structure",         "Historical Figure Prayed In Structure", ""},
            new[] { "hf viewed artifact",                 "Historical Figure Viewed Artifact", ""},
            new[] { "artifact given",                     "Artifact Given", ""},
            new[] { "artifact claim formed",              "Artifact Claim Formed", ""},
            new[] { "artifact copied",                    "Artifact Copied", ""},
            new[] { "artifact recovered",                 "Artifact Recovered", ""},
            new[] { "artifact found",                     "Artifact Found", ""},
            new[] { "sneak into site",                    "Sneak Into Site", ""},
            new[] { "spotted leaving site",               "Spotted Leaving Site", ""},
            new[] { "entity searched site",               "Entity Searched Site", ""},
            new[] { "hf freed",                           "Historical Figure Freed", ""},
            new[] { "tactical situation",                 "Tactical Situation", ""},
            new[] { "squad vs squad",                     "Squad vs. Squad", ""},
            new[] { "agreement void",                     "Agreement Void", ""},
            new[] { "entity rampaged in site",            "Entity Rampaged In Site", ""},
            new[] { "entity fled site",                   "Entity Fled Site", ""},
            new[] { "entity expels hf",                   "Entity Expels Historical Figure", ""},
            new[] { "site surrendered",                   "Site Surrendered", ""},

            // new 0.47.XX events
            new[] { "remove hf hf link",                  "Historical Figures Unlinked", ""},
            new[] { "holy city declaration",              "Holy City Declaration", ""},
            new[] { "hf performed horrible experiments",  "Historical Figure Performed Horrible Experiments", ""},
            new[] { "entity incorporated",                "Entity Incorporated", ""},
            new[] { "gamble",                             "Gamble", ""},
            new[] { "trade",                              "Trade", ""},
            new[] { "hf equipment purchase",              "Historical Figure Equipment Purchase", ""},
            new[] { "entity overthrown",                  "Entity Overthrown", ""},
            new[] { "failed frame attempt",               "Failed Frame Attempt", ""},
            new[] { "hf convicted",                       "Historical Figure Convicted", ""},
            new[] { "failed intrigue corruption",         "Failed Intrigue Corruption", ""},
            new[] { "hfs formed intrigue relationship",   "Historical Figures formed Intrigue Relationship", ""},
            new[] { "entity alliance formed",             "Entities formed Alliance", ""},
            new[] { "entity dissolved",                   "Entity Dissolved", ""},
            new[] { "add hf entity honor",                "Add Historical Figure Honor", ""},
            new[] { "entity breach feature layer",        "Entity Breach Caverns", ""},
            new[] { "entity equipment purchase",          "Entity Equipment Purchase", ""},
            new[] { "hf ransomed",                        "Historical Figure Ransomed", ""},

            new[] { "INVALID",                            "INVALID EVENT",                            ""}
        };

        private class ColumnBinding
        {
            public string PropertyName { get; set; }
            public string HeaderText { get; set; }
            public ColumnType Type { get; set; }

            public ColumnBinding(string property, string header, ColumnType type)
            {
                PropertyName = property;
                HeaderText = header;
                Type = ColumnType.Text;
            }

            public ColumnBinding(string property, string header) : this(property, header, ColumnType.Text) { }
            public ColumnBinding(string property, ColumnType type) : this(property, property, type) { }
            public ColumnBinding(string property) : this(property, property, ColumnType.Text) { }

        }

        private enum ColumnType
        {
            Text
        }

        public static List<DataGridViewColumn> GetColumns(Type dataType)
        {
            if (dataType.IsGenericType)
            {
                dataType = dataType.GetGenericArguments()[0];
            }

            List<DataGridViewColumn> columns = new List<DataGridViewColumn>();
            List<ColumnBinding> bindings = new List<ColumnBinding>();
            if (dataType == typeof(HistoricalFigure))
            {
                bindings = new List<ColumnBinding> {   new ColumnBinding ( "Name" ),
                                                        new ColumnBinding ( "Race" ),
                                                        new ColumnBinding ( "Caste" ),
                                                        new ColumnBinding ( "AssociatedType", "Associated Type"),
                                                        new ColumnBinding ( "Age"),
                                                        new ColumnBinding ( "CurrentState", "State"),
                                                        new ColumnBinding ( "Kills"),
                                                        new ColumnBinding ( "Battles"),
                                                        new ColumnBinding ( "Abductions"),
                                                        new ColumnBinding ( "Abducted"),
                                                        new ColumnBinding ( "BeastAttacks", "Beast Attacks") };
            }
            else if (dataType == typeof(Entity))
            {
                bindings = new List<ColumnBinding>
                { new ColumnBinding("Name"),
                                                       new ColumnBinding("CurrentSites", "Sites"),
                                                       new ColumnBinding("LostSites", "Lost Sites"),
                                                       new ColumnBinding("Population"),
                                                       new ColumnBinding("Wars"),
                                                       new ColumnBinding("WarVictoryRatio", "Wins : Losses"),
                                                       new ColumnBinding("WarKillDeathRatio", "Kills : Deaths") };
            }
            else if (dataType == typeof(Site))
            {
                bindings = new List<ColumnBinding>
                { new ColumnBinding("Name"),
                                                       new ColumnBinding("Type"),
                                                       new ColumnBinding("CurrentOwner", "Owner"),
                                                       new ColumnBinding("Warfare"),
                                                       new ColumnBinding("PreviousOwners", "Previous Owners"),
                                                       new ColumnBinding("Population"),
                                                       new ColumnBinding("Deaths"),
                                                       new ColumnBinding("BeastAttacks", "Beast Attacks") };
            }
            else if (dataType == typeof(WorldRegion))
            {
                bindings = new List<ColumnBinding>
                { new ColumnBinding("Name"),
                                                       new ColumnBinding("Type"),
                                                       new ColumnBinding("Deaths"),
                                                       new ColumnBinding("Battles") };
            }
            else if (dataType == typeof(UndergroundRegion))
            {
                bindings = new List<ColumnBinding>
                { new ColumnBinding("Type"),
                                                       new ColumnBinding("Depth")};
            }
            else if (dataType == typeof(War))
            {
                bindings = new List<ColumnBinding>
                { new ColumnBinding("Name"),
                                                        new ColumnBinding("Length"),
                                                        new ColumnBinding("Attacker"),
                                                        new ColumnBinding("Defender"),
                                                        //new ColumnBinding("Battles"),
                                                        //new ColumnBinding("DeathCount", "Deaths"),  
                                                        new ColumnBinding("AttackerToDefenderVictories", "Victories"),
                                                        new ColumnBinding("AttackerToDefenderKills", "Kills"),
                                                        new ColumnBinding("SitesLost", "Sites Lost") };

            }
            else if (dataType == typeof(Battle))
            {
                bindings = new List<ColumnBinding>
                { new ColumnBinding("Name"),
                                                       new ColumnBinding("StartYear", "Year"),
                                                       new ColumnBinding("Deaths"),
                                                       new ColumnBinding("Attacker"),
                                                       new ColumnBinding("Defender"),
                                                       new ColumnBinding("AttackersToDefenders", "Combatants"),
                                                       new ColumnBinding("AttackersToDefendersRemaining", "Remaining"),
                                                       new ColumnBinding("Outcome"),
                                                       new ColumnBinding("Conquering") };
            }
            else if (dataType == typeof(SiteConquered))
            {
                bindings = new List<ColumnBinding>
                { new ColumnBinding("Name"),
                                                       new ColumnBinding("StartYear", "Year"),
                                                       new ColumnBinding("Deaths") };
            }
            else if (dataType == typeof(BeastAttack))
            {
                bindings = new List<ColumnBinding>
                { new ColumnBinding("Name"),
                                                       new ColumnBinding("Deaths"),
                                                       new ColumnBinding("StartYear", "Year") };
            }
            else if (dataType == typeof(Artifact))
            {
                bindings = new List<ColumnBinding>
                {new ColumnBinding("Name"),
                                                      new ColumnBinding("Item") };
            }

            if (dataType.BaseType == typeof(WorldObject))
            {
                bindings.Add(new ColumnBinding("Events"));
            }

            if (dataType.BaseType == typeof(EventCollection))
            {
                bindings.Add(new ColumnBinding("AllEvents", "Events"));
            }

            foreach (ColumnBinding binding in bindings)
            {
                DataGridViewColumn propertyColumn;
                switch (binding.Type)
                {
                    case ColumnType.Text: propertyColumn = new DataGridViewTextBoxColumn(); break;
                    default: propertyColumn = new DataGridViewTextBoxColumn(); break;
                }

                propertyColumn.DataPropertyName = binding.PropertyName;
                propertyColumn.HeaderText = binding.HeaderText;
                columns.Add(propertyColumn);
            }

            return columns;
        }

        public static double AverageOrZero(this IEnumerable<double> values)
        {
            if (values.Any())
            {
                return values.Average();
            }

            return 0;
        }

        public static string GetDescription(this object enumerationValue)
        {
            Type type = enumerationValue.GetType();
            if (type == typeof(double))
            {
                return (enumerationValue as double?)?.ToString("R");
            }
            if (!type.IsEnum)
            {
                return enumerationValue.ToString();
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }
    }
}


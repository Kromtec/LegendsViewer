using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.WorldObjects
{
    public class Site : WorldObject
    {
        public string Icon = "<i class=\"fa fa-fw fa-home\"></i>";

        public string Type { get; set; }
        public WorldRegion Region { get; set; }
        public SiteType SiteType { get; set; }
        public string Name { get; set; }
        public string UntranslatedName { get; set; }
        public Location Coordinates { get; set; }
        public Rectangle Rectangle { get; set; }
        public bool HasStructures { get; set; }
        public List<Structure> Structures { get; set; }
        public List<EventCollection> Warfare { get; set; }
        public List<Battle> Battles { get { return Warfare.OfType<Battle>().ToList(); } set { } }
        public List<SiteConquered> Conquerings { get { return Warfare.OfType<SiteConquered>().ToList(); } set { } }
        public List<OwnerPeriod> OwnerHistory { get; set; }
        public List<HistoricalFigure> RelatedHistoricalFigures { get; set; }
        public static List<string> Filters;
        public DwarfObject CurrentOwner
        {
            get
            {
                if (OwnerHistory.Count(site => site.EndYear == -1) > 0)
                {
                    return OwnerHistory.First(site => site.EndYear == -1).Owner;
                }

                return null;
            }
            set { }
        }
        public List<DwarfObject> PreviousOwners { get { return OwnerHistory.Where(site => site.EndYear >= 0).Select(site => site.Owner).ToList(); } set { } }
        public List<Site> Connections { get; set; }
        public List<Population> Populations { get; set; }
        public List<string> PopulationsAsList
        {
            get
            {
                List<string> populations = new List<string>();
                foreach (Population population in Populations)
                {
                    for (int i = 0; i < population.Count; i++)
                    {
                        populations.Add(population.Race);
                    }
                }

                return populations;
            }
        }

        public List<Official> Officials { get; set; }
        public List<string> Deaths
        {
            get
            {
                List<string> deaths = new List<string>();
                deaths.AddRange(NotableDeaths.Select(death => death.Race));

                foreach (Battle.Squad squad in Battles.SelectMany(battle => battle.AttackerSquads.Concat(battle.DefenderSquads)).ToList())
                {
                    for (int i = 0; i < squad.Deaths; i++)
                    {
                        deaths.Add(squad.Race);
                    }
                }

                return deaths;
            }
            set { }
        }
        public List<HistoricalFigure> NotableDeaths { get { return Events.OfType<HfDied>().Select(death => death.HistoricalFigure).ToList(); } set { } }
        public List<BeastAttack> BeastAttacks { get; set; }
        public override List<WorldEvent> FilteredEvents
        {
            get { return Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }
        public class Official
        {
            public HistoricalFigure HistoricalFigure;
            public string Position;

            public Official(HistoricalFigure historicalFigure, string position)
            {
                HistoricalFigure = historicalFigure;
                Position = position;
            }
        }
        public string SiteMapPath { get; set; }

        public Site(List<Property> properties, World world)
            : base(properties, world)
        {
            Type = Name = UntranslatedName = "";
            Warfare = new List<EventCollection>();
            OwnerHistory = new List<OwnerPeriod>();
            Connections = new List<Site>();
            Populations = new List<Population>();
            Officials = new List<Official>();
            BeastAttacks = new List<BeastAttack>();
            Structures = new List<Structure>();
            RelatedHistoricalFigures = new List<HistoricalFigure>();

            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "type":
                        Type = Formatting.InitCaps(property.Value);
                        switch (property.Value)
                        {
                            case "cave": SiteType = SiteType.Cave; break;
                            case "fortress": SiteType = SiteType.Fortress; break;
                            case "forest retreat": SiteType = SiteType.ForestRetreat; break;
                            case "dark fortress": SiteType = SiteType.DarkFortress; break;
                            case "town": SiteType = SiteType.Town; break;
                            case "hamlet": SiteType = SiteType.Hamlet; break;
                            case "vault": SiteType = SiteType.Vault; break;
                            case "dark pits": SiteType = SiteType.DarkPits; break;
                            case "hillocks": SiteType = SiteType.Hillocks; break;
                            case "tomb": SiteType = SiteType.Tomb; break;
                            case "tower": SiteType = SiteType.Tower; break;
                            case "mountain halls": SiteType = SiteType.MountainHalls; break;
                            case "camp": SiteType = SiteType.Camp; break;
                            case "lair": SiteType = SiteType.Lair; break;
                            case "labyrinth": SiteType = SiteType.Labyrinth; break;
                            case "shrine": SiteType = SiteType.Shrine; break;
                            case "important location": SiteType = SiteType.ImportantLocation; break;
                            default:
                                property.Known = false;
                                break;
                        }
                        break;
                    case "name": Name = Formatting.InitCaps(property.Value); break;
                    case "coords": Coordinates = Formatting.ConvertToLocation(property.Value); break;
                    case "structures":
                        HasStructures = true;
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            foreach (Property subProperty in property.SubProperties)
                            {
                                subProperty.Known = true;
                                Structures.Add(new Structure(subProperty.SubProperties, world, this));
                            }
                        }
                        break;
                    case "civ_id": property.Known = true; break;
                    case "cur_owner_id": property.Known = true; break;
                    case "rectangle":
                        char[] delimiterChars = { ':', ',' };
                        string[] rectArray = property.Value.Split(delimiterChars);
                        if (rectArray.Length == 4)
                        {
                            int x0 = Convert.ToInt32(rectArray[0]);
                            int y0 = Convert.ToInt32(rectArray[1]);
                            int x1 = Convert.ToInt32(rectArray[2]);
                            int y1 = Convert.ToInt32(rectArray[3]);
                            Rectangle = new Rectangle(x0, y0, x1 - x0, y1 - y0);
                        }
                        else
                        {
                            property.Known = false;
                        }
                        break;
                }
            }
            SetIconByType(SiteType);
        }

        private void SetIconByType(SiteType siteType)
        {
            switch (siteType)
            {
                case SiteType.Cave:
                    Icon = "<i class=\"fa fa-fw fa-circle\"></i>";
                    break;
                case SiteType.Fortress:
                    Icon = "<i class=\"fa fa-fw fa-fort-awesome\"></i>";
                    break;
                case SiteType.ForestRetreat:
                    Icon = "<i class=\"glyphicon fa-fw glyphicon-tree-deciduous\"></i>";
                    break;
                case SiteType.DarkFortress:
                    Icon = "<i class=\"glyphicon fa-fw glyphicon-compressed fa-rotate-90\"></i>";
                    break;
                case SiteType.Town:
                    Icon = "<i class=\"glyphicon fa-fw glyphicon-home\"></i>";
                    break;
                case SiteType.Hamlet:
                    Icon = "<i class=\"fa fa-fw fa-home\"></i>";
                    break;
                case SiteType.Vault:
                    Icon = "<i class=\"fa fa-fw fa-key\"></i>";
                    break;
                case SiteType.DarkPits:
                    Icon = "<i class=\"fa fa-fw fa-chevron-circle-down\"></i>";
                    break;
                case SiteType.Hillocks:
                    Icon = "<i class=\"glyphicon fa-fw glyphicon-grain\"></i>";
                    break;
                case SiteType.Tomb:
                    Icon = "<i class=\"fa fa-fw fa-archive fa-flip-vertical\"></i>";
                    break;
                case SiteType.Tower:
                    Icon = "<i class=\"glyphicon fa-fw glyphicon-tower\"></i>";
                    break;
                case SiteType.MountainHalls:
                    Icon = "<i class=\"fa fa-fw fa-gg-circle\"></i>";
                    break;
                case SiteType.Camp:
                    Icon = "<i class=\"glyphicon fa-fw glyphicon-tent\"></i>";
                    break;
                case SiteType.Lair:
                    Icon = "<i class=\"fa fa-fw fa-database\"></i>";
                    break;
                case SiteType.Labyrinth:
                    Icon = "<i class=\"fa fa-fw fa-ils fa-rotate-90\"></i>";
                    break;
                case SiteType.Shrine:
                    Icon = "<i class=\"glyphicon fa-fw glyphicon-screenshot\"></i>";
                    break;
            }
        }

        public void AddConnection(Site connection)
        {
            if (!Connections.Contains(connection))
            {
                Connections.Add(connection);
            }
        }

        public override string ToString() { return Name; }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            if (link)
            {
                string title = Type;
                title += "&#13";
                title += "Events: " + Events.Count;

                if (pov != this)
                {
                    return Icon + "<a href = \"site#" + Id + "\" title=\"" + title + "\">" + Name + "</a>";
                }
                return Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(Name) + "</a>";
            }
            return Name;
        }

        public override string GetIcon()
        {
            return Icon;
        }
    }
}

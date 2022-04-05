﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LegendsViewer.Controls;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Controls.Query.Attributes;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;

namespace LegendsViewer.Legends.WorldObjects
{
    public class Entity : WorldObject
    {
        [AllowAdvancedSearch]
        public string Name { get; set; }
        [ShowInAdvancedSearchResults("Name")]
        public string NameForAdvancedSearch => string.IsNullOrWhiteSpace(Name) ? $"[{Race}]" : Name;

        [AllowAdvancedSearch]
        public Entity Parent { get; private set; }
        [AllowAdvancedSearch]
        [ShowInAdvancedSearchResults]
        public CreatureInfo Race { get; set; }
        [AllowAdvancedSearch("Worshiped Deities", true)]
        public List<HistoricalFigure> Worshiped { get; set; }
        public List<string> LeaderTypes { get; set; }
        public List<List<HistoricalFigure>> Leaders { get; set; }

        [AllowAdvancedSearch(true)]
        public List<Population> Populations { get; set; }
        [AllowAdvancedSearch("Origin Structure")]
        public Structure OriginStructure { get; set; }
        [AllowAdvancedSearch(true)]
        public List<Entity> Groups { get; set; }
        public List<OwnerPeriod> SiteHistory { get; set; }
        [AllowAdvancedSearch("Current Sites", true)]
        public List<Site> CurrentSites { get => SiteHistory.Where(site => site.EndYear == -1).Select(site => site.Site).ToList(); set { } }
        [AllowAdvancedSearch("Lost Sites", true)]
        public List<Site> LostSites { get => SiteHistory.Where(site => site.EndYear >= 0).Select(site => site.Site).ToList(); set { } }
        [AllowAdvancedSearch("Related Sites", true)]
        public List<Site> Sites { get => SiteHistory.ConvertAll(site => site.Site); set { } }
        public List<Honor> Honors { get; set; }

        [AllowAdvancedSearch]
        [ShowInAdvancedSearchResults]
        public EntityType Type { get; set; } // legends_plus.xml
        [AllowAdvancedSearch("Is Civilization")]
        [ShowInAdvancedSearchResults("Is Civilization")]
        public bool IsCiv { get; set; }
        public string TypeAsString { get => Type.GetDescription(); set { } }
        public List<EntitySiteLink> SiteLinks { get; set; } // legends_plus.xml
        public List<EntityEntityLink> EntityLinks { get; set; } // legends_plus.xml
        public List<EntityPosition> EntityPositions { get; set; } // legends_plus.xml
        public List<EntityPositionAssignment> EntityPositionAssignments { get; set; } // legends_plus.xml
        public List<Location> Claims { get; set; } // legends_plus.xml
        public List<EntityOccasion> Occassions { get; set; } // legends_plus.xml
        public List<string> Weapons { get; set; }
        public string Profession { get; set; }

        [AllowAdvancedSearch("Wars (All)", true)]
        public List<War> Wars { get; set; }
        [AllowAdvancedSearch("Wars (Attacking)", true)]
        public List<War> WarsAttacking { get => Wars.Where(war => war.Attacker == this).ToList(); set { } }
        [AllowAdvancedSearch("Wars (Defending)", true)]
        public List<War> WarsDefending { get => Wars.Where(war => war.Defender == this).ToList(); set { } }
        public int WarVictories { get => WarsAttacking.Sum(war => war.AttackerBattleVictories.Count) + WarsDefending.Sum(war => war.DefenderBattleVictories.Count); set { } }
        public int WarLosses { get => WarsAttacking.Sum(war => war.DefenderBattleVictories.Count) + WarsDefending.Sum(war => war.AttackerBattleVictories.Count); set { } }
        public int WarKills { get => WarsAttacking.Sum(war => war.DefenderDeathCount) + WarsDefending.Sum(war => war.AttackerDeathCount); set { } }
        public int WarDeaths { get => WarsAttacking.Sum(war => war.AttackerDeathCount) + WarsDefending.Sum(war => war.DefenderDeathCount); set { } }
        [AllowAdvancedSearch("Leaders", true)]
        public List<HistoricalFigure> AllLeaders => Leaders.SelectMany(l => l).ToList();
        public List<string> PopulationsAsList
        {
            get
            {
                var populations = new List<string>();
                foreach (var population in Populations)
                {
                    for (var i = 0; i < population.Count; i++)
                    {
                        populations.Add(population.Race.NamePlural);
                    }
                }

                return populations;
            }
        }

        public double WarKillDeathRatio
        {
            get
            {
                if (WarDeaths == 0 && WarKills == 0)
                {
                    return 0;
                }

                return WarDeaths == 0 ? double.MaxValue : Math.Round(WarKills / Convert.ToDouble(WarDeaths), 2);
            }
        }

        public Color LineColor { get; set; }
        public Bitmap Identicon { get; set; }
        public string IdenticonString { get; set; }
        public string SmallIdenticonString { get; set; }

        private string _icon;
        public string Icon
        {
            get
            {
                if (string.IsNullOrEmpty(_icon))
                {
                    string coloredIcon;
                    if (IsCiv)
                    {
                        coloredIcon = PrintIdenticon() + " ";
                    }
                    else if (World.MainRaces.ContainsKey(Race))
                    {
                        Color civilizedPopColor = LineColor;
                        if (civilizedPopColor == Color.Empty)
                        {
                            civilizedPopColor = World.MainRaces.FirstOrDefault(r => r.Key == Race).Value;
                        }
                        coloredIcon = "<span class=\"fa-stack fa-lg\" style=\"font-size:smaller;\">";
                        coloredIcon += "<i class=\"fa fa-square fa-stack-2x\"></i>";
                        coloredIcon += "<i class=\"fa fa-group fa-stack-1x\" style=\"color:" + ColorTranslator.ToHtml(civilizedPopColor) + ";\"></i>";
                        coloredIcon += "</span>";
                    }
                    else
                    {
                        coloredIcon = "<span class=\"fa-stack fa-lg\" style=\"font-size:smaller;\">";
                        coloredIcon += "<i class=\"fa fa-square fa-stack-2x\"></i>";
                        coloredIcon += "<i class=\"fa fa-group fa-stack-1x fa-inverse\"></i>";
                        coloredIcon += "</span>";
                    }
                    _icon = coloredIcon;
                }
                return _icon;
            }
            set => _icon = value;
        }

        public static List<string> Filters;

        public override List<WorldEvent> FilteredEvents => Events.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList();

        public Entity(List<Property> properties, World world)
            : base(properties, world)
        {
            Name = "";
            Race = CreatureInfo.Unknown;
            Type = EntityType.Unknown;
            Parent = null;
            Worshiped = new List<HistoricalFigure>();
            LeaderTypes = new List<string>();
            Leaders = new List<List<HistoricalFigure>>();
            Groups = new List<Entity>();
            SiteHistory = new List<OwnerPeriod>();
            SiteLinks = new List<EntitySiteLink>();
            EntityLinks = new List<EntityEntityLink>();
            Wars = new List<War>();
            Populations = new List<Population>();
            EntityPositions = new List<EntityPosition>();
            EntityPositionAssignments = new List<EntityPositionAssignment>();
            Claims = new List<Location>();
            Occassions = new List<EntityOccasion>();
            Honors = new List<Honor>();
            Weapons = new List<string>();

            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "name": Name = Formatting.InitCaps(property.Value); break;
                    case "race":
                        Race = world.GetCreatureInfo(property.Value);
                        break;
                    case "type":
                        switch (property.Value)
                        {
                            case "civilization":
                                Type = EntityType.Civilization;
                                break;
                            case "religion":
                                Type = EntityType.Religion;
                                break;
                            case "sitegovernment":
                                Type = EntityType.SiteGovernment;
                                break;
                            case "nomadicgroup":
                                Type = EntityType.NomadicGroup;
                                break;
                            case "outcast":
                                Type = EntityType.Outcast;
                                break;
                            case "migratinggroup":
                                Type = EntityType.MigratingGroup;
                                break;
                            case "performancetroupe":
                                Type = EntityType.PerformanceTroupe;
                                break;
                            case "guild":
                                Type = EntityType.Guild;
                                break;
                            case "militaryunit":
                                Type = EntityType.MilitaryUnit;
                                break;
                            case "merchantcompany":
                                Type = EntityType.MerchantCompany;
                                break;
                            default:
                                Type = EntityType.Unknown;
                                property.Known = false;
                                break;
                        }
                        break;
                    case "child":
                        property.Known = true; // TODO child entities
                        break;
                    case "site_link":
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            SiteLinks.Add(new EntitySiteLink(property.SubProperties, world));
                        }

                        break;
                    case "entity_link":
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            foreach (Property subProperty in property.SubProperties)
                            {
                                subProperty.Known = true;
                            }
                        }

                        world.AddEntityEntityLink(this, property);
                        break;
                    case "worship_id":
                        var worshippedDeity = world.GetHistoricalFigure(Convert.ToInt32(property.Value));
                        if (worshippedDeity != null)
                        {
                            Worshiped.Add(worshippedDeity);
                        }
                        break;
                    case "claims":
                        string[] coordinateStrings = property.Value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var coordinateString in coordinateStrings)
                        {
                            string[] xYCoordinates = coordinateString.Split(',');
                            if (xYCoordinates.Length == 2)
                            {
                                int x = Convert.ToInt32(xYCoordinates[0]);
                                int y = Convert.ToInt32(xYCoordinates[1]);
                                Claims.Add(new Location(x, y));
                            }
                        }
                        break;
                    case "entity_position":
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            EntityPositions.Add(new EntityPosition(property.SubProperties, world));
                        }
                        break;
                    case "entity_position_assignment":
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            EntityPositionAssignments.Add(new EntityPositionAssignment(property.SubProperties, world));
                        }
                        break;
                    case "histfig_id":
                        property.Known = true; // TODO historical figure == living members?
                        break;
                    case "occasion":
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            Occassions.Add(new EntityOccasion(property.SubProperties, world, this));
                        }
                        break;
                    case "honor":
                        property.Known = true;
                        if (property.SubProperties != null)
                        {
                            Honors.Add(new Honor(property.SubProperties, world, this));
                        }
                        break;
                    case "weapon":
                        Weapons.Add(property.Value);
                        break;
                    case "profession":
                        Profession = property.Value.Replace("_", " ");
                        break;
                }
            }
        }
        public override string ToString() { return Name; }

        public bool EqualsOrParentEquals(Entity entity)
        {
            return this == entity || Parent == entity;
        }

        public string PrintEntity(bool link = true, DwarfObject pov = null)
        {
            string entityString = ToLink(link, pov);
            if (Parent != null)
            {
                entityString += " of " + Parent.ToLink(link, pov);
            }
            return entityString;
        }

        //TODO: Check and possibly move logic
        public void AddOwnedSite(OwnerPeriod ownerPeriod)
        {
            if (ownerPeriod.StartCause == "UNKNOWN" && SiteHistory.All(s => s.Site != ownerPeriod.Site))
            {
                SiteHistory.Insert(0, ownerPeriod);
            }
            else
            {
                SiteHistory.Add(ownerPeriod);
            }

            if (ownerPeriod.Owner != this)
            {
                Groups.Add(ownerPeriod.Owner);
            }

            if (!IsCiv && Parent != null)
            {
                Parent.AddOwnedSite(ownerPeriod);
                Race = Parent.Race;
            }
        }

        public void AddPopulations(List<Population> populations)
        {
            foreach (Population population in populations)
            {
                Population popMatch = Populations.Find(pop => pop.Race.NamePlural.Equals(population.Race.NamePlural, StringComparison.InvariantCultureIgnoreCase));
                if (popMatch != null)
                {
                    popMatch.Count += population.Count;
                }
                else
                {
                    Populations.Add(new Population(population.Race, population.Count));
                }
            }
            Populations = Populations.OrderByDescending(pop => pop.Count).ToList();
            Parent?.AddPopulations(populations);
        }

        public string PrintIdenticon(bool fullSize = false)
        {
            if (IsCiv)
            {
                string printIdenticon = "<img src=\"data:image/gif;base64,";
                if (fullSize)
                {
                    printIdenticon += IdenticonString;
                }
                else
                {
                    printIdenticon += SmallIdenticonString;
                }

                printIdenticon += "\" align=absmiddle />";
                return printIdenticon;
            }
            return "";
        }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            if (link)
            {
                return pov != this
                    ? Icon + "<a href = \"entity#" + Id + "\" title=\"" + GetToolTip() + "\">" + Name + "</a>"
                    : Icon + "<a title=\"" + GetToolTip() + "\">" + HtmlStyleUtil.CurrentDwarfObject(Name) + "</a>";
            }
            return Name;
        }

        private string GetToolTip()
        {
            string title = GetTitle();
            if (Parent != null)
            {
                title += "&#13";
                title += "Part of " + Parent.Name;
            }
            title += "&#13";
            title += "Events: " + Events.Count;
            return title;
        }

        public string GetTitle()
        {
            var title = IsCiv ? "Civilization" : GetTypeAsString();
            if (Race != null && Race != CreatureInfo.Unknown)
            {
                title += " of ";
                title += Race.NamePlural;
            }
            return title;
        }

        private string GetTypeAsString()
        {
            switch (Type)
            {
                case EntityType.Civilization:
                    return "Civilization";
                case EntityType.NomadicGroup:
                    return "Nomadic group";
                case EntityType.MigratingGroup:
                    return "Migrating group";
                case EntityType.Outcast:
                    return "Collection of outcasts";
                case EntityType.Religion:
                    return "Religious group";
                case EntityType.SiteGovernment:
                    return "Site government";
                case EntityType.PerformanceTroupe:
                    return "Performance troupe";
                case EntityType.MercenaryCompany:
                    return "Mercenary company";
                case EntityType.MilitaryUnit:
                    return "Mercenary order";
                case EntityType.Guild:
                    return "Guild";
                case EntityType.MerchantCompany:
                    return "Merchant company";
                default:
                    return "Group";
            }
        }

        public string GetSummary(bool link = true, DwarfObject pov = null)
        {
            string summary = string.Empty;
            summary += ToLink(link, pov);
            summary += " was a ";
            summary += GetTypeAsString().ToLower();
            if (Race != CreatureInfo.Unknown)
            {
                summary += " of " + Race.NamePlural.ToLower();
            }
            if (Parent != null)
            {
                summary += " of " + Parent.ToLink(link, pov);
            }

            switch (Type)
            {
                case EntityType.Religion:
                    if (Worshiped.Count > 0)
                    {
                        summary += " centered around the worship of " + Worshiped[0].ToLink(link, pov);
                    }
                    break;
                case EntityType.MilitaryUnit:
                    bool isWorshipping = false;
                    if (Worshiped.Count > 0)
                    {
                        summary += " devoted to the worship of " + Worshiped[0].ToLink(link, pov);
                        isWorshipping = true;
                    }
                    if (Weapons.Count > 0)
                    {
                        if (isWorshipping)
                        {
                            summary += " and";
                        }
                        summary += " dedicated to the mastery of the " + string.Join(", the ", Weapons);
                    }
                    break;
                case EntityType.Guild:
                    summary += " of " + Profession + "s";
                    break;
            }
            summary += ".";
            return summary;
        }

        public override string GetIcon()
        {
            return Icon;
        }

        public void SetParent(Entity parent)
        {
            if (parent == null || parent == this)
            {
                return;
            }

            if (parent.Name == Name)
            {
                return;
            }

            Parent = parent;
        }
    }
}

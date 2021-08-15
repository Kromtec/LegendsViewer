using System;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Controls.Query.Attributes;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.Parser;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Legends.EventCollections
{
    public class SiteConquered : EventCollection
    {
        public string Icon = "<i class=\"glyphicon fa-fw glyphicon-pawn\"></i>";

        [ShowInAdvancedSearchResults]
        public string Name { get { return Formatting.AddOrdinal(Ordinal) + " " + ConquerType.GetDescription() + " of " + Site.Name; } set { } }
        [ShowInAdvancedSearchResults("Deaths")]
        public int DeathCount { get { return Deaths.Count; } set { } }

        [AllowAdvancedSearch]
        public int Ordinal { get; set; }
        [AllowAdvancedSearch("Conquered by")]
        [ShowInAdvancedSearchResults("Conquered By")]
        public SiteConqueredType ConquerType { get; set; }
        [AllowAdvancedSearch(true)]
        [ShowInAdvancedSearchResults]
        public Site Site { get; set; }
        [AllowAdvancedSearch]
        [ShowInAdvancedSearchResults]
        public Entity Attacker { get; set; }
        [AllowAdvancedSearch]
        [ShowInAdvancedSearchResults]
        public Entity Defender { get; set; }
        [AllowAdvancedSearch]
        [ShowInAdvancedSearchResults]
        public Battle Battle { get; set; }
        [AllowAdvancedSearch(true)]
        public List<HistoricalFigure> Deaths { get { return GetSubEvents().OfType<HfDied>().Select(death => death.HistoricalFigure).ToList(); } set { } }
        public static List<string> Filters;
        public override List<WorldEvent> FilteredEvents
        {
            get { return AllEvents.Where(dwarfEvent => !Filters.Contains(dwarfEvent.Type)).ToList(); }
        }
        public SiteConquered()
        {
            Initialize();
        }

        public SiteConquered(List<Property> properties, World world)
            : base(properties, world)
        {
            Initialize();
            foreach (Property property in properties)
            {
                switch (property.Name)
                {
                    case "ordinal": Ordinal = Convert.ToInt32(property.Value); break;
                    case "war_eventcol": ParentCollection = world.GetEventCollection(Convert.ToInt32(property.Value)); break;
                    case "site_id": Site = world.GetSite(Convert.ToInt32(property.Value)); break;
                    case "attacking_enid": Attacker = world.GetEntity(Convert.ToInt32(property.Value)); break;
                    case "defending_enid": Defender = world.GetEntity(Convert.ToInt32(property.Value)); break;
                }
            }

            if (Collection.OfType<PlunderedSite>().Any())
            {
                ConquerType = SiteConqueredType.Pillaging;
            }
            else if (Collection.OfType<DestroyedSite>().Any())
            {
                ConquerType = SiteConqueredType.Destruction;
            }
            else if (Collection.OfType<NewSiteLeader>().Any() || Collection.OfType<SiteTakenOver>().Any())
            {
                ConquerType = SiteConqueredType.Conquest;
            }
            else if (Collection.OfType<SiteTributeForced>().Any())
            {
                ConquerType = SiteConqueredType.TributeEnforcement;
            }
            else
            {
                ConquerType = SiteConqueredType.Invasion;
            }

            if (ConquerType == SiteConqueredType.Pillaging || 
                ConquerType == SiteConqueredType.Invasion || 
                ConquerType == SiteConqueredType.TributeEnforcement)
            {
                Notable = false;
            }

            Site.Warfare.Add(this);
            if (ParentCollection is War)
            {
                War war = ParentCollection as War;
                war.DeathCount += Collection.OfType<HfDied>().Count();

                if (Attacker == war.Attacker)
                {
                    war.AttackerVictories.Add(this);
                }
                else
                {
                    war.DefenderVictories.Add(this);
                }
            }
            Attacker.AddEventCollection(this);
            Defender.AddEventCollection(this);
            Site.AddEventCollection(this);
        }

        private void Initialize()
        {
            Ordinal = 1;
        }

        public override string ToLink(bool link = true, DwarfObject pov = null, WorldEvent worldEvent = null)
        {
            string name = "The ";
            name += Formatting.AddOrdinal(Ordinal) + " ";
            name += ConquerType.GetDescription() + " of " + Site.ToLink(false);
            if (link)
            {
                string title = Type;
                title += "&#13";
                if (Attacker != null)
                {
                    title += Attacker.PrintEntity(false) + " (Attacker)(V)";
                    title += "&#13";
                }
                if (Defender != null)
                {
                    title += Defender.PrintEntity(false) + " (Defender)";
                }

                string linkedString = "";
                if (pov != this)
                {
                    linkedString = "<a href = \"collection#" + Id + "\" title=\"" + title + "\"><font color=\"6E5007\">" + name + "</font></a>";
                    if (pov != Battle)
                    {
                        linkedString += " as a result of " + Battle.ToLink();
                    }
                }
                else
                {
                    linkedString = Icon + "<a title=\"" + title + "\">" + HtmlStyleUtil.CurrentDwarfObject(name) + "</a>";
                }
                return linkedString;
            }
            return name;
        }
        public override string ToString()
        {
            return ToLink(false);
        }

        public override string GetIcon()
        {
            return Icon;
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    internal class EntityPrinter : HtmlPrinter
    {
        private readonly Entity _entity;
        private readonly World _world;
        private List<Entity> _allEnemies;

        public EntityPrinter(Entity entity, World world)
        {
            _entity = entity;
            _world = world;
        }

        public override string GetTitle()
        {
            return _entity.Name;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            LoadCustomScripts();

            Html.AppendLine("<div class=\"container-fluid\">");

            PrintTitle();

            Html.AppendLine("<div class=\"row\">")
                .AppendLine("<div class=\"col-md-6 col-sm-12\">");
            PrintMaps();
            Html.AppendLine("</div>")
                .AppendLine("<div id=\"chart-populationbyrace-container\" class=\"col-md-4 col-sm-6\" style=\"height: 250px\">")
                .AppendLine("<canvas id=\"chart-populationbyrace\"></canvas>")
                .AppendLine("</div>")
                .AppendLine("</div>")
                .AppendLine("<div class=\"row\">")
                .AppendLine("<div class=\"col-md-12 col-sm-12\">")
                .Append(_entity.GetSummary(true, _entity)).AppendLine("</br></br>")
                .AppendLine("</div>")
                .AppendLine("<div class=\"row\">");

            PrintPopulations(_entity.Populations);
            Html.AppendLine("<div class=\"col-lg-4 col-md-6 col-sm-12\">");
            PrintEntityLinks();
            if (_entity.EntityLinks.Count > 5)
            {
                Html.AppendLine("</div>")
                    .AppendLine("<div class=\"col-lg-4 col-md-6 col-sm-12\">");
            }
            PrintOriginStructure();
            PrintWorships();
            PrintLeaders();
            PrintHonors();
            PrintCurrentPositionAssignments();
            Html.AppendLine("</div>")
                .AppendLine("</div>");
            PrintWars();
            PrintWarfareInfo();
            PrintSiteHistory();
            PrintEventLog(_world, _entity.Events, Entity.Filters, _entity);

            Html.AppendLine("</div>");
            return Html.ToString();
        }

        private void PrintMaps()
        {
            if (_entity.SiteHistory.Count == 0)
            {
                return;
            }

            List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _entity);
            TableMaker mapTable = new TableMaker();
            mapTable.StartRow();
            mapTable.AddData(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap));
            mapTable.AddData(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap));
            mapTable.EndRow();
            Html.Append(mapTable.GetTable()).AppendLine("</br>");
            maps[0].Dispose();
            maps[1].Dispose();
        }

        private void PrintOriginStructure()
        {
            if (_entity.OriginStructure == null)
            {
                return;
            }
            Html.Append(Bold("Originated in")).AppendLine(LineBreak);
            StartList(ListType.Unordered);
            Html.Append(ListItem).Append(_entity.OriginStructure.ToLink(true, _entity)).Append(" (").Append(_entity.OriginStructure.Site.ToLink(true, _entity)).AppendLine(")");
            EndList(ListType.Unordered);
        }

        private void PrintChildEntites(Entity entity)
        {
            if (entity.EntityLinks.Count(entityLink => entityLink.Type == EntityEntityLinkType.Child) == 0)
            {
                return;
            }
            StartList(ListType.Unordered);
            foreach (EntityEntityLink childLink in entity.EntityLinks.Where(entityLink => entityLink.Type == EntityEntityLinkType.Child))
            {
                Html.Append(ListItem).Append(childLink.Target.ToLink(true, entity)).Append(" (").Append(childLink.Target.Type.GetDescription()).AppendLine(")");
                PrintChildEntites(childLink.Target);
            }
            EndList(ListType.Unordered);
        }

        private void PrintEntityLinks()
        {
            if (_entity.EntityLinks.Count == 0)
            {
                return;
            }
            Html.Append(Bold("Related Entities")).AppendLine(LineBreak);
            StartList(ListType.Unordered);
            foreach (EntityEntityLink parentLink in _entity.EntityLinks.Where(entityLink => entityLink.Type == EntityEntityLinkType.Parent))
            {
                Html.Append(ListItem).Append(parentLink.Target.ToLink(true, _entity)).Append(" (").Append(parentLink.Target.Type.GetDescription()).AppendLine(")");
            }
            foreach (EntityEntityLink childLink in _entity.EntityLinks.Where(entityLink => entityLink.Type == EntityEntityLinkType.Child))
            {
                Html.Append(ListItem).Append(childLink.Target.ToLink(true, _entity)).Append(" (").Append(childLink.Target.Type.GetDescription()).AppendLine(")");
                PrintChildEntites(childLink.Target);
            }
            foreach (EntityEntityLink religiousLink in _entity.EntityLinks.Where(entityLink => entityLink.Type == EntityEntityLinkType.Religious))
            {
                Html.Append(ListItem).Append(religiousLink.Target.ToLink(true, _entity)).Append(" (").Append(religiousLink.Target.Type.GetDescription()).AppendLine(")");
                PrintChildEntites(religiousLink.Target);
            }
            EndList(ListType.Unordered);
        }

        private void LoadCustomScripts()
        {
            Html.AppendLine("<script>")
                .AppendLine("window.onload = function(){");

            PopulatePopulationChartData(_entity.Populations.Where(pop => pop.IsMainRace || pop.IsAnimalPeople).ToList());
            if (_entity.Wars.Count > 0)
            {
                PopulateWarOverview();
            }

            Html.AppendLine("}")
                .AppendLine("</script>");
        }

        private void PopulateWarOverview()
        {
            var allBattles = new List<Battle>();
            foreach (var war in _entity.Wars)
            {
                allBattles.AddRange(war.Battles);
            }

            _allEnemies = allBattles.Where(x => x.Attacker != null)
                .Select(x => x.Attacker)
                .Concat(allBattles.Where(x => x.Defender != null).Select(x => x.Defender))
                .Distinct()
                .OrderBy(entity => entity.Race.NamePlural)
                .ToList();
            var entityLabels = string.Join(",", _allEnemies.Where(x => x.Name != _entity.Name).Select(x => $"'{x.Name} - {x.Race.NamePlural}'"));
            var battleVictorData = string.Join(",", _allEnemies.Where(x => x.Name != _entity.Name).Select(x => $"{allBattles.Count(y => y.Victor == _entity && (y.Attacker?.Name == x.Name || y.Defender?.Name == x.Name))}"));
            var battleLoserData = string.Join(",", _allEnemies.Where(x => x.Name != _entity.Name).Select(x => $"{allBattles.Count(y => y.Victor != _entity && (y.Attacker?.Name == x.Name || y.Defender?.Name == x.Name))}"));

            const string victorColor = "255, 206, 86";
            const string loserColor = "153, 102, 255";

            var battleVictorEntity =
                "{ " +
                    "label: \"As Victor\", " +
                    "data: [" + battleVictorData + "], " +
                    "backgroundColor: 'rgba(" + victorColor + ", 0.25)', " +
                    "hoverBackgroundColor: 'rgba(" + victorColor + ", 0.5)', " +
                    "borderWidth: 2, " +
                    "borderColor: 'rgba(" + victorColor + ", 0.8)', " +
                    "hoverBorderColor: 'rgba(" + victorColor + ", 1)' " +
                "}";
            var battleLoserEntity =
                "{ " +
                    "label: \"As Loser\", " +
                    "data: [" + battleLoserData + "], " +
                    "backgroundColor: 'rgba(" + loserColor + ", 0.25)', " +
                    "hoverBackgroundColor: 'rgba(" + loserColor + ", 0.5)', " +
                    "borderWidth: 2, " +
                    "borderColor: 'rgba(" + loserColor + ", 0.8)', " +
                    "hoverBorderColor: 'rgba(" + loserColor + ", 1)' " +
                "}";

            Html.AppendLine("var warsByEntityChart = new Chart(document.getElementById('chart-countbyEntity').getContext('2d'), { type: 'horizontalBar', ")
                .AppendLine("data: {")
                .Append("labels: [").Append(entityLabels).AppendLine("], ")
                .Append("datasets:[").Append(battleVictorEntity).Append(',').Append(battleLoserEntity).AppendLine("],")
                .AppendLine("},")
                .AppendLine("options:{")
                .AppendLine("maxBarThickness: 20,")
                .AppendLine("legend:{")
                .AppendLine("position:'top',")
                .AppendLine("labels: { boxWidth: 12 }")
                .AppendLine("}")
                .AppendLine("}")
                .AppendLine("});");
        }

        private void PrintWarfareInfo()
        {
            if (_entity.Wars.Count == 0)
            {
                return;
            }

            Html.AppendLine("<div class=\"row\">");

            PrintWarfareGraph();
            PrintWarfareChart();

            Html.AppendLine("</div>")
                .AppendLine("</br>");
        }

        private void PrintWarfareChart()
        {
            if (_allEnemies.Count > 5)
            {
                Html.AppendLine("<div class=\"col-md-12\">");
            }
            else
            {
                Html.AppendLine("<div class=\"col-md-6 col-sm-12\">");
            }
            Html.Append(Bold("Battles against other Entities - Victory/Defeat Chart")).AppendLine(LineBreak)
                .AppendLine("<canvas id=\"chart-countbyEntity\" class=\"bar-chart\" width=\"600\" height=\"300\"></canvas>")
                .AppendLine("</div>");
        }

        private void PrintWarfareGraph()
        {
            if (_entity.Wars.Count == 0)
            {
                return;
            }

            List<string> nodes = new List<string>();
            Dictionary<string, int> edges = new Dictionary<string, int>();

            foreach (var war in _entity.Wars)
            {
                foreach (var battle in war.Battles)
                {
                    if (battle.Attacker != null)
                    {
                        string attacker = CreateNode(battle.Attacker);
                        if (!nodes.Contains(attacker))
                        {
                            nodes.Add(attacker);
                        }
                    }

                    if (battle.Defender != null)
                    {
                        string defender = CreateNode(battle.Defender);
                        if (!nodes.Contains(defender))
                        {
                            nodes.Add(defender);
                        }
                    }

                    if (battle.Attacker != null && battle.Defender != null)
                    {
                        string faveColor = GetHtmlColorByEntity(battle.Attacker);
                        string edge = "{ data: { source: '" + battle.Attacker.Id + "', target: '" + battle.Defender.Id + "', faveColor: '" + faveColor + "', width: WIDTH, label: LABEL } },";
                        if (edges.ContainsKey(edge))
                        {
                            edges[edge]++;
                        }
                        else
                        {
                            edges[edge] = 1;
                        }
                    }
                }
            }
            if (_allEnemies.Count > 5)
            {
                Html.AppendLine("<div class=\"col-md-12\">");
            }
            else
            {
                Html.AppendLine("<div class=\"col-md-6 col-sm-12\">");
            }
            Html.Append(Bold("Battles against other Entities - Sum of battles - Graph")).AppendLine(LineBreak)
                .AppendLine("<div id=\"warfaregraph\" class=\"legends_graph\"></div>")
                .AppendLine("</div>")
                .Append("<script type=\"text/javascript\" src=\"").Append(LocalFileProvider.LocalPrefix).AppendLine("WebContent/scripts/cytoscape.min.js\"></script>")
                .AppendLine("<script>")
                .AppendLine("window.warfaregraph_nodes = [");
            foreach (var node in nodes)
            {
                Html.AppendLine(node);
            }
            Html.AppendLine("]")
                .AppendLine("window.warfaregraph_edges = [");
            foreach (var edge in edges)
            {
                Html.AppendLine(edge.Key.Replace("WIDTH", edge.Value > 15 ? "15" : edge.Value.ToString()).Replace("LABEL", edge.Value.ToString()));
            }
            Html.AppendLine("]")
                .AppendLine("</script>")
                .Append("<script type=\"text/javascript\" src=\"").Append(LocalFileProvider.LocalPrefix).AppendLine("WebContent/scripts/warfaregraph.js\"></script>");
        }

        private string CreateNode(Entity entity)
        {
            string classes = entity.Equals(_entity) ? " current" : "";
            string faveColor = GetHtmlColorByEntity(entity);
            string title = "";
            if (entity.Race != null && entity.Race != CreatureInfo.Unknown)
            {
                title += entity.Race.NamePlural;
                title += "\\n--------------------\\n";
            }
            title += entity.Name;
            if (entity.Type != EntityType.Unknown)
            {
                title += "\\n--------------------\\n";
                title += entity.Type.GetDescription();
            }

            if (entity.IsCiv)
            {
                classes += " civilization";
            }

            string node = "{ data: { ";
            node += "id: '" + entity.Id + "', ";
            node += "name: '" + WebUtility.HtmlEncode(title) + "', ";
            node += "href: 'entity#" + entity.Id + "', ";
            node += "faveColor: '" + faveColor + "', ";
            node += "icon: 'url(data:image/png;base64," + entity.SmallIdenticonString + ")' ";
            node += "}, classes: '" + classes + "' },";
            return node;
        }

        private void PrintTitle()
        {
            Html.AppendLine("<div class=\"row\">")
                .AppendLine("<div class=\"col-md-12\">");
            string title = _entity.GetIcon() + " " + _entity.ToLink(false);
            title += " is a " + _entity.GetTitle().ToLower();
            if (_entity.Parent != null)
            {
                title += " of " + _entity.Parent.ToLink(true, _entity);
            }

            title += ".";
            Html.Append("<h1>").Append(title).AppendLine("</h1></br>");

            if (_entity.IsCiv)
            {
                Html.Append(_entity.PrintIdenticon(true)).Append(LineBreak).AppendLine(LineBreak);
            }

            if (_entity.SiteHistory.Count > 0 && _entity.SiteHistory.Count(sitePeriod => sitePeriod.EndYear == -1) == 0)
            {
                Html.AppendLine(Font("Last Known Sites. Year: " + (_entity.SiteHistory.Max(sitePeriod => sitePeriod.EndYear) - 1), "red"));
            }
            Html.AppendLine("</div>")
                .AppendLine("</div>");
        }

        private void PrintLeaders()
        {
            if (_entity.Leaders?.Count > 0)
            {
                Html.Append(Bold("Leaderhistory")).Append(' ').AppendLine(LineBreak);
                foreach (string leaderType in _entity.LeaderTypes)
                {
                    Html.Append(leaderType).Append('s').AppendLine(LineBreak);
                    TableMaker leaderTable = new TableMaker(true);
                    foreach (HistoricalFigure leader in _entity.Leaders[_entity.LeaderTypes.IndexOf(leaderType)])
                    {
                        leaderTable.StartRow();
                        leaderTable.AddData(leader.Positions.Last(position => position.Title == leaderType).Began.ToString(), 0, TableDataAlign.Right);
                        leaderTable.AddData(leader.ToLink());
                        leaderTable.EndRow();
                    }
                    Html.Append(leaderTable.GetTable()).AppendLine(LineBreak);
                }
            }
        }

        private void PrintHonors()
        {
            if (_entity.Honors.Count > 0)
            {
                Html.Append(Bold("Honors")).Append(' ').AppendLine(LineBreak)
                    .AppendLine("<ul>");
                foreach (Honor honor in _entity.Honors)
                {
                    Html.AppendLine("<li>")
                        .AppendLine(honor.Print(true))
                        .AppendLine("</li>");
                }
                Html.AppendLine("</ul>");
            }
        }

        private void PrintCurrentPositionAssignments()
        {
            if (_entity.EntityPositionAssignments.Count == 0 || !_entity.EntityPositionAssignments.Any(epa => epa.HistoricalFigure != null))
            {
                return;
            }
            Html.AppendLine("<b>Current Leadership</b><br />")
                .AppendLine("<ul>");
            foreach (EntityPositionAssignment assignment in _entity.EntityPositionAssignments.Where(a => a.HistoricalFigure != null))
            {
                EntityPosition position = _entity.EntityPositions.Find(pos => pos.Id == assignment.PositionId);
                if (position == null)
                {
                    continue;
                }

                PrintCurrentPositionAssignment(assignment, position);
                PrintCurrentPositionAssignmentSpouse(assignment, position);
            }
            Html.AppendLine("</ul>");
        }

        private void PrintCurrentPositionAssignment(EntityPositionAssignment assignment, EntityPosition position)
        {
            string positionName = position.GetTitleByCaste(assignment.HistoricalFigure.Caste);

            Html.Append("<li>").Append(assignment.HistoricalFigure.ToLink()).Append(", ").Append(positionName).AppendLine("</li>");
        }

        private void PrintCurrentPositionAssignmentSpouse(EntityPositionAssignment assignment, EntityPosition position)
        {
            if (string.IsNullOrEmpty(position.Spouse))
            {
                return;
            }
            HistoricalFigureLink spouseLink = assignment.HistoricalFigure.RelatedHistoricalFigures.Find(hfLink => hfLink.Type == HistoricalFigureLinkType.Spouse);
            if (spouseLink == null)
            {
                return;
            }
            HistoricalFigure spouse = spouseLink.HistoricalFigure;
            if (spouse == null)
            {
                return;
            }
            string spousePositionName = position.GetTitleByCaste(spouse.Caste, true);
            Html.Append("<li>").Append(spouse.ToLink()).Append(", ").Append(spousePositionName).AppendLine("</li>");
        }

        private void PrintWorships()
        {
            if (_entity.Worshiped?.Count > 0)
            {
                Html.Append(Bold("Worships")).AppendLine(LineBreak);
                StartList(ListType.Unordered);
                foreach (HistoricalFigure worship in _entity.Worshiped)
                {
                    string associations = "";
                    foreach (string association in worship.Spheres)
                    {
                        if (associations.Length > 0)
                        {
                            associations += ", ";
                        }

                        associations += association;
                    }
                    Html.Append(ListItem).Append(worship.ToLink()).Append(" (").Append(associations).AppendLine(")");
                }
                EndList(ListType.Unordered);
            }
        }

        private void PrintWars()
        {
            if (_entity.Wars.Count(war => !_world.FilterBattles || war.Notable) > 0)
            {
                Html.Append(Bold("Wars")).AppendLine(LineBreak);
                TableMaker warTable = new TableMaker(true);
                foreach (War war in _entity.Wars.Where(war => !_world.FilterBattles || war.Notable))
                {
                    warTable.StartRow();
                    string endString = war.EndYear == -1 ? "Present" : war.EndYear.ToString();
                    warTable.AddData(war.StartYear + " - " + endString);
                    warTable.AddData(war.ToLink());

                    if (war.Attacker == _entity)
                    {
                        warTable.AddData("waged against");
                        warTable.AddData(war.Defender.PrintEntity(), 0);
                        warTable.AddData("");
                    }
                    else if (war.Attacker.Parent == _entity)
                    {
                        warTable.AddData("waged against");
                        warTable.AddData(war.Defender.PrintEntity(), 0);
                        warTable.AddData("by " + war.Attacker.ToLink());
                    }
                    else if (war.Defender == _entity)
                    {
                        warTable.AddData("defended against");
                        warTable.AddData(war.Attacker.PrintEntity(), 0);
                        warTable.AddData("");
                    }
                    else if (war.Defender.Parent == _entity)
                    {
                        warTable.AddData("defended against");
                        warTable.AddData(war.Attacker.PrintEntity(), 0);
                        warTable.AddData("by " + war.Defender.ToLink());
                    }

                    int battleVictories = 0, battleLossses = 0, sitesDestroyed = 0, sitesLost = 0, kills, losses;
                    if (war.Attacker == _entity || war.Attacker.Parent == _entity)
                    {
                        battleVictories = war.AttackerVictories.OfType<Battle>().Count();
                        battleLossses = war.DefenderVictories.OfType<Battle>().Count();
                        sitesDestroyed = war.AttackerVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType != SiteConqueredType.Pillaging);
                        sitesLost = war.DefenderVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType != SiteConqueredType.Pillaging);
                    }
                    else if (war.Defender == _entity || war.Defender.Parent == _entity)
                    {
                        battleVictories = war.DefenderVictories.OfType<Battle>().Count();
                        battleLossses = war.AttackerVictories.OfType<Battle>().Count();
                        sitesDestroyed = war.DefenderVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType != SiteConqueredType.Pillaging);
                        sitesLost = war.AttackerVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType != SiteConqueredType.Pillaging);
                    }

                    kills = war.Collections.OfType<Battle>().Where(battle => battle.Attacker == _entity || battle.Attacker?.Parent == _entity).Sum(battle => battle.DefenderDeathCount) + war.Collections.OfType<Battle>().Where(battle => battle.Defender == _entity || battle.Defender?.Parent == _entity).Sum(battle => battle.AttackerDeathCount);
                    losses = war.Collections.OfType<Battle>().Where(battle => battle.Attacker == _entity || battle.Attacker?.Parent == _entity).Sum(battle => battle.AttackerDeathCount) + war.Collections.OfType<Battle>().Where(battle => battle.Defender == _entity || battle.Defender?.Parent == _entity).Sum(battle => battle.DefenderDeathCount);

                    warTable.AddData("(V/L)");
                    warTable.AddData("Battles:");
                    warTable.AddData(battleVictories.ToString(), 0, TableDataAlign.Right);
                    warTable.AddData("/");
                    warTable.AddData(battleLossses.ToString());
                    warTable.AddData("Sites:");
                    warTable.AddData(sitesDestroyed.ToString(), 0, TableDataAlign.Right);
                    warTable.AddData("/");
                    warTable.AddData(sitesLost.ToString());
                    warTable.AddData("Deaths:");
                    warTable.AddData(kills.ToString(), 0, TableDataAlign.Right);
                    warTable.AddData("/");
                    warTable.AddData(losses.ToString());
                    warTable.EndRow();
                }
                Html.Append(warTable.GetTable()).AppendLine(LineBreak);
            }
        }

        private void PrintSiteHistory()
        {
            if (_entity.SiteHistory.Count > 0)
            {
                Html.Append(Bold("Site History")).AppendLine(LineBreak);
                TableMaker siteTable = new TableMaker(true);
                foreach (OwnerPeriod ownedSite in _entity.SiteHistory.OrderBy(sh => sh.StartYear))
                {
                    siteTable.StartRow();
                    siteTable.AddData(ownedSite.Owner.ToLink(true, _entity));
                    siteTable.AddData(ownedSite.StartCause);
                    siteTable.AddData(ownedSite.Site.ToLink());
                    var startYear = ownedSite.StartYear >= 0 ? ownedSite.StartYear.ToString() : "in a time before time";
                    siteTable.AddData(startYear, 0, TableDataAlign.Right);
                    if (ownedSite.EndYear >= 0)
                    {
                        siteTable.AddData(ownedSite.EndCause);
                        siteTable.AddData(ownedSite.EndYear.ToString(), 0, TableDataAlign.Right);
                    }
                    if (ownedSite.Ender != null)
                    {
                        if (ownedSite.Ender is Entity entity)
                        {
                            siteTable.AddData(" by " + entity.PrintEntity());
                        }
                        else
                        {
                            siteTable.AddData(" by " + ownedSite.Ender.ToLink(true, _entity));
                        }
                    }
                    siteTable.EndRow();
                }
                Html.Append(siteTable.GetTable()).AppendLine(LineBreak);
            }
        }
    }
}

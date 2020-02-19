using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class WorldStatsPrinter : HtmlPrinter
    {
        private readonly World _world;

        public WorldStatsPrinter(World world)
        {
            _world = world;
        }

        public override string GetTitle()
        {
            return "Summary";
        }

        public override string Print()
        {
            Html = new StringBuilder();

            LoadCustomScripts();

            PrintWorldInfo();
            PrintPlayerRelatedDwarfObjects();
            PrintEras();
            PrintCivs();
            PrintWarCharts();
            PrintEvents();
            PrintEntitiesAndHFs();
            PrintGeography();
            PrintPopulations();
            
            return Html.ToString();
        }

        private void PrintEvents()
        {
            Html.AppendLine("<div class=\"container-fluid\">");
            Html.AppendLine("<div class=\"row\">");
            Html.AppendLine("<div class=\"col-md-4 col-sm-6\">");
            Html.AppendLine("<h1>Events: " + _world.Events.Count + "</h1> ");
            Html.AppendLine(MakeLink(Font("[Event Overview]", "Maroon"), LinkOption.LoadEventOverview));
            Html.AppendLine("</br>");
            Html.AppendLine("</br>");
            Html.AppendLine("</div>");
            Html.AppendLine("</div>");
            Html.AppendLine("</div>");
        }

        private void PrintWorldInfo()
        {
            Html.AppendLine("<h1>" + _world.Name + "</h1></br>");

            Html.AppendLine("<div class=\"container-fluid\">");
            Html.AppendLine("<div class=\"row\">");

            Html.AppendLine("<div class=\"col-md-4 col-sm-6\">");
            PrintMap();
            Html.AppendLine("</div>");

            Html.AppendLine("<div id=\"chart-populationbyrace-container\" class=\"col-md-4 col-sm-6\" style=\"height: 300px\">");
            Html.AppendLine("<canvas id=\"chart-populationbyrace\"></canvas>");
            Html.AppendLine("</div>");

            Html.AppendLine("</div>");
            Html.AppendLine("</div>");
        }

        private void PrintWarCharts()
        {
            if (!_world.Wars.Any())
            {
                return;
            }

            Html.AppendLine("<div class=\"container-fluid\">");

            Html.AppendLine("<div class=\"row\">");
            Html.AppendLine(_world.CivilizedPopulations.Count > 20
               ? "<div class=\"col-lg-12\">"
               : "<div class=\"col-md-6 col-sm-12\">");
            Html.AppendLine("<h1>Wars Fought by Race</h1>");
            Html.AppendLine("<canvas id=\"chart-countbyrace\" class=\"bar-chart\"></canvas>");
            Html.AppendLine("</div>");
            
            var warParticipants =
                _world.Wars.Select(x => x.Attacker.Name).Concat(_world.Wars.Select(x => x.Defender.Name)).Distinct().ToList();
            Html.AppendLine(warParticipants.Count > 20
                ? "<div class=\"col-lg-12\">"
                : "<div class=\"col-md-6 col-sm-12\">");
            Html.AppendLine("<h1>Wars Fought by Civilization</h1>");
            Html.AppendLine("<canvas id=\"chart-countbyciv\" class=\"bar-chart\"></canvas>");
            Html.AppendLine("</div>");
            Html.AppendLine("</div>");

            Html.AppendLine("</br>");
            Html.AppendLine("</br>");

            Html.AppendLine("</div>");
        }

        private void LoadCustomScripts()
        {
            Html.AppendLine("<script>");
            Html.AppendLine("window.onload = function(){");

            PopulatePopulationChartData(_world.CivilizedPopulations);
            if (_world.Wars.Any())
            {
                PopulateWarOverview();
            }

            Html.AppendLine("}");
            Html.AppendLine("</script>");
        }

        private void PopulateWarOverview()
        {
            var defColor = "0, 128, 255";
            var atkColor = "255, 51, 51";

            var allWars = (from war in _world.Wars
                           join race in World.MainRaces on war.Attacker.Race equals race.Key
                           select new
                           {
                               attackerRace = war.Attacker.Race,
                               attackerCiv = war.Attacker.Parent != null ? war.Attacker.Parent.Name : war.Attacker.Name,
                               attackerColor = ColorTranslator.ToHtml(race.Value),
                               defenderRace = war.Defender.Race,
                               defenderCiv = war.Defender.Parent != null ? war.Defender.Parent.Name : war.Defender.Name,
                               defenderColor = ColorTranslator.ToHtml(World.MainRaces.ContainsKey(war.Defender.Race) ? Color.Gray : World.MainRaces.FirstOrDefault(x => x.Key == war.Defender.Race).Value)
                           }).ToList();

            var allRaces = (from civilizedPopulations in _world.CivilizedPopulations select civilizedPopulations.Race).OrderBy(race => race.NamePlural).ToList();
            var raceLabels = string.Join(",", allRaces.Select(x => $"'{x.NamePlural}'"));
            var defenderRaceData = string.Join(",", allRaces.Select(x => $"{allWars.Count(y => y.defenderRace == x)}"));
            var attackerRaceData = string.Join(",", allRaces.Select(x => $"{allWars.Count(y => y.attackerRace == x)}"));

            var defendersByRace =
                "{ " +
                    "label: 'As Defender', " +
                    "data: [" + defenderRaceData + "], " +
                    "backgroundColor: 'rgba(" + defColor + ", 0.25)', " +
                    "hoverBackgroundColor: 'rgba(" + defColor + ", 0.5)', " +
                    "borderWidth: 1, " +
                    "borderColor: 'rgba(" + defColor + ", 0.8)', " +
                    "hoverBorderColor: 'rgba(" + defColor + ", 1)' " +
                "}";
            var attackersByRace =
                "{ " +
                    "label: 'As Attacker', " +
                    "data: [" + attackerRaceData + "], " +
                    "backgroundColor: 'rgba(" + atkColor + ", 0.25)', " +
                    "hoverBackgroundColor: 'rgba(" + atkColor + ", 0.5)', " +
                    "borderWidth: 1, " +
                    "borderColor: 'rgba(" + atkColor + ", 0.8)', " +
                    "hoverBorderColor: 'rgba(" + atkColor + ", 1)' " +
                "}";


            var allCivs = allWars.Select(x => x.attackerCiv).Concat(allWars.Select(x => x.defenderCiv)).Distinct().ToList();
            Dictionary<Tuple<string, string>, Tuple<int, int>> warInfo = new Dictionary<Tuple<string, string>, Tuple<int, int>>();

            foreach (string civ in allCivs)
            {
                CreatureInfo race = allWars.FirstOrDefault(w => w.defenderCiv == civ)?.defenderRace ?? allWars.FirstOrDefault(w => w.attackerCiv == civ)?.attackerRace;
                Tuple<string, string> raceAndCiv = new Tuple<string, string>(race.NamePlural, civ);
                Tuple<int, int> defatkCounts = new Tuple<int, int>(allWars.Count(y => y.defenderCiv == civ), allWars.Count(y => y.attackerCiv == civ));
                warInfo.Add(raceAndCiv, defatkCounts);
            }
            var sortedWarInfo = warInfo.OrderBy(entry => entry.Key.Item1).ThenByDescending(entry => entry.Value.Item1 + entry.Value.Item2).ToList();
            if (warInfo.Count > 80)
            {
                string lastRace = "";
                int civCountInRace = 0;
                List<KeyValuePair<Tuple<string, string>, Tuple<int, int>>> toDelete = new List<KeyValuePair<Tuple<string, string>, Tuple<int, int>>>();
                List<KeyValuePair<Tuple<string, string>, Tuple<int, int>>> toAdd = new List<KeyValuePair<Tuple<string, string>, Tuple<int, int>>>();
                int otherDefCount = 0;
                int otherAtkCount = 0;
                foreach (var civWarCount in sortedWarInfo)
                {
                    if (civWarCount.Key.Item1 != lastRace)
                    {
                        civCountInRace = 0;
                        if (otherDefCount > 0 || otherAtkCount > 0)
                        {
                            toAdd.Add(new KeyValuePair<Tuple<string, string>, Tuple<int, int>>(new Tuple<string, string>(lastRace, "Other"), new Tuple<int, int>(otherDefCount, otherAtkCount)));
                            otherDefCount = 0;
                            otherAtkCount = 0;
                        }
                        lastRace = civWarCount.Key.Item1;
                    }
                    if (civCountInRace >= 10)
                    {
                        toDelete.Add(civWarCount);
                        otherDefCount += civWarCount.Value.Item1;
                        otherAtkCount += civWarCount.Value.Item2;
                    }
                    civCountInRace++;
                }
                if (otherDefCount > 0 || otherAtkCount > 0)
                {
                    toAdd.Add(new KeyValuePair<Tuple<string, string>, Tuple<int, int>>(new Tuple<string, string>(lastRace, "Other"), new Tuple<int, int>(otherDefCount, otherAtkCount)));
                    otherDefCount = 0;
                    otherAtkCount = 0;
                }
                foreach (var item in toDelete)
                {
                    sortedWarInfo.Remove(item);
                }
                foreach (var item in toAdd)
                {
                    sortedWarInfo.Add(item);
                }
                sortedWarInfo = sortedWarInfo.OrderBy(entry => entry.Key.Item1).ThenByDescending(entry => entry.Value.Item1 + entry.Value.Item2).ToList();
            }
            var civLabels = string.Join(",", sortedWarInfo.Select(x => $"'{x.Key.Item2} - {x.Key.Item1}'"));
            var defenderCivData = string.Join(",", sortedWarInfo.Select(x => $"{x.Value.Item1}"));
            var attackerCivData = string.Join(",", sortedWarInfo.Select(x => $"{x.Value.Item2}"));

            var defendersByCiv =
                "{ " +
                    "label: 'As Defender', " +
                    "data: [" + defenderCivData + "], " +
                    "backgroundColor: 'rgba(" + defColor + ", 0.25)', " +
                    "hoverBackgroundColor: 'rgba(" + defColor + ", 0.5)', " +
                    "borderWidth: 1, " +
                    "borderColor: 'rgba(" + defColor + ", 0.8)', " +
                    "hoverBorderColor: 'rgba(" + defColor + ", 1)' " +
                "}";
            var attackersByCiv =
                "{ " +
                    "label: 'As Attacker', " +
                    "data: [" + attackerCivData + "], " +
                    "backgroundColor: 'rgba(" + atkColor + ", 0.25)', " +
                    "hoverBackgroundColor: 'rgba(" + atkColor + ", 0.5)', " +
                    "borderWidth: 1, " +
                    "borderColor: 'rgba(" + atkColor + ", 0.8)', " +
                    "hoverBorderColor: 'rgba(" + atkColor + ", 1)' " +
                "}";

            Html.AppendLine("var warsByRaceChart = new Chart(document.getElementById('chart-countbyrace').getContext('2d'), { type: 'horizontalBar', ");
            Html.AppendLine("data: {");
            Html.AppendLine("labels: [" + raceLabels + "], ");
            Html.AppendLine("datasets:[" + defendersByRace + "," + attackersByRace + "],");
            Html.AppendLine("},");
            Html.AppendLine("options:{");
            Html.AppendLine("scales: { xAxes: [{ stacked: true }], yAxes: [{ stacked: true }] },");
            Html.AppendLine("legend:{");
            Html.AppendLine("position:'top',");
            Html.AppendLine("labels: { boxWidth: 12 }");
            Html.AppendLine("}");
            Html.AppendLine("}");
            Html.AppendLine("});");

            Html.AppendLine("var warsByCivChart = new Chart(document.getElementById('chart-countbyciv').getContext('2d'), { type: 'horizontalBar', ");
            Html.AppendLine("data: {");
            Html.AppendLine("labels: [" + civLabels + "], ");
            Html.AppendLine("datasets:[" + defendersByCiv + "," + attackersByCiv + "],");
            Html.AppendLine("},");
            Html.AppendLine("options:{");
            Html.AppendLine("maxBarThickness: 20,");
            Html.AppendLine("scales: { xAxes: [{ stacked: true }], yAxes: [{ stacked: true }] },");
            Html.AppendLine("legend:{");
            Html.AppendLine("position:'top',");
            Html.AppendLine("labels: { boxWidth: 12 }");
            Html.AppendLine("}");
            Html.AppendLine("}");
            Html.AppendLine("});");
        }

        private void PrintEras()
        {
            Html.AppendLine("<div class=\"container-fluid\">");
            Html.AppendLine("<div class=\"row\">");

            Html.AppendLine("<div class=\"col-md-4 col-sm-6\">");
            Html.AppendLine("<h1>Eras</h1>");
            Html.AppendLine("<ol>");
            foreach (Era era in _world.Eras)
            {
                Html.AppendLine("<li>" + era.Name + " (" + (era.StartYear < 0 ? 0 : era.StartYear) + " - " + era.EndYear + ")</li>");
            }
            Html.AppendLine("</ol>");
            Html.AppendLine("</br>");
            Html.AppendLine("</div>");

            Html.AppendLine("</div>");
            Html.AppendLine("</div>");
        }

        private void PrintPlayerRelatedDwarfObjects()
        {
            if (!_world.PlayerRelatedObjects.Any())
            {
                return;
            }
            Html.AppendLine("<div class=\"container-fluid\">");
            Html.AppendLine("<div class=\"row\">");

            Html.AppendLine("<div class=\"col-md-4 col-sm-6\">");
            Html.AppendLine("<h1>Player Related</h1>");
            var fortressModeLinks = _world.PlayerRelatedObjects.Where(o => !(o is HistoricalFigure));
            var adventureModeLinks = _world.PlayerRelatedObjects.Where(o => o is HistoricalFigure);
            if (fortressModeLinks.Any())
            {
                Html.AppendLine("<h2>Fortress Mode</h2>");
                Html.AppendLine("<ol>");
                foreach (DwarfObject dwarfObject in fortressModeLinks)
                {
                    Html.AppendLine("<li>" + dwarfObject.ToLink() + "</li>");
                }
                Html.AppendLine("</ol>");
            }
            if (adventureModeLinks.Any())
            {
                Html.AppendLine("<h2>Adventure Mode</h2>");
                Html.AppendLine("<ol>");
                foreach (DwarfObject dwarfObject in adventureModeLinks)
                {
                    Html.AppendLine("<li>" + dwarfObject.ToLink() + "</li>");
                }
                Html.AppendLine("</ol>");
            }
            Html.AppendLine("</br>");
            Html.AppendLine("</div>");

            Html.AppendLine("</div>");
            Html.AppendLine("</div>");
        }

        private void PrintCivs()
        {
            var civsByRace = from civ in _world.Entities.Where(entity => entity.IsCiv)
                             orderby civ.Race.NamePlural
                             select civ;

            var currentCivs = civsByRace.Where(civ => civ.Populations.Any(pop => pop.IsMainRace && pop.Count > 0)).ToList();
            var fallenCivs = civsByRace.Where(civ => !civ.Populations.Any(pop => pop.IsMainRace && pop.Count > 0)).ToList();

            Html.AppendLine("<div class=\"container-fluid\">");
            Html.AppendLine("<div class=\"row\">");
            if (currentCivs.Any())
            {
                Html.AppendLine("<div class=\"col-md-4 col-sm-6\">");
                Html.AppendLine("<h1>Current Civilizations: " + currentCivs.Count() + "</h1>");
                Html.AppendLine("<ul>");
                foreach (var civRace in currentCivs.Select(cc => cc.Race).Distinct())
                {
                    Html.AppendLine("<li><b>" + civRace.NamePlural + ":</b> " + currentCivs.Count(cc => cc.Race == civRace) + "</li>");
                    Html.AppendLine("<ul>");
                    foreach (var civ in currentCivs.Where(civ => civ.Race == civRace))
                    {
                        var intelligentPop = civ.Populations.Where(pop => pop.IsMainRace).ToList();
                        var intelligentPopCount = intelligentPop.Sum(cp => cp.Count);
                        var civPop = intelligentPop.FirstOrDefault(pop => pop.Race == civ.Race);
                        var civPopCount = civPop != null ? civPop.Count : 0;
                        Html.AppendLine("<li class=\"legends_civilization_listitem\">");
                        Html.AppendLine(civ.ToLink());
                        Html.AppendLine("<div class=\"legends_civilization_metainformation\">");
                        var civPopString = civPopCount + " " + civRace.NamePlural;
                        Html.AppendLine("<i class=\"fa fa-fw fa-user\" title=\"" + civPopString + "\"></i> " + civPopString);
                        if (intelligentPopCount - civPopCount > 0)
                        {
                            var otherPopCount = intelligentPopCount - civPopCount;
                            var otherPopString = otherPopCount + " Others";
                            Html.AppendLine(", <i class=\"fa fa-fw fa-plus-circle\" title=\"" + otherPopString + "\"></i> " + otherPopString);
                        }
                        var siteString = civ.CurrentSites.Count == 1 ? "Site" : "Sites";
                        var siteCountString = civ.CurrentSites.Count + " " + siteString;
                        Html.AppendLine(", <i class=\"fa fa-fw fa-home\" title=\"" + siteCountString + "\"></i> " + siteCountString);
                        Html.AppendLine("<div>");
                        Html.AppendLine("</ li > ");
                    }
                    Html.AppendLine("</ul>");
                }
                Html.AppendLine("</ul>");
                Html.AppendLine("</div>");
            }

            if (fallenCivs.Any())
            {
                Html.AppendLine("<div class=\"col-md-4 col-sm-6\">");
                Html.AppendLine("<h1>Fallen Civilizations: " + fallenCivs.Count() + "</h1>");
                Html.AppendLine("<ul>");
                foreach (var civRace in fallenCivs.Select(fc => fc.Race).Distinct())
                {
                    Html.AppendLine("<li><b>" + civRace.NamePlural + ":</b> " + fallenCivs.Count(fc => fc.Race == civRace) + "</li>");
                    Html.AppendLine("<ul>");
                    foreach (var civ in fallenCivs.Where(civ => civ.Race == civRace))
                    {
                        Html.AppendLine("<li>" + civ.ToLink() + " " + HtmlStyleUtil.SymbolDead + "</li>");
                    }
                    Html.AppendLine("</ul>");
                }
                Html.AppendLine("</ul>");
                Html.AppendLine("</div>");
            }
            Html.AppendLine("</div>");
            Html.AppendLine("</div>");

            Html.AppendLine("</br>");
        }

        private void PrintMap()
        {
            int mapSideLength = 300;
            double resizePercent;
            Size mapSize;
            if (_world.Map.Width > _world.Map.Height)
            {
                resizePercent = mapSideLength / Convert.ToDouble(_world.Map.Width);
            }
            else
            {
                resizePercent = mapSideLength / Convert.ToDouble(_world.Map.Height);
            }

            mapSize = new Size(Convert.ToInt32(_world.Map.Width * resizePercent), Convert.ToInt32(_world.Map.Height * resizePercent));
            using (Bitmap resizedMap = new Bitmap(mapSize.Width, mapSize.Height))
            {
                using (Graphics resize = Graphics.FromImage(resizedMap))
                {
                    resize.SmoothingMode = SmoothingMode.HighQuality;
                    resize.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    resize.DrawImage(_world.Map, new Rectangle(0, 0, mapSize.Width, mapSize.Height), new Rectangle(0, 0, _world.Map.Width, _world.Map.Height), GraphicsUnit.Pixel);
                }
                Html.AppendLine(MakeLink(BitmapToHtml(resizedMap), LinkOption.LoadMap) + "</br>");
            }
        }

        private void PrintVarious()
        {
            Html.AppendLine("<h1>Wars: " + _world.EventCollections.OfType<War>().Count() + "</h1>");
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Battles: " + _world.EventCollections.OfType<Battle>().Count() + "</li>");
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Notable: " + _world.EventCollections.OfType<Battle>().Count(battle => battle.Notable) + "</li>");
            Html.AppendLine("<li>Unnotable: " + _world.EventCollections.OfType<Battle>().Count(battle => !battle.Notable) + "</li>");
            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Conquerings: " + _world.EventCollections.OfType<SiteConquered>().Count() + "</li>");
            var conquerings = from conquering in _world.EventCollections.OfType<SiteConquered>()
                              group conquering by conquering.ConquerType into conquerType
                              select new { Type = conquerType.Key, Count = conquerType.Count() };
            conquerings = conquerings.OrderByDescending(conquering => conquering.Count);
            Html.AppendLine("<ul>");
            foreach (var conquering in conquerings)
            {
                Html.AppendLine("<li>" + conquering.Type + "s: " + conquering.Count + "</li>");
            }

            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Deaths</li>");
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Historical Figures: " + _world.EventCollections.OfType<Battle>().Sum(battle => battle.Collection.OfType<HfDied>().Count()) + "</li>");
            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");
            Html.AppendLine("</br>");

            Dictionary<string, int> deaths = new Dictionary<string, int>();
            foreach (Battle battle in _world.Battles)
            {
                foreach (KeyValuePair<string, int> deathByRace in battle.Deaths)
                {
                    if (deaths.ContainsKey(deathByRace.Key))
                    {
                        deaths[deathByRace.Key] += deathByRace.Value;
                    }
                    else
                    {
                        deaths[deathByRace.Key] = deathByRace.Value;
                    }
                }
            }

            Html.AppendLine("<h1><b>Battle Deaths by Race: " + deaths.Count + "</b></h2>");
            Html.AppendLine("<ol>");
            foreach (var raceDeath in deaths)
            {
                Html.AppendLine("<li>" + raceDeath.Key + ": " + raceDeath.Value + "</li>");
            }

            Html.AppendLine("</ol>");
            Html.AppendLine("</br>");
        }

        private void PrintEntitiesAndHFs()
        {
            var entityRaces = from entity in _world.Entities.Where(entity => !entity.IsCiv)
                              group entity by entity.Race into entityRace
                              select new { Type = entityRace.Key, Count = entityRace.Count() };
            var hfRaces = from hf in _world.HistoricalFigures
                          group hf by hf.Race into hfRace
                          select new { Race = hfRace.Key, Count = hfRace.Count() };
            var aliveHFs = from hf in _world.HistoricalFigures.Where(hf => hf.DeathYear == -1)
                           group hf by hf.Race into hfRace
                           select new { Type = hfRace.Key, Count = hfRace.Count() };
            entityRaces = entityRaces.OrderByDescending(entity => entity.Count);
            hfRaces = hfRaces.OrderByDescending(hf => hf.Count);
            aliveHFs = aliveHFs.OrderByDescending(hf => hf.Count);

            Html.AppendLine("<div class=\"container-fluid\">");
            Html.AppendLine("<div class=\"row\">");

            Html.AppendLine("<div class=\"col-sm-4\">");
            Html.AppendLine("<h1>Historical Figures: " + _world.HistoricalFigures.Count + "</h1>");
            Html.AppendLine("<ol>");
            foreach (var hfRace in hfRaces)
            {
                PrintPopulationEntry(hfRace.Race, hfRace.Count);
            }

            Html.AppendLine("</ol>");
            Html.AppendLine("</div>");

            Html.AppendLine("<div class=\"col-sm-4\">");
            Html.AppendLine("<h2><b>Alive: " + _world.HistoricalFigures.Count(hf => hf.DeathYear == -1) + "</b></h2>");
            Html.AppendLine("<ol>");
            foreach (var aliveHf in aliveHFs)
            {
                PrintPopulationEntry(aliveHf.Type, aliveHf.Count);
            }

            Html.AppendLine("</ol>");
            Html.AppendLine("</div>");

            Html.AppendLine("<div class=\"col-sm-4\">");
            PrintVarious();
            Html.AppendLine("<h1>Entities: " + _world.Entities.Count(entity => !entity.IsCiv) + "</h1>");
            Html.AppendLine("<ol>");
            foreach (var entityRace in entityRaces)
            {
                PrintPopulationEntry(entityRace.Type, entityRace.Count);
            }

            Html.AppendLine("</ol>");
            Html.AppendLine("</div>");

            Html.AppendLine("</div>");
            Html.AppendLine("</div>");

            Html.AppendLine("</br>");
        }

        private void PrintPopulationEntry(CreatureInfo creatureInfo, int count)
        {
            if (count == int.MaxValue)
            {
                Html.AppendLine("<li>" + creatureInfo.NamePlural + ": Unnumbered" + "</li>");
            }
            else if (count == 1)
            {
                Html.AppendLine("<li>" + creatureInfo.NameSingular + ": " + count + "</li>");
            }
            else
            {
                Html.AppendLine("<li>" + creatureInfo.NamePlural + ": " + count + "</li>");
            }
        }

        private void PrintPopulationList(List<Population> populations, string header)
        {
            if (populations.Any())
            {
                Html.AppendLine("<div class=\"col-sm-4\">");
                Html.AppendLine($"<h1>{header}</h1>");
                Html.AppendLine("<ol>");
                foreach (Population population in populations)
                {
                    PrintPopulationEntry(population.Race, population.Count);
                }

                Html.AppendLine("</ol>");
                Html.AppendLine("</div>");
            }
        }

        private void PrintPopulations()
        {
            Html.AppendLine("<div class=\"container-fluid\">");
            Html.AppendLine("<div class=\"row\">");

            PrintPopulationList(_world.OutdoorPopulations, "Outdoor Populations");
            PrintPopulationList(_world.UndergroundPopulations, "Underground Populations");

            if (_world.CivilizedPopulations.Any())
            {
                Html.AppendLine("<div class=\"col-sm-4\">");
                Html.AppendLine("<h1>Civilized Populations</h1>");
                Html.AppendLine("<ol>");
                foreach (Population population in _world.CivilizedPopulations)
                {
                    PrintPopulationEntry(population.Race, population.Count);
                }

                Html.AppendLine("</ol>");
                Html.AppendLine("</br>");

                Html.AppendLine("<h1>Site Populations: " + _world.SitePopulations.Sum(population => population.Count) + "</h1>");
                var sitePopulations = from population in _world.SitePopulations
                                      group population by population.Race into popType
                                      select new { Type = popType.Key, Count = popType.Sum(population => population.Count) };
                sitePopulations = sitePopulations.OrderByDescending(population => population.Count);
                Html.AppendLine("<ol>");
                foreach (var sitePopulation in sitePopulations)
                {
                    PrintPopulationEntry(sitePopulation.Type, sitePopulation.Count);
                }

                Html.AppendLine("</ol>");

                Html.AppendLine("</div>");
            }

            Html.AppendLine("</div>");
            Html.AppendLine("</div>");

            Html.AppendLine("</br>");
        }

        private void PrintGeography()
        {
            var sites = from site in _world.Sites
                        group site by site.Type into siteType
                        select new { Type = siteType.Key, Count = siteType.Count() };
            var regions = from region in _world.Regions
                          group region by region.Type into regionType
                          select new { Type = regionType.Key, Count = regionType.Count() };
            var undergroundRegions = from undergroundRegion in _world.UndergroundRegions
                                     group undergroundRegion by undergroundRegion.Type into type
                                     select new { Type = type.Key, Count = type.Count() };

            sites = sites.OrderByDescending(site => site.Count);
            regions = regions.OrderByDescending(region => region.Count);
            undergroundRegions = undergroundRegions.OrderByDescending(undergroundRegion => undergroundRegion.Count);

            Html.AppendLine("<div class=\"container-fluid\">");
            Html.AppendLine("<div class=\"row\">");

            Html.AppendLine("<div class=\"col-sm-4\">");
            Html.AppendLine("<h1>Sites: " + _world.Sites.Count + "</h1>");
            Html.AppendLine("<ol>");
            foreach (var site in sites)
            {
                Html.AppendLine("<li>" + site.Type + ": " + site.Count + "</li>");
            }

            Html.AppendLine("</ol>");
            Html.AppendLine("</div>");

            Html.AppendLine("<div class=\"col-sm-4\">");
            Html.AppendLine("<h1>Regions: " + _world.Regions.Count + "</h1>");
            Html.AppendLine("<ol>");
            foreach (var region in regions)
            {
                Html.AppendLine("<li>" + region.Type + ": " + region.Count + "</li>");
            }

            Html.AppendLine("</ol>");
            Html.AppendLine("</div>");

            Html.AppendLine("<div class=\"col-sm-4\">");
            Html.AppendLine("<h1>Underground Regions: " + _world.UndergroundRegions.Count + "</h1>");
            Html.AppendLine("<ol>");
            foreach (var undergroundRegion in undergroundRegions)
            {
                Html.AppendLine("<li>" + undergroundRegion.Type + ": " + undergroundRegion.Count + "</li>");
            }

            Html.AppendLine("</ol>");
            Html.AppendLine("</div>");

            Html.AppendLine("</div>");
            Html.AppendLine("</div>");

            Html.AppendLine("</br>");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;

namespace LegendsViewer.Controls.HTML
{
    public abstract class HtmlPrinter : IDisposable
    {
        private bool _disposed;
        protected StringBuilder Html;
        protected const string LineBreak = "</br>";
        protected const string ListItem = "<li>";

        protected HtmlPrinter()
        {
            _disposed = false;
        }

        public abstract string GetTitle();
        public abstract string Print();

        private readonly List<string> _temporaryFiles = new List<string>();

        public static HtmlPrinter GetPrinter(object printObject, World world, ControlOption controlOption)
        {
            if (controlOption == ControlOption.EventOverview)
            {
                return new EventOverviewPrinter(printObject as DwarfObject, world);
            }
            Type printType = printObject.GetType();
            if (printType == typeof(Battle))
            {
                return new BattlePrinter(printObject as Battle, world);
            }

            if (printType == typeof(BeastAttack))
            {
                return new BeastAttackPrinter(printObject as BeastAttack, world);
            }

            if (printType == typeof(Entity))
            {
                return new EntityPrinter(printObject as Entity, world);
            }

            if (printType == typeof(Era))
            {
                return new EraPrinter(printObject as Era, world);
            }

            if (printType == typeof(HistoricalFigure))
            {
                return new HistoricalFigureHtmlPrinter(printObject as HistoricalFigure, world);
            }

            if (printType == typeof(WorldRegion))
            {
                return new RegionPrinter(printObject as WorldRegion, world);
            }

            if (printType == typeof(SiteConquered))
            {
                return new SiteConqueredPrinter(printObject as SiteConquered, world);
            }

            if (printType == typeof(Site))
            {
                return new SitePrinter(printObject as Site, world);
            }

            if (printType == typeof(UndergroundRegion))
            {
                return new UndergroundRegionPrinter(printObject as UndergroundRegion, world);
            }

            if (printType == typeof(War))
            {
                return new WarPrinter(printObject as War, world);
            }

            if (printType == typeof(World))
            {
                return new WorldStatsPrinter(world);
            }

            if (printType == typeof(Artifact))
            {
                return new ArtifactPrinter(printObject as Artifact, world);
            }

            if (printType == typeof(WorldConstruction))
            {
                return new WorldConstructionPrinter(printObject as WorldConstruction, world);
            }

            if (printType == typeof(WrittenContent))
            {
                return new WrittenContentPrinter(printObject as WrittenContent, world);
            }

            if (printType == typeof(DanceForm))
            {
                return new ArtFormPrinter(printObject as ArtForm, world);
            }

            if (printType == typeof(MusicalForm))
            {
                return new ArtFormPrinter(printObject as ArtForm, world);
            }

            if (printType == typeof(PoeticForm))
            {
                return new ArtFormPrinter(printObject as ArtForm, world);
            }

            if (printType == typeof(Structure))
            {
                return new StructurePrinter(printObject as Structure, world);
            }

            if (printType == typeof(Landmass))
            {
                return new LandmassPrinter(printObject as Landmass, world);
            }

            if (printType == typeof(MountainPeak))
            {
                return new MountainPeakPrinter(printObject as MountainPeak, world);
            }

            if (printType == typeof(Raid))
            {
                return new RaidPrinter(printObject as Raid, world);
            }

            if (printType == typeof(string))
            {
                return new StringPrinter(printObject as string);
            }

            throw new Exception("No HTML Printer for type: " + printObject.GetType());
        }

        public string GetHtmlPage()
        {
            var htmlPage = new StringBuilder();
            htmlPage.AppendLine("<!DOCTYPE html><html><head>");
            htmlPage.AppendLine("<title>" + GetTitle() + "</title>");
            htmlPage.AppendLine("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            htmlPage.AppendLine("<script type=\"text/javascript\" src=\"" + LocalFileProvider.LocalPrefix + "WebContent/scripts/jquery-3.1.1.min.js\"></script>");
            htmlPage.AppendLine("<script type=\"text/javascript\" src=\"" + LocalFileProvider.LocalPrefix + "WebContent/scripts/jquery.dataTables.min.js\"></script>");
            htmlPage.AppendLine("<script type=\"text/javascript\" src=\"" + LocalFileProvider.LocalPrefix + "WebContent/scripts/Chart.bundle.min.js\"></script>");
            htmlPage.AppendLine("<link rel=\"stylesheet\" href=\"" + LocalFileProvider.LocalPrefix + "WebContent/styles/bootstrap.min.css\">");
            htmlPage.AppendLine("<link rel=\"stylesheet\" href=\"" + LocalFileProvider.LocalPrefix + "WebContent/styles/font-awesome.min.css\">");
            htmlPage.AppendLine("<link rel=\"stylesheet\" href=\"" + LocalFileProvider.LocalPrefix + "WebContent/styles/legends.css\">");
            htmlPage.AppendLine("<link rel=\"stylesheet\" href=\"" + LocalFileProvider.LocalPrefix + "WebContent/styles/jquery.dataTables.min.css\">");
            htmlPage.AppendLine("</head>");
            htmlPage.AppendLine("<body>");
            htmlPage.AppendLine(Print());
            htmlPage.AppendLine("</body>");
            htmlPage.AppendLine("</html>");
            return htmlPage.ToString();
        }

        protected static string Bold(string text)
        {
            return "<b>" + text + "</b>";
        }

        protected static string Font(string text, string color)
        {
            return "<font color=\"" + color + "\">" + text + "</font>";
        }

        protected enum ListType
        {
            Unordered,
            Ordered
        }

        protected void StartList(ListType listType)
        {
            switch (listType)
            {
                case ListType.Ordered:
                    Html.AppendLine("<ol>"); break;
                case ListType.Unordered:
                    Html.AppendLine("<ul>"); break;
            }
        }

        protected void EndList(ListType listType)
        {
            switch (listType)
            {
                case ListType.Ordered:
                    Html.AppendLine("</ol>"); break;
                case ListType.Unordered:
                    Html.AppendLine("</ul>"); break;
            }
        }

        protected string MakeLink(string text, LinkOption option)
        {
            return "<a href=\"" + option + "\">" + text + "</a>";
        }

        protected string MakeLink(string text, DwarfObject dObject, ControlOption option = ControlOption.Html)
        {
            string objectType;
            int id;
            if (dObject is EventCollection eventCollection)
            {
                objectType = "collection";
                id = eventCollection.Id;
            }
            else if (dObject is HistoricalFigure historicalFigure)
            {
                objectType = "hf";
                id = historicalFigure.Id;
            }
            else if (dObject is Entity entity)
            {
                objectType = "entity";
                id = entity.Id;
            }
            else if (dObject is WorldRegion worldRegion)
            {
                objectType = "region";
                id = worldRegion.Id;
            }
            else if (dObject is UndergroundRegion undergroundRegion)
            {
                objectType = "uregion";
                id = undergroundRegion.Id;
            }
            else if (dObject is Site site)
            {
                objectType = "site";
                id = site.Id;
            }
            else
            {
                throw new Exception("Unhandled make link for type: " + dObject.GetType());
            }

            string optionString = "";
            if (option != ControlOption.Html)
            {
                optionString = "-" + option;
            }

            return "<a href=\"" + objectType + "#" + id + optionString + "\">" + text + "</a>";
        }

        protected string BitmapToHtml(Bitmap image)
        {
            int imageSectionCount = 2;
            Size imageSectionSize = new Size(image.Width / imageSectionCount, image.Height / imageSectionCount);
            string html = "";
            using (Bitmap section = new Bitmap(imageSectionSize.Width, imageSectionSize.Height))
            {
                using (Graphics drawSection = Graphics.FromImage(section))
                {
                    for (int row = 0; row < imageSectionCount; row++)
                    {
                        for (int column = 0; column < imageSectionCount; column++)
                        {
                            drawSection.DrawImage(image, new Rectangle(new Point(0, 0), section.Size),
                                new Rectangle(new Point(section.Size.Width * column, section.Size.Height * row),
                                    section.Size), GraphicsUnit.Pixel);
                            string tempName;
                            while (true)
                            {
                                tempName = Path.Combine(LocalFileProvider.RootFolder, "temp",
                                    Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + Formatting.RemoveSpecialCharacters(GetTitle()) + ".png");
                                if (!File.Exists(tempName))
                                {
                                    break;
                                }
                            }

                            var directoryName = Path.GetDirectoryName(tempName);
                            if (directoryName != null && !Directory.Exists(directoryName))
                            {
                                Directory.CreateDirectory(directoryName);
                            }
                            section.Save(tempName);
                            html += ImageToHtml("temp/" + Path.GetFileName(tempName));
                            _temporaryFiles.Add(tempName);
                        }
                        html += "</br>";
                    }
                }
            }
            image.Dispose();
            return html;
        }

        protected string BitmapToString(Bitmap image)
        {
            string imageString;
            using (MemoryStream miniStream = new MemoryStream())
            {
                image.Save(miniStream, ImageFormat.Bmp);
                byte[] miniMapBytes = miniStream.GetBuffer();
                imageString = Convert.ToBase64String(miniMapBytes);
            }

            return imageString;
        }

        private string ImageToHtml(string image)
        {
            string html = "<img src=\"" + LocalFileProvider.LocalPrefix + image + "\" align=absmiddle />";
            return html;
        }

        protected string Base64ToHtml(string base64)
        {
            string html = "<img src=\"data:image/gif;base64," + base64 + "\" align=absmiddle />";
            return html;
        }

        protected string SkillToString(SkillDescription desc)
        {
            string subrank = desc.Rank.ToLower().Replace(" ", string.Empty).Substring(0, 5);

            return
                "<li class='" + desc.Category
                + " " + subrank
                + "' title='" + desc.Token
                + " | " + desc.Rank
                + " | " + desc.Points
                + "'>" + desc.Rank + " " + desc.Name + "</li>";
        }

        protected string GetHtmlColorByEntity(Entity entity)
        {
            string htmlColor = ColorTranslator.ToHtml(entity.LineColor);
            if (string.IsNullOrEmpty(htmlColor) && entity.Parent != null)
            {
                htmlColor = GetHtmlColorByEntity(entity.Parent);
            }
            if (string.IsNullOrEmpty(htmlColor))
            {
                htmlColor = "#888888";
            }
            return htmlColor;
        }

        protected void PrintPopulations(List<Population> populations)
        {
            if (!populations.Any())
            {
                return;
            }
            Html.AppendLine("<div class=\"col-lg-4 col-md-6 col-sm-12\">");
            var mainRacePops = new List<Population>();
            var animalPeoplePops = new List<Population>();
            var visitorsPops = new List<Population>();
            var outcastsPops = new List<Population>();
            var prisonersPops = new List<Population>();
            var slavesPops = new List<Population>();
            var otherRacePops = new List<Population>();
            foreach (var population in populations)
            {
                if (population.IsMainRace)
                {
                    mainRacePops.Add(population);
                }
                else if (population.IsAnimalPeople)
                {
                    animalPeoplePops.Add(population);
                }
                else if (population.IsVisitors)
                {
                    visitorsPops.Add(population);
                }
                else if (population.IsOutcasts)
                {
                    outcastsPops.Add(population);
                }
                else if (population.IsPrisoners)
                {
                    prisonersPops.Add(population);
                }
                else if (population.IsSlaves)
                {
                    slavesPops.Add(population);
                }
                else
                {
                    otherRacePops.Add(population);
                }
            }
            AddPopulationList(mainRacePops, "Civilized Populations");
            AddPopulationList(animalPeoplePops, "Animal People");
            AddPopulationList(visitorsPops, "Visitors");
            AddPopulationList(outcastsPops, "Outcasts");
            AddPopulationList(prisonersPops, "Prisoners");
            AddPopulationList(slavesPops, "Slaves");
            AddPopulationList(otherRacePops, "Other Populations");
            Html.AppendLine("</div>");
        }

        private void AddPopulationList(List<Population> populations, string populationName)
        {
            if (!populations.Any())
            {
                return;
            }
            Html.AppendLine($"<b>{populationName}</b></br>");
            Html.AppendLine("<ul>");
            foreach (Population population in populations)
            {
                Html.AppendLine("<li>" + population.Count + " " + population.Race + "</li>");
            }

            Html.AppendLine("</ul>");
        }

        protected void PopulatePopulationChartData(List<Population> populations)
        {
            if (populations.Count == 0)
            {
                Html.AppendLine("$('#chart-populationbyrace-container').hide()");
                return;
            }
            string races = "";
            string popCounts = "";
            string backgroundColors = "";

            for (int i = 0; i < populations.Count; i++)
            {
                Population civilizedPop = populations[i];
                Color civilizedPopColor = World.MainRaces.ContainsKey(civilizedPop.Race)
                    ? World.MainRaces.First(r => r.Key == civilizedPop.Race).Value
                    : Color.Gray;

                Color darkenedPopColor = HtmlStyleUtil.ChangeColorBrightness(civilizedPopColor, -0.1f);

                races += (i != 0 ? "," : "") + "'" + civilizedPop.Race + "'";
                popCounts += (i != 0 ? "," : "") + civilizedPop.Count;
                backgroundColors += (i != 0 ? "," : "") + "'" + ColorTranslator.ToHtml(darkenedPopColor) + "'";
            }

            Html.AppendLine("setTimeout(function(){");
            Html.AppendLine("var chartPopulationByRace = new Chart(document.getElementById('chart-populationbyrace').getContext('2d'), { type: 'doughnut', ");
            Html.AppendLine("data: {");
            Html.AppendLine("labels: [" + races + "], ");
            Html.AppendLine("datasets:[{");
            Html.AppendLine("data:[" + popCounts + "], ");
            Html.AppendLine("backgroundColor:[" + backgroundColors + "]");
            Html.AppendLine("}],");
            Html.AppendLine("},");
            Html.AppendLine("options:{");
            Html.AppendLine("maintainAspectRatio: false,");
            Html.AppendLine("legend:{");
            Html.AppendLine("position:'right',");
            Html.AppendLine("labels: { boxWidth: 12 }");
            Html.AppendLine("}");
            Html.AppendLine("}");
            Html.AppendLine("});");
            Html.AppendLine("}, 10);");
        }

        protected void PrintEventLog(World world, List<WorldEvent> events, List<string> filters, DwarfObject dfo)
        {
            if (!events.Any())
            {
                return;
            }

            var chartLabels = new List<string>();
            var eventDataSet = new List<string>();
            var filteredEventDataSet = new List<string>();
            var groupedEvents = events
                .GroupBy(e => e.Year)
                .ToDictionary(group => group.Key, group => group.ToList())
                .OrderBy(g => g.Key);
            var startYear = world.Eras.Min(e => e.StartYear);
            var endYear = world.Eras.Max(e => e.EndYear);

            for (int currentYear = startYear; currentYear < endYear; currentYear++)
            {
                chartLabels.Add(currentYear.ToString());
                var eventsOfCurrentYear = groupedEvents.FirstOrDefault(group => group.Key == currentYear);
                if (eventsOfCurrentYear.Value == null)
                {
                    eventDataSet.Add("0");
                    if (filters != null && filters.Any())
                    {
                        filteredEventDataSet.Add("0");
                    }

                    continue;
                }
                var eventsOfCurrentYearCount = eventsOfCurrentYear.Value.Count;
                eventDataSet.Add(eventsOfCurrentYearCount.ToString());
                if (filters != null && filters.Any())
                {
                    var hiddenEventsOfCurrentYearCount = eventsOfCurrentYear.Value.Count(e => filters.Contains(e.Type));
                    var filteredEventsPerYearCount = eventsOfCurrentYearCount - hiddenEventsOfCurrentYearCount;
                    filteredEventDataSet.Add(filteredEventsPerYearCount.ToString());
                }
            }

            Html.AppendLine(Bold("Event Chart"));
            Html.AppendLine("<div class=\"line-chart\">");
            Html.AppendLine("<canvas id=\"event-chart\"></canvas>");
            Html.AppendLine("</div>" + LineBreak);

            Html.AppendLine("<b>Event Log</b> ");
            Html.AppendLine(MakeLink(Font("[Chart]", "Maroon"), LinkOption.LoadChart));
            Html.AppendLine(MakeLink(Font("[Event Overview]", "Maroon"), LinkOption.LoadEventOverview));
            Html.AppendLine("<br/><br/>");
            Html.AppendLine("<table id=\"lv_eventtable\" class=\"display\" width=\"100 %\"></table>");
            Html.AppendLine("<script>");
            Html.AppendLine("$(document).ready(function() {");
            Html.AppendLine("   var dataSet = [");
            foreach (var e in events)
            {
                if (filters == null || !filters.Contains(e.Type))
                {
                    Html.AppendLine("['" + e.Date + "','" + e.Print(true, dfo).Replace("'", "`") + "','" + e.Type + "'],");
                }
            }

            Html.AppendLine("   ];");
            Html.AppendLine("   $('#lv_eventtable').dataTable({");
            Html.AppendLine("       pageLength: 100,");
            Html.AppendLine("       data: dataSet,");
            Html.AppendLine("       columns: [");
            Html.AppendLine("           { title: \"Date\", type: \"string\", width: \"60px\" },");
            Html.AppendLine("           { title: \"Description\", type: \"html\" },");
            Html.AppendLine("           { title: \"Type\", type: \"string\" }");
            Html.AppendLine("       ]");
            Html.AppendLine("   });");
            Html.AppendLine("   var eventChart = new Chart(document.getElementById('event-chart').getContext('2d'), { ");
            Html.AppendLine("       type: 'line', ");
            Html.AppendLine("       data: {");
            Html.AppendLine("           labels: [" + string.Join(",", chartLabels) + "], ");
            Html.AppendLine("           datasets:[");

            if (filters != null && filters.Any())
            {
                Html.AppendLine("               {");
                Html.AppendLine("                   label:'Events (filtered)',");
                Html.AppendLine("                   borderColor:'rgba(255, 205, 86, 0.4)',");
                Html.AppendLine("                   backgroundColor:'rgba(255, 205, 86, 0.2)',");
                Html.AppendLine("                   data:[" + string.Join(",", filteredEventDataSet) + "]");
                Html.AppendLine("               }");
            }
            else
            {
                Html.AppendLine("               {");
                Html.AppendLine("                   label:'Events',");
                Html.AppendLine("                   borderColor:'rgba(54, 162, 235, 0.4)',");
                Html.AppendLine("                   backgroundColor:'rgba(54, 162, 235, 0.2)',");
                Html.AppendLine("                   data:[" + string.Join(",", eventDataSet) + "]");
                Html.AppendLine("               }");
            }

            Html.AppendLine("           ]");
            Html.AppendLine("       },");
            Html.AppendLine("       options:{");
            Html.AppendLine("           maintainAspectRatio: false, ");
            Html.AppendLine("           legend:{");
            Html.AppendLine("               position:'top',");
            Html.AppendLine("               labels: { boxWidth: 12 }");
            Html.AppendLine("           },");
            Html.AppendLine("           scales:{");
            Html.AppendLine("               yAxes: [{");
            Html.AppendLine("                   ticks: {");
            Html.AppendLine("                       beginAtZero: true,");
            Html.AppendLine("                       min: 0,");
            Html.AppendLine("                       precision: 0");
            Html.AppendLine("                   }");
            Html.AppendLine("               }]");
            Html.AppendLine("           }");
            Html.AppendLine("       }");
            Html.AppendLine("   });");
            Html.AppendLine("});");
            Html.AppendLine("</script>");
        }

        protected void PrintEventDetails(List<WorldEvent> eventList)
        {
            Html.AppendLine("<h1>Events: " + eventList.Count + "</h1>");
            var eventTypesAndCounts = from dEvent in eventList
                         group dEvent by dEvent.Type into eventType
                         select new { Type = eventType.Key, Count = eventType.Count() };
            eventTypesAndCounts = eventTypesAndCounts.OrderByDescending(dEvent => dEvent.Count);
            Html.AppendLine("<ul>");
            foreach (var eventTypeAndCount in eventTypesAndCounts)
            {
                Html.AppendLine("<li title='" + eventTypeAndCount.Type + "'>" + AppHelpers.EventInfo[Array.IndexOf(AppHelpers.EventInfo, AppHelpers.EventInfo.Single(eventInfo => eventInfo[0] == eventTypeAndCount.Type))][1] + ": " + eventTypeAndCount.Count + "</li>");
                switch (eventTypeAndCount.Type)
                {
                    case "hf died":
                        PrintEventDetailsHfDied(eventList);
                        break;
                    case "hf simple battle event":
                        PrintEventDetailsHfSimpleBattle(eventList);
                        break;
                    case "change hf state":
                        PrintEventDetailsChangeHfState(eventList);
                        break;
                    case "change hf job":
                        PrintEventDetailsChangeHfJob(eventList);
                        break;
                    case "written content composed":
                        PrintEventDetailsWrittenContentComposed(eventList);
                        break;
                    case "add hf entity link":
                        PrintEventDetailsAddHfEntityLink(eventList);
                        break;
                    case "add hf hf link":
                        PrintEventDetailsAddHfHfLink(eventList);
                        break;
                    case "creature devoured":
                        PrintEventDetailsCreatureDevoured(eventList);
                        break;
                }
            }
            Html.AppendLine("</ul>");
            Html.AppendLine("</br>");
        }

        private void PrintEventDetailsCreatureDevoured(List<WorldEvent> eventList)
        {
            var eventInfos = eventList.OfType<CreatureDevoured>()
                .GroupBy(eventInfo => eventInfo.Eater?.GetRaceString() ?? "Unknown")
                .Select(grouping => new
                {
                    Type = grouping.Key,
                    Count = grouping.Count()
                });
            eventInfos = eventInfos.OrderByDescending(grouping => grouping.Count);
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Eaten by</li>");
            Html.AppendLine("<ul>");
            foreach (var grouping in eventInfos)
            {
                Html.AppendLine("<li>" + Formatting.InitCaps(grouping.Type) + ": " + grouping.Count + "</li>");
            }

            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");
        }

        private void PrintEventDetailsAddHfHfLink(List<WorldEvent> eventList)
        {
            var eventInfos = eventList.OfType<AddHfhfLink>()
                .GroupBy(eventInfo => eventInfo.LinkType)
                .Select(grouping => new
                {
                    Type = grouping.Key,
                    Count = grouping.Count()
                });
            eventInfos = eventInfos.OrderByDescending(grouping => grouping.Count);
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Types</li>");
            Html.AppendLine("<ul>");
            foreach (var grouping in eventInfos)
            {
                Html.AppendLine("<li>" + grouping.Type.GetDescription() + ": " + grouping.Count + "</li>");
            }

            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");
        }

        private void PrintEventDetailsAddHfEntityLink(List<WorldEvent> eventList)
        {
            var eventInfos = eventList.OfType<AddHfEntityLink>()
                .GroupBy(eventInfo => eventInfo.LinkType)
                .Select(grouping => new
                {
                    Type = grouping.Key,
                    Count = grouping.Count()
                });
            eventInfos = eventInfos.OrderByDescending(grouping => grouping.Count);
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Types</li>");
            Html.AppendLine("<ul>");
            foreach (var grouping in eventInfos)
            {
                Html.AppendLine("<li>" + grouping.Type.GetDescription() + ": " + grouping.Count + "</li>");
            }

            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");
        }

        private void PrintEventDetailsWrittenContentComposed(List<WorldEvent> eventList)
        {
            var eventInfos = eventList.OfType<WrittenContentComposed>()
                .GroupBy(eventInfo => eventInfo.WrittenContent?.Type ?? WrittenContentType.Unknown)
                .Select(grouping => new
                {
                    Type = grouping.Key,
                    Count = grouping.Count()
                });
            eventInfos = eventInfos.OrderByDescending(grouping => grouping.Count);
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Types</li>");
            Html.AppendLine("<ul>");
            foreach (var grouping in eventInfos)
            {
                Html.AppendLine("<li>" + grouping.Type.GetDescription() + ": " + grouping.Count + "</li>");
            }

            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");
        }

        private void PrintEventDetailsChangeHfJob(List<WorldEvent> eventList)
        {
            var jobChangeInfo = eventList.OfType<ChangeHfJob>()
                .GroupBy(jobChange => jobChange.NewJob)
                .Select(jobs => new
                {
                    Type = jobs.Key,
                    Count = jobs.Count()
                });
            jobChangeInfo = jobChangeInfo.OrderByDescending(job => job.Count);
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Jobs</li>");
            Html.AppendLine("<ul>");
            foreach (var job in jobChangeInfo)
            {
                Html.AppendLine("<li>" + Formatting.InitCaps(job.Type) + ": " + job.Count + "</li>");
            }

            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");
        }

        private void PrintEventDetailsChangeHfState(List<WorldEvent> eventList)
        {
            var stateChangeInfo = eventList.OfType<ChangeHfState>().Where(e => e.State != HfState.Unknown)
                .GroupBy(changeStateEvent => changeStateEvent.State.GetDescription())
                .Select(changeStateEventsGroupedByState => new
                {
                    Type = changeStateEventsGroupedByState.Key,
                    Count = changeStateEventsGroupedByState.Count()
                });
            stateChangeInfo = stateChangeInfo.OrderByDescending(state => state.Count);

            var moodChangeInfo = eventList.OfType<ChangeHfState>().Where(e => e.Mood != Mood.Unknown)
                .GroupBy(changeMoodEvent => changeMoodEvent.Mood.GetDescription())
                .Select(changeStateEventsGroupedByMood => new
                {
                    Type = changeStateEventsGroupedByMood.Key,
                    Count = changeStateEventsGroupedByMood.Count()
                });
            moodChangeInfo = moodChangeInfo.OrderByDescending(mood => mood.Count);
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>States</li>");
            Html.AppendLine("<ul>");
            foreach (var state in stateChangeInfo)
            {
                Html.AppendLine("<li>" + state.Type.GetDescription() + ": " + state.Count + "</li>");
            }

            if (moodChangeInfo.Any())
            {
                Html.AppendLine("<li>Moods</li>");
                Html.AppendLine("<ul>");
                foreach (var mood in moodChangeInfo)
                {
                    Html.AppendLine("<li>" + mood.Type.GetDescription() + ": " + mood.Count + "</li>");
                }
                Html.AppendLine("</ul>");
            }
            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");
        }

        private void PrintEventDetailsHfSimpleBattle(List<WorldEvent> eventList)
        {
            var fightTypes = from fight in eventList.OfType<HfSimpleBattleEvent>()
                group fight by fight.SubType
                into fightType
                select new {Type = fightType.Key, Count = fightType.Count()};
            fightTypes = fightTypes.OrderByDescending(fightType => fightType.Count);
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Fight SubTypes</li>");
            Html.AppendLine("<ul>");
            foreach (var fightType in fightTypes)
            {
                Html.AppendLine("<li>" + fightType.Type.GetDescription() + ": " + fightType.Count + "</li>");
            }

            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");
        }

        private void PrintEventDetailsHfDied(List<WorldEvent> eventList)
        {
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>As part of Collection</li>");
            var deathCollections = from death in eventList.OfType<HfDied>().Where(death => death.ParentCollection != null)
                group death by death.ParentCollection.Type
                into collectionType
                select new {Type = collectionType.Key, Count = collectionType.Count()};
            deathCollections = deathCollections.OrderByDescending(collectionType => collectionType.Count);
            Html.AppendLine("<ul>");
            foreach (var deathCollection in deathCollections)
            {
                Html.AppendLine("<li>" + Formatting.InitCaps(deathCollection.Type) + ": " + deathCollection.Count + "</li>");
            }

            Html.AppendLine("<li>None: " + eventList.OfType<HfDied>().Count(death => death.ParentCollection == null) + "</li>");
            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");

            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Deceased by Race</li>");
            Html.AppendLine("<ol>");
            var hfDeaths = from death in eventList.OfType<HfDied>()
                group death by death.HistoricalFigure?.GetRaceString() ?? "Unknown"
                into deathRace
                select new {Type = deathRace.Key, Count = deathRace.Count()};
            hfDeaths = hfDeaths.OrderByDescending(death => death.Count);
            foreach (var hfdeath in hfDeaths)
            {
                Html.AppendLine("<li>" + Formatting.InitCaps(hfdeath.Type) + ": " + hfdeath.Count + "</li>");
            }

            Html.AppendLine("</ol>");
            Html.AppendLine("</ul>");

            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Slayers by Race</li>");
            Html.AppendLine("<ol>");
            var eventInfos = from death in eventList.OfType<HfDied>().Where(e => e.Slayer != null)
                group death by death.Slayer?.GetRaceString() ?? "Unknown"
                into deathRace
                select new { Type = deathRace.Key, Count = deathRace.Count() };
            eventInfos = eventInfos.OrderByDescending(death => death.Count);
            foreach (var eventInfo in eventInfos)
            {
                Html.AppendLine("<li>" + Formatting.InitCaps(eventInfo.Type) + ": " + eventInfo.Count + "</li>");
            }

            Html.AppendLine("</ol>");
            Html.AppendLine("</ul>");

            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Deaths by Cause</li>");
            Html.AppendLine("<ul>");
            var deathCauses = from death in eventList.OfType<HfDied>()
                group death by death.Cause
                into deathCause
                select new {Cause = deathCause.Key, Count = deathCause.Count()};
            deathCauses = deathCauses.OrderByDescending(death => death.Count);
            foreach (var deathCause in deathCauses)
            {
                Html.AppendLine("<li>" + deathCause.Cause.GetDescription() + ": " + deathCause.Count + "</li>");
            }

            Html.AppendLine("</ul>");
            Html.AppendLine("</ul>");
        }

        protected void PrintEventCollectionDetails(List<EventCollection> eventCollectionList)
        {
            Html.AppendLine("<h1>Event Collections: " + eventCollectionList.Count + "</h1>");
            var collectionTypes = from collection in eventCollectionList
                                  group collection by collection.Type into collectionType
                                  select new { Type = collectionType.Key, Count = collectionType.Count() };
            collectionTypes = collectionTypes.OrderByDescending(collection => collection.Count);
            foreach (var collectionType in collectionTypes)
            {
                Html.AppendLine("<h2>" + Formatting.InitCaps(collectionType.Type) + ": " + collectionType.Count + "</h2>");
                Html.AppendLine("<ul>");
                var subCollections = from subCollection in eventCollectionList.Where(collection => collection.Type == collectionType.Type).SelectMany(collection => collection.Collections)
                                     group subCollection by subCollection.Type into subCollectionType
                                     select new { Type = subCollectionType.Key, Count = subCollectionType.Count() };
                subCollections = subCollections.OrderByDescending(collection => collection.Count);
                if (subCollections.Any())
                {
                    Html.AppendLine("<li>Sub Collections");
                    Html.AppendLine("<ul>");
                    foreach (var subCollection in subCollections)
                    {
                        Html.AppendLine("<li>" + Formatting.InitCaps(subCollection.Type) + ": " + subCollection.Count + "</li>");
                    }

                    Html.AppendLine("</ul>");
                }

                Html.AppendLine("<li>Events");
                var eventTypes = from dwarfEvent in eventCollectionList.Where(collection => collection.Type == collectionType.Type).SelectMany(collection => collection.Collection)
                                 group dwarfEvent by dwarfEvent.Type into eventType
                                 select new { Type = eventType.Key, Count = eventType.Count() };
                eventTypes = eventTypes.OrderByDescending(eventType => eventType.Count);
                Html.AppendLine("<ul>");
                foreach (var eventType in eventTypes)
                {
                    Html.AppendLine("<li>" + AppHelpers.EventInfo[Array.IndexOf(AppHelpers.EventInfo, AppHelpers.EventInfo.Single(eventInfo => eventInfo[0] == eventType.Type))][1] + ": " + eventType.Count + "</li>");
                }

                Html.AppendLine("</ul>");

                Html.AppendLine("</ul>");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _disposed &= !_temporaryFiles.Any();
            if (!_disposed)
            {
                if (disposing)
                {
                }
            }
            _disposed = true;
        }

        public void DeleteTemporaryFiles()
        {
            foreach (string filename in _temporaryFiles)
            {
                try
                {
                    File.Delete(filename);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            _temporaryFiles.Clear();
        }
    }

    public enum LinkOption
    {
        LoadMap,
        LoadChart,
        LoadSiteMap,
        LoadEventOverview
    }

    public enum ControlOption
    {
        Html,
        Map,
        Chart,
        Search,
        ReadMe,
        EventOverview
    }

    public class TableMaker
    {
        readonly StringBuilder _html;
        readonly bool _numbered;
        int _count;
        public TableMaker(bool numbered = false, int width = 0)
        {
            _html = new StringBuilder();
            string tableStart = "<table border=\"0\"";
            if (width > 0)
            {
                tableStart += " width=\"" + width + "\"";
            }

            tableStart += ">";
            _html.AppendLine(tableStart);
            _numbered = numbered;
            _count = 1;
        }

        public void StartRow()
        {
            _html.AppendLine("<tr>");
            if (_numbered)
            {
                AddData(_count.ToString(), 20, TableDataAlign.Right);
                AddData("", 10);
            }
        }

        public void EndRow()
        {
            _html.AppendLine("</tr>");
            _count++;
        }

        public void AddData(string data, int width = 0, TableDataAlign align = TableDataAlign.Left)
        {
            string dataHtml = "<td";
            if (width > 0)
            {
                dataHtml += " width=\"" + width + "\"";
            }

            if (align != TableDataAlign.Left)
            {
                dataHtml += " align=";
                switch (align)
                {
                    case TableDataAlign.Right:
                        dataHtml += "\"right\""; break;
                    case TableDataAlign.Center:
                        dataHtml += "\"center\""; break;
                }
            }
            dataHtml += ">";
            dataHtml += data + "</td>";
            _html.AppendLine(dataHtml);
        }

        public string GetTable()
        {
            _html.AppendLine("</table>");
            return _html.ToString();
        }
    }

    public enum TableDataAlign
    {
        Left,
        Right,
        Center
    }
}

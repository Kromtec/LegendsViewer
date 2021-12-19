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
using LegendsViewer.Legends.WorldObjects;

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

            if (printType == typeof(River))
            {
                return new RiverPrinter(printObject as River, world);
            }

            if (printType == typeof(Raid))
            {
                return new RaidPrinter(printObject as Raid, world);
            }

            return printType == typeof(string)
                ? new StringPrinter(printObject as string)
                : throw new Exception("No HTML Printer for type: " + printObject.GetType());
        }

        public string GetHtmlPage()
        {
            var htmlPage = new StringBuilder();
            htmlPage.AppendLine("<!DOCTYPE html><html><head>")
                .Append("<title>").Append(GetTitle()).AppendLine("</title>")
                .AppendLine("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">")
                .Append("<script type=\"text/javascript\" src=\"").Append(LocalFileProvider.LocalPrefix).AppendLine("WebContent/scripts/jquery-3.1.1.min.js\"></script>")
                .Append("<script type=\"text/javascript\" src=\"").Append(LocalFileProvider.LocalPrefix).AppendLine("WebContent/scripts/jquery.dataTables.min.js\"></script>")
                .Append("<script type=\"text/javascript\" src=\"").Append(LocalFileProvider.LocalPrefix).AppendLine("WebContent/scripts/Chart.bundle.min.js\"></script>")
                .Append("<link rel=\"stylesheet\" href=\"").Append(LocalFileProvider.LocalPrefix).AppendLine("WebContent/styles/bootstrap.min.css\">")
                .Append("<link rel=\"stylesheet\" href=\"").Append(LocalFileProvider.LocalPrefix).AppendLine("WebContent/styles/font-awesome.min.css\">")
                .Append("<link rel=\"stylesheet\" href=\"").Append(LocalFileProvider.LocalPrefix).AppendLine("WebContent/styles/legends.css\">")
                .Append("<link rel=\"stylesheet\" href=\"").Append(LocalFileProvider.LocalPrefix).AppendLine("WebContent/styles/jquery.dataTables.min.css\">")
                .AppendLine("</head>")
                .AppendLine("<body>")
                .AppendLine(Print())
                .AppendLine("</body>")
                .AppendLine("</html>");
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
            const int imageSectionCount = 2;
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
                            do
                            {
                                tempName = Path.Combine(LocalFileProvider.RootFolder, "temp",
                                    Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + Formatting.RemoveSpecialCharacters(GetTitle()) + ".png");
                            }
                            while (File.Exists(tempName));

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

        public static string SkillToString(SkillDescription desc)
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
            if (populations.Count == 0)
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

        private void PrintPopulationEntry(CreatureInfo creatureInfo, int count)
        {
            if (count == int.MaxValue)
            {
                Html.Append("<li>Unnumbered ").Append(creatureInfo.NamePlural).AppendLine("</li>");
            }
            else if (count == 1)
            {
                Html.Append("<li>").Append(count).Append(' ').Append(creatureInfo.NameSingular).AppendLine("</li>");
            }
            else
            {
                Html.Append("<li>").Append(count).Append(' ').Append(creatureInfo.NamePlural).AppendLine("</li>");
            }
        }

        private void AddPopulationList(List<Population> populations, string populationName)
        {
            if (populations.Count == 0)
            {
                return;
            }
            Html.Append("<b>").Append(populationName).AppendLine("</b></br>")
                .AppendLine("<ul>");
            foreach (Population population in populations)
            {
                PrintPopulationEntry(population.Race, population.Count);
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

                races += (i != 0 ? "," : "") + "'" + civilizedPop.Race.NamePlural + "'";
                popCounts += (i != 0 ? "," : "") + civilizedPop.Count;
                backgroundColors += (i != 0 ? "," : "") + "'" + ColorTranslator.ToHtml(darkenedPopColor) + "'";
            }

            Html.AppendLine("setTimeout(function(){")
                .AppendLine("var chartPopulationByRace = new Chart(document.getElementById('chart-populationbyrace').getContext('2d'), { type: 'doughnut', ")
                .AppendLine("data: {")
                .Append("labels: [").Append(races).AppendLine("], ")
                .AppendLine("datasets:[{")
                .Append("data:[").Append(popCounts).AppendLine("], ")
                .Append("backgroundColor:[").Append(backgroundColors).AppendLine("]")
                .AppendLine("}],")
                .AppendLine("},")
                .AppendLine("options:{")
                .AppendLine("maintainAspectRatio: false,")
                .AppendLine("legend:{")
                .AppendLine("position:'right',")
                .AppendLine("labels: { boxWidth: 12 }")
                .AppendLine("}")
                .AppendLine("}")
                .AppendLine("});")
                .AppendLine("}, 10);");
        }

        protected void PrintEventLog(World world, List<WorldEvent> events, List<string> filters, DwarfObject dfo)
        {
            if (events.Count == 0)
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

            for (int currentYear = startYear; currentYear <= endYear; currentYear++)
            {
                chartLabels.Add(currentYear.ToString());
                var eventsOfCurrentYear = groupedEvents.FirstOrDefault(group => group.Key == currentYear);
                if (eventsOfCurrentYear.Value == null)
                {
                    eventDataSet.Add("0");
                    if (filters?.Any() == true)
                    {
                        filteredEventDataSet.Add("0");
                    }

                    continue;
                }
                var eventsOfCurrentYearCount = eventsOfCurrentYear.Value.Count;
                eventDataSet.Add(eventsOfCurrentYearCount.ToString());
                if (filters?.Any() == true)
                {
                    var hiddenEventsOfCurrentYearCount = eventsOfCurrentYear.Value.Count(e => filters.Contains(e.Type));
                    var filteredEventsPerYearCount = eventsOfCurrentYearCount - hiddenEventsOfCurrentYearCount;
                    filteredEventDataSet.Add(filteredEventsPerYearCount.ToString());
                }
            }

            Html.AppendLine(Bold("Event Chart"))
                .AppendLine("<div class=\"line-chart\">")
                .AppendLine("<canvas id=\"event-chart\"></canvas>")
                .AppendLine("</div>" + LineBreak)
                .AppendLine("<b>Event Log</b> ")
                .AppendLine("(");
            if (filters?.Any() == true)
            {
                Html.Append(events.Count(e => !filters.Contains(e.Type))).AppendLine("/");
            }
            Html.AppendLine(events.Count.ToString())
                .AppendLine(") ")
                .AppendLine(MakeLink(Font("[Chart]", "Maroon"), LinkOption.LoadChart))
                .AppendLine(MakeLink(Font("[Event Overview]", "Maroon"), LinkOption.LoadEventOverview))
                .AppendLine("<br/><br/>")
                .AppendLine("<table id=\"lv_eventtable\" class=\"display\" width=\"100 %\"></table>")
                .AppendLine("<script>")
                .AppendLine("$(document).ready(function() {")
                .AppendLine("   var dataSet = [");
            foreach (var e in events.OrderBy(e => e.Year).ThenBy(e => e.Id))
            {
                if (filters?.Contains(e.Type) != true)
                {
                    Html.Append("['").Append(e.Date).Append("','").Append(e.Print(true, dfo).Replace("'", "`")).Append("','").Append(e.Type).AppendLine("'],");
                }
            }

            Html.AppendLine("   ];")
                .AppendLine("   $('#lv_eventtable').dataTable({")
                .AppendLine("       pageLength: 100,")
                .AppendLine("       data: dataSet,")
                .AppendLine("       columns: [")
                .AppendLine("           { title: \"Date\", type: \"string\", width: \"60px\" },")
                .AppendLine("           { title: \"Description\", type: \"html\" },")
                .AppendLine("           { title: \"Type\", type: \"string\" }")
                .AppendLine("       ]")
                .AppendLine("   });")
                .AppendLine("   var eventChart = new Chart(document.getElementById('event-chart').getContext('2d'), { ")
                .AppendLine("       type: 'line', ")
                .AppendLine("       data: {")
                .Append("           labels: [").Append(string.Join(",", chartLabels)).AppendLine("], ")
                .AppendLine("           datasets:[");

            if (filters?.Any() == true)
            {
                Html.AppendLine("               {")
                    .AppendLine("                   label:'Events (filtered)',")
                    .AppendLine("                   borderColor:'rgba(255, 205, 86, 0.4)',")
                    .AppendLine("                   backgroundColor:'rgba(255, 205, 86, 0.2)',")
                    .Append("                   data:[").Append(string.Join(",", filteredEventDataSet)).AppendLine("]")
                    .AppendLine("               }");
            }
            else
            {
                Html.AppendLine("               {")
                    .AppendLine("                   label:'Events',")
                    .AppendLine("                   borderColor:'rgba(54, 162, 235, 0.4)',")
                    .AppendLine("                   backgroundColor:'rgba(54, 162, 235, 0.2)',")
                    .Append("                   data:[").Append(string.Join(",", eventDataSet)).AppendLine("]")
                    .AppendLine("               }");
            }

            Html.AppendLine("           ]")
                .AppendLine("       },")
                .AppendLine("       options:{")
                .AppendLine("           maintainAspectRatio: false, ")
                .AppendLine("           legend:{")
                .AppendLine("               position:'top',")
                .AppendLine("               labels: { boxWidth: 12 }")
                .AppendLine("           },")
                .AppendLine("           scales:{")
                .AppendLine("               yAxes: [{")
                .AppendLine("                   ticks: {")
                .AppendLine("                       beginAtZero: true,")
                .AppendLine("                       min: 0,")
                .AppendLine("                       precision: 0")
                .AppendLine("                   }")
                .AppendLine("               }]")
                .AppendLine("           }")
                .AppendLine("       }")
                .AppendLine("   });")
                .AppendLine("});")
                .AppendLine("</script>");
        }

        protected void PrintEventDetails(List<WorldEvent> eventList)
        {
            Html.Append("<h1>Events: ").Append(eventList.Count).AppendLine("</h1>");
            var eventTypesAndCounts = from dEvent in eventList
                                      group dEvent by dEvent.Type into eventType
                                      select new { Type = eventType.Key, Count = eventType.Count() };
            eventTypesAndCounts = eventTypesAndCounts.OrderByDescending(dEvent => dEvent.Count);
            Html.AppendLine("<ul>");
            foreach (var eventTypeAndCount in eventTypesAndCounts)
            {
                Html.Append("<li title='").Append(eventTypeAndCount.Type).Append("'>").Append(AppHelpers.EventInfo[Array.IndexOf(AppHelpers.EventInfo, AppHelpers.EventInfo.Single(eventInfo => eventInfo[0] == eventTypeAndCount.Type))][1]).Append(": ").Append(eventTypeAndCount.Count).AppendLine("</li>");
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
            Html.AppendLine("</ul>")
                .AppendLine("</br>");
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
            Html.AppendLine("<ul>")
                .AppendLine("<li>Eaten by</li>")
                .AppendLine("<ul>");
            foreach (var grouping in eventInfos)
            {
                Html.Append("<li>").Append(Formatting.InitCaps(grouping.Type)).Append(": ").Append(grouping.Count).AppendLine("</li>");
            }

            Html.AppendLine("</ul>")
                .AppendLine("</ul>");
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
            Html.AppendLine("<ul>")
                .AppendLine("<li>Types</li>")
                .AppendLine("<ul>");
            foreach (var grouping in eventInfos)
            {
                Html.Append("<li>").Append(grouping.Type.GetDescription()).Append(": ").Append(grouping.Count).AppendLine("</li>");
            }

            Html.AppendLine("</ul>")
                .AppendLine("</ul>");
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
            Html.AppendLine("<ul>")
                .AppendLine("<li>Types</li>")
                .AppendLine("<ul>");
            foreach (var grouping in eventInfos)
            {
                Html.Append("<li>").Append(grouping.Type.GetDescription()).Append(": ").Append(grouping.Count).AppendLine("</li>");
            }

            Html.AppendLine("</ul>")
                .AppendLine("</ul>");
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
            Html.AppendLine("<ul>")
                .AppendLine("<li>Types</li>")
                .AppendLine("<ul>");
            foreach (var grouping in eventInfos)
            {
                Html.Append("<li>").Append(grouping.Type.GetDescription()).Append(": ").Append(grouping.Count).AppendLine("</li>");
            }

            Html.AppendLine("</ul>")
                .AppendLine("</ul>");
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
            Html.AppendLine("<ul>")
                .AppendLine("<li>Jobs</li>")
                .AppendLine("<ul>");
            foreach (var job in jobChangeInfo)
            {
                Html.Append("<li>").Append(Formatting.InitCaps(job.Type)).Append(": ").Append(job.Count).AppendLine("</li>");
            }

            Html.AppendLine("</ul>")
                .AppendLine("</ul>");
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
            Html.AppendLine("<ul>")
                .AppendLine("<li>States</li>")
                .AppendLine("<ul>");
            foreach (var state in stateChangeInfo)
            {
                Html.Append("<li>").Append(state.Type.GetDescription()).Append(": ").Append(state.Count).AppendLine("</li>");
            }

            if (moodChangeInfo.Any())
            {
                Html.AppendLine("<li>Moods</li>")
                    .AppendLine("<ul>");
                foreach (var mood in moodChangeInfo)
                {
                    Html.Append("<li>").Append(mood.Type.GetDescription()).Append(": ").Append(mood.Count).AppendLine("</li>");
                }
                Html.AppendLine("</ul>");
            }
            Html.AppendLine("</ul>")
                .AppendLine("</ul>");
        }

        private void PrintEventDetailsHfSimpleBattle(List<WorldEvent> eventList)
        {
            var fightTypes = from fight in eventList.OfType<HfSimpleBattleEvent>()
                             group fight by fight.SubType
                into fightType
                             select new { Type = fightType.Key, Count = fightType.Count() };
            fightTypes = fightTypes.OrderByDescending(fightType => fightType.Count);
            Html.AppendLine("<ul>")
                .AppendLine("<li>Fight SubTypes</li>")
                .AppendLine("<ul>");
            foreach (var fightType in fightTypes)
            {
                Html.Append("<li>").Append(fightType.Type.GetDescription()).Append(": ").Append(fightType.Count).AppendLine("</li>");
            }

            Html.AppendLine("</ul>")
                .AppendLine("</ul>");
        }

        private void PrintEventDetailsHfDied(List<WorldEvent> eventList)
        {
            Html.AppendLine("<ul>")
                .AppendLine("<li>As part of Collection</li>");
            var deathCollections = from death in eventList.OfType<HfDied>().Where(death => death.ParentCollection != null)
                                   group death by death.ParentCollection.Type
                into collectionType
                                   select new { Type = collectionType.Key, Count = collectionType.Count() };
            deathCollections = deathCollections.OrderByDescending(collectionType => collectionType.Count);
            Html.AppendLine("<ul>");
            foreach (var deathCollection in deathCollections)
            {
                Html.Append("<li>").Append(Formatting.InitCaps(deathCollection.Type)).Append(": ").Append(deathCollection.Count).AppendLine("</li>");
            }

            Html.Append("<li>None: ").Append(eventList.OfType<HfDied>().Count(death => death.ParentCollection == null)).AppendLine("</li>")
                .AppendLine("</ul>")
                .AppendLine("</ul>")
                .AppendLine("<ul>")
                .AppendLine("<li>Deceased by Race</li>")
                .AppendLine("<ol>");
            var hfDeaths = from death in eventList.OfType<HfDied>()
                           group death by death.HistoricalFigure?.GetRaceString() ?? "Unknown"
                into deathRace
                           select new { Type = deathRace.Key, Count = deathRace.Count() };
            hfDeaths = hfDeaths.OrderByDescending(death => death.Count);
            foreach (var hfdeath in hfDeaths)
            {
                Html.Append("<li>").Append(Formatting.InitCaps(hfdeath.Type)).Append(": ").Append(hfdeath.Count).AppendLine("</li>");
            }

            Html.AppendLine("</ol>")
                .AppendLine("</ul>")
                .AppendLine("<ul>")
                .AppendLine("<li>Slayers by Race</li>")
                .AppendLine("<ol>");
            var eventInfos = from death in eventList.OfType<HfDied>().Where(e => e.Slayer != null)
                             group death by death.Slayer?.GetRaceString() ?? "Unknown"
                into deathRace
                             select new { Type = deathRace.Key, Count = deathRace.Count() };
            eventInfos = eventInfos.OrderByDescending(death => death.Count);
            foreach (var eventInfo in eventInfos)
            {
                Html.Append("<li>").Append(Formatting.InitCaps(eventInfo.Type)).Append(": ").Append(eventInfo.Count).AppendLine("</li>");
            }

            Html.AppendLine("</ol>")
                .AppendLine("</ul>")
                .AppendLine("<ul>")
                .AppendLine("<li>Deaths by Cause</li>")
                .AppendLine("<ul>");
            var deathCauses = from death in eventList.OfType<HfDied>()
                              group death by death.Cause
                into deathCause
                              select new { Cause = deathCause.Key, Count = deathCause.Count() };
            deathCauses = deathCauses.OrderByDescending(death => death.Count);
            foreach (var deathCause in deathCauses)
            {
                Html.Append("<li>").Append(deathCause.Cause.GetDescription()).Append(": ").Append(deathCause.Count).AppendLine("</li>");
            }

            Html.AppendLine("</ul>")
                .AppendLine("</ul>");
        }

        protected void PrintEventCollectionDetails(List<EventCollection> eventCollectionList)
        {
            Html.Append("<h1>Event Collections: ").Append(eventCollectionList.Count).AppendLine("</h1>");
            var collectionTypes = from collection in eventCollectionList
                                  group collection by collection.Type into collectionType
                                  select new { Type = collectionType.Key, Count = collectionType.Count() };
            collectionTypes = collectionTypes.OrderByDescending(collection => collection.Count);
            foreach (var collectionType in collectionTypes)
            {
                Html.Append("<h2>").Append(Formatting.InitCaps(collectionType.Type)).Append(": ").Append(collectionType.Count).AppendLine("</h2>")
                    .AppendLine("<ul>");
                var subCollections = from subCollection in eventCollectionList.Where(collection => collection.Type == collectionType.Type).SelectMany(collection => collection.Collections)
                                     group subCollection by subCollection.Type into subCollectionType
                                     select new { Type = subCollectionType.Key, Count = subCollectionType.Count() };
                subCollections = subCollections.OrderByDescending(collection => collection.Count);
                if (subCollections.Any())
                {
                    Html.AppendLine("<li>Sub Collections")
                        .AppendLine("<ul>");
                    foreach (var subCollection in subCollections)
                    {
                        Html.Append("<li>").Append(Formatting.InitCaps(subCollection.Type)).Append(": ").Append(subCollection.Count).AppendLine("</li>");
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
                    Html.Append("<li>").Append(AppHelpers.EventInfo[Array.IndexOf(AppHelpers.EventInfo, AppHelpers.EventInfo.Single(eventInfo => eventInfo[0] == eventType.Type))][1]).Append(": ").Append(eventType.Count).AppendLine("</li>");
                }

                Html.AppendLine("</ul>")
                    .AppendLine("</ul>");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _disposed &= _temporaryFiles.Count == 0;
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
        private readonly StringBuilder _html;
        private readonly bool _numbered;
        private int _count;
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

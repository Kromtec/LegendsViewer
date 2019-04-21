using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends;
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

        public static HtmlPrinter GetPrinter(object printObject, World world)
        {
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
                return new EraPrinter(printObject as Era);
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
            htmlPage.Append("<!DOCTYPE html><html><head>");
            htmlPage.Append("<title>" + GetTitle() + "</title>");
            htmlPage.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            htmlPage.AppendLine("<script type=\"text/javascript\" src=\"" + LocalFileProvider.LocalPrefix + "WebContent/scripts/jquery-3.1.1.min.js\"></script>");
            htmlPage.AppendLine("<script type=\"text/javascript\" src=\"" + LocalFileProvider.LocalPrefix + "WebContent/scripts/jquery.dataTables.min.js\"></script>");
            htmlPage.Append("<link rel=\"stylesheet\" href=\"" + LocalFileProvider.LocalPrefix + "WebContent/styles/bootstrap.min.css\">");
            htmlPage.Append("<link rel=\"stylesheet\" href=\"" + LocalFileProvider.LocalPrefix + "WebContent/styles/font-awesome.min.css\">");
            htmlPage.Append("<link rel=\"stylesheet\" href=\"" + LocalFileProvider.LocalPrefix + "WebContent/styles/legends.css\">");
            htmlPage.Append("<link rel=\"stylesheet\" href=\"" + LocalFileProvider.LocalPrefix + "WebContent/styles/jquery.dataTables.min.css\">");
            htmlPage.Append("</head>");
            htmlPage.Append("<body>");
            htmlPage.Append(Print());
            htmlPage.Append("</body>");
            htmlPage.Append("</html>");
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

        protected string ImageToHtml(string image)
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
            for (int i = 0; i < populations.Count; i++)
            {
                if (populations[i].IsMainRace)
                {
                    mainRacePops.Add(populations[i]);
                }
                else if (populations[i].IsAnimalPeople)
                {
                    animalPeoplePops.Add(populations[i]);
                }
                else if (populations[i].IsVisitors)
                {
                    visitorsPops.Add(populations[i]);
                }
                else if (populations[i].IsOutcasts)
                {
                    outcastsPops.Add(populations[i]);
                }
                else if (populations[i].IsPrisoners)
                {
                    prisonersPops.Add(populations[i]);
                }
                else if (populations[i].IsSlaves)
                {
                    slavesPops.Add(populations[i]);
                }
                else
                {
                    otherRacePops.Add(populations[i]);
                }
            }
            if (mainRacePops.Any())
            {
                Html.AppendLine("<b>Civilized Populations</b></br>");
                Html.AppendLine("<ul>");
                foreach (Population population in mainRacePops)
                {
                    Html.AppendLine("<li>" + population.Count + " " + population.Race + "</li>");
                }

                Html.AppendLine("</ul>");
            }
            if (animalPeoplePops.Any())
            {
                Html.AppendLine("<b>Animal People</b></br>");
                Html.AppendLine("<ul>");
                foreach (Population population in animalPeoplePops)
                {
                    Html.AppendLine("<li>" + population.Count + " " + population.Race + "</li>");
                }

                Html.AppendLine("</ul>");
            }
            if (visitorsPops.Any())
            {
                Html.AppendLine("<b>Visitors</b></br>");
                Html.AppendLine("<ul>");
                foreach (Population population in visitorsPops)
                {
                    Html.AppendLine("<li>" + population.Count + " " + population.Race + "</li>");
                }

                Html.AppendLine("</ul>");
            }
            if (outcastsPops.Any())
            {
                Html.AppendLine("<b>Outcasts</b></br>");
                Html.AppendLine("<ul>");
                foreach (Population population in outcastsPops)
                {
                    Html.AppendLine("<li>" + population.Count + " " + population.Race + "</li>");
                }

                Html.AppendLine("</ul>");
            }
            if (prisonersPops.Any())
            {
                Html.AppendLine("<b>Prisoners</b></br>");
                Html.AppendLine("<ul>");
                foreach (Population population in prisonersPops)
                {
                    Html.AppendLine("<li>" + population.Count + " " + population.Race + "</li>");
                }

                Html.AppendLine("</ul>");
            }
            if (slavesPops.Any())
            {
                Html.AppendLine("<b>Slaves</b></br>");
                Html.AppendLine("<ul>");
                foreach (Population population in slavesPops)
                {
                    Html.AppendLine("<li>" + population.Count + " " + population.Race + "</li>");
                }

                Html.AppendLine("</ul>");
            }
            if (otherRacePops.Any())
            {
                Html.AppendLine("<b>Other Populations</b></br>");
                Html.AppendLine("<ul>");
                foreach (Population population in otherRacePops)
                {
                    Html.AppendLine("<li>" + population.Count + " " + population.Race + "</li>");
                }

                Html.AppendLine("</ul>");
            }
            Html.AppendLine("</div>");
        }

        protected void PrintEventLog(List<WorldEvent> events, List<string> filters, DwarfObject dfo)
        {
            if (!events.Any())
            {
                return;
            }
            Html.AppendLine("<b>Event Log</b> " + MakeLink(Font("[Chart]", "Maroon"), LinkOption.LoadChart) + "<br/><br/>");
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
            Html.AppendLine("});");
            Html.AppendLine("</script>");
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
        LoadSiteMap
    }

    public enum ControlOption
    {
        Html,
        Map,
        Chart,
        Search,
        ReadMe
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

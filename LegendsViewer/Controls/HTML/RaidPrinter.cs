using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;

namespace LegendsViewer.Controls.HTML
{
    public class RaidPrinter : HtmlPrinter
    {
        Raid _raid;
        World _world;

        public RaidPrinter(Raid raid, World world)
        {
            _raid = raid;
            _world = world;
        }

        public override string GetTitle()
        {
            return _raid.Name;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.AppendLine("<h1>" + _raid.Name + "</h1><br />");

            PrintMaps();

            PrintEventLog(_world, _raid.GetSubEvents(), Raid.Filters, _raid);

            return Html.ToString();
        }

        private void PrintMaps()
        {
            if (_raid.Site?.Coordinates == null)
            {
                return;
            }

            Html.AppendLine("<div class=\"row\">");
            Html.AppendLine("<div class=\"col-md-12\">");
            var maps = MapPanel.CreateBitmaps(_world, _raid.Site);
            Html.AppendLine("<table>");
            Html.AppendLine("<tr>");
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("</tr></table></br>");
            Html.AppendLine("</div>");
            Html.AppendLine("</div>");
        }
    }
}

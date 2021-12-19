using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;

namespace LegendsViewer.Controls.HTML
{
    public class RaidPrinter : HtmlPrinter
    {
        private readonly Raid _raid;
        private readonly World _world;

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

            Html.Append("<h1>").Append(_raid.GetIcon()).Append(' ').Append(_raid.Name).AppendLine("</h1><br />");

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

            Html.AppendLine("<div class=\"row\">")
                .AppendLine("<div class=\"col-md-12\">");
            var maps = MapPanel.CreateBitmaps(_world, _raid.Site);
            Html.AppendLine("<table>")
                .AppendLine("<tr>")
                .Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                .AppendLine("</tr></table></br>")
                .AppendLine("</div>")
                .AppendLine("</div>");
        }
    }
}

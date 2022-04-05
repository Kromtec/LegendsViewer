using System.Collections.Generic;
using System.Drawing;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class RiverPrinter : HtmlPrinter
    {
        private readonly River _river;
        private readonly World _world;

        public RiverPrinter(River river, World world)
        {
            _river = river;
            _world = world;
        }

        public override string GetTitle()
        {
            return _river.Name;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.Append("<h1>").Append(_river.GetIcon()).Append(' ').Append(_river.Name).AppendLine(", River</h1><br />");

            if (_river.Coordinates.Count > 0)
            {
                List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _river);

                Html.AppendLine("<table>")
                    .AppendLine("<tr>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                    .AppendLine("</tr></table></br>");
            }

            PrintEventLog(_world, _river.Events, River.Filters, _river);

            return Html.ToString();
        }
    }
}

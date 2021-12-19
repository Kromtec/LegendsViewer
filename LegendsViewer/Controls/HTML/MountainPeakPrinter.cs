using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class MountainPeakPrinter : HtmlPrinter
    {
        private readonly MountainPeak _mountainPeak;
        private readonly World _world;

        public MountainPeakPrinter(MountainPeak mountainPeak, World world)
        {
            _mountainPeak = mountainPeak;
            _world = world;
        }

        public override string GetTitle()
        {
            return _mountainPeak.Name;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.Append("<h1>").Append(_mountainPeak.GetIcon()).Append(' ').Append(_mountainPeak.Name).Append(", ").Append(_mountainPeak.TypeAsString).AppendLine("</h1><br />");

            if (_mountainPeak.Coordinates.Count > 0)
            {
                List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _mountainPeak);

                Html.AppendLine("<table>")
                    .AppendLine("<tr>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                    .AppendLine("</tr></table></br>");
            }

            if (_mountainPeak.Region != null)
            {
                Html.AppendLine("<b>Geography</b><br/>")
                    .AppendLine("<ul>");
                if (_mountainPeak.Region != null)
                {
                    Html.Append("<li>Region: ").Append(_mountainPeak.Region.ToLink()).Append(", ").Append(_mountainPeak.Region.Type.GetDescription()).AppendLine("</li>");
                }
                Html.AppendLine("</ul>");
            }

            if (_mountainPeak.Height > 0)
            {
                Html.Append(Bold("Height of " + _mountainPeak.ToLink(true, _mountainPeak))).AppendLine(LineBreak)
                    .AppendLine("<ul>")
                    .Append("<li>").Append(_mountainPeak.Height).Append(" tiles ~ ").Append(3 * _mountainPeak.Height).AppendLine(" m</li>")
                    .AppendLine("</ul>");
            }

            PrintEventLog(_world, _mountainPeak.Events, MountainPeak.Filters, _mountainPeak);

            return Html.ToString();
        }
    }
}

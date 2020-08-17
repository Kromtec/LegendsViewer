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
        MountainPeak _mountainPeak;
        World _world;

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

            Html.AppendLine("<h1>" + _mountainPeak.GetIcon() + " " + _mountainPeak.Name + ", " + _mountainPeak.TypeAsString + "</h1><br />");

            if (_mountainPeak.Coordinates.Any())
            {
                List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _mountainPeak);

                Html.AppendLine("<table>");
                Html.AppendLine("<tr>");
                Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap) + "</td>");
                Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap) + "</td>");
                Html.AppendLine("</tr></table></br>");
            }

            if (_mountainPeak.Region != null)
            {
                Html.AppendLine("<b>Geography</b><br/>");
                Html.AppendLine("<ul>");
                if (_mountainPeak.Region != null)
                {
                    Html.AppendLine("<li>Region: " + _mountainPeak.Region.ToLink() + ", " + _mountainPeak.Region.Type.GetDescription() + "</li>");
                }
                Html.AppendLine("</ul>");
            }

            if (_mountainPeak.Height > 0)
            {
                Html.AppendLine(Bold("Height of " + _mountainPeak.ToLink(true, _mountainPeak)) + LineBreak);
                Html.AppendLine("<ul>");
                Html.AppendLine("<li>" + _mountainPeak.Height + " tiles ~ " + 3 * _mountainPeak.Height + " m</li>");
                Html.AppendLine("</ul>");
            }

            PrintEventLog(_world, _mountainPeak.Events, MountainPeak.Filters, _mountainPeak);

            return Html.ToString();
        }
    }
}

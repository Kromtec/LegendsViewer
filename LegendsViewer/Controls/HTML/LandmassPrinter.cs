using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class LandmassPrinter : HtmlPrinter
    {
        private readonly Landmass _landmass;
        private readonly World _world;

        public LandmassPrinter(Landmass landmass, World world)
        {
            _landmass = landmass;
            _world = world;
        }

        public override string GetTitle()
        {
            return _landmass.Name;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.Append("<h1>").Append(_landmass.GetIcon()).Append(' ').Append(_landmass.Name).AppendLine(", Landmass</h1><br />");

            if (_landmass.Coordinates.Count > 0)
            {
                List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _landmass);

                Html.AppendLine("<table>")
                    .AppendLine("<tr>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                    .AppendLine("</tr></table></br>");
            }

            PrintEventLog(_world, _landmass.Events, Landmass.Filters, _landmass);

            return Html.ToString();
        }
    }
}

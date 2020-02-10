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
        Landmass _landmass;
        World _world;

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

            Html.AppendLine("<h1>" + _landmass.GetIcon() + " " + _landmass.Name + ", Landmass</h1><br />");

            if (_landmass.Coordinates.Any())
            {
                List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _landmass);

                Html.AppendLine("<table>");
                Html.AppendLine("<tr>");
                Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap) + "</td>");
                Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap) + "</td>");
                Html.AppendLine("</tr></table></br>");
            }

            PrintEventLog(_world, _landmass.Events, Landmass.Filters, _landmass);

            return Html.ToString();
        }
    }
}

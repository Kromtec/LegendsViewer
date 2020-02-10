using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class WorldConstructionPrinter : HtmlPrinter
    {
        WorldConstruction _worldConstruction;
        World _world;

        public WorldConstructionPrinter(WorldConstruction worldConstruction, World world)
        {
            _worldConstruction = worldConstruction;
            _world = world;
        }

        public override string Print()
        {
            Html = new StringBuilder();
            Html.AppendLine("<h1>" + _worldConstruction.GetIcon() + " " + _worldConstruction.Name + ", " + _worldConstruction.Type + "</h1><br />");

            if (_worldConstruction.Coordinates.Any())
            {
                List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _worldConstruction);

                Html.AppendLine("<table>");
                Html.AppendLine("<tr>");
                Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap) + "</td>");
                Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap) + "</td>");
                Html.AppendLine("</tr></table></br>");
            }

            Html.AppendLine("<b>Connects</b><br />");
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>" + (_worldConstruction.Site1 != null ? _worldConstruction.Site1.ToLink() : "UNKNOWN SITE") + "</li>");
            Html.AppendLine("<li>" + (_worldConstruction.Site2 != null ? _worldConstruction.Site2.ToLink() : "UNKNOWN SITE") + "</li>");
            Html.AppendLine("</ul>");
            Html.AppendLine("</br>");

            if (_worldConstruction.MasterConstruction != null)
            {
                Html.AppendLine("<b>Part of</b><br />");
                Html.AppendLine("<ul>");
                Html.AppendLine("<li>" + _worldConstruction.MasterConstruction.ToLink() + ", " + _worldConstruction.MasterConstruction.Type + "</li>");
                Html.AppendLine("</ul>");
                Html.AppendLine("</br>");
            }

            if (_worldConstruction.Sections.Any())
            {
                Html.AppendLine("<b>Sections</b><br />");
                Html.AppendLine("<ul>");
                foreach (WorldConstruction segment in _worldConstruction.Sections)
                {
                    Html.AppendLine("<li>" + segment.ToLink() + ", " + segment.Type + "</li>");
                }
                Html.AppendLine("</ul>");
                Html.AppendLine("</br>");
            }

            PrintEventLog(_world, _worldConstruction.Events, WorldConstruction.Filters, _worldConstruction);
            return Html.ToString();
        }

        public override string GetTitle()
        {
            return _worldConstruction.Name;
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class WorldConstructionPrinter : HtmlPrinter
    {
        private readonly WorldConstruction _worldConstruction;
        private readonly World _world;

        public WorldConstructionPrinter(WorldConstruction worldConstruction, World world)
        {
            _worldConstruction = worldConstruction;
            _world = world;
        }

        public override string Print()
        {
            Html = new StringBuilder();
            Html.Append("<h1>").Append(_worldConstruction.GetIcon()).Append(' ').Append(_worldConstruction.Name).Append(", ").Append(_worldConstruction.Type).AppendLine("</h1><br />");

            if (_worldConstruction.Coordinates.Count > 0)
            {
                List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _worldConstruction);

                Html.AppendLine("<table>")
                    .AppendLine("<tr>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                    .AppendLine("</tr></table></br>");
            }

            Html.AppendLine("<b>Connects</b><br />")
                .AppendLine("<ul>")
                .Append("<li>").Append(_worldConstruction.Site1 != null ? _worldConstruction.Site1.ToLink() : "UNKNOWN SITE").AppendLine("</li>")
                .Append("<li>").Append(_worldConstruction.Site2 != null ? _worldConstruction.Site2.ToLink() : "UNKNOWN SITE").AppendLine("</li>")
                .AppendLine("</ul>")
                .AppendLine("</br>");

            if (_worldConstruction.MasterConstruction != null)
            {
                Html.AppendLine("<b>Part of</b><br />")
                    .AppendLine("<ul>")
                    .Append("<li>").Append(_worldConstruction.MasterConstruction.ToLink()).Append(", ").Append(_worldConstruction.MasterConstruction.Type).AppendLine("</li>")
                    .AppendLine("</ul>")
                    .AppendLine("</br>");
            }

            if (_worldConstruction.Sections.Count > 0)
            {
                Html.AppendLine("<b>Sections</b><br />")
                    .AppendLine("<ul>");
                foreach (WorldConstruction segment in _worldConstruction.Sections)
                {
                    Html.Append("<li>").Append(segment.ToLink()).Append(", ").Append(segment.Type).AppendLine("</li>");
                }
                Html.AppendLine("</ul>")
                    .AppendLine("</br>");
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

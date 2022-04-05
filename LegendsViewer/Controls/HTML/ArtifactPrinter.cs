using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class ArtifactPrinter : HtmlPrinter
    {
        private readonly Artifact _artifact;
        private readonly World _world;

        public ArtifactPrinter(Artifact artifact, World world)
        {
            _artifact = artifact;
            _world = world;
        }

        public override string Print()
        {
            Html = new StringBuilder();
            Html.Append("<h1>").Append(_artifact.GetIcon()).Append(' ').AppendLine(_artifact.Name);
            if (!string.IsNullOrWhiteSpace(_artifact.Item) && _artifact.Name != _artifact.Item)
            {
                Html.Append(" \"").Append(_artifact.Item).AppendLine("\"");
            }
            Html.AppendLine("</h1>");
            if (!string.IsNullOrWhiteSpace(_artifact.Type) && _artifact.Type != "Unknown")
            {
                Html.Append("<b>").Append(_artifact.Name).Append(" was a legendary ").Append(_artifact.Material).AppendLine(" ")
                    .Append(!string.IsNullOrWhiteSpace(_artifact.SubType) ? _artifact.SubType : _artifact.Type.ToLower()).AppendLine(".</b><br />");
            }
            else
            {
                Html.Append("<b>").Append(_artifact.Name).AppendLine(" was a legendary item.</b><br />");
            }
            if (!string.IsNullOrWhiteSpace(_artifact.Description))
            {
                Html.Append("<i>\"").Append(_artifact.Description).AppendLine("\"</i><br />");
            }
            Html.AppendLine("<br />");

            PrintMaps();

            if (_artifact.Site != null || _artifact.Region != null)
            {
                Html.AppendLine("<b>Current Location:</b><br/>")
                    .AppendLine("<ul>");
                if (_artifact.Site != null)
                {
                    Html.Append("<li>").AppendLine(_artifact.Site.ToLink());
                    if (_artifact.Structure != null)
                    {
                        Html.Append(" (").Append(_artifact.Structure.ToLink()).AppendLine(")");
                    }
                    Html.AppendLine("</li>");
                }
                else if (_artifact.Region != null)
                {
                    Html.Append("<li>").Append(_artifact.Region.ToLink()).AppendLine("</li>");
                }

                Html.AppendLine("</ul>");
            }
            if (_artifact.Holder != null)
            {
                Html.AppendLine("<b>Current Holder:</b><br/>")
                    .AppendLine("<ul>")
                    .Append("<li>").Append(_artifact.Holder.ToLink()).AppendLine("</li>")
                    .AppendLine("</ul>");
            }
            if (_artifact.WrittenContents != null)
            {
                Html.AppendLine("<b>Written Content:</b><br/>")
                    .AppendLine("<ul>");
                if (_artifact.PageCount > 0)
                {
                    Html.Append("<li>Pages: ").Append(_artifact.PageCount).AppendLine("</li>");
                }
                foreach (var writtenContent in _artifact.WrittenContents)
                {
                    Html.Append("<li>").Append(writtenContent.ToLink()).AppendLine("</li>");
                }
                Html.AppendLine("</ul>");
            }

            PrintEventLog(_world, _artifact.Events, Artifact.Filters, _artifact);
            return Html.ToString();
        }

        private void PrintMaps()
        {
            if (_artifact.Coordinates == null)
            {
                return;
            }

            Html.AppendLine("<div class=\"row\">")
                .AppendLine("<div class=\"col-md-12\">");
            var maps = MapPanel.CreateBitmaps(_world, _artifact);
            Html.AppendLine("<table>")
                .AppendLine("<tr>")
                .Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                .AppendLine("</tr></table></br>")
                .AppendLine("</div>")
                .AppendLine("</div>");
        }

        public override string GetTitle()
        {
            return _artifact.Name;
        }
    }
}

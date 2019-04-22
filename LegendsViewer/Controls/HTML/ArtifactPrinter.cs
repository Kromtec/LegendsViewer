using System.Collections.Generic;
using System.Drawing;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;

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
            Html.AppendLine("<h1>" + _artifact.Name);
            if (!string.IsNullOrWhiteSpace(_artifact.Item) && _artifact.Name != _artifact.Item)
            {
                Html.AppendLine(" \"" + _artifact.Item + "\"");
            }
            Html.AppendLine("</h1>");
            if (!string.IsNullOrWhiteSpace(_artifact.Type) && _artifact.Type != "Unknown")
            {
                Html.AppendLine("<b>" + _artifact.Name + " was a legendary " + _artifact.Material + " ");
                Html.AppendLine((!string.IsNullOrWhiteSpace(_artifact.SubType) ? _artifact.SubType : _artifact.Type.ToLower()) + ".</b><br />");
            }
            else
            {
                Html.AppendLine("<b>" + _artifact.Name + " was a legendary item.</b><br />");
            }
            if (!string.IsNullOrWhiteSpace(_artifact.Description))
            {
                Html.AppendLine("<i>\"" + _artifact.Description + "\"</i><br />");
            }
            Html.AppendLine("<br />");

            PrintMaps();

            if (_artifact.Site != null || _artifact.Region != null)
            {
                Html.AppendLine("<b>Current Location:</b><br/>");
                Html.AppendLine("<ul>");
                if (_artifact.Site != null)
                {
                Html.AppendLine("<li>" + _artifact.Site.ToLink());
                if (_artifact.Structure != null)
                {
                    Html.AppendLine(" (" + _artifact.Structure.ToLink() + ")");
                }
                Html.AppendLine("</li>");
                    
                }
                else if (_artifact.Region != null)
                {
                    Html.AppendLine("<li>" + _artifact.Region.ToLink() + "</li>");
                }

                Html.AppendLine("</ul>");
            }
            if (_artifact.Holder != null)
            {
                Html.AppendLine("<b>Current Holder:</b><br/>");
                Html.AppendLine("<ul>");
                Html.AppendLine("<li>" + _artifact.Holder.ToLink() + "</li>");
                Html.AppendLine("</ul>");
            }
            if (_artifact.WrittenContents != null)
            {
                Html.AppendLine("<b>Written Content:</b><br/>");
                Html.AppendLine("<ul>");
                if (_artifact.PageCount > 0)
                {
                    Html.AppendLine("<li>Pages: " + _artifact.PageCount + "</li>");
                }
                foreach (var writtenContent in _artifact.WrittenContents)
                {
                    Html.AppendLine("<li>" + writtenContent.ToLink() + "</li>");
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

            Html.AppendLine("<div class=\"row\">");
            Html.AppendLine("<div class=\"col-md-12\">");
            var maps = MapPanel.CreateBitmaps(_world, _artifact);
            Html.AppendLine("<table>");
            Html.AppendLine("<tr>");
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("</tr></table></br>");
            Html.AppendLine("</div>");
            Html.AppendLine("</div>");
        }

        public override string GetTitle()
        {
            return _artifact.Name;
        }
    }
}

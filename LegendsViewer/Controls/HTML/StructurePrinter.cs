using System.Linq;
using System.Text;
using LegendsViewer.Legends;
using LegendsViewer.Legends.Enums;

namespace LegendsViewer.Controls.HTML
{
    public class StructurePrinter : HtmlPrinter
    {
        Structure _structure;
        World _world;

        public StructurePrinter(Structure structure, World world)
        {
            _structure = structure;
            _world = world;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.AppendLine("<h1>" + _structure.Name + "</h1>");
            Html.AppendLine("<b>");
            Html.AppendLine(_structure.TypeAsString);
            Html.AppendLine(" in " + _structure.Site.ToLink() + "</b><br/><br/>");

            if (_structure.Deity != null)
            {
                Html.AppendLine("<b>Deity:</b><br/>");
                Html.AppendLine("<ul>");
                Html.AppendLine("<li>" + _structure.Deity.ToLink() + "</li>");
                Html.AppendLine("</ul>");
            }
            if (_structure.Religion != null)
            {
                Html.AppendLine("<b>Religion:</b><br/>");
                Html.AppendLine("<ul>");
                Html.AppendLine("<li>" + _structure.Religion.ToLink() + "</li>");
                Html.AppendLine("</ul>");
            }
            if (_structure.Entity != null)
            {
                Html.AppendLine("<b>Entity:</b><br/>");
                Html.AppendLine("<ul>");
                Html.AppendLine("<li>" + _structure.Entity.ToLink() + "</li>");
                Html.AppendLine("</ul>");
            }
            if (_structure.Inhabitants.Any())
            {
                Html.AppendLine("<b>Inhabitants:</b><br/>");
                Html.AppendLine("<ul>");
                foreach (var inhabitant in _structure.Inhabitants)
                {
                    Html.AppendLine("<li>" + inhabitant.ToLink() + "</li>");
                }
                Html.AppendLine("</ul>");
            }
            if (_structure.CopiedArtifacts.Any())
            {
                Html.AppendLine("<b>Copied Artifacts:</b><br/>");
                Html.AppendLine("<ul>");
                foreach (var artifact in _structure.CopiedArtifacts)
                {
                    Html.AppendLine("<li>" + artifact.ToLink() + "</li>");
                }
                Html.AppendLine("</ul>");
            }

            PrintEventLog(_world, _structure.Events, Structure.Filters, _structure);
            return Html.ToString();
        }

        public override string GetTitle()
        {
            return _structure.Name;
        }
    }
}

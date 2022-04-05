using System.Text;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class StructurePrinter : HtmlPrinter
    {
        private readonly Structure _structure;
        private readonly World _world;

        public StructurePrinter(Structure structure, World world)
        {
            _structure = structure;
            _world = world;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.Append("<h1>").Append(_structure.GetIcon()).Append(' ').Append(_structure.Name).AppendLine("</h1>")
                .AppendLine("<b>")
                .AppendLine(_structure.TypeAsString)
                .Append(" in ").Append(_structure.Site.ToLink()).AppendLine("</b><br/><br/>");

            if (_structure.Deity != null)
            {
                Html.AppendLine("<b>Deity:</b><br/>")
                    .AppendLine("<ul>")
                    .Append("<li>").Append(_structure.Deity.ToLink()).AppendLine("</li>")
                    .AppendLine("</ul>");
            }
            if (_structure.Religion != null)
            {
                Html.AppendLine("<b>Religion:</b><br/>")
                    .AppendLine("<ul>")
                    .Append("<li>").Append(_structure.Religion.ToLink()).AppendLine("</li>")
                    .AppendLine("</ul>");
            }
            if (_structure.Entity != null)
            {
                Html.AppendLine("<b>Entity:</b><br/>")
                    .AppendLine("<ul>")
                    .Append("<li>").Append(_structure.Entity.ToLink()).AppendLine("</li>")
                    .AppendLine("</ul>");
            }
            if (_structure.Owner != null)
            {
                Html.AppendLine("<b>Owner:</b><br/>")
                    .AppendLine("<ul>")
                    .Append("<li>").Append(_structure.Owner.ToLink()).AppendLine("</li>")
                    .AppendLine("</ul>");
            }
            if (_structure.Inhabitants.Count > 0)
            {
                Html.AppendLine("<b>Inhabitants:</b><br/>")
                    .AppendLine("<ul>");
                foreach (var inhabitant in _structure.Inhabitants)
                {
                    Html.Append("<li>").Append(inhabitant.ToLink()).AppendLine("</li>");
                }
                Html.AppendLine("</ul>");
            }
            if (_structure.CopiedArtifacts.Count > 0)
            {
                Html.AppendLine("<b>Copied Artifacts:</b><br/>")
                    .AppendLine("<ul>");
                foreach (var artifact in _structure.CopiedArtifacts)
                {
                    Html.Append("<li>").Append(artifact.ToLink()).AppendLine("</li>");
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

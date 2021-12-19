using System.Collections.Generic;
using System.Drawing;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;

namespace LegendsViewer.Controls.HTML
{
    internal class SiteConqueredPrinter : HtmlPrinter
    {
        private readonly SiteConquered _conquering;
        private readonly World _world;

        public SiteConqueredPrinter(SiteConquered conquering, World world)
        {
            _conquering = conquering;
            _world = world;
        }

        public override string GetTitle()
        {
            return "The " + Formatting.AddOrdinal(_conquering.Ordinal) + " " + _conquering.ConquerType + " of " + _conquering.Site.ToLink(false);
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.Append("<h1>").Append(_conquering.GetIcon()).Append(' ').Append(GetTitle()).AppendLine("</h1></br>")
                .Append(_conquering.GetYearTime()).Append(", the ").Append(Formatting.AddOrdinal(_conquering.Ordinal)).Append(' ').Append(_conquering.ConquerType).Append(" of ").Append(_conquering.Site.ToLink()).Append(" occurred as a result of ").Append(_conquering.Battle.ToLink()).Append((_conquering.ParentCollection == null ? "" : " in " + _conquering.ParentCollection.ToLink() + " waged by " + (_conquering.ParentCollection as War).Attacker.PrintEntity() + " on " + (_conquering.ParentCollection as War).Defender.PrintEntity())).AppendLine(".</br></br>");

            List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _conquering);

            Html.AppendLine("<table>")
                .AppendLine("<tr>")
                .Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                .AppendLine("</tr></table></br>")
                .Append("<b>").Append(_conquering.Attacker.PrintEntity()).AppendLine(" (Attacker)</b></br>")
                .Append("<b>").Append(_conquering.Defender.PrintEntity()).AppendLine(" (Defender)</b></br></br>");

            PrintEventLog(_world, _conquering.GetSubEvents(), SiteConquered.Filters, _conquering);

            return Html.ToString();
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;

namespace LegendsViewer.Controls.HTML
{
    class SiteConqueredPrinter : HtmlPrinter
    {
        SiteConquered _conquering;
        World _world;

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

            Html.AppendLine("<h1>" + _conquering.GetIcon() + " " + GetTitle() + "</h1></br>");

            Html.AppendLine($"{_conquering.GetYearTime()}, the {Formatting.AddOrdinal(_conquering.Ordinal)} {_conquering.ConquerType} of {_conquering.Site.ToLink()} occurred as a result of {_conquering.Battle.ToLink()}{(_conquering.ParentCollection == null ? "" : " in " + _conquering.ParentCollection.ToLink() + " waged by " + (_conquering.ParentCollection as War).Attacker.PrintEntity() + " on " + (_conquering.ParentCollection as War).Defender.PrintEntity() )}.</br></br>");

            List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _conquering);

            Html.AppendLine("<table>");
            Html.AppendLine("<tr>");
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("</tr></table></br>");

            Html.AppendLine("<b>" + _conquering.Attacker.PrintEntity() + " (Attacker)</b></br>");
            Html.AppendLine("<b>" + _conquering.Defender.PrintEntity() + " (Defender)</b></br></br>");

            PrintEventLog(_world, _conquering.GetSubEvents(), SiteConquered.Filters, _conquering);

            return Html.ToString();
        }
    }
}

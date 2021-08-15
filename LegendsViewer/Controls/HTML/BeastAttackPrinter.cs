using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;

namespace LegendsViewer.Controls.HTML
{
    public class BeastAttackPrinter : HtmlPrinter
    {
        private readonly BeastAttack _attack;
        private readonly World _world;

        public BeastAttackPrinter(BeastAttack attack, World world)
        {
            _attack = attack;
            _world = world;
        }

        public override string GetTitle()
        {
            string beastName;
            if (_attack.Beast != null)
            {
                var spaceIndex = _attack.Beast.Name.IndexOf(" ", StringComparison.Ordinal);
                beastName = spaceIndex > 0 ? _attack.Beast.Name.Substring(0, spaceIndex) : _attack.Beast.Name;
            }
            else
            {
                beastName = "an unknown creature";
            }

            return "Rampage of " + beastName;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.AppendLine("<h1>" + _attack.GetIcon() + " " + GetTitle() + "</h1></br>");

            string beastName = "an unknown creature";
            if (_attack.Beast != null)
            {
                beastName = _attack.Beast.ToLink();
            }

            Html.AppendLine("The " + Formatting.AddOrdinal(_attack.Ordinal) + " Rampage of " + beastName + " in " + _attack.Site.ToLink() + ".</br></br>");

            List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _attack.Site);
            Html.AppendLine("<table>");
            Html.AppendLine("<tr>");
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("</tr></table></br>");

            PrintEventLog(_world, _attack.GetSubEvents(), BeastAttack.Filters, _attack);

            return Html.ToString();
        }
    }
}

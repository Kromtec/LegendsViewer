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

            Html.Append("<h1>").Append(_attack.GetIcon()).Append(' ').Append(GetTitle()).AppendLine("</h1></br>");

            string beastName = "an unknown creature";
            if (_attack.Beast != null)
            {
                beastName = _attack.Beast.ToLink();
            }

            Html.Append("The ").Append(Formatting.AddOrdinal(_attack.Ordinal)).Append(" Rampage of ").Append(beastName).Append(" in ").Append(_attack.Site.ToLink()).AppendLine(".</br></br>");

            List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _attack.Site);
            Html.AppendLine("<table>")
                .AppendLine("<tr>")
                .Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                .AppendLine("</tr></table></br>");

            PrintEventLog(_world, _attack.GetSubEvents(), BeastAttack.Filters, _attack);

            return Html.ToString();
        }
    }
}

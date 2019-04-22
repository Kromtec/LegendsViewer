using System.Collections.Generic;
using System.Drawing;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;

namespace LegendsViewer.Controls.HTML
{
    class BeastAttackPrinter : HtmlPrinter
    {
        BeastAttack _attack;
        World _world;

        public BeastAttackPrinter(BeastAttack attack, World world)
        {
            _attack = attack;
            _world = world;
        }

        public override string GetTitle()
        {
            string beast;
            if (_attack.Beast != null)
            {
                if (_attack.Beast.Name.IndexOf(" ") > 0)
                {
                    beast = _attack.Beast.Name.Substring(0, _attack.Beast.Name.IndexOf(" "));
                }
                else
                {
                    beast = _attack.Beast.Name;
                }
            }
            else
            {
                beast = "Unknown";
            }

            return "Rampage of " + beast;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.AppendLine("<h1>" + GetTitle() + "</h1></br>");

            string beast = "UNKNOWN BEAST";
            if (_attack.Beast != null)
            {
                beast = _attack.Beast.ToLink();
            }

            Html.AppendLine("The " + _attack.GetOrdinal(_attack.Ordinal) + " Rampage of " + beast + " in " + _attack.Site.ToLink() + ".</br></br>");

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

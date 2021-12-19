using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;

namespace LegendsViewer.Controls.HTML
{
    internal class WarPrinter : HtmlPrinter
    {
        private readonly War _war;
        private readonly World _world;

        public WarPrinter(War war, World world)
        {
            _war = war;
            _world = world;
        }

        public override string GetTitle()
        {
            return _war.Name;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.Append("<h1>").Append(_war.GetIcon()).Append(' ').Append(GetTitle()).AppendLine("</h1></br>")
                .Append("Started ").Append(_war.GetYearTime().ToLower()).AppendLine(" and ");
            if (_war.EndYear == -1)
            {
                Html.AppendLine("is still ongoing.");
            }
            else
            {
                Html.Append("ended ").Append(_war.GetYearTime(false)).AppendLine(". ");
            }

            Html.Append(_war.Name).Append(" was waged by ").Append(_war.Attacker.PrintEntity()).Append(" on ").Append(_war.Defender.PrintEntity()).AppendLine(".<br/>")
                .AppendLine("</br></br>");

            List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _war);

            Html.AppendLine("<table>")
                .AppendLine("<tr>")
                .Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                .AppendLine("</tr></table></br>");

            _war.Collections.OfType<Battle>().Sum(battle => battle.Collection.OfType<HfDied>().Count(death => battle.NotableAttackers.Contains(death.HistoricalFigure)));
            Html.Append("<b>").Append(_war.Attacker.PrintEntity()).AppendLine(" (Attacker)</b>")
                .AppendLine("<ul>")
                .Append("<li>Kills: ").Append((_war.Collections.OfType<Battle>().Where(battle => battle.Attacker == _war.Attacker).Sum(battle => battle.DefenderDeathCount) + _war.Collections.OfType<Battle>().Where(battle => battle.Defender == _war.Attacker).Sum(battle => battle.AttackerDeathCount))).AppendLine("</br>")
                .Append("<li>Battle Victories: ").Append(_war.AttackerVictories.OfType<Battle>().Count()).AppendLine()
                .Append("<li>Conquerings: ").Append(_war.AttackerVictories.OfType<SiteConquered>().Count()).AppendLine()
                .Append(" (").Append(_war.AttackerVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType == SiteConqueredType.Pillaging)).AppendLine(" Pillagings, ")
                .Append(_war.AttackerVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType == SiteConqueredType.Destruction)).AppendLine(" Destructions, ")
                .Append(_war.AttackerVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType == SiteConqueredType.Conquest)).AppendLine(" Conquests)")
                .AppendLine("</ul>")
                .Append("<b>").Append(_war.Defender.PrintEntity()).AppendLine(" (Defender)</b>")
                .AppendLine("<ul>")
                .Append("<li>Kills: ").Append((_war.Collections.OfType<Battle>().Where(battle => battle.Attacker == _war.Defender).Sum(battle => battle.DefenderDeathCount) + _war.Collections.OfType<Battle>().Where(battle => battle.Defender == _war.Defender).Sum(battle => battle.AttackerDeathCount))).AppendLine("</br>")
                .Append("<li>Battle Victories: ").Append(_war.DefenderVictories.OfType<Battle>().Count()).AppendLine()
                .Append("<li>Conquerings: ").Append(_war.DefenderVictories.OfType<SiteConquered>().Count()).AppendLine()
                .Append(" (").Append(_war.DefenderVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType == SiteConqueredType.Pillaging)).AppendLine(" Pillagings, ")
                .Append(_war.DefenderVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType == SiteConqueredType.Destruction)).AppendLine(" Destructions, ")
                .Append(_war.DefenderVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType == SiteConqueredType.Conquest)).AppendLine(" Conquests)")
                .AppendLine("</ul>");

            if (_war.Collections.Count(battle => !_world.FilterBattles || battle.Notable) > 0)
            {
                int warfareCount = 1;
                Html.AppendLine("<b>Warfare</b>");
                if (_world.FilterBattles)
                {
                    Html.Append(" (Notable)");
                }

                Html.Append("</br>")
                    .AppendLine("<table>")
                    .AppendLine("<tr>")
                    .AppendLine("<th align=right>#</th>")
                    .AppendLine("<th align=right>Year</th>")
                    .AppendLine("<th>Battle</th>")
                    .AppendLine("<th>Victor</th>")
                    .Append("<th align=right>").Append(Base64ToHtml(_war.Attacker.SmallIdenticonString)).AppendLine("</th>")
                    .AppendLine("<th>/</th>")
                    .Append("<th align=left>").Append(Base64ToHtml(_war.Defender.SmallIdenticonString)).AppendLine("</th>")
                    .AppendLine("</tr>");
                foreach (EventCollection warfare in _war.Collections.Where(battle => !_world.FilterBattles || battle.Notable))
                {
                    Html.AppendLine("<tr>")
                        .Append("<td align=right>").Append(warfareCount).AppendLine(".</td>")
                        .Append("<td align=right>").Append(warfare.StartYear).AppendLine("</td>");
                    string warfareString = warfare.ToLink();
                    if (warfareString.Contains(" as a result of"))
                    {
                        warfareString = warfareString.Insert(warfareString.IndexOf(" as a result of"), "</br>");
                    }

                    Html.Append("<td>").Append(warfareString).Append("</td>");
                    if (warfare.GetType() == typeof(Battle))
                    {
                        Battle battle = warfare as Battle;
                        Html.Append("<td>").Append((battle.Victor == _war.Attacker ? battle.Attacker.PrintEntity() : battle.Defender.PrintEntity())).AppendLine("</td>")
                            .Append("<td align=right>").Append((battle.Attacker == _war.Attacker ? battle.DefenderDeathCount : battle.AttackerDeathCount)).AppendLine("</td>")
                            .AppendLine("<td>/</td>")
                            .Append("<td align=left>").Append((battle.Defender == _war.Attacker ? battle.DefenderDeathCount : battle.AttackerDeathCount)).AppendLine("</td>");
                    }
                    else if (warfare.GetType() == typeof(SiteConquered))
                    {
                        Html.Append("<td align=right>").Append((warfare as SiteConquered).Attacker.PrintEntity()).AppendLine("</td>");
                    }

                    Html.AppendLine("</tr>");
                    warfareCount++;
                }
                Html.AppendLine("</table></br>");
            }

            if (_world.FilterBattles && _war.Collections.Count(battle => !battle.Notable) > 0)
            {
                Html.AppendLine("<b>Warfare</b> (Unnotable)</br>")
                    .AppendLine("<ul>")
                    .Append("<li>Battles: ").Append(_war.Collections.OfType<Battle>().Count(battle => !battle.Notable)).AppendLine()
                    .Append("<li>Pillagings: ").Append(_war.Collections.OfType<SiteConquered>().Count(conquering => conquering.ConquerType == SiteConqueredType.Pillaging)).AppendLine()
                    .AppendLine("</ul>");
            }

            PrintEventLog(_world, _war.GetSubEvents(), War.Filters, _war);

            return Html.ToString();
        }
    }
}

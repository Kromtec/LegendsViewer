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
    class WarPrinter : HtmlPrinter
    {
        War _war;
        World _world;

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

            Html.AppendLine("<h1>" + _war.GetIcon() + " " + GetTitle() + "</h1></br>");
            Html.AppendLine("Started " + _war.GetYearTime().ToLower() + "and ");
            if (_war.EndYear == -1)
            {
                Html.AppendLine("is still ongoing.");
            }
            else
            {
                Html.AppendLine("ended " + _war.GetYearTime(false).ToLower().Substring(0, _war.GetYearTime(false).Length - 1) + ". ");
            }

            Html.AppendLine(_war.Name + " was waged by " + _war.Attacker.PrintEntity() + " on " + _war.Defender.PrintEntity() + ".<br/>");
            Html.AppendLine("</br></br>");

            List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _war);

            Html.AppendLine("<table>");
            Html.AppendLine("<tr>");
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("</tr></table></br>");

            _war.Collections.OfType<Battle>().Sum(battle => battle.Collection.OfType<HfDied>().Where(death => battle.NotableAttackers.Contains(death.HistoricalFigure)).Count());
            Html.AppendLine("<b>" + _war.Attacker.PrintEntity() + " (Attacker)</b>");
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Kills: " + (_war.Collections.OfType<Battle>().Where(battle => battle.Attacker == _war.Attacker).Sum(battle => battle.DefenderDeathCount) + _war.Collections.OfType<Battle>().Where(battle => battle.Defender == _war.Attacker).Sum(battle => battle.AttackerDeathCount)) + "</br>");
            Html.AppendLine("<li>Battle Victories: " + _war.AttackerVictories.OfType<Battle>().Count());
            Html.AppendLine("<li>Conquerings: " + _war.AttackerVictories.OfType<SiteConquered>().Count());
            Html.AppendLine(" (" + _war.AttackerVictories.OfType<SiteConquered>().Where(conquering => conquering.ConquerType == SiteConqueredType.Pillaging).Count() + " Pillagings, ");
            Html.AppendLine(_war.AttackerVictories.OfType<SiteConquered>().Where(conquering => conquering.ConquerType == SiteConqueredType.Destruction).Count() + " Destructions, ");
            Html.AppendLine(_war.AttackerVictories.OfType<SiteConquered>().Where(conquering => conquering.ConquerType == SiteConqueredType.Conquest).Count() + " Conquests)");
            Html.AppendLine("</ul>");

            Html.AppendLine("<b>" + _war.Defender.PrintEntity() + " (Defender)</b>");
            Html.AppendLine("<ul>");
            Html.AppendLine("<li>Kills: " + (_war.Collections.OfType<Battle>().Where(battle => battle.Attacker == _war.Defender).Sum(battle => battle.DefenderDeathCount) + _war.Collections.OfType<Battle>().Where(battle => battle.Defender == _war.Defender).Sum(battle => battle.AttackerDeathCount)) + "</br>");
            Html.AppendLine("<li>Battle Victories: " + _war.DefenderVictories.OfType<Battle>().Count());
            Html.AppendLine("<li>Conquerings: " + _war.DefenderVictories.OfType<SiteConquered>().Count());
            Html.AppendLine(" (" + _war.DefenderVictories.OfType<SiteConquered>().Where(conquering => conquering.ConquerType == SiteConqueredType.Pillaging).Count() + " Pillagings, ");
            Html.AppendLine(_war.DefenderVictories.OfType<SiteConquered>().Where(conquering => conquering.ConquerType == SiteConqueredType.Destruction).Count() + " Destructions, ");
            Html.AppendLine(_war.DefenderVictories.OfType<SiteConquered>().Where(conquering => conquering.ConquerType == SiteConqueredType.Conquest).Count() + " Conquests)");
            Html.AppendLine("</ul>");

            if (_war.Collections.Count(battle => !_world.FilterBattles || battle.Notable) > 0)
            {
                int warfareCount = 1;
                Html.AppendLine("<b>Warfare</b>");
                if (_world.FilterBattles)
                {
                    Html.Append(" (Notable)");
                }

                Html.Append("</br>");
                Html.AppendLine("<table>");
                Html.AppendLine("<tr>");
                Html.AppendLine("<th align=right>#</th>");
                Html.AppendLine("<th align=right>Year</th>");
                Html.AppendLine("<th>Battle</th>");
                Html.AppendLine("<th>Victor</th>");
                Html.AppendLine("<th align=right>" + Base64ToHtml(_war.Attacker.SmallIdenticonString) + "</th>");
                Html.AppendLine("<th>/</th>");
                Html.AppendLine("<th align=left>" + Base64ToHtml(_war.Defender.SmallIdenticonString) + "</th>");
                Html.AppendLine("</tr>");
                foreach (EventCollection warfare in _war.Collections.Where(battle => !_world.FilterBattles || battle.Notable))
                {
                    Html.AppendLine("<tr>");
                    Html.AppendLine("<td align=right>" + warfareCount + ".</td>");
                    Html.AppendLine("<td align=right>" + warfare.StartYear + "</td>");
                    string warfareString = warfare.ToLink();
                    if (warfareString.Contains(" as a result of"))
                    {
                        warfareString = warfareString.Insert(warfareString.IndexOf(" as a result of"), "</br>");
                    }

                    Html.Append("<td>" + warfareString + "</td>");
                    if (warfare.GetType() == typeof(Battle))
                    {
                        Battle battle = warfare as Battle;
                        Html.AppendLine("<td>"+ (battle.Victor == _war.Attacker? battle.Attacker.PrintEntity(): battle.Defender.PrintEntity())+ "</td>");
                        Html.AppendLine("<td align=right>" + (battle.Attacker == _war.Attacker ? battle.DefenderDeathCount : battle.AttackerDeathCount) + "</td>");
                        Html.AppendLine("<td>/</td>");
                        Html.AppendLine("<td align=left>" + (battle.Defender == _war.Attacker ? battle.DefenderDeathCount : battle.AttackerDeathCount) + "</td>");
                    }
                    else if (warfare.GetType() == typeof(SiteConquered))
                    {
                        Html.AppendLine("<td align=right>" + (warfare as SiteConquered).Attacker.PrintEntity() + "</td>");
                    }

                    Html.AppendLine("</tr>");
                    warfareCount++;
                }
                Html.AppendLine("</table></br>");
            }

            if (_world.FilterBattles && _war.Collections.Count(battle => !battle.Notable) > 0)
            {
                Html.AppendLine("<b>Warfare</b> (Unnotable)</br>");
                Html.AppendLine("<ul>");
                Html.AppendLine("<li>Battles: " + _war.Collections.OfType<Battle>().Where(battle => !battle.Notable).Count());
                Html.AppendLine("<li>Pillagings: " + _war.Collections.OfType<SiteConquered>().Where(conquering => conquering.ConquerType == SiteConqueredType.Pillaging).Count());
                Html.AppendLine("</ul>");
            }

            PrintEventLog(_world, _war.GetSubEvents(), War.Filters, _war);

            return Html.ToString();
        }
    }
}

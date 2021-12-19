using System.Linq;
using System.Text;
using LegendsViewer.Legends;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.EventCollections;

namespace LegendsViewer.Controls.HTML
{
    public class EraPrinter : HtmlPrinter
    {
        private readonly Era _era;
        private readonly World _world;

        public EraPrinter(Era era, World world)
        {
            _era = era;
            _world = world;
        }

        public override string GetTitle()
        {
            return _era.Name != "" ? _era.Name : "(" + (_era.StartYear < 0 ? 0 : _era.StartYear) + " - " + _era.EndYear + ")";
        }

        public override string Print()
        {
            Html = new StringBuilder();
            string title = string.IsNullOrWhiteSpace(_era.Name) ? "" : _era.Name + " ";
            string timespan = "(" + _era.StartYear + " - " + _era.EndYear + ")";
            Html.Append("<h1>").Append(title).Append(timespan).AppendLine("</h1></br></br>");

            PrintEventLog(_world, _era.Events, Era.Filters, _era);
            PrintWars();

            return Html.ToString();
        }

        private void PrintWars()
        {
            if (_era.Wars.Count > 0)
            {
                int warCount = 1;
                Html.AppendLine("<b>Wars</b></br>")
                    .AppendLine("<table>");
                foreach (War war in _era.Wars)
                {
                    Html.AppendLine("<tr>")
                        .Append("<td width=\"20\" align=\"right\">").Append(warCount).AppendLine(".</td><td width=\"10\"></td><td>");
                    if (war.StartYear < _era.StartYear)
                    {
                        Html.Append("<font color=\"Blue\">").Append(war.StartYear).Append("</font> - ");
                    }
                    else
                    {
                        Html.Append(war.StartYear).Append(" - ");
                    }

                    if (war.EndYear == -1)
                    {
                        Html.Append("<font color=\"Blue\">Present</font>");
                    }
                    else if (war.EndYear > _era.EndYear)
                    {
                        Html.Append("<font color=\"Blue\">").Append(war.EndYear).Append("</font>");
                    }
                    else
                    {
                        Html.Append(war.EndYear);
                    }

                    Html.Append("</td><td>").Append(war.ToLink()).Append("</td><td>").Append(war.Attacker.PrintEntity()).Append("</td><td>against</td><td>").Append(war.Defender.PrintEntity()).Append("</td>");

                    int attackerVictories = 0, defenderVictories = 0, attackerConquerings = 0, defenderConquerings = 0, attackerKills, defenderKills;
                    attackerVictories = war.AttackerVictories.OfType<Battle>().Count();
                    defenderVictories = war.DefenderVictories.OfType<Battle>().Count();
                    attackerConquerings = war.AttackerVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType != SiteConqueredType.Pillaging);
                    defenderConquerings = war.DefenderVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType != SiteConqueredType.Pillaging);
                    attackerKills = war.Collections.OfType<Battle>().Where(battle => war.Attacker == battle.Attacker).Sum(battle => battle.DefenderDeathCount) + war.Collections.OfType<Battle>().Where(battle => war.Attacker == battle.Defender).Sum(battle => battle.AttackerDeathCount);
                    defenderKills = war.Collections.OfType<Battle>().Where(battle => war.Defender == battle.Attacker).Sum(battle => battle.DefenderDeathCount) + war.Collections.OfType<Battle>().Where(battle => war.Defender == battle.Defender).Sum(battle => battle.AttackerDeathCount);

                    Html.AppendLine("<td>(A/D)</td>")
                        .Append("<td>Battles:</td><td align=right>").Append(attackerVictories).Append("</td><td>/</td><td>").Append(defenderVictories).AppendLine("</td>")
                        .Append("<td>Sites:</td><td align=right>").Append(attackerConquerings).Append("</td><td>/</td><td>").Append(defenderConquerings).AppendLine("</td>")
                        .Append("<td>Kills:</td><td align=right>").Append(attackerKills).Append("</td><td>/</td><td>").Append(defenderKills).AppendLine("</td>")
                        .AppendLine("</tr>")
                        .AppendLine("</tr>");
                    warCount++;
                }
                Html.AppendLine("</table></br>");
            }
        }
    }
}

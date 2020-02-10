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
            if (_era.Name != "")
            {
                return _era.Name;
            }

            return "(" + (_era.StartYear < 0 ? 0 : _era.StartYear) + " - " + _era.EndYear + ")";
        }

        public override string Print()
        {
            Html = new StringBuilder();
            string title = string.IsNullOrWhiteSpace(_era.Name) ? "" : _era.Name + " ";
            string timespan = "(" + _era.StartYear + " - " + _era.EndYear + ")";
            Html.AppendLine("<h1>" + title + timespan + "</h1></br></br>");

            PrintEventLog(_world, _era.Events, Era.Filters, _era);
            PrintWars();

            return Html.ToString();
        }

        private void PrintWars()
        {
            if (_era.Wars.Count > 0)
            {
                int warCount = 1;
                Html.AppendLine("<b>Wars</b></br>");
                Html.AppendLine("<table>");
                foreach (War war in _era.Wars)
                {
                    Html.AppendLine("<tr>");
                    Html.AppendLine("<td width=\"20\" align=\"right\">" + warCount + ".</td><td width=\"10\"></td><td>");
                    if (war.StartYear < _era.StartYear)
                    {
                        Html.Append("<font color=\"Blue\">" + war.StartYear + "</font> - ");
                    }
                    else
                    {
                        Html.Append(war.StartYear + " - ");
                    }

                    if (war.EndYear == -1)
                    {
                        Html.Append("<font color=\"Blue\">Present</font>");
                    }
                    else if (war.EndYear > _era.EndYear)
                    {
                        Html.Append("<font color=\"Blue\">" + war.EndYear + "</font>");
                    }
                    else
                    {
                        Html.Append(war.EndYear);
                    }

                    Html.Append("</td><td>" + war.ToLink() + "</td><td>" + war.Attacker.PrintEntity() + "</td><td>against</td><td>" + war.Defender.PrintEntity() + "</td>");

                    int attackerVictories = 0, defenderVictories = 0, attackerConquerings = 0, defenderConquerings = 0, attackerKills, defenderKills;
                    attackerVictories = war.AttackerVictories.OfType<Battle>().Count();
                    defenderVictories = war.DefenderVictories.OfType<Battle>().Count();
                    attackerConquerings = war.AttackerVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType != SiteConqueredType.Pillaging);
                    defenderConquerings = war.DefenderVictories.OfType<SiteConquered>().Count(conquering => conquering.ConquerType != SiteConqueredType.Pillaging);
                    attackerKills = war.Collections.OfType<Battle>().Where(battle => war.Attacker == battle.Attacker).Sum(battle => battle.DefenderDeathCount) + war.Collections.OfType<Battle>().Where(battle => war.Attacker == battle.Defender).Sum(battle => battle.AttackerDeathCount);
                    defenderKills = war.Collections.OfType<Battle>().Where(battle => war.Defender == battle.Attacker).Sum(battle => battle.DefenderDeathCount) + war.Collections.OfType<Battle>().Where(battle => war.Defender == battle.Defender).Sum(battle => battle.AttackerDeathCount);

                    Html.AppendLine("<td>(A/D)</td>");
                    Html.AppendLine("<td>Battles:</td><td align=right>" + attackerVictories + "</td><td>/</td><td>" + defenderVictories + "</td>");
                    Html.AppendLine("<td>Sites:</td><td align=right>" + attackerConquerings + "</td><td>/</td><td>" + defenderConquerings + "</td>");
                    Html.AppendLine("<td>Kills:</td><td align=right>" + attackerKills + "</td><td>/</td><td>" + defenderKills + "</td>");
                    Html.AppendLine("</tr>");

                    Html.AppendLine("</tr>");
                    warCount++;

                }
                Html.AppendLine("</table></br>");
            }
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    internal class BattlePrinter : HtmlPrinter
    {
        private readonly Battle _battle;
        private readonly World _world;

        public BattlePrinter(Battle battle, World world)
        {
            _battle = battle;
            _world = world;
        }

        public override string GetTitle()
        {
            return _battle.Name;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.Append("<h1>").Append(_battle.GetIcon()).Append(' ').Append(GetTitle()).AppendLine("</h1></br>");

            string battleDescription = _battle.GetYearTime() + ", " + _battle.ToLink(false);
            if (_battle.ParentCollection != null)
            {
                battleDescription += " occured as part of " + _battle.ParentCollection.ToLink();
                battleDescription += " waged by " + (_battle.ParentCollection as War).Attacker.PrintEntity();
                battleDescription += " on " + (_battle.ParentCollection as War).Defender.PrintEntity();
            }
            if (_battle.Site != null)
            {
                battleDescription += " at " + _battle.Site.ToLink();
            }
            if (_battle.Region != null)
            {
                battleDescription += " in " + _battle.Region.ToLink();
            }
            Html.Append(battleDescription).AppendLine(".</br>");

            if (_battle.Conquering != null)
            {
                Html.Append("<b>Outcome:</b> ").Append(_battle.Conquering.ToLink(true, _battle)).AppendLine("</br>");
            }

            Html.AppendLine("</br>");

            if (_battle.Attacker != null && _battle.Defender != null)
            {
                List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _battle);
                Html.AppendLine("<table>")
                    .AppendLine("<tr>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                    .AppendLine("</tr></table></br>");
            }

            Html.Append("<center>").Append(MakeLink(Font("[Chart]", "Maroon"), LinkOption.LoadChart)).Append("</center>").AppendLine(LineBreak)
                .AppendLine("<table>")
                .AppendLine("<tr><td valign=\"top\">");
            if (_battle.Victor == _battle.Attacker)
            {
                Html.AppendLine("<center><b><u>Victor</u></b></center></br>");
            }
            else
            {
                Html.AppendLine("</br></br>");
            }

            Html.Append("<b>").Append((_battle.Attacker?.PrintEntity() ?? "an unknown civilization")).Append(" (Attacker) ").Append((_battle.NotableAttackers.Count + _battle.AttackerSquads.Sum(squad => squad.Numbers))).Append(" Members, ").Append(_battle.AttackerDeathCount).Append(" Losses</b> ").AppendLine(LineBreak)
                .AppendLine("<ul>");
            var squadRaces = from squad in _battle.AttackerSquads
                             group squad by squad.Race into squads
                             select new { Race = squads.Key, Numbers = squads.Sum(squad => squad.Numbers), Deaths = squads.Sum(squad => squad.Deaths) };
            squadRaces = squadRaces.OrderByDescending(squad => squad.Numbers);

            foreach (var squadRace in squadRaces)
            {
                Html.Append("<li>").Append(squadRace.Numbers).Append(' ').AppendLine(squadRace.Race.NamePlural)
                    .Append(", ").Append(squadRace.Deaths).Append(" Losses</br>");
            }
            foreach (HistoricalFigure attacker in _battle.NotableAttackers)
            {
                Html.Append("<li>").AppendLine(attacker.ToLink());
                if (_battle.Collection.OfType<FieldBattle>().Any(fieldBattle => fieldBattle.AttackerGeneral == attacker) ||
                    _battle.Collection.OfType<AttackedSite>().Any(attack => attack.AttackerGeneral == attacker))
                {
                    Html.Append(" <b>(Led the Attack)</b> ");
                }

                if (_battle.GetSubEvents().OfType<HfDied>().Any(death => death.HistoricalFigure == attacker))
                {
                    Html.Append(" (Died) ");
                }
            }
            Html.AppendLine("</ul>")
                .AppendLine("</td><td width=\"20\"></td><td valign=\"top\">");

            if (_battle.Victor == _battle.Defender)
            {
                Html.AppendLine("<center><b><u>Victor</u></b></center></br>");
            }
            else
            {
                Html.AppendLine("</br></br>");
            }

            Html.Append("<b>").Append((_battle.Defender?.PrintEntity() ?? "an unknown civilization")).Append(" (Defender) ").Append((_battle.NotableDefenders.Count + _battle.DefenderSquads.Sum(squad => squad.Numbers))).Append(" Members, ").Append(_battle.DefenderDeathCount).Append(" Losses</b> ").AppendLine(LineBreak)
                .AppendLine("<ul>");
            squadRaces = from squad in _battle.DefenderSquads
                         group squad by squad.Race into squads
                         select new { Race = squads.Key, Numbers = squads.Sum(squad => squad.Numbers), Deaths = squads.Sum(squad => squad.Deaths) };
            squadRaces = squadRaces.OrderByDescending(squad => squad.Numbers);

            foreach (var squadRace in squadRaces)
            {
                Html.Append("<li>").Append(squadRace.Numbers).Append(' ').AppendLine(squadRace.Race.NamePlural)
                    .Append(", ").Append(squadRace.Deaths).Append(" Losses</br>");
            }
            foreach (HistoricalFigure defender in _battle.NotableDefenders)
            {
                Html.Append("<li>").AppendLine(defender.ToLink());
                if (_battle.Collection.OfType<FieldBattle>().Any(fieldBattle => fieldBattle.DefenderGeneral == defender) ||
                    _battle.Collection.OfType<AttackedSite>().Any(attack => attack.DefenderGeneral == defender))
                {
                    Html.Append(" <b>(Led the Defense)</b> ");
                }

                if (_battle.GetSubEvents().OfType<HfDied>().Any(death => death.HistoricalFigure == defender))
                {
                    Html.Append(" (Died) ");
                }
            }
            Html.AppendLine("</ul>")
                .AppendLine("</td></tr></table></br>");

            if (_battle.NonCombatants.Count > 0)
            {
                Html.AppendLine("<b>Non Combatants</b></br>")
                    .AppendLine("<ol>");
                foreach (HistoricalFigure nonCombatant in _battle.NonCombatants)
                {
                    Html.Append("<li>").AppendLine(nonCombatant.ToLink());
                    if (_battle.Collection.OfType<HfDied>().Any(death => death.HistoricalFigure == nonCombatant))
                    {
                        Html.Append(" (Died) ");
                    }
                }
                Html.AppendLine("</ol>");
            }

            PrintEventLog(_world, _battle.GetSubEvents(), Battle.Filters, _battle);

            return Html.ToString();
        }
    }
}

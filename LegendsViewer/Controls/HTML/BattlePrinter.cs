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
    class BattlePrinter : HtmlPrinter
    {
        Battle _battle;
        World _world;

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

            Html.AppendLine("<h1>" + _battle.GetIcon() + " " + GetTitle() + "</h1></br>");

            string battleDescription = _battle.GetYearTime() + _battle.ToLink(false);
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
            Html.AppendLine(battleDescription+".</br>");

            if (_battle.Conquering != null)
            {
                Html.AppendLine("<b>Outcome:</b> " + _battle.Conquering.ToLink(true, _battle) + "</br>");
            }

            Html.AppendLine("</br>");

            if (_battle.Attacker != null && _battle.Defender != null)
            {
                List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _battle);
                Html.AppendLine("<table>");
                Html.AppendLine("<tr>");
                Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap) + "</td>");
                Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap) + "</td>");
                Html.AppendLine("</tr></table></br>");
            }

            Html.AppendLine("<center>" + MakeLink(Font("[Chart]", "Maroon"), LinkOption.LoadChart) + "</center>" + LineBreak);

            Html.AppendLine("<table>");
            Html.AppendLine("<tr><td valign=\"top\">");
            if (_battle.Victor == _battle.Attacker)
            {
                Html.AppendLine("<center><b><u>Victor</u></b></center></br>");
            }
            else
            {
                Html.AppendLine("</br></br>");
            }

            Html.AppendLine("<b>" + (_battle.Attacker?.PrintEntity() ?? "an unknown civilization") + " (Attacker) " + (_battle.NotableAttackers.Count + _battle.AttackerSquads.Sum(squad => squad.Numbers)) + " Members, " + _battle.AttackerDeathCount + " Losses</b> " + LineBreak);
            Html.AppendLine("<ul>");
            var squadRaces = from squad in _battle.AttackerSquads
                             group squad by squad.Race into squads
                             select new { Race = squads.Key, Numbers = squads.Sum(squad => squad.Numbers), Deaths = squads.Sum(squad => squad.Deaths) };
            squadRaces = squadRaces.OrderByDescending(squad => squad.Numbers);

            foreach (var squadRace in squadRaces)
            {
                Html.AppendLine("<li>" + squadRace.Numbers + " " + squadRace.Race.NamePlural);
                Html.Append(", " + squadRace.Deaths + " Losses</br>");
            }
            foreach (HistoricalFigure attacker in _battle.NotableAttackers)
            {
                Html.AppendLine("<li>" + attacker.ToLink());
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
            Html.AppendLine("</ul>");
            Html.AppendLine("</td><td width=\"20\"></td><td valign=\"top\">");


            if (_battle.Victor == _battle.Defender)
            {
                Html.AppendLine("<center><b><u>Victor</u></b></center></br>");
            }
            else
            {
                Html.AppendLine("</br></br>");
            }

            Html.AppendLine("<b>" + (_battle.Defender?.PrintEntity() ?? "an unknown civilization") + " (Defender) " + (_battle.NotableDefenders.Count + _battle.DefenderSquads.Sum(squad => squad.Numbers)) + " Members, " + _battle.DefenderDeathCount + " Losses</b> "+ LineBreak);
            Html.AppendLine("<ul>");
            squadRaces = from squad in _battle.DefenderSquads
                         group squad by squad.Race into squads
                         select new { Race = squads.Key, Numbers = squads.Sum(squad => squad.Numbers), Deaths = squads.Sum(squad => squad.Deaths) };
            squadRaces = squadRaces.OrderByDescending(squad => squad.Numbers);

            foreach (var squadRace in squadRaces)
            {
                Html.AppendLine("<li>" + squadRace.Numbers + " " + squadRace.Race.NamePlural);
                Html.Append(", " + squadRace.Deaths + " Losses</br>");
            }
            foreach (HistoricalFigure defender in _battle.NotableDefenders)
            {
                Html.AppendLine("<li>" + defender.ToLink());
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
            Html.AppendLine("</ul>");
            Html.AppendLine("</td></tr></table></br>");

            if (_battle.NonCombatants.Count > 0)
            {
                Html.AppendLine("<b>Non Combatants</b></br>");
                Html.AppendLine("<ol>");
                foreach (HistoricalFigure nonCombatant in _battle.NonCombatants)
                {
                    Html.AppendLine("<li>" + nonCombatant.ToLink());
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

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
    public class RegionPrinter : HtmlPrinter
    {
        private readonly WorldRegion _region;
        private readonly World _world;

        public RegionPrinter(WorldRegion region, World world)
        {
            _region = region;
            _world = world;
        }

        public override string GetTitle()
        {
            return _region.Name;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.Append("<h1>").Append(_region.GetIcon()).Append(' ').Append(_region.Name).Append(", ").Append(_region.Type).AppendLine("</h1><br />");

            if (_region.Coordinates.Count > 0)
            {
                List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _region);

                Html.AppendLine("<table>")
                    .AppendLine("<tr>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                    .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                    .AppendLine("</tr></table></br>")
                    .AppendLine("<b>Geography</b><br/>")
                    .AppendLine("<ul>")
                    .Append("<li>Area: ").Append(_region.SquareTiles).AppendLine(" region tiles²</li>")
                    .AppendLine("</ul>");
            }

            if (_region.Force != null)
            {
                Html.AppendLine("<b>Local Force</b> " + LineBreak)
                    .AppendLine("<ul>")
                    .Append("<li>").Append(_region.Force.ToLink()).AppendLine("</li>")
                    .AppendLine("</ul>");
            }

            Html.AppendLine("<b>Evilness</b> " + LineBreak)
                .AppendLine("<ul>")
                .Append("<li>").Append(_region.Evilness.GetDescription()).AppendLine("</li>")
                .AppendLine("</ul>");

            if (_region.Battles.Count(battle => !_world.FilterBattles || battle.Notable) > 0)
            {
                int battleCount = 1;
                Html.AppendLine("<b>Warfare</b> ");
                if (_world.FilterBattles)
                {
                    Html.Append(" (Notable)");
                }

                Html.Append("<table border=\"0\">");
                foreach (Battle battle in _region.Battles.Where(battle => !_world.FilterBattles || battle.Notable))
                {
                    Html.AppendLine("<tr>")
                        .Append("<td width=\"20\"  align=\"right\">").Append(battleCount).AppendLine(".</td><td width=\"10\"></td>")
                        .Append("<td>").Append(battle.StartYear).AppendLine("</td>")
                        .Append("<td>").Append(battle.ToLink()).AppendLine("</td>")
                        .AppendLine("<td>as part of</td>")
                        .Append("<td>").Append(battle.ParentCollection.ToLink()).AppendLine("</td>")
                        .Append("<td>").AppendLine(battle.Attacker.PrintEntity());
                    if (battle.Victor == battle.Attacker)
                    {
                        Html.Append("<td>(V)</td>");
                    }
                    else
                    {
                        Html.AppendLine("<td></td>");
                    }

                    Html.AppendLine("<td>Vs.</td>")
                        .Append("<td>").AppendLine(battle.Defender.PrintEntity());
                    if (battle.Victor == battle.Defender)
                    {
                        Html.AppendLine("<td>(V)</td>");
                    }
                    else
                    {
                        Html.AppendLine("<td></td>");
                    }

                    Html.Append("<td>(Deaths: ").Append(battle.AttackerDeathCount + battle.DefenderDeathCount).AppendLine(")</td>")
                        .AppendLine("</tr>");
                    battleCount++;
                }
                Html.AppendLine("</table></br>");
            }

            if (_world.FilterBattles && _region.Battles.Count(battle => !battle.Notable) > 0)
            {
                Html.Append("<b>Battles</b> (Unnotable): ").Append(_region.Battles.Count(battle => !battle.Notable)).AppendLine("</br></br>");
            }

            if (_region.Sites.Count > 0)
            {
                Html.AppendLine("<b>Sites</b> " + LineBreak)
                    .AppendLine("<ul>");
                foreach (Site site in _region.Sites)
                {
                    Html.Append("<li>").Append(site.ToLink()).Append(", ").Append(site.SiteType.GetDescription()).AppendLine("</li>");
                }
                Html.AppendLine("</ul>");
            }

            if (_region.MountainPeaks.Count > 0)
            {
                Html.AppendLine("<b>Mountain Peaks</b> " + LineBreak)
                    .AppendLine("<ul>");
                foreach (MountainPeak peak in _region.MountainPeaks)
                {
                    Html.Append("<li>").Append(peak.ToLink()).Append(", ").Append(peak.Height).Append(" tiles ~ ").Append(3 * peak.Height).AppendLine(" m</li>");
                }
                Html.AppendLine("</ul>");
            }

            int deathCount = _region.Events.OfType<HfDied>().Count();
            if (deathCount > 0 || _region.Battles.Count > 0)
            {
                var popInBattle =
                    _region.Battles
                        .Sum(
                            battle =>
                                battle.AttackerSquads.Sum(squad => squad.Deaths) +
                                battle.DefenderSquads.Sum(squad => squad.Deaths));
                Html.AppendLine("<b>Deaths</b> " + LineBreak);
                if (deathCount > 100)
                {
                    Html.AppendLine("<ul>")
                        .Append("<li>Historical figures died in this Region: ").Append(deathCount).AppendLine();
                    if (popInBattle > 0)
                    {
                        Html.Append("<li>Population in Battle: ").Append(popInBattle).AppendLine();
                    }
                    Html.AppendLine("</ul>");
                }
                else
                {
                    Html.AppendLine("<ol>");
                    foreach (HfDied death in _region.Events.OfType<HfDied>())
                    {
                        Html.Append("<li>").Append(death.HistoricalFigure.ToLink()).Append(", in ").Append(death.Year).Append(" (").Append(death.Cause.GetDescription()).AppendLine(")");
                    }
                    if (popInBattle > 0)
                    {
                        Html.Append("<li>Population died in Battle: ").Append(popInBattle).AppendLine();
                    }
                    Html.AppendLine("</ol>");
                }
            }

            PrintEventLog(_world, _region.Events, WorldRegion.Filters, _region);

            return Html.ToString();
        }
    }
}

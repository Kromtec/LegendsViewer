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
        WorldRegion _region;
        World _world;

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

            Html.AppendLine("<h1>" + _region.GetIcon() + " " + _region.Name + ", " + _region.Type + "</h1><br />");

            if (_region.Coordinates.Any())
            {
                List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _region);

                Html.AppendLine("<table>");
                Html.AppendLine("<tr>");
                Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap) + "</td>");
                Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap) + "</td>");
                Html.AppendLine("</tr></table></br>");

                Html.AppendLine("<b>Geography</b><br/>");
                Html.AppendLine("<ul>");
                Html.AppendLine("<li>Area: " + _region.SquareTiles + " region tiles²</li>");
                Html.AppendLine("</ul>");
            }

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
                    Html.AppendLine("<tr>");
                    Html.AppendLine("<td width=\"20\"  align=\"right\">" + battleCount + ".</td><td width=\"10\"></td>");
                    Html.AppendLine("<td>" + battle.StartYear + "</td>");
                    Html.AppendLine("<td>" + battle.ToLink() + "</td>");
                    Html.AppendLine("<td>as part of</td>");
                    Html.AppendLine("<td>" + battle.ParentCollection.ToLink() + "</td>");
                    Html.AppendLine("<td>" + battle.Attacker.PrintEntity());
                    if (battle.Victor == battle.Attacker)
                    {
                        Html.Append("<td>(V)</td>");
                    }
                    else
                    {
                        Html.AppendLine("<td></td>");
                    }

                    Html.AppendLine("<td>Vs.</td>");
                    Html.AppendLine("<td>" + battle.Defender.PrintEntity());
                    if (battle.Victor == battle.Defender)
                    {
                        Html.AppendLine("<td>(V)</td>");
                    }
                    else
                    {
                        Html.AppendLine("<td></td>");
                    }

                    Html.AppendLine("<td>(Deaths: " + (battle.AttackerDeathCount + battle.DefenderDeathCount) + ")</td>");
                    Html.AppendLine("</tr>");
                    battleCount++;
                }
                Html.AppendLine("</table></br>");
            }

            if (_world.FilterBattles && _region.Battles.Count(battle => !battle.Notable) > 0)
            {
                Html.AppendLine("<b>Battles</b> (Unnotable): " + _region.Battles.Count(battle => !battle.Notable) + "</br></br>");
            }

            if (_region.Sites.Any())
            {
                Html.AppendLine("<b>Sites</b> " + LineBreak);
                Html.AppendLine("<ul>");
                foreach (Site site in _region.Sites)
                {
                    Html.AppendLine("<li>" + site.ToLink() + ", " + site.SiteType.GetDescription() + "</li>");
                }
                Html.AppendLine("</ul>");
            }

            if (_region.MountainPeaks.Any())
            {
                Html.AppendLine("<b>Mountain Peaks</b> " + LineBreak);
                Html.AppendLine("<ul>");
                foreach (MountainPeak peak in _region.MountainPeaks)
                {
                    Html.AppendLine("<li>" + peak.ToLink() + ", " + peak.Height + " tiles ~ " + 3 * peak.Height + " m</li>");
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
                    Html.AppendLine("<ul>");
                    Html.AppendLine("<li>Historical figures died in this Region: " + deathCount);
                    if (popInBattle > 0)
                    {
                        Html.AppendLine("<li>Population in Battle: " + popInBattle);
                    }
                    Html.AppendLine("</ul>");
                }
                else
                {
                    Html.AppendLine("<ol>");
                    foreach (HfDied death in _region.Events.OfType<HfDied>())
                    {
                        Html.AppendLine("<li>" + death.HistoricalFigure.ToLink() + ", in " + death.Year + " (" + death.Cause.GetDescription() + ")");
                    }
                    if (popInBattle > 0)
                    {
                        Html.AppendLine("<li>Population died in Battle: " + popInBattle);
                    }
                    Html.AppendLine("</ol>");
                }
            }

            PrintEventLog(_world, _region.Events, WorldRegion.Filters, _region);

            return Html.ToString();
        }
    }
}

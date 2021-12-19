using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class UndergroundRegionPrinter : HtmlPrinter
    {
        private readonly UndergroundRegion _region;
        private readonly World _world;

        public UndergroundRegionPrinter(UndergroundRegion region, World world)
        {
            _region = region;
            _world = world;
        }

        public override string GetTitle()
        {
            return _region.Type;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            Html.Append("<h1>").Append(_region.GetIcon()).Append(' ').Append("Depth: ").Append(_region.Depth).AppendLine("</h1></br></br>");

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
                        Html.Append("<li>Population died in Battle: ").Append(popInBattle).AppendLine();
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

            PrintEventLog(_world, _region.Events, UndergroundRegion.Filters, _region);

            return Html.ToString();
        }
    }
}

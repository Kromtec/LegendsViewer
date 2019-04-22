using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.Events;

namespace LegendsViewer.Controls.HTML
{
    public class UndergroundRegionPrinter : HtmlPrinter
    {
        UndergroundRegion _region;
        World _world;

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

            Html.AppendLine("<h1>Depth: " + _region.Depth + "</h1></br></br>");

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
                        Html.AppendLine("<li>Population died in Battle: " + popInBattle);
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

            PrintEventLog(_world, _region.Events, UndergroundRegion.Filters, _region);

            return Html.ToString();
        }
    }
}

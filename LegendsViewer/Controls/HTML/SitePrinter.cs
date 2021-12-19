using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using LegendsViewer.Controls.Map;
using LegendsViewer.Legends;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    internal class SitePrinter : HtmlPrinter
    {
        private readonly Site _site;
        private readonly World _world;

        public SitePrinter(Site site, World world)
        {
            _site = site;
            _world = world;
        }

        public override string GetTitle()
        {
            return _site.Name;
        }

        public override string Print()
        {
            Html = new StringBuilder();

            LoadCustomScripts();

            Html.AppendLine("<div class=\"container-fluid\">");

            PrintTitle();

            Html.AppendLine("<div class=\"row\">");

            string siteMapPath = GetSiteMapPath();
            string colClassMaps = string.IsNullOrEmpty(siteMapPath) ? "col-md-6 col-sm-12" : "col-lg-9 col-md-12 col-sm-12";
            Html.Append("<div class=\"").Append(colClassMaps).AppendLine("\">");
            PrintMaps(siteMapPath);
            Html.AppendLine("</div>");

            string colClassChart = string.IsNullOrEmpty(siteMapPath) ? "col-md-3 col-sm-12" : "col-lg-3 col-md-6 col-sm-6";
            Html.Append("<div id=\"chart-populationbyrace-container\" class=\"").Append(colClassChart).AppendLine("\" style=\"height: 250px\">")
                .AppendLine("<canvas id=\"chart-populationbyrace\"></canvas>")
                .AppendLine("</div>")
                .AppendLine("</div>")
                .AppendLine("<div class=\"row\">");
            PrintPopulations(_site.Populations);
            Html.AppendLine("</div>");

            PrintGeographyInfo();
            PrintStructures();
            PrintSiteProperties();
            PrintRelatedArtifacts();
            PrintRelatedHistoricalFigures();
            PrintWarfareInfo();
            PrintOwnerHistory();
            PrintOfficials();
            PrintConnections();

            Html.AppendLine("<div class=\"row\">");
            PrintBeastAttacks();
            PrintDeaths();
            Html.AppendLine("</div>");

            PrintEventLog(_world, _site.Events, Site.Filters, _site);
            Html.AppendLine("</div>");

            return Html.ToString();
        }

        private void PrintRelatedHistoricalFigures()
        {
            if (_site.RelatedHistoricalFigures.Count == 0)
            {
                return;
            }
            Html.AppendLine("<div class=\"row\">")
                .AppendLine("<div class=\"col-md-12\">")
                .Append(Bold("Related Historical Figures")).AppendLine(LineBreak);
            StartList(ListType.Unordered);
            foreach (HistoricalFigure hf in _site.RelatedHistoricalFigures)
            {
                SiteLink hfToSiteLink = hf.RelatedSites.Find(link => link.Site == _site);
                if (hfToSiteLink != null)
                {
                    Html.Append(ListItem).AppendLine(hf.ToLink(true, _site));
                    if (hfToSiteLink.SubId != 0)
                    {
                        Structure structure = _site.Structures.Find(s => s.Id == hfToSiteLink.SubId);
                        if (structure != null)
                        {
                            Html.Append(" - ").Append(structure.ToLink(true, _site)).AppendLine(" - ");
                        }
                    }
                    if (hfToSiteLink.OccupationId != 0)
                    {
                        Structure structure = _site.Structures.Find(s => s.Id == hfToSiteLink.OccupationId);
                        if (structure != null)
                        {
                            Html.Append(" - ").Append(structure.ToLink(true, _site)).AppendLine(" - ");
                        }
                    }
                    Html.Append(" (").Append(hfToSiteLink.Type.GetDescription()).AppendLine(")");
                }
            }
            EndList(ListType.Unordered);
            Html.AppendLine("</div>")
                .AppendLine("</div>");
        }

        private void LoadCustomScripts()
        {
            Html.AppendLine("<script>")
                .AppendLine("window.onload = function(){");

            PopulatePopulationChartData(_site.Populations.Where(pop => pop.IsMainRace || pop.IsAnimalPeople).ToList());

            Html.AppendLine("}")
                .AppendLine("</script>");
        }

        private void PrintRelatedArtifacts()
        {
            var createdArtifacts = _site.Events.OfType<ArtifactCreated>().Where(e => e.Artifact != null).Select(e => e.Artifact).ToList();
            var storedArtifacts = _site.Events.OfType<ArtifactStored>().Where(e => e.Artifact != null).Select(e => e.Artifact).ToList();
            var stolenArtifacts = _site.Events.OfType<ItemStolen>().Where(e => e.Artifact != null).Select(e => e.Artifact).ToList();
            var lostArtifacts = _site.Events.OfType<ArtifactLost>().Where(e => e.Artifact != null).Select(e => e.Artifact).ToList();
            var relatedArtifacts = createdArtifacts
                .Union(storedArtifacts)
                .Union(lostArtifacts)
                .Union(stolenArtifacts)
                .Distinct()
                .ToList();
            if (relatedArtifacts.Count == 0)
            {
                return;
            }
            Html.AppendLine("<div class=\"row\">")
                .AppendLine("<div class=\"col-md-12\">")
                .Append(Bold("Related Artifacts")).AppendLine(LineBreak);
            StartList(ListType.Unordered);
            foreach (Artifact artifact in relatedArtifacts)
            {
                Html.Append(ListItem).AppendLine(artifact.ToLink(true, _site));
                if (!string.IsNullOrWhiteSpace(artifact.Type))
                {
                    Html.Append(" a legendary ").Append(artifact.Material).AppendLine(" ")
                        .AppendLine(!string.IsNullOrWhiteSpace(artifact.SubType) ? artifact.SubType : artifact.Type.ToLower());
                }
                List<string> relations = new List<string>();
                if (createdArtifacts.Contains(artifact))
                {
                    relations.Add("created");
                }
                if (storedArtifacts.Contains(artifact) && (artifact.Site?.Equals(_site) != true))
                {
                    relations.Add("previously stored");
                }
                if (stolenArtifacts.Contains(artifact))
                {
                    relations.Add("stolen");
                }
                if (lostArtifacts.Contains(artifact))
                {
                    relations.Add("lost");
                }
                if (storedArtifacts.Contains(artifact) && artifact.Site?.Equals(_site) == true)
                {
                    relations.Add("stored");
                }
                if (artifact.Holder != null)
                {
                    relations.Add("currently in possession of " + artifact.Holder.ToLink(true, _site));
                }
                else if (artifact.Site != null && artifact.Site != _site)
                {
                    relations.Add("currently stored in " + artifact.Site.ToLink(true, _site));
                }
                if (relations.Count > 0)
                {
                    Html.Append(" (").Append(string.Join(", ", relations)).AppendLine(")");
                }
            }
            EndList(ListType.Unordered);
            Html.AppendLine("</div>")
                .AppendLine("</div>");
        }

        private void PrintConnections()
        {
            if (_site.Connections.Count > 0)
            {
                Html.AppendLine("<div class=\"row\">")
                    .AppendLine("<div class=\"col-md-12\">")
                    .AppendLine("<b>Connections</b></br>")
                    .AppendLine("<ol>");
                foreach (Site connection in _site.Connections)
                {
                    Html.Append("<li>").AppendLine(connection.ToLink());
                }
                Html.AppendLine("</ol>")
                    .AppendLine("</div>")
                    .AppendLine("</div>");
            }
        }

        private void PrintOfficials()
        {
            if (_site.Officials.Count > 0)
            {
                Html.AppendLine("<div class=\"row\">")
                    .AppendLine("<div class=\"col-md-12\">")
                    .AppendLine("<b>Officials</b></br>")
                    .AppendLine("<ol>");
                foreach (Site.Official official in _site.Officials)
                {
                    Html.Append("<li>").Append(official.HistoricalFigure.ToLink()).Append(", ").AppendLine(official.Position);
                }
                Html.AppendLine("</ol>")
                    .AppendLine("</div>")
                    .AppendLine("</div>");
            }
        }

        private void PrintOwnerHistory()
        {
            if (_site.OwnerHistory.Count > 0)
            {
                Html.AppendLine("<div class=\"row\">")
                    .AppendLine("<div class=\"col-md-12\">")
                    .AppendLine("<b>Owner History</b><br />")
                    .AppendLine("<ol>");
                foreach (OwnerPeriod ownerPeriod in _site.OwnerHistory)
                {
                    string ownerString = "An unknown civilization";
                    if (ownerPeriod.Founder != null)
                    {
                        ownerString = ownerPeriod.Founder.ToLink(true, _site);
                    }
                    else if (ownerPeriod.Owner != null)
                    {
                        ownerString = ownerPeriod.Owner.PrintEntity();
                    }
                    string startYear = ownerPeriod.StartYear == -1 ? "a time before time" : ownerPeriod.StartYear.ToString();
                    Html.Append("<li>").Append(ownerString).Append(", ").Append(ownerPeriod.StartCause).Append(' ').Append(_site.ToLink(true, _site));

                    if (ownerPeriod.Founder != null && ownerPeriod.Owner != null)
                    {
                        Html.Append(" for ").Append(ownerPeriod.Owner.PrintEntity());
                    }

                    Html.Append(" in ").Append(startYear);

                    if (ownerPeriod.EndYear >= 0)
                    {
                        Html.Append(" and it was ").Append(ownerPeriod.EndCause).Append(" in ").Append(ownerPeriod.EndYear);
                    }

                    if (ownerPeriod.Destroyer != null)
                    {
                        Html.Append(" by ").Append(ownerPeriod.Destroyer.ToLink(true, _site));
                    }
                    else if (ownerPeriod.Ender != null)
                    {
                        Html.Append(" by ").Append(ownerPeriod.Ender.PrintEntity());
                    }
                    Html.AppendLine(".");
                }
                Html.AppendLine("</ol>")
                    .AppendLine("</div>")
                    .AppendLine("</div>");
            }
        }

        private void PrintWarfareInfo()
        {
            if (_site.Warfare.Count(battle => !_world.FilterBattles || battle.Notable) > 0)
            {
                Html.AppendLine("<div class=\"row\">")
                    .AppendLine("<div class=\"col-md-12\">");
                int warfareCount = 1;
                Html.AppendLine("<b>Warfare</b> ");
                if (_world.FilterBattles)
                {
                    Html.Append(" (Notable)");
                }

                Html.Append("<table border=\"0\">");
                foreach (EventCollection warfare in _site.Warfare.Where(battle => !_world.FilterBattles || battle.Notable))
                {
                    Html.AppendLine("<tr>")
                        .Append("<td width=\"20\"  align=\"right\">").Append(warfareCount).AppendLine(".</td><td width=\"10\"></td>")
                        .Append("<td>").Append(warfare.StartYear).AppendLine("</td>");
                    string warfareString = warfare.ToLink();
                    if (warfareString.Contains(" as a result of"))
                    {
                        warfareString = warfareString.Insert(warfareString.IndexOf(" as a result of"), "</br>");
                    }

                    Html.Append("<td>").Append(warfareString).AppendLine("</td>")
                        .AppendLine("<td>as part of</td>")
                        .Append("<td>").Append((warfare.ParentCollection == null ? "UNKNOWN" : warfare.ParentCollection.ToLink())).AppendLine("</td>")
                        .AppendLine("<td>by ");
                    if (warfare.GetType() == typeof(Battle))
                    {
                        Battle battle = warfare as Battle;
                        Html.Append(battle.Attacker?.PrintEntity()).Append("</td>");
                        if (battle.Victor == battle.Attacker)
                        {
                            Html.AppendLine("<td>(V)</td>");
                        }
                        else
                        {
                            Html.AppendLine("<td></td>");
                        }

                        Html.Append("<td>(Deaths: ").Append((battle.AttackerDeathCount + battle.DefenderDeathCount)).AppendLine(")</td>");
                    }
                    if (warfare.GetType() == typeof(SiteConquered))
                    {
                        Html.Append((warfare as SiteConquered).Attacker.PrintEntity()).Append("</td>");
                    }

                    Html.AppendLine("</tr>");
                    warfareCount++;
                }
                Html.AppendLine("</table></br>");

                if (_world.FilterBattles && _site.Warfare.Count(battle => !battle.Notable) > 0)
                {
                    Html.AppendLine("<b>Warfare</b> (Unnotable)</br>")
                        .AppendLine("<ul>")
                        .Append("<li>Battles: ").Append(_site.Warfare.OfType<Battle>().Count(battle => !battle.Notable)).AppendLine()
                        .Append("<li>Pillagings: ").Append(_site.Warfare.OfType<SiteConquered>().Count(conquering => conquering.ConquerType == SiteConqueredType.Pillaging)).AppendLine()
                        .AppendLine("</ul>");
                }

                Html.AppendLine("</div>")
                    .AppendLine("</div>");
            }
        }

        private void PrintStructures()
        {
            if (_site.Structures.Count > 0)
            {
                Html.AppendLine("<div class=\"row\">")
                    .AppendLine("<div class=\"col-md-12\">")
                    .AppendLine("<b>Structures</b><br/>")
                    .AppendLine("<ul>");
                foreach (Structure structure in _site.Structures)
                {
                    Html.Append("<li>").Append(structure.ToLink()).AppendLine(", ")
                        .AppendLine(structure.TypeAsString)
                        .AppendLine("</li>");
                }
                Html.AppendLine("</ul>")
                    .AppendLine("</div>")
                    .AppendLine("</div>");
            }
        }

        private void PrintSiteProperties()
        {
            if (_site.SiteProperties.Count > 0)
            {
                Html.AppendLine("<div class=\"row\">")
                    .AppendLine("<div class=\"col-md-12\">")
                    .AppendLine("<b>Site Properties</b><br/>")
                    .AppendLine("<ul>");
                foreach (SiteProperty siteProperty in _site.SiteProperties)
                {
                    Html.AppendLine("<li>");
                    if (siteProperty.Structure != null)
                    {
                        Html.AppendLine(siteProperty.Structure.Type.GetDescription())
                            .Append(" (").Append(siteProperty.Structure.ToLink()).AppendLine(")");
                    }
                    else if (siteProperty.Type != SitePropertyType.Unknown)
                    {
                        Html.AppendLine(siteProperty.Type.GetDescription());
                    }
                    else
                    {
                        Html.AppendLine("Property");
                    }
                    if (siteProperty.Owner != null)
                    {
                        Html.AppendLine(", ")
                            .AppendLine(siteProperty.Owner.ToLink());
                    }

                    Html.AppendLine("</li>");
                }
                Html.AppendLine("</ul>")
                    .AppendLine("</div>")
                    .AppendLine("</div>");
            }
        }

        private void PrintGeographyInfo()
        {
            if (_site.Region != null || !_site.Rectangle.IsEmpty)
            {
                Html.AppendLine("<div class=\"row\">")
                    .AppendLine("<div class=\"col-md-12\">")
                    .AppendLine("<b>Geography</b><br/>")
                    .AppendLine("<ul>");
                if (_site.Region != null)
                {
                    Html.Append("<li>Region: ").Append(_site.Region.ToLink()).Append(", ").Append(_site.Region.Type.GetDescription()).AppendLine("</li>");
                }
                if (!_site.Rectangle.IsEmpty)
                {
                    Html.Append("<li>Position: X ").Append(_site.Rectangle.X).Append(" Y ").Append(_site.Rectangle.Y).AppendLine("</li>");
                    if (_site.Rectangle.Width != 0 && _site.Rectangle.Height != 0)
                    {
                        Html.Append("<li>Size: ").Append(_site.Rectangle.Width).Append(" x ").Append(_site.Rectangle.Height).AppendLine("</li>");
                    }
                }
                Html.AppendLine("</ul>")
                    .AppendLine("</div>")
                    .AppendLine("</div>");
            }
        }

        private void PrintMaps(string siteMapPath)
        {
            Html.AppendLine("<div class=\"row\">")
                .AppendLine("<div class=\"col-md-12\">");
            List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _site);
            Html.AppendLine("<table>")
                .AppendLine("<tr>");
            PrintSiteMap(siteMapPath);
            Html.Append("<td>").Append(MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap)).AppendLine("</td>")
                .Append("<td>").Append(MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap)).AppendLine("</td>")
                .AppendLine("</tr></table></br>")
                .AppendLine("</div>")
                .AppendLine("</div>");
        }

        private void PrintTitle()
        {
            Html.AppendLine("<div class=\"row\">")
                .AppendLine("<div class=\"col-md-12\">");
            if (!string.IsNullOrWhiteSpace(_site.Name))
            {
                Html.Append("<h1>").Append(_site.GetIcon()).Append(' ').Append(_site.UntranslatedName).Append(", \"").Append(_site.Name).AppendLine("\"</h1>")
                    .Append("<b>").Append(_site.ToLink(false)).Append(" is a ").Append(_site.Type).AppendLine("</b><br /><br />");
            }
            else
            {
                Html.Append("<h1>").Append(_site.Type).AppendLine("</h1>");
            }
            Html.AppendLine("</div>")
                .AppendLine("</div>");
        }

        private void PrintDeaths()
        {
            int deathCount = _site.Events.OfType<HfDied>().Count();
            if (deathCount > 0 || _site.Warfare.OfType<Battle>().Any())
            {
                Html.AppendLine("<div class=\"col-md-6 col-sm-12\">");
                var popInBattle =
                    _site.Warfare.OfType<Battle>()
                        .Sum(
                            battle =>
                                battle.AttackerSquads.Sum(squad => squad.Deaths) +
                                battle.DefenderSquads.Sum(squad => squad.Deaths));
                Html.AppendLine("<b>Deaths</b> " + LineBreak);
                if (deathCount > 100)
                {
                    Html.AppendLine("<ul>")
                        .Append("<li>Historical figures died at this site: ").Append(deathCount).AppendLine();
                    if (popInBattle > 0)
                    {
                        Html.Append("<li>Population died in Battle: ").Append(popInBattle).AppendLine();
                    }
                    Html.AppendLine("</ul>");
                }
                else
                {
                    Html.AppendLine("<ol>");
                    foreach (HfDied death in _site.Events.OfType<HfDied>())
                    {
                        Html.Append("<li>").Append(death.HistoricalFigure.ToLink()).Append(", in ").Append(death.Year).Append(" (").Append(death.Cause.GetDescription()).AppendLine(")");
                    }
                    if (popInBattle > 0)
                    {
                        Html.Append("<li>Population in Battle: ").Append(popInBattle).AppendLine();
                    }
                    Html.AppendLine("</ol>");
                }
                Html.AppendLine("</div>");
            }
        }

        private void PrintBeastAttacks()
        {
            if (_site.BeastAttacks == null || _site.BeastAttacks.Count == 0)
            {
                return;
            }
            Html.AppendLine("<div class=\"col-md-6 col-sm-12\">")
                .AppendLine("<b>Beast Attacks</b>")
                .AppendLine("<ol>");
            foreach (BeastAttack attack in _site.BeastAttacks)
            {
                Html.Append("<li>").Append(attack.StartYear).Append(", ").AppendLine(attack.ToLink(true, _site));
                if (attack.GetSubEvents().OfType<HfDied>().Any())
                {
                    Html.Append(" (Deaths: ").Append(attack.GetSubEvents().OfType<HfDied>().Count()).Append(')');
                }
            }
            Html.AppendLine("</ol>")
                .AppendLine("</div>");
        }

        private string GetSiteMapPath()
        {
            if (string.IsNullOrEmpty(FileLoader.SaveDirectory) || string.IsNullOrEmpty(FileLoader.RegionId))
            {
                return null;
            }
            string sitemapPath = Path.Combine(FileLoader.SaveDirectory, FileLoader.RegionId + "-site_map-" + _site.Id);
            string sitemapPathFromProcessScript = Path.Combine(FileLoader.SaveDirectory, "site_maps\\" + FileLoader.RegionId + "-site_map-" + _site.Id);
            if (File.Exists(sitemapPath + ".bmp"))
            {
                return sitemapPath + ".bmp";
            }

            if (File.Exists(sitemapPath + ".png"))
            {
                return sitemapPath + ".png";
            }
            if (File.Exists(sitemapPath + ".jpg"))
            {
                return sitemapPath + ".jpg";
            }
            if (File.Exists(sitemapPath + ".jpeg"))
            {
                return sitemapPath + ".jpeg";
            }
            if (File.Exists(sitemapPathFromProcessScript + ".bmp"))
            {
                return sitemapPathFromProcessScript + ".bmp";
            }
            if (File.Exists(sitemapPathFromProcessScript + ".png"))
            {
                return sitemapPathFromProcessScript + ".png";
            }
            if (File.Exists(sitemapPathFromProcessScript + ".jpg"))
            {
                return sitemapPathFromProcessScript + ".jpg";
            }
            return File.Exists(sitemapPathFromProcessScript + ".jpeg") ? sitemapPathFromProcessScript + ".jpeg" : null;
        }

        private void PrintSiteMap(string siteMapPath)
        {
            if (string.IsNullOrEmpty(siteMapPath))
            {
                return;
            }
            CreateSitemapBitmap(siteMapPath);
        }

        private void CreateSitemapBitmap(string sitemapPath)
        {
            _site.SiteMapPath = sitemapPath;
            Bitmap sitemap = null;
            Bitmap map = null;
            using (FileStream mapStream = new FileStream(sitemapPath, FileMode.Open))
            {
                map = new Bitmap(mapStream);
            }
            if (map != null)
            {
                Formatting.ResizeImage(map, ref sitemap, 250, 250, true, true);
            }
            if (sitemap != null)
            {
                string htmlImage = BitmapToHtml(sitemap);
                string mapLink = MakeLink(htmlImage, LinkOption.LoadSiteMap);
                Html.Append("<td>").Append(mapLink).AppendLine("</td>");
            }
        }
    }
}

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
    class SitePrinter : HtmlPrinter
    {
        Site _site;
        World _world;

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
            Html.AppendLine("<div class=\"" + colClassMaps + "\">");
            PrintMaps(siteMapPath);
            Html.AppendLine("</div>");

            string colClassChart = string.IsNullOrEmpty(siteMapPath) ? "col-md-3 col-sm-12" : "col-lg-3 col-md-6 col-sm-6";
            Html.AppendLine("<div id=\"chart-populationbyrace-container\" class=\"" + colClassChart + "\" style=\"height: 250px\">");
            Html.AppendLine("<canvas id=\"chart-populationbyrace\"></canvas>");
            Html.AppendLine("</div>");

            Html.AppendLine("</div>");

            Html.AppendLine("<div class=\"row\">");
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
            Html.AppendLine("<div class=\"row\">");
            Html.AppendLine("<div class=\"col-md-12\">");
            Html.AppendLine(Bold("Related Historical Figures") + LineBreak);
            StartList(ListType.Unordered);
            foreach (HistoricalFigure hf in _site.RelatedHistoricalFigures)
            {
                SiteLink hfToSiteLink = hf.RelatedSites.FirstOrDefault(link => link.Site == _site);
                if (hfToSiteLink != null)
                {
                    Html.AppendLine(ListItem + hf.ToLink(true, _site));
                    if (hfToSiteLink.SubId != 0)
                    {
                        Structure structure = _site.Structures.FirstOrDefault(s => s.Id == hfToSiteLink.SubId);
                        if (structure != null)
                        {
                            Html.AppendLine(" - " + structure.ToLink(true, _site) + " - ");
                        }
                    }
                    if (hfToSiteLink.OccupationId != 0)
                    {
                        Structure structure = _site.Structures.FirstOrDefault(s => s.Id == hfToSiteLink.OccupationId);
                        if (structure != null)
                        {
                            Html.AppendLine(" - " + structure.ToLink(true, _site) + " - ");
                        }
                    }
                    Html.AppendLine(" (" + hfToSiteLink.Type.GetDescription() + ")");
                }
            }
            EndList(ListType.Unordered);
            Html.AppendLine("</div>");
            Html.AppendLine("</div>");
        }

        private void LoadCustomScripts()
        {
            Html.AppendLine("<script>");
            Html.AppendLine("window.onload = function(){");

            PopulatePopulationChartData(_site.Populations.Where(pop => pop.IsMainRace || pop.IsAnimalPeople).ToList());

            Html.AppendLine("}");
            Html.AppendLine("</script>");
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
            Html.AppendLine("<div class=\"row\">");
            Html.AppendLine("<div class=\"col-md-12\">");
            Html.AppendLine(Bold("Related Artifacts") + LineBreak);
            StartList(ListType.Unordered);
            foreach (Artifact artifact in relatedArtifacts)
            {
                Html.AppendLine(ListItem + artifact.ToLink(true, _site));
                if (!string.IsNullOrWhiteSpace(artifact.Type))
                {
                    Html.AppendLine(" a legendary " + artifact.Material + " ");
                    Html.AppendLine(!string.IsNullOrWhiteSpace(artifact.SubType) ? artifact.SubType : artifact.Type.ToLower());
                }
                List<string> relations = new List<string>();
                if (createdArtifacts.Contains(artifact))
                {
                    relations.Add("created");
                }
                if (storedArtifacts.Contains(artifact) && (artifact.Site == null || !artifact.Site.Equals(_site)))
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
                if (storedArtifacts.Contains(artifact) && artifact.Site != null && artifact.Site.Equals(_site))
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
                if (relations.Any())
                {
                    Html.AppendLine(" (" + string.Join(", ", relations) + ")");
                }
            }
            EndList(ListType.Unordered);
            Html.AppendLine("</div>");
            Html.AppendLine("</div>");
        }

        private void PrintConnections()
        {
            if (_site.Connections.Count > 0)
            {
                Html.AppendLine("<div class=\"row\">");
                Html.AppendLine("<div class=\"col-md-12\">");
                Html.AppendLine("<b>Connections</b></br>");
                Html.AppendLine("<ol>");
                foreach (Site connection in _site.Connections)
                {
                    Html.AppendLine("<li>" + connection.ToLink());
                }
                Html.AppendLine("</ol>");
                Html.AppendLine("</div>");
                Html.AppendLine("</div>");
            }
        }

        private void PrintOfficials()
        {
            if (_site.Officials.Count > 0)
            {
                Html.AppendLine("<div class=\"row\">");
                Html.AppendLine("<div class=\"col-md-12\">");
                Html.AppendLine("<b>Officials</b></br>");
                Html.AppendLine("<ol>");
                foreach (Site.Official official in _site.Officials)
                {
                    Html.AppendLine("<li>" + official.HistoricalFigure.ToLink() + ", " + official.Position);
                }
                Html.AppendLine("</ol>");
                Html.AppendLine("</div>");
                Html.AppendLine("</div>");
            }
        }

        private void PrintOwnerHistory()
        {
            if (_site.OwnerHistory.Count > 0)
            {
                Html.AppendLine("<div class=\"row\">");
                Html.AppendLine("<div class=\"col-md-12\">");
                Html.AppendLine("<b>Owner History</b><br />");
                Html.AppendLine("<ol>");
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
                    string startYear;
                    if (ownerPeriod.StartYear == -1)
                    {
                        startYear = "a time before time";
                    }
                    else
                    {
                        startYear = ownerPeriod.StartYear.ToString();
                    }

                    Html.Append("<li>" + ownerString + ", " + ownerPeriod.StartCause + " " + _site.ToLink(true, _site));

                    if (ownerPeriod.Founder != null && ownerPeriod.Owner != null)
                    {
                        Html.Append(" for " + ownerPeriod.Owner.PrintEntity());
                    }

                    Html.Append(" in " + startYear);

                    if (ownerPeriod.EndYear >= 0)
                    {
                        Html.Append(" and it was " + ownerPeriod.EndCause + " in " + ownerPeriod.EndYear);
                    }

                    if (ownerPeriod.Destroyer != null)
                    {
                        Html.Append(" by " + ownerPeriod.Destroyer.ToLink(true, _site));
                    }
                    else if (ownerPeriod.Ender != null)
                    {
                        Html.Append(" by " + ownerPeriod.Ender.PrintEntity());
                    }
                    Html.AppendLine(".");
                }
                Html.AppendLine("</ol>");
                Html.AppendLine("</div>");
                Html.AppendLine("</div>");
            }
        }

        private void PrintWarfareInfo()
        {
            if (_site.Warfare.Count(battle => !_world.FilterBattles || battle.Notable) > 0)
            {
                Html.AppendLine("<div class=\"row\">");
                Html.AppendLine("<div class=\"col-md-12\">");
                int warfareCount = 1;
                Html.AppendLine("<b>Warfare</b> ");
                if (_world.FilterBattles)
                {
                    Html.Append(" (Notable)");
                }

                Html.Append("<table border=\"0\">");
                foreach (EventCollection warfare in _site.Warfare.Where(battle => !_world.FilterBattles || battle.Notable))
                {
                    Html.AppendLine("<tr>");
                    Html.AppendLine("<td width=\"20\"  align=\"right\">" + warfareCount + ".</td><td width=\"10\"></td>");
                    Html.AppendLine("<td>" + warfare.StartYear + "</td>");
                    string warfareString = warfare.ToLink();
                    if (warfareString.Contains(" as a result of"))
                    {
                        warfareString = warfareString.Insert(warfareString.IndexOf(" as a result of"), "</br>");
                    }

                    Html.AppendLine("<td>" + warfareString + "</td>");
                    Html.AppendLine("<td>as part of</td>");
                    Html.AppendLine("<td>" + (warfare.ParentCollection == null ? "UNKNOWN" : warfare.ParentCollection.ToLink()) + "</td>");
                    Html.AppendLine("<td>by ");
                    if (warfare.GetType() == typeof(Battle))
                    {
                        Battle battle = warfare as Battle;
                        Html.Append(battle.Attacker?.PrintEntity() + "</td>");
                        if (battle.Victor == battle.Attacker)
                        {
                            Html.AppendLine("<td>(V)</td>");
                        }
                        else
                        {
                            Html.AppendLine("<td></td>");
                        }

                        Html.AppendLine("<td>(Deaths: " + (battle.AttackerDeathCount + battle.DefenderDeathCount) + ")</td>");
                    }
                    if (warfare.GetType() == typeof(SiteConquered))
                    {
                        Html.Append((warfare as SiteConquered).Attacker.PrintEntity() + "</td>");
                    }

                    Html.AppendLine("</tr>");
                    warfareCount++;
                }
                Html.AppendLine("</table></br>");

                if (_world.FilterBattles && _site.Warfare.Count(battle => !battle.Notable) > 0)
                {
                    Html.AppendLine("<b>Warfare</b> (Unnotable)</br>");
                    Html.AppendLine("<ul>");
                    Html.AppendLine("<li>Battles: " + _site.Warfare.OfType<Battle>().Where(battle => !battle.Notable).Count());
                    Html.AppendLine("<li>Pillagings: " + _site.Warfare.OfType<SiteConquered>().Where(conquering => conquering.ConquerType == SiteConqueredType.Pillaging).Count());
                    Html.AppendLine("</ul>");
                }

                Html.AppendLine("</div>");
                Html.AppendLine("</div>");
            }
        }

        private void PrintStructures()
        {
            if (_site.Structures.Any())
            {
                Html.AppendLine("<div class=\"row\">");
                Html.AppendLine("<div class=\"col-md-12\">");
                Html.AppendLine("<b>Structures</b><br/>");
                Html.AppendLine("<ul>");
                foreach (Structure structure in _site.Structures)
                {
                    Html.AppendLine("<li>" + structure.ToLink() + ", ");
                    Html.AppendLine(structure.TypeAsString);
                    Html.AppendLine("</li>");
                }
                Html.AppendLine("</ul>");
                Html.AppendLine("</div>");
                Html.AppendLine("</div>");
            }
        }

        private void PrintSiteProperties()
        {
            if (_site.SiteProperties.Any())
            {
                Html.AppendLine("<div class=\"row\">");
                Html.AppendLine("<div class=\"col-md-12\">");
                Html.AppendLine("<b>Site Properties</b><br/>");
                Html.AppendLine("<ul>");
                foreach (SiteProperty siteProperty in _site.SiteProperties)
                {
                    Html.AppendLine("<li>");
                    if (siteProperty.Structure != null)
                    {
                        Html.AppendLine(siteProperty.Structure.Type.GetDescription());
                        Html.AppendLine(" (" + siteProperty.Structure.ToLink() + ")");
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
                        Html.AppendLine(", ");
                        Html.AppendLine(siteProperty.Owner.ToLink());
                    }

                    Html.AppendLine("</li>");
                }
                Html.AppendLine("</ul>");
                Html.AppendLine("</div>");
                Html.AppendLine("</div>");
            }
        }

        private void PrintGeographyInfo()
        {
            if (_site.Region != null || !_site.Rectangle.IsEmpty)
            {
                Html.AppendLine("<div class=\"row\">");
                Html.AppendLine("<div class=\"col-md-12\">");
                Html.AppendLine("<b>Geography</b><br/>");
                Html.AppendLine("<ul>");
                if (_site.Region != null)
                {
                    Html.AppendLine("<li>Region: " + _site.Region.ToLink() + ", " + _site.Region.Type.GetDescription() + "</li>");
                }
                if (!_site.Rectangle.IsEmpty)
                {
                    Html.AppendLine("<li>Position: X " + _site.Rectangle.X + " Y " + _site.Rectangle.Y + "</li>");
                    if (_site.Rectangle.Width != 0 && _site.Rectangle.Height != 0)
                    {
                        Html.AppendLine("<li>Size: " + _site.Rectangle.Width + " x " + _site.Rectangle.Height + "</li>");
                    }
                }
                Html.AppendLine("</ul>");
                Html.AppendLine("</div>");
                Html.AppendLine("</div>");
            }
        }

        private void PrintMaps(string siteMapPath)
        {
            Html.AppendLine("<div class=\"row\">");
            Html.AppendLine("<div class=\"col-md-12\">");
            List<Bitmap> maps = MapPanel.CreateBitmaps(_world, _site);
            Html.AppendLine("<table>");
            Html.AppendLine("<tr>");
            PrintSiteMap(siteMapPath);
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[0]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("<td>" + MakeLink(BitmapToHtml(maps[1]), LinkOption.LoadMap) + "</td>");
            Html.AppendLine("</tr></table></br>");
            Html.AppendLine("</div>");
            Html.AppendLine("</div>");
        }

        private void PrintTitle()
        {
            Html.AppendLine("<div class=\"row\">");
            Html.AppendLine("<div class=\"col-md-12\">");
            if (!string.IsNullOrWhiteSpace(_site.Name))
            {
                Html.AppendLine("<h1>" + _site.GetIcon() + " " + _site.UntranslatedName + ", \"" + _site.Name + "\"</h1>");
                Html.AppendLine("<b>" + _site.ToLink(false) + " is a " + _site.Type + "</b><br /><br />");
            }
            else
            {
                Html.AppendLine("<h1>" + _site.Type + "</h1>");
            }
            Html.AppendLine("</div>");
            Html.AppendLine("</div>");
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
                    Html.AppendLine("<ul>");
                    Html.AppendLine("<li>Historical figures died at this site: " + deathCount);
                    if (popInBattle > 0)
                    {
                        Html.AppendLine("<li>Population died in Battle: " + popInBattle);
                    }
                    Html.AppendLine("</ul>");
                }
                else
                {
                    Html.AppendLine("<ol>");
                    foreach (HfDied death in _site.Events.OfType<HfDied>())
                    {
                        Html.AppendLine("<li>" + death.HistoricalFigure.ToLink() + ", in " + death.Year + " (" + death.Cause.GetDescription() + ")");
                    }
                    if (popInBattle > 0)
                    {
                        Html.AppendLine("<li>Population in Battle: " + popInBattle);
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
            Html.AppendLine("<div class=\"col-md-6 col-sm-12\">");
            Html.AppendLine("<b>Beast Attacks</b>");
            Html.AppendLine("<ol>");
            foreach (BeastAttack attack in _site.BeastAttacks)
            {
                Html.AppendLine("<li>" + attack.StartYear + ", " + attack.ToLink(true, _site));
                if (attack.GetSubEvents().OfType<HfDied>().Any())
                {
                    Html.Append(" (Deaths: " + attack.GetSubEvents().OfType<HfDied>().Count() + ")");
                }
            }
            Html.AppendLine("</ol>");
            Html.AppendLine("</div>");
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
            if (File.Exists(sitemapPathFromProcessScript + ".jpeg"))
            {
                return sitemapPathFromProcessScript + ".jpeg";
            }

            return null;
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
                Html.AppendLine("<td>" + mapLink + "</td>");
            }
        }
    }
}

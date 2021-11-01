using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using LegendsViewer.Controls.HTML.Utilities;
using LegendsViewer.Legends;
using LegendsViewer.Legends.Enums;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.HTML
{
    public class HistoricalFigureHtmlPrinter : HtmlPrinter
    {
        private readonly World _world;
        private readonly HistoricalFigure _historicalFigure;

        public HistoricalFigureHtmlPrinter(HistoricalFigure hf, World world)
        {
            _world = world;
            _historicalFigure = hf;
        }

        public override string Print()
        {
            Html = new StringBuilder();
            PrintTitle();
            PrintMiscInfo();
            PrintRelatedArtifacts();
            PrintIdentities();
            PrintBreedInfo();
            PrintSiteProperties();
            PrintFamilyGraph();
            PrintCurseLineage();
            PrintPositions();
            PrintRelatedHistoricalFigures();
            PrintRelationships();
            PrintVagueRelationships();
            PrintHonorEntity();
            PrintRelatedPopulation();
            PrintRelatedEntities();
            PrintReputations();
            PrintRelatedSites();
            PrintRelatedRegions();
            PrintDedicatedStructures();
            PrintSkills();
            PrintBattles();
            PrintKills();
            PrintBeastAttacks();
            PrintEventLog(_world, _historicalFigure.Events, HistoricalFigure.Filters, _historicalFigure);
            return Html.ToString();
        }

        private void PrintRelatedRegions()
        {
            if (_historicalFigure.RelatedRegions.Count == 0)
            {
                return;
            }
            Html.AppendLine(Bold("Related Regions") + LineBreak);
            StartList(ListType.Unordered);
            foreach (var region in _historicalFigure.RelatedRegions)
            {
                Html.AppendLine(ListItem + region.ToLink(true, _historicalFigure));
            }
            EndList(ListType.Unordered);
        }

        private void PrintRelatedArtifacts()
        {
            var createdArtifacts = _historicalFigure.Events.OfType<ArtifactCreated>().Where(e => e.HistoricalFigure == _historicalFigure && e.Artifact != null).Select(e => e.Artifact).ToList();
            var sanctifyArtifacts = _historicalFigure.Events.OfType<ArtifactCreated>().Where(e => e.SanctifyFigure == _historicalFigure && e.Artifact != null).Select(e => e.Artifact).ToList();
            var possessedArtifacts = _historicalFigure.Events.OfType<ArtifactPossessed>().Where(e => e.Artifact != null).Select(e => e.Artifact).ToList();
            var stolenArtifacts = _historicalFigure.Events.OfType<ItemStolen>().Where(e => e.Artifact != null).Select(e => e.Artifact).ToList();
            var storedArtifacts = _historicalFigure.Events.OfType<ArtifactStored>().Where(e => e.Artifact != null).Select(e => e.Artifact).ToList();
            var relatedArtifacts = createdArtifacts
                .Union(sanctifyArtifacts)
                .Union(possessedArtifacts)
                .Union(stolenArtifacts)
                .Union(storedArtifacts)
                .Union(_historicalFigure.HoldingArtifacts)
                .Distinct()
                .ToList();
            if (relatedArtifacts.Count == 0)
            {
                return;
            }
            Html.AppendLine(Bold("Related Artifacts") + LineBreak);
            StartList(ListType.Unordered);
            foreach (Artifact artifact in relatedArtifacts)
            {
                Html.AppendLine(ListItem + artifact.ToLink(true, _historicalFigure));
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
                if (sanctifyArtifacts.Contains(artifact))
                {
                    relations.Add("sanctified");
                }
                if (possessedArtifacts.Contains(artifact))
                {
                    relations.Add("possessed");
                }
                if (stolenArtifacts.Contains(artifact))
                {
                    relations.Add("stolen");
                }
                if (storedArtifacts.Contains(artifact))
                {
                    relations.Add("stored");
                }
                if (_historicalFigure.HoldingArtifacts.Contains(artifact))
                {
                    relations.Add("currently in possession");
                }
                if (artifact.Holder != _historicalFigure)
                {
                    if (artifact.Holder != null)
                    {
                        relations.Add("currently in possession of " + artifact.Holder.ToLink(true, _historicalFigure));
                    }
                    else if (artifact.Site != null)
                    {
                        relations.Add("currently stored in " + artifact.Site.ToLink(true, _historicalFigure));
                    }
                }
                if (relations.Any())
                {
                    Html.AppendLine(" (" + string.Join(", ", relations) + ")");
                }
            }
            EndList(ListType.Unordered);
        }

        private void PrintDedicatedStructures()
        {
            if (_historicalFigure.DedicatedStructures.Count == 0)
            {
                return;
            }
            Html.AppendLine(Bold("Dedicated Structures") + LineBreak);
            StartList(ListType.Unordered);
            foreach (Structure structure in _historicalFigure.DedicatedStructures)
            {
                Html.AppendLine(ListItem + structure.ToLink(true, _historicalFigure) + " in " + structure.Site.ToLink(true, _historicalFigure));
                if (structure.Religion != null)
                {
                    Html.AppendLine(" origin of " + structure.Religion.ToLink(true, _historicalFigure));
                }
            }
            EndList(ListType.Unordered);
        }

        private void PrintRelatedPopulation()
        {
            if (_historicalFigure.EntityPopulation != null)
            {
                Html.AppendLine(Bold("Related Population ") + LineBreak);

                Html.AppendLine("<ul>");
                Html.AppendLine("<li>");
                Html.AppendLine(_historicalFigure.EntityPopulation.Entity.ToLink());
                Html.AppendLine(" (" + _historicalFigure.EntityPopulation.Race.NamePlural + ")");
                Html.AppendLine("</li>");
                Html.AppendLine("</ul>");
            }
        }

        private void PrintFamilyGraph()
        {
            if (_historicalFigure.RelatedHistoricalFigures.Any(rel => rel.Type == HistoricalFigureLinkType.Mother ||
                                                                     rel.Type == HistoricalFigureLinkType.Father ||
                                                                     rel.Type == HistoricalFigureLinkType.Child))
            {
                string nodes = CreateNode(_historicalFigure);
                string edges = "";
                int mothertreesize = 0;
                int fathertreesize = 0;
                GetFamilyDataParents(_historicalFigure, ref nodes, ref edges, ref mothertreesize, ref fathertreesize);
                GetFamilyDataChildren(_historicalFigure, ref nodes, ref edges);

                Html.AppendLine(Bold("Family Tree") + LineBreak);
                Html.AppendLine("<div id=\"familygraph\" class=\"legends_graph\"></div>");
                Html.AppendLine("<script type=\"text/javascript\" src=\"" + LocalFileProvider.LocalPrefix +
                                "WebContent/scripts/cytoscape.min.js\"></script>");
                Html.AppendLine("<script type=\"text/javascript\" src=\"" + LocalFileProvider.LocalPrefix +
                                "WebContent/scripts/cytoscape-dagre.js\"></script>");
                Html.AppendLine("<script>");
                Html.AppendLine("window.familygraph_nodes = [");
                Html.AppendLine(nodes);
                Html.AppendLine("]");
                Html.AppendLine("window.familygraph_edges = [");
                Html.AppendLine(edges);
                Html.AppendLine("]");
                Html.AppendLine("</script>");
                Html.AppendLine("<script type=\"text/javascript\" src=\"" + LocalFileProvider.LocalPrefix +
                                "WebContent/scripts/familygraph.js\"></script>");
            }
        }

        private string CreateNode(HistoricalFigure hf)
        {
            string classes = hf.Equals(_historicalFigure) ? " current" : "";

            string title = "";
            if (hf.Positions.Any())
            {
                title += hf.GetLastNoblePosition();
                title += "\\n--------------------\\n";
                classes += " leader";
            }
            title += hf.Race != null && hf.Race != _historicalFigure.Race ? hf.Race.NameSingular + " " : "";

            string description = "";
            if (hf.ActiveInteractions.Any(it => it.Contains("VAMPIRE")))
            {
                description += "Vampire ";
                classes += " vampire";
            }
            if (hf.ActiveInteractions.Any(it => it.Contains("WEREBEAST")))
            {
                description += "Werebeast ";
                classes += " werebeast";
            }
            if (hf.ActiveInteractions.Any(it => it.Contains("SECRET") && !it.Contains("ANIMATE") && !it.Contains("UNDEAD_RES")))
            {
                description += "Necromancer ";
                classes += " necromancer";
            }
            if (hf.Ghost)
            {
                description += "Ghost ";
                classes += " ghost";
            }
            description += !string.IsNullOrWhiteSpace(hf.AssociatedType) && hf.AssociatedType != "Standard" ? hf.AssociatedType : "";
            if (!string.IsNullOrWhiteSpace(description))
            {
                description += "\\n--------------------\\n";
            }
            title += description;
            title += hf.Name;
            if (!hf.Alive)
            {
                title += "\\n✝";
                classes += " dead";
            }
            string node = "{ data: { id: '" + hf.Id + "', name: '" + WebUtility.HtmlEncode(title) + "', href: 'hf#" + hf.Id + "' , faveColor: '" + (hf.Caste == "Male" ? "#6FB1FC" : "#EDA1ED") + "' }, classes: '" + classes + "' },";
            return node;
        }

        private void GetFamilyDataChildren(HistoricalFigure hf, ref string nodes, ref string edges)
        {
            foreach (HistoricalFigure child in hf.RelatedHistoricalFigures.Where(rel => rel.Type == HistoricalFigureLinkType.Child).Select(rel => rel.HistoricalFigure))
            {
                string node = CreateNode(child);
                if (!nodes.Contains(node))
                {
                    nodes += node;
                }
                string edge = "{ data: { source: '" + hf.Id + "', target: '" + child.Id + "' } },";
                if (!edges.Contains(edge))
                {
                    edges += edge;
                }
            }
        }

        private void GetFamilyDataParents(HistoricalFigure hf, ref string nodes, ref string edges, ref int mothertreesize, ref int fathertreesize)
        {
            foreach (HistoricalFigure mother in hf.RelatedHistoricalFigures.Where(rel => rel.Type == HistoricalFigureLinkType.Mother).Select(rel => rel.HistoricalFigure))
            {
                mothertreesize++;
                string node = CreateNode(mother);
                if (!nodes.Contains(node))
                {
                    nodes += node;
                }
                string edge = "{ data: { source: '" + mother.Id + "', target: '" + hf.Id + "' } },";
                if (!edges.Contains(edge))
                {
                    edges += edge;
                }
                if (mothertreesize < 3)
                {
                    GetFamilyDataParents(mother, ref nodes, ref edges, ref mothertreesize, ref fathertreesize);
                }
                mothertreesize--;
            }
            foreach (HistoricalFigure father in hf.RelatedHistoricalFigures.Where(rel => rel.Type == HistoricalFigureLinkType.Father).Select(rel => rel.HistoricalFigure))
            {
                fathertreesize++;
                string node = CreateNode(father);
                if (!nodes.Contains(node))
                {
                    nodes += node;
                }
                string edge = "{ data: { source: '" + father.Id + "', target: '" + hf.Id + "' } },";
                if (!edges.Contains(edge))
                {
                    edges += edge;
                }
                if (fathertreesize < 3)
                {
                    GetFamilyDataParents(father, ref nodes, ref edges, ref mothertreesize, ref fathertreesize);
                }
                fathertreesize--;
            }
        }

        private void PrintCurseLineage()
        {
            if (_historicalFigure.ActiveInteractions.Any(interaction => interaction.Contains("CURSE")))
            {
                HistoricalFigure curser = _historicalFigure;
                while (curser.LineageCurseParent != null && !curser.LineageCurseParent.Deity)
                {
                    curser = curser.LineageCurseParent;
                }
                string curse = "Curse";
                if (!string.IsNullOrWhiteSpace(_historicalFigure.Interaction))
                {
                    curse = Formatting.InitCaps(_historicalFigure.Interaction);
                }
                Html.AppendLine(Bold(curse + " Lineage") + LineBreak);
                Html.AppendLine("<div class=\"tree\">");
                Html.AppendLine("<ul>");
                Html.AppendLine("<li>");
                Html.AppendLine(curser.LineageCurseParent != null ? curser.LineageCurseParent.ToTreeLeafLink(_historicalFigure) : "<a>UNKNOWN DEITY</a>");
                Html.AppendLine("<ul>");
                PrintLineageTreeLevel(curser);
                Html.AppendLine("</ul>");
                Html.AppendLine("</li>");
                Html.AppendLine("</ul>");
                Html.AppendLine("</div>");
                Html.AppendLine("</br>");
                Html.AppendLine("</br>");
            }
        }

        private void PrintLineageTreeLevel(HistoricalFigure curseBearer)
        {
            Html.AppendLine("<li>");
            Html.AppendLine(curseBearer.ToTreeLeafLink(_historicalFigure));
            if (curseBearer.LineageCurseChilds.Any())
            {
                Html.AppendLine("<ul>");
                foreach (HistoricalFigure curseChild in curseBearer.LineageCurseChilds)
                {
                    PrintLineageTreeLevel(curseChild);
                }
                Html.AppendLine("</ul>");
            }
            Html.AppendLine("</li>");
        }

        private void PrintBreedInfo()
        {
            if (!string.IsNullOrWhiteSpace(_historicalFigure.BreedId))
            {
                Html.AppendLine(Bold("Breed") + LineBreak);
                Html.AppendLine("<ol>");
                foreach (HistoricalFigure hfOfSameBreed in _world.Breeds[_historicalFigure.BreedId])
                {
                    Html.AppendLine("<li>" + hfOfSameBreed.ToLink() + "</li>");
                }
                Html.AppendLine("</ol>");
            }
        }

        private void PrintIdentities()
        {
            if (_historicalFigure.Identities.Any())
            {
                Html.AppendLine(Bold("Secret Identities") + LineBreak);
                Html.AppendLine("<ul>");
                foreach (Identity identity in _historicalFigure.Identities)
                {
                    Html.AppendLine("<li>" + identity.Print() + "</li>");
                }
                Html.AppendLine("</ul>");
            }
        }

        private void PrintSiteProperties()
        {
            if (_historicalFigure.SiteProperties.Any())
            {
                Html.AppendLine(Bold("Site Properties") + LineBreak);
                Html.AppendLine("<ul>");
                foreach (SiteProperty siteProperty in _historicalFigure.SiteProperties)
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
                    if (siteProperty.Site != null)
                    {
                        Html.AppendLine(" in ");
                        Html.AppendLine(siteProperty.Site.ToLink());
                    }

                    Html.AppendLine("</li>");
                }
                Html.AppendLine("</ul>");
            }
        }

        public override string GetTitle()
        {
            return _historicalFigure.Name;
        }

        private void PrintTitle()
        {
            Html.AppendLine("<h1>" + _historicalFigure.GetIcon() + " " + _historicalFigure.Name + "</h1>");
            string title = string.Empty;
            if (_historicalFigure.Deity)
            {
                title = "Is a deity";
                if (_historicalFigure.WorshippedBy != null)
                {
                    title += " that occurs in the myths of " + _historicalFigure.WorshippedBy.ToLink() + ". ";
                }
                else
                {
                    title += ". ";
                }

                title += _historicalFigure.ToLink(false, _historicalFigure) + " is most often depicted as " + _historicalFigure.GetRaceTitleString() + ". ";
            }
            else if (_historicalFigure.Force)
            {
                title = "Is a force said to permeate nature. ";
                if (_historicalFigure.WorshippedBy != null)
                {
                    title += "Worshiped by " + _historicalFigure.WorshippedBy.ToLink();
                }
            }
            else
            {
                if (_historicalFigure.DeathYear >= 0)
                {
                    title += "Was " + _historicalFigure.GetRaceTitleString();
                }
                else
                {
                    title += "Is " + _historicalFigure.GetRaceTitleString();
                }
                title += " born in " + _historicalFigure.BirthYear;

                if (_historicalFigure.DeathYear > 0)
                {
                    HfDied death = _historicalFigure.Events.OfType<HfDied>().First(hfDeath => hfDeath.HistoricalFigure == _historicalFigure);
                    title += " and died in " + _historicalFigure.DeathYear + " (" + death.Cause.GetDescription() + ")";
                    if (death.Slayer != null)
                    {
                        title += " by " + death.Slayer.ToLink();
                    }
                    else if (death.SlayerRace != "UNKNOWN" && death.SlayerRace != "-1")
                    {
                        title += " by a " + death.SlayerRace.ToLower();
                    }

                    if (death.ParentCollection != null)
                    {
                        title += ", " + death.PrintParentCollection();
                    }
                }
                if (!title.EndsWith(". "))
                {
                    title += ". ";
                }
            }
            Html.AppendLine("<b>" + title + "</b></br>");
            if (!string.IsNullOrWhiteSpace(_historicalFigure.Caste) && _historicalFigure.Caste != "Default")
            {
                Html.AppendLine("<b>Caste:</b> " + _historicalFigure.Caste + "</br>");
            }
            if (!string.IsNullOrWhiteSpace(_historicalFigure.AssociatedType) && _historicalFigure.AssociatedType != "Standard")
            {
                Html.AppendLine("<b>Type:</b> " + _historicalFigure.AssociatedType + "</br>");
            }
        }

        private void PrintMiscInfo()
        {
            // The identities do not make sense (demon disguised as a hydra etc.)
            //if (HistoricalFigure.CurrentIdentity != null)
            //{
            //    HTML.AppendLine(Bold("Current Identity: ") + HistoricalFigure.CurrentIdentity.ToLink() + LineBreak);
            //}
            //if (HistoricalFigure.UsedIdentity != null)
            //{
            //    HTML.AppendLine(Bold("Used Identity: ") + HistoricalFigure.UsedIdentity.ToLink() + LineBreak);
            //}
            if (_historicalFigure.Spheres.Count > 0)
            {
                string spheres = "";
                foreach (string sphere in _historicalFigure.Spheres)
                {
                    if (_historicalFigure.Spheres.Last() == sphere && _historicalFigure.Spheres.Count > 1)
                    {
                        spheres += " and ";
                    }
                    else if (spheres.Length > 0)
                    {
                        spheres += ", ";
                    }

                    spheres += sphere;
                }
                Html.Append(Bold("Associated Spheres: ") + spheres + LineBreak);
            }
            if (_historicalFigure.Goal != "")
            {
                Html.AppendLine(Bold("Goal: ") + _historicalFigure.Goal + LineBreak);
            }

            if (_historicalFigure.ActiveInteractions.Count > 0)
            {
                string interactions = "";
                foreach (string interaction in _historicalFigure.ActiveInteractions)
                {
                    if (_historicalFigure.ActiveInteractions.Last() == interaction && _historicalFigure.ActiveInteractions.Count > 1)
                    {
                        interactions += " and ";
                    }
                    else if (interactions.Length > 0)
                    {
                        interactions += ", ";
                    }

                    interactions += interaction;
                }
                Html.AppendLine(Bold("Active Interactions: ") + interactions + LineBreak);
            }
            if (_historicalFigure.InteractionKnowledge.Count > 0)
            {
                string interactions = "";
                foreach (string interaction in _historicalFigure.InteractionKnowledge)
                {
                    if (_historicalFigure.InteractionKnowledge.Last() == interaction && _historicalFigure.InteractionKnowledge.Count > 1)
                    {
                        interactions += " and ";
                    }
                    else if (interactions.Length > 0)
                    {
                        interactions += ", ";
                    }

                    interactions += interaction;
                }
                Html.AppendLine(Bold("Interaction Knowledge: ") + interactions + LineBreak);
            }
            if (_historicalFigure.Animated)
            {
                Html.AppendLine(Bold("Animated as: ") + _historicalFigure.AnimatedType + LineBreak);
            }

            if (_historicalFigure.JourneyPets.Count > 0)
            {
                string pets = "";
                foreach (string pet in _historicalFigure.JourneyPets)
                {
                    if (_historicalFigure.JourneyPets.Last() == pet && _historicalFigure.JourneyPets.Count > 1)
                    {
                        pets += " and ";
                    }
                    else if (pets.Length > 0)
                    {
                        pets += ", ";
                    }

                    pets += pet;
                }
                Html.AppendLine(Bold("Journey Pets: ") + pets + LineBreak);
            }
            Html.AppendLine(LineBreak);
        }

        private void PrintPositions()
        {
            if (_historicalFigure.Positions.Count > 0)
            {
                Html.AppendLine(Bold("Positions") + LineBreak);
                StartList(ListType.Ordered);
                foreach (HistoricalFigure.Position hfposition in _historicalFigure.Positions)
                {
                    Html.AppendLine("<li>");
                    EntityPosition position = hfposition.Entity.EntityPositions.FirstOrDefault(pos => pos.Name.ToLower() == hfposition.Title.ToLower());
                    if (position != null)
                    {
                        string positionName = position.GetTitleByCaste(_historicalFigure.Caste);
                        Html.Append(positionName);
                    }
                    else
                    {
                        Html.Append(hfposition.Title + " of " + hfposition.Entity.PrintEntity() + " (" + hfposition.Began + " - ");
                    }
                    string end = hfposition.Ended == -1 ? "Present" : hfposition.Ended.ToString();
                    Html.Append(" of " + hfposition.Entity.PrintEntity() + " (" + hfposition.Began + " - " + end + ")");
                }
                EndList(ListType.Ordered);
            }
        }

        private void PrintRelatedHistoricalFigures()
        {
            PrintRelatedHFs("Worshiped Deities", _historicalFigure.RelatedHistoricalFigures.Where(hf => hf.Type == HistoricalFigureLinkType.Deity).OrderByDescending(hfl => hfl.Strength).ToList());
            PrintRelatedHFs("Related Historical Figures", _historicalFigure.RelatedHistoricalFigures.Where(hf => hf.Type != HistoricalFigureLinkType.Deity).ToList());
        }

        private void PrintRelatedHFs(string title, List<HistoricalFigureLink> relations)
        {
            if (relations.Any())
            {
                Html.AppendLine(Bold(title) + LineBreak);
                Html.AppendLine("<ul>");
                foreach (HistoricalFigureLink relation in relations)
                {
                    string hf = "UNKNOWN HISTORICAL FIGURE";
                    if (relation.HistoricalFigure != null)
                    {
                        hf = relation.HistoricalFigure.ToLink();
                    }

                    string relationString = hf + ", " + relation.Type.GetDescription();
                    if (relation.Type == HistoricalFigureLinkType.Deity)
                    {
                        relationString += " (" + relation.Strength + "%)";
                    }

                    Html.AppendLine("<li>" + relationString + "</li>");
                }
                Html.AppendLine("</ul>");
            }
        }

        private void PrintRelatedEntities()
        {
            if (_historicalFigure.RelatedEntities.Count > 0)
            {
                Html.AppendLine(Bold("Related Entities") + LineBreak);
                StartList(ListType.Unordered);
                foreach (EntityLink link in _historicalFigure.RelatedEntities)
                {
                    string linkString = link.Entity.PrintEntity() + " (" + link.Type.GetDescription();
                    if (link.Strength > 0)
                    {
                        linkString += " " + link.Strength + "%";
                    }

                    if (link.StartYear > -1)
                    {
                        linkString += " ";
                        var hfposition = _historicalFigure.Positions.FirstOrDefault(hfpos => hfpos.Began == link.StartYear && hfpos.Ended == link.EndYear);
                        if (hfposition != null)
                        {
                            EntityPosition position = link.Entity.EntityPositions.FirstOrDefault(pos => pos.Name == hfposition.Title);
                            if (position != null)
                            {
                                string positionName = position.GetTitleByCaste(_historicalFigure.Caste);
                                linkString += positionName;
                            }
                            else
                            {
                                linkString += hfposition.Title;
                            }
                        }
                        else
                        {
                            linkString += "Noble";
                        }
                        linkString += ", " + link.StartYear + "-";
                        if (link.EndYear > -1)
                        {
                            linkString += link.EndYear;
                        }
                        else
                        {
                            linkString += "Present";
                        }
                    }
                    linkString += ")";
                    Html.AppendLine(ListItem + linkString);
                }
                EndList(ListType.Unordered);
            }
        }

        private void PrintRelatedSites()
        {
            if (_historicalFigure.RelatedSites.Count > 0)
            {
                Html.AppendLine(Bold("Related Sites") + LineBreak);
                Html.AppendLine("<ul>");
                foreach (SiteLink hfToSiteLink in _historicalFigure.RelatedSites)
                {
                    Html.AppendLine("<li>");
                    Html.AppendLine(hfToSiteLink.Site.ToLink(true, _historicalFigure));
                    if (hfToSiteLink.SubId != 0)
                    {
                        Structure structure = hfToSiteLink.Site.Structures.FirstOrDefault(s => s.Id == hfToSiteLink.SubId);
                        if (structure != null)
                        {
                            Html.AppendLine(" - " + structure.ToLink(true, _historicalFigure) + " - ");
                        }
                    }
                    if (hfToSiteLink.OccupationId != 0)
                    {
                        Structure structure = hfToSiteLink.Site.Structures.FirstOrDefault(s => s.Id == hfToSiteLink.OccupationId);
                        if (structure != null)
                        {
                            Html.AppendLine(" - " + structure.ToLink(true, _historicalFigure) + " - ");
                        }
                    }
                    Html.AppendLine(" (" + hfToSiteLink.Type.GetDescription() + ")");
                }
                Html.AppendLine("</ul>");
            }
        }

        private void PrintRelationships()
        {
            if (_historicalFigure.RelationshipProfiles.Count > 0)
            {
                Html.AppendLine(Bold("Relationships") + LineBreak);
                Html.AppendLine("<ol>");
                foreach (var relationshipProfile in _historicalFigure.RelationshipProfiles.OrderByDescending(profile => profile.Reputations.OrderBy(rep => rep.Strength).FirstOrDefault()?.Strength))
                {
                    HistoricalFigure hf = _world.GetHistoricalFigure(relationshipProfile.HistoricalFigureId);
                    if (hf != null)
                    {
                        Html.AppendLine("<li>");
                        Html.AppendLine(hf.ToLink());
                        if (relationshipProfile.Type != RelationShipProfileType.Unknown)
                        {
                            Html.Append(" (" + relationshipProfile.Type.GetDescription() + ")");
                        }
                        foreach (var reputation in relationshipProfile.Reputations)
                        {
                            if (reputation.Strength != 0)
                            {
                                Html.Append(", " + reputation.Print() + " ");
                            }
                        }
                        Html.AppendLine("</li>");
                    }
                }
                Html.AppendLine("</ol>");
            }
        }

        private void PrintVagueRelationships()
        {
            if (_historicalFigure.VagueRelationships.Count > 0)
            {
                Html.AppendLine(Bold("Vague Relationships") + LineBreak);
                Html.AppendLine("<ul>");
                foreach (var vagueRelationship in _historicalFigure.VagueRelationships)
                {
                    HistoricalFigure hf = _world.GetHistoricalFigure(vagueRelationship.HfId);
                    if (hf != null)
                    {
                        Html.AppendLine("<li>");
                        Html.AppendLine(hf.ToLink());
                        if (vagueRelationship.Type != VagueRelationshipType.Unknown)
                        {
                            Html.Append(" (" + vagueRelationship.Type.GetDescription() + ")");
                        }
                        Html.AppendLine("</li>");
                    }
                }
                Html.AppendLine("</ul>");
            }
        }

        private void PrintHonorEntity()
        {
            if (_historicalFigure.HonorEntity != null)
            {
                Html.AppendLine(Bold("Honors") + LineBreak);
                Html.AppendLine("<ul>");
                Html.AppendLine(_historicalFigure.HonorEntity.Entity.ToLink() + LineBreak);
                foreach (var honor in _historicalFigure.HonorEntity.Honors)
                {
                    Html.AppendLine("<li>");
                    Html.AppendLine(honor.Print());
                    Html.AppendLine("</li>");
                }
                Html.AppendLine("</ul>");
            }
        }

        private void PrintReputations()
        {
            if (_historicalFigure.Reputations.Count > 0)
            {
                Html.AppendLine(Bold("Entity Reputations") + LineBreak);
                StartList(ListType.Unordered);
                foreach (EntityReputation reputation in _historicalFigure.Reputations)
                {
                    Html.AppendLine(ListItem + reputation.Entity.PrintEntity() + ": ");
                    StartList(ListType.Unordered);
                    if (reputation.UnsolvedMurders > 0)
                    {
                        Html.AppendLine(ListItem + "Unsolved Murders: " + reputation.UnsolvedMurders);
                    }

                    if (reputation.FirstSuspectedAgelessYear > 0)
                    {
                        Html.AppendLine(ListItem + "First Suspected Ageless Year: " + reputation.FirstSuspectedAgelessYear + ", " + reputation.FirstSuspectedAgelessSeason);
                    }

                    foreach (var item in reputation.Reputations)
                    {
                        Html.AppendLine(ListItem + item.Key.GetDescription() + ": " + item.Value + "%");
                    }
                    EndList(ListType.Unordered);
                }
                EndList(ListType.Unordered);
            }
        }

        private void PrintSkills()
        {
            if (_historicalFigure.Skills.Count > 0)
            {
                var described = _historicalFigure.Skills.ConvertAll(SkillDictionary.LookupSkill);

                Html.AppendLine(Bold("Skills") + LineBreak);

                foreach (var group in described.Where(d => d.Category != "non").GroupBy(d => d.Category).OrderByDescending(g => g.Count()))
                {
                    Html.AppendLine("<ol class='skills'>");

                    foreach (var desc in group.OrderByDescending(d => d.Points))
                    {
                        Html.AppendLine(SkillToString(desc));
                    }

                    Html.AppendLine("</ol>");
                }

                Html.AppendLine(LineBreak);
            }
        }

        private void PrintBattles()
        {
            Battle unnotableDeathBattle = null; //Temporarily make the battle that the HF died in Notable so it shows up.
            if (_historicalFigure.Battles.Count > 0 && _historicalFigure.Battles.Last().Collection.OfType<HfDied>().Count(death => death.HistoricalFigure == _historicalFigure) == 1 && !_historicalFigure.Battles.Last().Notable)
            {
                unnotableDeathBattle = _historicalFigure.Battles.Last();
                unnotableDeathBattle.Notable = true;
            }

            if (_historicalFigure.Battles.Count(battle => !_world.FilterBattles || battle.Notable) > 0)
            {
                Html.AppendLine(Bold("Battles"));
                if (_world.FilterBattles)
                {
                    Html.Append(" (Notable)");
                }

                Html.Append(LineBreak);
                TableMaker battleTable = new TableMaker(true);
                foreach (Battle battle in _historicalFigure.Battles.Where(battle => !_world.FilterBattles || battle.Notable || battle.Collection.OfType<HfDied>().Count(death => death.HistoricalFigure == _historicalFigure) > 0))
                {
                    battleTable.StartRow();
                    battleTable.AddData(battle.StartYear.ToString());
                    battleTable.AddData(battle.ToLink());
                    if (battle.ParentCollection != null)
                    {
                        battleTable.AddData("as part of");
                        battleTable.AddData(battle.ParentCollection.ToLink());
                    }
                    string involvement = "";
                    if (battle.NotableAttackers.Count > 0 && battle.NotableAttackers.Contains(_historicalFigure))
                    {
                        if (battle.Collection.OfType<FieldBattle>().Any(fieldBattle => fieldBattle.AttackerGeneral == _historicalFigure) ||
                            battle.Collection.OfType<AttackedSite>().Any(attack => attack.AttackerGeneral == _historicalFigure))
                        {
                            involvement += "Led the attack";
                        }
                        else
                        {
                            involvement += "Fought in the attack";
                        }
                    }
                    else if (battle.NotableDefenders.Count > 0 && battle.NotableDefenders.Contains(_historicalFigure))
                    {
                        if (battle.Collection.OfType<FieldBattle>().Any(fieldBattle => fieldBattle.DefenderGeneral == _historicalFigure) ||
                            battle.Collection.OfType<AttackedSite>().Any(attack => attack.DefenderGeneral == _historicalFigure))
                        {
                            involvement += "Led the defense";
                        }
                        else
                        {
                            involvement += "Aided in the defense";
                        }
                    }
                    else
                    {
                        involvement += "A non combatant";
                    }

                    if (battle.GetSubEvents().OfType<HfDied>().Any(death => death.HistoricalFigure == _historicalFigure))
                    {
                        involvement += " and died";
                    }

                    battleTable.AddData(involvement);
                    if (battle.NotableAttackers.Contains(_historicalFigure))
                    {
                        battleTable.AddData("against");
                        battleTable.AddData(battle.Defender?.PrintEntity() ?? " an unknown civilization ");
                        if (battle.Victor == battle.Attacker)
                        {
                            battleTable.AddData("and won");
                        }
                        else
                        {
                            battleTable.AddData("and lost");
                        }
                    }
                    else if (battle.NotableDefenders.Contains(_historicalFigure))
                    {
                        battleTable.AddData("against");
                        battleTable.AddData(battle.Attacker?.PrintEntity() ?? " an unknown civilization ");
                        if (battle.Victor == battle.Defender)
                        {
                            battleTable.AddData("and won");
                        }
                        else
                        {
                            battleTable.AddData("and lost");
                        }
                    }

                    battleTable.AddData("Deaths: " + (battle.AttackerDeathCount + battle.DefenderDeathCount) + ")");

                    battleTable.EndRow();
                }
                Html.AppendLine(battleTable.GetTable() + LineBreak);
            }

            if (_world.FilterBattles && _historicalFigure.Battles.Count(battle => !battle.Notable) > 0)
            {
                Html.AppendLine(Bold("Battles") + " (Unnotable): " + _historicalFigure.Battles.Where(battle => !battle.Notable).Count() + LineBreak + LineBreak);
            }

            if (unnotableDeathBattle != null)
            {
                unnotableDeathBattle.Notable = false;
            }
        }

        private void PrintKills()
        {
            if (_historicalFigure.NotableKills.Count > 0)
            {
                Html.AppendLine(Bold("Kills"));
                StartList(ListType.Ordered);
                if (_historicalFigure.NotableKills.Count > 100)
                {
                    Html.AppendLine("<li>" + _historicalFigure.NotableKills.Count + " notable kills</li>");
                }
                else
                {
                    foreach (HfDied kill in _historicalFigure.NotableKills)
                    {
                        Html.AppendLine("<li>" + kill.HistoricalFigure.ToLink() + ", in " + kill.Year + " (" + kill.Cause.GetDescription() + ")</li>");
                    }
                }
                EndList(ListType.Ordered);
            }
        }

        private void PrintBeastAttacks()
        {
            if (_historicalFigure.BeastAttacks != null && _historicalFigure.BeastAttacks.Count > 0)
            {
                Html.AppendLine(Bold("Beast Attacks"));
                StartList(ListType.Ordered);
                foreach (BeastAttack attack in _historicalFigure.BeastAttacks)
                {
                    Html.AppendLine(ListItem + attack.StartYear + ", " + MakeLink(Formatting.AddOrdinal(attack.Ordinal) + " rampage in ", attack) + attack.Site.ToLink());
                    if (attack.GetSubEvents().OfType<HfDied>().Any())
                    {
                        Html.Append(" (Kills: " + attack.GetSubEvents().OfType<HfDied>().Count() + ")");
                    }
                }
                EndList(ListType.Ordered);
                Html.AppendLine(LineBreak);
            }
        }
    }
}

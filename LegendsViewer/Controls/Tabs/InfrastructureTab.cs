using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BrightIdeasSoftware;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.Tabs
{
    public partial class InfrastructureTab : BaseSearchTab
    {
        private WorldConstructionsList _worldConstructionSearch;
        private StructuresList _structureSearch;
        private SitesList _siteSearch;

        public InfrastructureTab()
        {
            InitializeComponent();
        }


        internal override void InitializeTab()
        {
            lnkMaxResults.Text = WorldObjectList.MaxResults.ToString();
            MaxResultsLabels.Add(lnkMaxResults);

            EventTabs = new[] { tpSiteEvents, tpStructureEvents, tpWorldConstructionEvents };
            EventTabTypes = new[] { typeof(Site), typeof(Structure), typeof(WorldConstruction) };

            listSiteSearch.ShowGroups = false;
            listSiteSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Structures",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = item => ((Site)item).Structures.Count
            });
            listSiteSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Warfare",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = item => ((Site)item).Warfare.Count
            });
            listSiteSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Battles",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = item => ((Site)item).Battles.Count
            });
            listSiteSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Conquerings",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = item => ((Site)item).Conquerings.Count
            });
            listSiteSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Current Owner",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = item => ((Site)item).CurrentOwner?.ToLink(false)
            });
            listSiteSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Deaths",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = item => ((Site)item).Deaths.Count
            });
            listSiteSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Beast Attacks",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = item => ((Site)item).BeastAttacks.Count
            });
            listSiteSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Events",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = rowObject => ((Site)rowObject).Events.Count
            });

            listStructureSearch.ShowGroups = false;
            listWorldConstructionsSearch.ShowGroups = false;
        }

        internal override void AfterLoad(World world)
        {
            base.AfterLoad(world);

            _worldConstructionSearch = new WorldConstructionsList(World);
            _structureSearch = new StructuresList(World);

            var structures = from structure in World.Structures
                             orderby structure.TypeAsString
                             group structure by structure.TypeAsString into structuretype
                             select structuretype;
            var worldconstructions = from construction in World.WorldConstructions
                                     orderby construction.Type.GetDescription()
                                     group construction by construction.Type.GetDescription() into constructiontype
                                     select constructiontype;

            cmbStructureType.Items.Add("All"); cmbStructureType.SelectedIndex = 0;
            foreach (var structure in structures)
            {
                cmbStructureType.Items.Add(structure.Key);
            }

            cmbConstructionType.Items.Add("All"); cmbConstructionType.SelectedIndex = 0;
            foreach (var construction in worldconstructions)
            {
                cmbConstructionType.Items.Add(construction.Key);
            }

            var worldConstructionEvents = from eventType in World.WorldConstructions.SelectMany(element => element.Events)
                                          group eventType by eventType.Type into type
                                          select type.Key;

            var structureEvents = from eventType in World.Structures.SelectMany(element => element.Events)
                                  group eventType by eventType.Type into type
                                  select type.Key;

            _siteSearch = new SitesList(World);

            var sites = from site in World.Sites
                        where !string.IsNullOrWhiteSpace(site.Name)
                        orderby site.Type
                        group site by site.Type into sitetype
                        select sitetype;

            var populationTypes = from population in World.SitePopulations
                                  orderby population.Race
                                  group population by population.Race into type
                                  select type;

            cmbSiteType.Items.Add("All");
            cmbSiteType.SelectedIndex = 0;

            foreach (var site in sites)
            {
                cmbSiteType.Items.Add(site.Key);
            }

            cmbSitePopulation.Items.Add("All"); cmbSitePopulation.SelectedIndex = 0;
            foreach (var populationType in populationTypes)
            {
                cmbSitePopulation.Items.Add(populationType.Key);
            }

            var siteEvents = from eventType in World.Sites.SelectMany(site => site.Events)
                             group eventType by eventType.Type into type
                             select type.Key;

            TabEvents.Clear();
            TabEvents.Add(siteEvents.ToList());
            TabEvents.Add(structureEvents.ToList());
            TabEvents.Add(worldConstructionEvents.ToList());
        }

        internal override void DoSearch()
        {
            SearchSiteList(null, null);
            SearchStructureList(null, null);
            SearchWorldConstructionList(null, null);
            base.DoSearch();
        }

        internal override void ResetTab()
        {
            txtSiteSearch.Clear();
            listSiteSearch.Items.Clear();
            cmbSiteType.Items.Clear();
            cmbSitePopulation.Items.Clear();
            radSiteNone.Checked = true;

            txtWorldConstructionsSearch.Clear();
            cmbConstructionType.Items.Clear();
            listWorldConstructionsSearch.Items.Clear();
            radWorldConstructionsSortNone.Checked = true;

            txtStructuresSearch.Clear();
            cmbStructureType.Items.Clear();
            listStructureSearch.Items.Clear();
            radStructuresSortNone.Checked = true;
        }

        public void ResetSiteBaseList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                txtSiteSearch.Clear();
                cmbSiteType.SelectedIndex = 0;
                cmbSitePopulation.SelectedIndex = 0;
                radSiteNone.Checked = true;
            }
        }

        private void UpdateCounts(Label label, int shown, int total)
        {
            label.Text = $"{shown} / {total}";
        }

        private void SearchSiteList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                if (sender == cmbSitePopulation && !radSiteSortPopulation.Checked)
                {
                    radSiteSortPopulation.Checked = true;
                }
                else
                {
                    _siteSearch.Name = txtSiteSearch.Text;
                    _siteSearch.Type = cmbSiteType.SelectedItem.ToString();
                    _siteSearch.PopulationType = cmbSitePopulation.SelectedItem.ToString();
                    _siteSearch.SortOwners = radSiteOwners.Checked;
                    _siteSearch.SortEvents = radSiteSortEvents.Checked;
                    _siteSearch.SortFiltered = radSiteSortFiltered.Checked;
                    _siteSearch.SortWarfare = radSiteSortWarfare.Checked;
                    _siteSearch.SortPopulation = radSiteSortPopulation.Checked;
                    _siteSearch.SortConnections = radSortConnections.Checked;
                    _siteSearch.SortDeaths = radSiteSortDeaths.Checked;
                    _siteSearch.SortBeastAttacks = radSiteBeastAttacks.Checked;
                    IEnumerable<Site> list = _siteSearch.GetList();
                    var results = list.ToArray();
                    listSiteSearch.SetObjects(results);
                    UpdateCounts(lblShownResults, results.Length, _siteSearch.BaseList.Count);
                }
            }
        }

        private void SearchWorldConstructionList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _worldConstructionSearch.Name = txtWorldConstructionsSearch.Text;
                _worldConstructionSearch.Type = cmbConstructionType.SelectedItem.ToString();
                _worldConstructionSearch.SortEvents = radWorldConstructionsSortEvents.Checked;
                _worldConstructionSearch.SortFiltered = radWorldConstructionsSortFiltered.Checked;
                IEnumerable<WorldConstruction> list = _worldConstructionSearch.GetList();
                var results = list.ToArray();
                listWorldConstructionsSearch.SetObjects(results);
                UpdateCounts(lblWorldConstructionResult, results.Length, _worldConstructionSearch.BaseList.Count);
            }
        }

        private void SearchStructureList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _structureSearch.Name = txtStructuresSearch.Text;
                _structureSearch.Type = cmbStructureType.SelectedItem.ToString();
                _structureSearch.SortEvents = radStructuresSortEvents.Checked;
                _structureSearch.SortFiltered = radStructuresSortFiltered.Checked;
                IEnumerable<Structure> list = _structureSearch.GetList();
                var results = list.ToArray();
                listStructureSearch.SetObjects(results);
                UpdateCounts(lblStructureResults, results.Length, _structureSearch.BaseList.Count);
            }
        }

        private void lnkMaxResults_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // InputBox with value validation - first define validation delegate, which
            // returns empty string for valid values and error message for invalid values
            InputBoxValidation validation = delegate (string val)
            {
                if (val == "")
                {
                    return "Value cannot be empty.";
                }

                if (!new Regex(@"^[0-9]+$").IsMatch(val))
                {
                    return "Value is not valid.";
                }

                return "";
            };

            string value = WorldObjectList.MaxResults.ToString();
            if (InputBox.Show("Max Results:", "Enter maximum search results. (0 for All)", ref value, validation) == DialogResult.OK)
            {
                WorldObjectList.MaxResults = int.Parse(value);
                foreach (LinkLabel lnkLabel in MaxResultsLabels)
                {
                    lnkLabel.Text = WorldObjectList.MaxResults.ToString();
                    lnkLabel.Left = lnkLabel.Parent.Right - lnkLabel.Width - 3;
                }
                lblShownResults.Left = lnkMaxResults.Left - lblShownResults.Width - 3;
                ListSearch_SelectedIndexChanged(this, EventArgs.Empty);
                SearchSiteList(null, null);
            }
        }

        private void listWorldConstructionsSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listStructuresSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listSiteSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }
    }
}

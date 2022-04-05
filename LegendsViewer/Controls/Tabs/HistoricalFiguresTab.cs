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
    public partial class HistoricalFiguresTab : BaseSearchTab
    {
        private HistoricalFigureList _hfSearch;

        public HistoricalFiguresTab()
        {
            InitializeComponent();
        }

        internal override void InitializeTab()
        {
            EventTabs = new[] { tpHFEvents };
            EventTabTypes = new[] { typeof(HistoricalFigure) };
            lnkMaxResults.Text = WorldObjectList.MaxResults.ToString();
            MaxResultsLabels.Add(lnkMaxResults);
            listHFSearch.ShowGroups = false;

            listHFSearch.AllColumns.Add(new OLVColumn { AspectName = "TitleRaceString", IsVisible = false, Text = "Race", TextAlign = HorizontalAlignment.Left });
            listHFSearch.AllColumns.Add(new OLVColumn { AspectName = "Caste", IsVisible = false, Text = "Caste", TextAlign = HorizontalAlignment.Left });
            listHFSearch.AllColumns.Add(new OLVColumn { AspectName = "PreviousRace", IsVisible = false, Text = "Previous Race", TextAlign = HorizontalAlignment.Left });
            listHFSearch.AllColumns.Add(new OLVColumn { AspectName = "Alive", IsVisible = false, Text = "Alive", TextAlign = HorizontalAlignment.Center, CheckBoxes = true });
            listHFSearch.AllColumns.Add(new OLVColumn { AspectName = "Skeleton", IsVisible = false, Text = "Skeleton", TextAlign = HorizontalAlignment.Center, CheckBoxes = true });
            listHFSearch.AllColumns.Add(new OLVColumn { AspectName = "Force", IsVisible = false, Text = "Force", TextAlign = HorizontalAlignment.Center, CheckBoxes = true });
            listHFSearch.AllColumns.Add(new OLVColumn { AspectName = "Zombie", IsVisible = false, Text = "Zombie", TextAlign = HorizontalAlignment.Center, CheckBoxes = true });
            listHFSearch.AllColumns.Add(new OLVColumn { AspectName = "Ghost", IsVisible = false, Text = "Ghost", TextAlign = HorizontalAlignment.Center, CheckBoxes = true });
            listHFSearch.AllColumns.Add(new OLVColumn { AspectName = "Animated", IsVisible = false, Text = "Animated", TextAlign = HorizontalAlignment.Center, CheckBoxes = true });
            listHFSearch.AllColumns.Add(new OLVColumn { AspectName = "Adventurer", IsVisible = false, Text = "Adventurer", TextAlign = HorizontalAlignment.Center, CheckBoxes = true });
            listHFSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Kills",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = rowObject => ((HistoricalFigure)rowObject).NotableKills.Count
            });
            listHFSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Events",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = rowObject => ((HistoricalFigure)rowObject).Events.Count
            });
        }

        internal override void AfterLoad(World world)
        {
            base.AfterLoad(world);
            _hfSearch = new HistoricalFigureList(World);

            var races = from hf in World.HistoricalFigures
                        orderby hf.Race.NameSingular
                        group hf by hf.Race.NameSingular into race
                        select race;
            var castes = from hf in World.HistoricalFigures
                         orderby hf.Caste
                         group hf by hf.Caste into caste
                         select caste;
            var types = from hf in World.HistoricalFigures
                        orderby hf.AssociatedType
                        group hf by hf.AssociatedType into type
                        select type;

            cmbRace.Items.Add("All"); cmbRace.SelectedIndex = 0;
            foreach (var race in races)
            {
                cmbRace.Items.Add(race.Key);
            }

            cmbCaste.Items.Add("All"); cmbCaste.SelectedIndex = 0;
            foreach (var caste in castes)
            {
                cmbCaste.Items.Add(caste.Key);
            }

            cmbType.Items.Add("All"); cmbType.SelectedIndex = 0;
            foreach (var type in types)
            {
                cmbType.Items.Add(type.Key);
            }

            TabEvents.Clear();

            var historicalFigureEvents = from eventType in World.HistoricalFigures.SelectMany(hf => hf.Events)
                                         group eventType by eventType.Type into type
                                         select type.Key;
            TabEvents.Add(historicalFigureEvents.ToList());
        }

        internal override void DoSearch()
        {
            SearchHfList(null, null);
            base.DoSearch();
        }

        internal override void ResetTab()
        {
            txtHFSearch.Clear();
            listHFSearch.SetObjects(new object[0]);
            chkDeity.Checked = false;
            chkForce.Checked = false;
            chkVampire.Checked = false;
            chkWerebeast.Checked = false;
            chkNecromancer.Checked = false;
            chkAnimated.Checked = false;
            chkGhost.Checked = false;
            chkAlive.Checked = false;
            chkHFLeader.Checked = false;
            cmbRace.Items.Clear();
            cmbCaste.Items.Clear();
            cmbType.Items.Clear();
            radHFNone.Checked = true;
        }

        private void SearchHfList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _hfSearch.Name = txtHFSearch.Text.Length > 1 ? txtHFSearch.Text : "";

                _hfSearch.Race = cmbRace.SelectedItem.ToString();
                _hfSearch.Caste = cmbCaste.SelectedItem.ToString();
                _hfSearch.Type = cmbType.SelectedItem.ToString();
                _hfSearch.Deity = chkDeity.Checked;
                _hfSearch.Force = chkForce.Checked;
                _hfSearch.Vampire = chkVampire.Checked;
                _hfSearch.Werebeast = chkWerebeast.Checked;
                _hfSearch.Necromancer = chkNecromancer.Checked;
                _hfSearch.Animated = chkAnimated.Checked;
                _hfSearch.Ghost = chkGhost.Checked;
                _hfSearch.Alive = chkAlive.Checked;
                _hfSearch.Leader = chkHFLeader.Checked;
                _hfSearch.SortKills = radSortKills.Checked;
                _hfSearch.SortEvents = radHFSortEvents.Checked;
                _hfSearch.SortFiltered = radHFSortFiltered.Checked;
                _hfSearch.SortBattles = radHFSortBattles.Checked;
                _hfSearch.SortMiscKills = radSortMiscKills.Checked;

                IEnumerable<HistoricalFigure> list = _hfSearch.GetList();
                var results = list.ToArray();
                listHFSearch.SetObjects(results);
                //listHFSearch.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                UpdateCounts(results.Length, _hfSearch.BaseList.Count);
            }
        }

        public void ResetHfBaseList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                txtHFSearch.Clear();
                cmbRace.SelectedIndex = 0;
                cmbCaste.SelectedIndex = 0;
                cmbType.SelectedIndex = 0;
                chkDeity.Checked = false;
                chkForce.Checked = false;
                chkVampire.Checked = false;
                chkWerebeast.Checked = false;
                chkNecromancer.Checked = false;
                chkAnimated.Checked = false;
                chkGhost.Checked = false;
                chkAlive.Checked = false;
                chkHFLeader.Checked = false;
                radHFNone.Checked = true;
            }
        }

        private void ListHFSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void LnkMaxResults_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // InputBox with value validation - first define validation delegate, which
            // returns empty string for valid values and error message for invalid values
            InputBoxValidation validation = delegate (string val)
            {
                if (val?.Length == 0)
                {
                    return "Value cannot be empty.";
                }

                return !new Regex(@"^[0-9]+$").IsMatch(val) ? "Value is not valid." : "";
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
                SearchHfList(null, null);
            }
        }

        private void UpdateCounts(int shown, int total)
        {
            lblShownResults.Text = $"{shown} / {total}";
        }

        private void OnSelected(object sender, TabControlEventArgs e)
        {
            SearchHfList(null, null);
        }
    }
}
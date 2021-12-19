using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.Tabs
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class GeographyTab : BaseSearchTab
    {
        private LandmassesList _landmassSearch;
        private MountainPeaksList _mountainPeaksSearch;
        private RiverList _riverSearch;
        private RegionsList _regionSearch;
        private UndergroundRegionsList _uRegionSearch;

        public GeographyTab()
        {
            InitializeComponent();
        }

        internal override void InitializeTab()
        {
            EventTabs = new[] { tpRegionEvents, tpURegionEvents, tpLandmassEvents, tpMountainPeakEvents, tpRiverEvents };
            EventTabTypes = new[] { typeof(Region), typeof(UndergroundRegion), typeof(Landmass), typeof(MountainPeak), typeof(River) };

            listRegionSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Deaths",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = item => ((WorldRegion)item).Deaths.Count
            });
            listRegionSearch.AllColumns.Add(new OLVColumn
            {
                Text = "Events",
                TextAlign = HorizontalAlignment.Right,
                IsVisible = false,
                AspectGetter = rowObject => ((WorldRegion)rowObject).Events.Count
            });
            listRegionSearch.ShowGroups = false;

            listURegionSearch.ShowGroups = false;

            listLandmassesSearch.ShowGroups = false;

            listMountainPeakSearch.ShowGroups = false;

            listRiverSearch.ShowGroups = false;
        }

        internal override void AfterLoad(World world)
        {
            base.AfterLoad(world);
            _regionSearch = new RegionsList(World);
            _uRegionSearch = new UndergroundRegionsList(World);
            _landmassSearch = new LandmassesList(World);
            _mountainPeaksSearch = new MountainPeaksList(World);
            _riverSearch = new RiverList(World);

            var regions = from region in World.Regions
                          orderby region.Type
                          group region by region.Type into regiontype
                          select regiontype;
            var uregions = from uregion in World.UndergroundRegions
                           orderby uregion.Type
                           group uregion by uregion.Type into uregiontype
                           select uregiontype;

            cmbRegionType.Items.Add("All"); cmbRegionType.SelectedIndex = 0;
            foreach (var region in regions)
            {
                cmbRegionType.Items.Add(region.Key);
            }

            cmbURegionType.Items.Add("All"); cmbURegionType.SelectedIndex = 0;
            foreach (var uregion in uregions)
            {
                cmbURegionType.Items.Add(uregion.Key);
            }

            var regionEvents = from eventType in World.Regions.SelectMany(region => region.Events)
                               group eventType by eventType.Type into type
                               select type.Key;
            var undergroundRegionEvents = from eventType in World.UndergroundRegions.SelectMany(uRegion => uRegion.Events)
                                          group eventType by eventType.Type into type
                                          select type.Key;
            var landmassEvents = from eventType in World.Landmasses.SelectMany(element => element.Events)
                                 group eventType by eventType.Type into type
                                 select type.Key;

            var mountainPeakEvents = from eventType in World.MountainPeaks.SelectMany(element => element.Events)
                                     group eventType by eventType.Type into type
                                     select type.Key;

            var riverEvents = from eventType in World.Rivers.SelectMany(element => element.Events)
                              group eventType by eventType.Type into type
                              select type.Key;

            TabEvents.Clear();
            TabEvents.Add(regionEvents.ToList());
            TabEvents.Add(undergroundRegionEvents.ToList());
            TabEvents.Add(landmassEvents.ToList());
            TabEvents.Add(mountainPeakEvents.ToList());
            TabEvents.Add(riverEvents.ToList());
        }

        internal override void DoSearch()
        {
            SearchLandmassList(null, null);
            SearchMountainPeakList(null, null);
            SearchRiverList(null, null);
            SearchRegionList(null, null);
            SearchURegionList(null, null);
            base.DoSearch();
        }

        internal override void ResetTab()
        {
            txtRegionSearch.Clear();
            listRegionSearch.Items.Clear();
            cmbRegionType.Items.Clear();
            radRegionNone.Checked = true;

            listURegionSearch.Items.Clear();
            cmbURegionType.Items.Clear();
            radURegionNone.Checked = true;
        }

        private void SearchRegionList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _regionSearch.Name = txtRegionSearch.Text;
                _regionSearch.Type = cmbRegionType.SelectedItem.ToString();
                _regionSearch.SortEvents = radRegionSortEvents.Checked;
                _regionSearch.SortFiltered = radRegionSortFiltered.Checked;
                _regionSearch.SortBattles = radRegionSortBattles.Checked;
                _regionSearch.SortDeaths = radRegionSortDeaths.Checked;
                _regionSearch.SortArea = radRegionSortArea.Checked;
                IEnumerable<WorldRegion> list = _regionSearch.GetList();
                var results = list.ToArray();
                listRegionSearch.SetObjects(results);
                UpdateCounts(lblShownResults, results.Length, _regionSearch.BaseList.Count);
            }
        }

        private void UpdateCounts(Label label, int shown, int total)
        {
            label.Text = $"{shown} / {total}";
        }

        private void SearchURegionList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _uRegionSearch.Type = cmbURegionType.SelectedItem.ToString();
                _uRegionSearch.SortEvents = radURegionSortEvents.Checked;
                _uRegionSearch.SortFiltered = radURegionSortFiltered.Checked;
                _uRegionSearch.SortArea = radURegionSortArea.Checked;
                IEnumerable<UndergroundRegion> list = _uRegionSearch.GetList();
                var results = list.ToArray();
                listURegionSearch.SetObjects(results);
                UpdateCounts(lblURegionResults, results.Length, _uRegionSearch.BaseList.Count);
            }
        }

        private void SearchLandmassList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _landmassSearch.Name = txtLandmassSearch.Text;
                _landmassSearch.SortEvents = radLandmassEvents.Checked;
                _landmassSearch.SortFiltered = radLandmassFiltered.Checked;
                IEnumerable<Landmass> list = _landmassSearch.GetList();
                var results = list.ToArray();
                listLandmassesSearch.SetObjects(results);
                UpdateCounts(lblLandmassResults, results.Length, _landmassSearch.BaseList.Count);
            }
        }

        private void SearchMountainPeakList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _mountainPeaksSearch.Name = txtMountainPeakSearch.Text;
                _mountainPeaksSearch.SortEvents = radMountainPeakEvents.Checked;
                _mountainPeaksSearch.SortFiltered = radMountainPeakFiltered.Checked;
                IEnumerable<MountainPeak> list = _mountainPeaksSearch.GetList();
                var results = list.ToArray();
                listMountainPeakSearch.SetObjects(results);
                UpdateCounts(lblMountainPeakResults, results.Length, _mountainPeaksSearch.BaseList.Count);
            }
        }

        private void SearchRiverList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _riverSearch.Name = txtRiverSearch.Text;
                _riverSearch.SortEvents = radRiverEvents.Checked;
                _riverSearch.SortFiltered = radRiverFiltered.Checked;
                IEnumerable<River> list = _riverSearch.GetList();
                var results = list.ToArray();
                listRiverSearch.SetObjects(results);
                UpdateCounts(lblRiverResults, results.Length, _riverSearch.BaseList.Count);
            }
        }

        private void listRegionSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listURegionSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listLandmassSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listMountainPeakSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listRiverSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }
    }
}

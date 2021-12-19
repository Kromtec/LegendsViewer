using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using LegendsViewer.Legends;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.Tabs
{
    public partial class ArtAndCraftsTab : BaseSearchTab
    {
        private ArtifactsList _artifactSearch;
        private WrittenContentList _writtenContentSearch;
        private DanceFormsList _danceFormSearch;
        private MusicalFormsList _musicalFormSearch;
        private PoeticFormsList _poeticFormSearch;

        public ArtAndCraftsTab()
        {
            InitializeComponent();
        }

        internal override void InitializeTab()
        {
            EventTabs = new[] { tpArtifactsEvents, tpWrittenContentEvents, tpDanceFormsEvents, tpMusicalFormsEvents, tpPoeticFormsEvents };
            EventTabTypes = new[] { typeof(Artifact), typeof(WrittenContent), typeof(DanceForm), typeof(MusicalForm), typeof(PoeticForm) };

            listArtifactSearch.AllColumns.Add(new OLVColumn
            {
                IsVisible = false,
                Text = "Creator",
                TextAlign = HorizontalAlignment.Left,
                AspectGetter = obj => ((Artifact)obj).Creator.Name
            });
            listArtifactSearch.AllColumns.Add(new OLVColumn { AspectName = "SubType", IsVisible = false, Text = "SubType", TextAlign = HorizontalAlignment.Left });
            listArtifactSearch.AllColumns.Add(new OLVColumn { AspectName = "Material", IsVisible = false, Text = "Material", TextAlign = HorizontalAlignment.Left });
            listArtifactSearch.AllColumns.Add(new OLVColumn { AspectName = "PageCount", IsVisible = false, Text = "Page Count", TextAlign = HorizontalAlignment.Right });
            listArtifactSearch.ShowGroups = false;

            listWrittenContentSearch.AllColumns.Add(new OLVColumn
            {
                IsVisible = false,
                Text = "Author",
                TextAlign = HorizontalAlignment.Left,
                AspectGetter = obj => ((WrittenContent)obj).Author.Name
            });
            listWrittenContentSearch.AllColumns.Add(new OLVColumn { AspectName = "PageCount", IsVisible = false, Text = "Page Count", TextAlign = HorizontalAlignment.Right });
            listWrittenContentSearch.ShowGroups = false;

            listDanceFormsSearch.ShowGroups = false;
            listMusicalFormsSearch.ShowGroups = false;
            listPoeticFormsSearch.ShowGroups = false;
        }

        internal override void AfterLoad(World world)
        {
            base.AfterLoad(world);

            _artifactSearch = new ArtifactsList(World);
            _writtenContentSearch = new WrittenContentList(World);
            _danceFormSearch = new DanceFormsList(World);
            _musicalFormSearch = new MusicalFormsList(World);
            _poeticFormSearch = new PoeticFormsList(World);

            var writtencontents = from writtenContent in World.WrittenContents
                                  orderby writtenContent.Type.GetDescription()
                                  group writtenContent by writtenContent.Type.GetDescription() into writtenContentType
                                  select writtenContentType;

            var artifactTypes = World.Artifacts.Select(x => x.Type).SkipWhile(string.IsNullOrEmpty).Distinct().OrderBy(x => x);

            var artifactMaterials = World.Artifacts.Select(x => string.IsNullOrEmpty(x.Material) ? "" : x.Material).SkipWhile(string.IsNullOrEmpty).Distinct().OrderBy(x => x);

            cmbWrittenContentType.Items.Add("All"); cmbWrittenContentType.SelectedIndex = 0;
            foreach (var writtencontent in writtencontents)
            {
                cmbWrittenContentType.Items.Add(writtencontent.Key);
            }

            cbmArtTypeFilter.Items.Add("All"); cbmArtTypeFilter.SelectedIndex = 0;
            cbmArtTypeFilter.Items.AddRange(artifactTypes.ToArray());
            lblArtTypeFilter.Visible = cbmArtTypeFilter.Visible = artifactTypes.Any();

            cbmArtMatFilter.Items.Add("All"); cbmArtMatFilter.SelectedIndex = 0;
            cbmArtMatFilter.Items.AddRange(artifactMaterials.ToArray());
            lblArtMatFilter.Visible = cbmArtMatFilter.Visible = artifactMaterials.Any();

            var artifactEvents = from eventType in World.Artifacts.SelectMany(artifact => artifact.Events)
                                 group eventType by eventType.Type into type
                                 select type.Key;

            var writtenContentEvents = from eventType in World.WrittenContents.SelectMany(element => element.Events)
                                       group eventType by eventType.Type into type
                                       select type.Key;

            var danceFormEvents = from eventType in World.DanceForms.SelectMany(element => element.Events)
                                  group eventType by eventType.Type into type
                                  select type.Key;
            var musicalFormEvents = from eventType in World.MusicalForms.SelectMany(element => element.Events)
                                    group eventType by eventType.Type into type
                                    select type.Key;
            var poeticFormEvents = from eventType in World.PoeticForms.SelectMany(element => element.Events)
                                   group eventType by eventType.Type into type
                                   select type.Key;

            TabEvents.Clear();
            TabEvents.Add(artifactEvents.ToList());
            TabEvents.Add(writtenContentEvents.ToList());
            TabEvents.Add(danceFormEvents.ToList());
            TabEvents.Add(musicalFormEvents.ToList());
            TabEvents.Add(poeticFormEvents.ToList());
        }

        internal override void DoSearch()
        {
            SearchArtifactList(null, null);
            SearchWrittenContentList(null, null);
            SearchDanceFormsList(null, null);
            SearchMusicalFormsList(null, null);
            SearchPoeticFormsList(null, null);
            base.DoSearch();
        }

        internal override void ResetTab()
        {
            txtArtifactSearch.Clear();
            listArtifactSearch.SetObjects(new object[0]);
            radArtifactSortNone.Checked = true;

            cbmArtMatFilter.Items.Clear();
            cbmArtTypeFilter.Items.Clear();

            txtWrittenContentSearch.Clear();
            cmbWrittenContentType.Items.Clear();
            listWrittenContentSearch.Items.Clear();
            radWrittenContentSortNone.Checked = true;
        }

        private void SearchArtifactList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _artifactSearch.Name = txtArtifactSearch.Text;
                _artifactSearch.SortEvents = radArtifactSortEvents.Checked;
                _artifactSearch.SortFiltered = radArtifactSortFiltered.Checked;
                _artifactSearch.Type = cbmArtTypeFilter.SelectedIndex == 0 ? null : cbmArtTypeFilter.SelectedItem.ToString();
                _artifactSearch.Material = cbmArtMatFilter.SelectedIndex == 0 ? null : cbmArtMatFilter.SelectedItem.ToString();
                _artifactSearch.ShowWrittenContent = chkWrittenContent.Checked;
                IEnumerable<Artifact> list = _artifactSearch.GetList();
                var results = list.ToArray();
                listArtifactSearch.SetObjects(results);
                UpdateCounts(lblArtifactResults, results.Length, _artifactSearch.BaseList.Count);
            }
        }

        private void UpdateCounts(Label label, int shown, int total)
        {
            label.Text = $"{shown} / {total}";
        }

        private void ResetArtifactBaseList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                lblArtifactList.Text = "All";
                lblArtifactList.ForeColor = DefaultForeColor;
                lblArtifactList.Font = new Font(lblArtifactList.Font.FontFamily, lblArtifactList.Font.Size, FontStyle.Regular);
                cbmArtTypeFilter.SelectedIndex = 0;
                cbmArtMatFilter.SelectedIndex = 0;
                SearchArtifactList(null, null);
            }
        }

        private void SearchWrittenContentList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _writtenContentSearch.Name = txtWrittenContentSearch.Text;
                _writtenContentSearch.Type = cmbWrittenContentType.SelectedItem.ToString();
                _writtenContentSearch.SortEvents = radWrittenContentSortEvents.Checked;
                _writtenContentSearch.SortFiltered = radWrittenContentSortFiltered.Checked;
                IEnumerable<WrittenContent> list = _writtenContentSearch.GetList();
                var results = list.ToArray();
                listWrittenContentSearch.SetObjects(results);
                UpdateCounts(lblWrittenContentResults, results.Length, _writtenContentSearch.BaseList.Count);
            }
        }

        private void SearchDanceFormsList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _danceFormSearch.Name = txtDanceFormsSearch.Text;
                _danceFormSearch.SortEvents = radDanceFormsEvents.Checked;
                _danceFormSearch.SortFiltered = radDanceFormsFiltered.Checked;
                IEnumerable<DanceForm> list = _danceFormSearch.GetList();
                var results = list.ToArray();
                listDanceFormsSearch.SetObjects(results);
                UpdateCounts(lblDanceFormsResults, results.Length, _danceFormSearch.BaseList.Count);
            }
        }

        private void SearchMusicalFormsList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _musicalFormSearch.Name = txtMusicalFormsSearch.Text;
                _musicalFormSearch.SortEvents = radMusicalFormsEvents.Checked;
                _musicalFormSearch.SortFiltered = radMusicalFormsFiltered.Checked;
                IEnumerable<MusicalForm> list = _musicalFormSearch.GetList();
                var results = list.ToArray();
                listMusicalFormsSearch.SetObjects(results);
                UpdateCounts(lblMusicalFormsResults, results.Length, _musicalFormSearch.BaseList.Count);
            }
        }

        private void SearchPoeticFormsList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _poeticFormSearch.Name = txtPoeticFormsSearch.Text;
                _poeticFormSearch.SortEvents = radPoeticFormsEvents.Checked;
                _poeticFormSearch.SortFiltered = radPoeticFormsFiltered.Checked;
                IEnumerable<PoeticForm> list = _poeticFormSearch.GetList();
                var results = list.ToArray();
                listPoeticFormsSearch.SetObjects(results);
                UpdateCounts(lblPoeticFormsResults, results.Length, _poeticFormSearch.BaseList.Count);
            }
        }

        private void listArtifactSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listWrittenContentSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listDanceFormsSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listMusicalFormsSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listPoeticFormsSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }
    }
}

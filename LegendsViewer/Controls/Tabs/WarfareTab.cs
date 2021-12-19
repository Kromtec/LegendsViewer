using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;

namespace LegendsViewer.Controls.Tabs
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class WarfareTab : BaseSearchTab
    {
        private WarsList _warSearch;
        private BattlesList _battleSearch;
        private ConqueringsList _conqueringsSearch;
        private BeastAttackList _beastAttackSearch;
        private RaidsList _raidsSearch;

        public WarfareTab()
        {
            InitializeComponent();
        }

        internal override void InitializeTab()
        {
            hint.SetToolTip(chkFilterWarfare, "Unnotable Battle = Attackers outnumber defenders 10 to 1 and win and suffer < 10% losses. \nUnnotable Conquering = All Pillagings.");

            EventTabs = new[] { tpWarEvents, tpBattlesEvents, tpConqueringsEvents, tpBeastAttackEvents, tpRaidsEvents };
            EventTabTypes = new[] { typeof(War), typeof(Battle), typeof(SiteConquered), typeof(BeastAttack), typeof(Raid) };

            listWarSearch.ShowGroups = false;
            listBattleSearch.ShowGroups = false;
            listConqueringSearch.ShowGroups = false;
            listBeastAttackSearch.ShowGroups = false;
            listRaidSearch.ShowGroups = false;
        }

        internal override void ResetEvents() { }

        internal override void AfterLoad(World world)
        {
            base.AfterLoad(world);
            _warSearch = new WarsList(World);
            _battleSearch = new BattlesList(World);
            _conqueringsSearch = new ConqueringsList(World);
            _beastAttackSearch = new BeastAttackList(World);
            _raidsSearch = new RaidsList(World);

            var conquerTypes = from conquer in World.EventCollections.OfType<SiteConquered>()
                               orderby conquer.ConquerType
                               group conquer by conquer.ConquerType into conquers
                               select conquers;

            cmbConqueringType.Items.Add("All"); cmbConqueringType.SelectedIndex = 0;
            foreach (var conquerType in conquerTypes)
            {
                cmbConqueringType.Items.Add(conquerType.Key);
            }

            var warEvents = from eventType in World.EventCollections.OfType<War>().SelectMany(war => war.GetSubEvents())
                            group eventType by eventType.Type into type
                            select type.Key;
            var battleEvents = from eventType in World.EventCollections.OfType<Battle>().SelectMany(battle => battle.GetSubEvents())
                               group eventType by eventType.Type into type
                               select type.Key;
            var conqueringEvents = from eventType in World.EventCollections.OfType<SiteConquered>().SelectMany(conquering => conquering.GetSubEvents())
                                   group eventType by eventType.Type into type
                                   select type.Key;
            var beastAttackEvents = from eventType in World.EventCollections.OfType<BeastAttack>().SelectMany(beastAttack => beastAttack.GetSubEvents())
                                    group eventType by eventType.Type into type
                                    select type.Key;
            var raidEvents = from eventType in World.EventCollections.OfType<Raid>().SelectMany(raid => raid.GetSubEvents())
                             group eventType by eventType.Type into type
                             select type.Key;

            TabEvents.Clear();
            TabEvents.Add(warEvents.ToList());
            TabEvents.Add(battleEvents.ToList());
            TabEvents.Add(conqueringEvents.ToList());
            TabEvents.Add(beastAttackEvents.ToList());
            TabEvents.Add(raidEvents.ToList());
        }

        internal override void DoSearch()
        {
            SearchBattleList(null, null);
            SearchConqueringList(null, null);
            SearchWarList(null, null);
            SearchBeastAttackList(null, null);
            SearchRaidsList(null, null);
            base.DoSearch();
        }

        internal override void ResetTab()
        {
            lblBattleList.Text = lblWarList.Text = "All";
            lblWarList.ForeColor = DefaultForeColor;
            lblWarList.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);

            txtWarSearch.Clear();
            listWarSearch.Items.Clear();
            chkWarOngoing.Checked = false;
            radWarSortNone.Checked = true;
            chkFilterWarfare.Checked = true;

            txtBattleSearch.Clear();
            listBattleSearch.Items.Clear();
            radBattleSortNone.Checked = true;

            txtConqueringSearch.Clear();
            listConqueringSearch.Items.Clear();
            cmbConqueringType.Items.Clear();
            radConqueringSortNone.Checked = true;

            txtRaidSearch.Clear();
            listRaidSearch.Items.Clear();
            radRaidSortNone.Checked = true;
        }

        private void chkFilterWarfare_CheckedChanged(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                World.FilterBattles = chkFilterWarfare.Checked;
            }
        }

        private void SearchWarList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _warSearch.Name = txtWarSearch.Text;
                _warSearch.SortEvents = radWarSortEvents.Checked;
                _warSearch.SortFiltered = radWarSortFiltered.Checked;
                _warSearch.SortLength = radWarLength.Checked;
                _warSearch.SortDeaths = radWarDeaths.Checked;
                _warSearch.Ongoing = chkWarOngoing.Checked;
                _warSearch.SortWarfare = radWarSortWarfare.Checked;
                _warSearch.SortConquerings = radWarsSortConquerings.Checked;
                IEnumerable<War> list = _warSearch.GetList();
                var results = list.ToArray();
                listWarSearch.SetObjects(results);
                UpdateCounts(lblWarResults, results.Length, _warSearch.BaseList.Count);
            }
        }

        private void UpdateCounts(Label label, int shown, int total)
        {
            label.Text = $"{shown} / {total}";
        }

        private void ResetWarBaseList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                lblWarList.Text = "All";
                lblWarList.ForeColor = DefaultForeColor;
                lblWarList.Font = new Font(lblWarList.Font.FontFamily, lblWarList.Font.Size, FontStyle.Regular);
                _warSearch.BaseList = World.EventCollections.OfType<War>().ToList();
                SearchWarList(null, null);
            }
        }

        private void ResetRaidBaseList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                lblRaidList.Text = "All";
                lblRaidList.ForeColor = DefaultForeColor;
                lblRaidList.Font = new Font(lblRaidList.Font.FontFamily, lblRaidList.Font.Size, FontStyle.Regular);
                _raidsSearch.BaseList = World.EventCollections.OfType<Raid>().ToList();
                SearchRaidsList(null, null);
            }
        }

        private void SearchBattleList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _battleSearch.Name = txtBattleSearch.Text;
                _battleSearch.SortEvents = radBattleSortEvents.Checked;
                _battleSearch.SortFiltered = radBattleSortFiltered.Checked;
                _battleSearch.SortDeaths = radBattleSortDeaths.Checked;
                IEnumerable<Battle> list = _battleSearch.GetList();
                var results = list.ToArray();
                listBattleSearch.SetObjects(results);
                UpdateCounts(lblBattleResults, results.Length, _battleSearch.BaseList.Count);
            }
        }

        public void ChangeBattleBaseList(List<Battle> list, string listName)
        {
            FileLoader.Working = true;
            lblBattleList.Text = listName;
            lblBattleList.ForeColor = Color.Blue;
            lblBattleList.Font = new Font(lblBattleList.Font.FontFamily, lblBattleList.Font.Size, FontStyle.Bold);
            _battleSearch.BaseList = list;
            txtBattleSearch.Clear();

            radBattleSortNone.Checked = true;
            tcWarfare.SelectedTab = tpBattles;
            tcBattles.SelectedTab = tpBattlesSearch;
            SearchBattleList(null, null);
            FileLoader.Working = false;
        }

        private void ResetBattleBaseList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                lblBattleList.Text = "All";
                lblBattleList.ForeColor = DefaultForeColor;
                lblBattleList.Font = new Font(lblBattleList.Font.FontFamily, lblBattleList.Font.Size, FontStyle.Regular);
                _battleSearch.BaseList = World.EventCollections.OfType<Battle>().ToList();
                SearchBattleList(null, null);
            }
        }

        private void SearchConqueringList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _conqueringsSearch.Name = txtConqueringSearch.Text;
                _conqueringsSearch.SortEvents = radConqueringSortEvents.Checked;
                _conqueringsSearch.SortFiltered = radConqueringSortFiltered.Checked;
                _conqueringsSearch.SortSite = radConqueringSortSite.Checked;
                _conqueringsSearch.Type = cmbConqueringType.SelectedItem.ToString();
                IEnumerable<SiteConquered> list = _conqueringsSearch.GetList();
                var results = list.ToArray();
                listConqueringSearch.SetObjects(results);
                UpdateCounts(lblConqueringResult, results.Length, _conqueringsSearch.BaseList.Count);
            }
        }

        private void SearchBeastAttackList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _beastAttackSearch.Name = txtBeastAttacksSearch.Text;
                _beastAttackSearch.SortEvents = radBeastAttacksEvents.Checked;
                _beastAttackSearch.SortFiltered = radBeastAttacksFiltered.Checked;
                _beastAttackSearch.SortDeaths = radBeastAttacksDeaths.Checked;
                IEnumerable<BeastAttack> list = _beastAttackSearch.GetList();
                var results = list.ToArray();
                listBeastAttackSearch.SetObjects(results);
                UpdateCounts(lblBeastAttackResults, results.Length, _beastAttackSearch.BaseList.Count);
            }
        }

        private void SearchRaidsList(object sender, EventArgs e)
        {
            if (!FileLoader.Working && World != null)
            {
                _raidsSearch.Name = txtRaidSearch.Text;
                _raidsSearch.SortEvents = radRaidSortEvents.Checked;
                _raidsSearch.SortFiltered = radRaidSortFiltered.Checked;
                _raidsSearch.SortItemsStolen = radRaidSortStolen.Checked;
                IEnumerable<Raid> list = _raidsSearch.GetList();
                var results = list.ToArray();
                listRaidSearch.SetObjects(results);
                UpdateCounts(lblRaidResults, results.Length, _raidsSearch.BaseList.Count);
            }
        }

        private void listWarSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listBattleSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listConqueringSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listBeastAttacks_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }

        private void listRaids_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListSearch_SelectedIndexChanged(sender, e);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LegendsViewer.Controls.HTML;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.WorldObjects;

namespace LegendsViewer.Controls.Query
{
    public partial class QueryControl : UserControl
    {

        SearchList _searchList;
        PropertyBox _selectProperties;
        public DwarfTabControl Browser;
        public World World;
        List<Object> _results;
        public QueryControl(World world, DwarfTabControl browser)
        {
            World = world;
            Browser = browser;

            InitializeComponent();
            SelectionPanel.SizeChanged += PanelResized;
            SearchPanel.SizeChanged += PanelResized;
            OrderByPanel.SizeChanged += PanelResized;

            SelectionPanel.CriteriaStartLocation = lblSelectCriteria.Bottom + 3;
            SearchPanel.CriteriaStartLocation = lblSearchCriteria.Bottom + 3;
            OrderByPanel.CriteriaStartLocation = lblOrderCriteria.Bottom + 3;

            SelectionPanel.SelectCriteria = true;
            SearchPanel.SearchCriteria = true;
            OrderByPanel.OrderByCriteria = true;

            PanelResized(this, null);
            SelectList.SelectedIndex = 0;
        }

        private void SelectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selectProperties != null)
            {
                _selectProperties.Remove();
            }

            _selectProperties = new PropertyBox
            {
                Location = new Point(SelectList.Right + 3, SelectList.Top),
                ListPropertiesOnly = true
            };
            _selectProperties.SelectedIndexChanged += SelectSubListChanged;
            Controls.Add(_selectProperties);

            switch (SelectList.SelectedItem.ToString())
            {
                case "Historical Figures": 
                    _searchList = new SearchList<HistoricalFigure>(World.HistoricalFigures);
                    _selectProperties.ParentType = typeof(HistoricalFigure);
                    break;
                case "Entities":
                    _searchList = new SearchList<Entity>(World.Entities);
                    _selectProperties.ParentType = typeof(Entity);
                    break;
                case "Sites": 
                    _searchList = new SearchList<Site>(World.Sites);
                    _selectProperties.ParentType = typeof(Site);
                    break;
                case "Regions":
                    _searchList = new SearchList<WorldRegion>(World.Regions);
                    _selectProperties.ParentType = typeof(WorldRegion);
                    break;
                case "Underground Regions":
                    _searchList = new SearchList<UndergroundRegion>(World.UndergroundRegions);
                    _selectProperties.ParentType = typeof(UndergroundRegion);
                    break;
                case "Wars":
                    _searchList = new SearchList<War>(World.Wars);
                    _selectProperties.ParentType = typeof(War);
                    break;
                case "Battles":
                    _searchList = new SearchList<Battle>(World.Battles);
                    _selectProperties.ParentType = typeof(Battle);
                    break;
                case "Conquerings":
                    _searchList = new SearchList<SiteConquered>(World.EventCollections.OfType<SiteConquered>().ToList());
                    _selectProperties.ParentType = typeof(SiteConquered);
                    break;
                case "Beast Attacks":
                    _searchList = new SearchList<BeastAttack>(World.BeastAttacks);
                    _selectProperties.ParentType = typeof(BeastAttack);
                    break;
                case "Artifacts":
                    _searchList = new SearchList<Artifact>(World.Artifacts);
                    _selectProperties.ParentType = typeof(Artifact);
                    break;
            }

            if (_selectProperties.ParentType == typeof(Site) || _selectProperties.ParentType == typeof(Battle))
            {
                btnMapResults.Visible = true;
            }

            SelectionPanel.Clear();

            SelectionPanel.CriteriaType = _searchList.GetMainListType();
            SearchPanel.CriteriaType = _selectProperties.GetLowestPropertyType();
            OrderByPanel.CriteriaType = _selectProperties.GetLowestPropertyType();

            SelectSubListChanged(this, null);


            //lblSelectCriteria.Text = "Select " + SelectList.Text + " Where:";
            //lblSearchCriteria.Text = "Search " + SelectList.Text + " Where:";
            //lblOrderCriteria.Text = "Order " + SelectList.Text + " By:";
        }

        private void UpdateCriteriaLabels()
        {
           /* if (SelectProperties.Text != "")
            {
                lblSelectCriteria.Text = "Select " + SelectList.Text + "' " + SelectProperties.Text + " Where " + S;
                lblSearchCriteria.Text = "Search " + SelectList.Text + "' " + SelectProperties.Text + " Where:";
                lblOrderCriteria.Text = "Order " + SelectList.Text + "' " + SelectProperties.Text + " By:";
            }*/
        }

        private void SelectSubListChanged(object sender, EventArgs e)
        {
            if (_selectProperties.SelectedIndex > 0) {
                SelectionPanel.Visible = true;
                if (SelectionPanel.Criteria.Count == 0)
                {
                    SelectionPanel.AddNew();
                }
            }
            else { SelectionPanel.Visible = false; SelectionPanel.Clear(); }

            Type selectedType = _selectProperties.GetLowestPropertyType();
            if (selectedType.IsGenericType)
            {
                selectedType = selectedType.GetGenericArguments()[0];
            }

            SearchPanel.Clear();
            SearchPanel.CriteriaType = selectedType;
            SearchPanel.AddNew();

            OrderByPanel.Clear();
            OrderByPanel.CriteriaType = selectedType;
            OrderByPanel.AddNew();
            PanelResized(this, null);

            if (_selectProperties.SelectedIndex > 0)
            {
                if (selectedType == typeof(Site) || selectedType == typeof(Battle))
                {
                    btnMapResults.Visible = true;
                }
                else
                {
                    btnMapResults.Visible = false;
                }
            }
            else if (_selectProperties.ParentType == typeof(Site) || _selectProperties.ParentType == typeof(Battle))
            {
                btnMapResults.Visible = true;
            }
            else
            {
                btnMapResults.Visible = false;
            }

            //if (SelectProperties.SelectedIndex != 0)
            //    lblSearchCriteria.Text = "Search " + SelectProperties.Text + " Where:";
            //else
            //    lblSearchCriteria.Text = "Search " + SelectList.Text + " Where:";
        }

        void PanelResized(object sender, EventArgs e)
        {
            SelectionPanel.Top = SelectList.Bottom + 3;
            if (SelectionPanel.Visible)
            {
                SearchPanel.Top = SelectionPanel.Bottom;
            }
            else
            {
                SearchPanel.Top = SelectList.Bottom + 3;
            }

            OrderByPanel.Top = SearchPanel.Bottom;
            btnSearch.Top = OrderByPanel.Bottom + 3;
            btnMapResults.Top = OrderByPanel.Bottom + 3;
            lblResults.Top = OrderByPanel.Bottom + 6;
            dgResults.Top = btnSearch.Bottom + 3;
            dgResults.Height = ClientSize.Height - dgResults.Top - 3;
            dgResults.Width = ClientSize.Width - dgResults.Left - 3;
        }

        private void QueryControl_Resize(object sender, EventArgs e)
        {
            PanelResized(sender, e);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lblResults.Text = "Searching...";
            Application.DoEvents();
            _results = Search();
            lblResults.Text = _results.Count + " Results";

            dgResults.Columns.Clear();

            List<DataGridViewColumn> columns = new List<DataGridViewColumn>();
            if (_selectProperties.SelectedProperty == null)
            {
                columns = AppHelpers.GetColumns(_selectProperties.ParentType);
            }
            else
            {
                columns = AppHelpers.GetColumns(_selectProperties.SelectedProperty.Type);
            }

            if (columns.Count > 0)
            {
                dgResults.AutoGenerateColumns = false;
                dgResults.Columns.AddRange(columns.ToArray());
            }
            else
            {
                dgResults.AutoGenerateColumns = true;
            }

            if (_results.Count > 1000)
            {
                dgResults.DataSource = _results.Take(1000).ToList();
                lblResults.Text += " (First 1000)";
            }
            else
            {
                dgResults.DataSource = _results;
            }
            /*if (results.Count > 0 && results.First().GetType() == typeof(HistoricalFigure))
{
   DataGridViewTextBoxColumn killCount = new DataGridViewTextBoxColumn();
   killCount.DataPropertyName = "NotableKills";
   killCount.HeaderText = "NotableKills";
   dgResults.Columns.Insert(killCount);
   dgResults.Columns.AddRange(
}*/
        }


        public List<object> Search(CriteriaLine criteria = null)
        {
            if (_selectProperties.SelectedIndex > 0)
            {
                _searchList.Select(_selectProperties.GetSelectedProperties());
            }
            /*if (SelectProperties.SelectedProperty.IsList)
   SearchList.Select(SelectProperties.SelectedProperty.Name, SelectProperties.SelectedProperty.Type.GetGenericArguments()[0]);
else
   SearchList.Select(SelectProperties.SelectedProperty.Name, SelectProperties.SelectedProperty.Type);*/
            else
            {
                _searchList.ResetSelect();
            }

            _searchList.Search(SelectionPanel.BuildQuery());
            _searchList.SubListSearch(SearchPanel.BuildQuery(criteria));
            if (criteria == null)
            {
                _searchList.OrderBy(OrderByPanel.BuildQuery());
            }

            return _searchList.GetResults();
        }

        public List<object> SearchSelection(CriteriaLine criteria = null)
        {
            if (_selectProperties.SelectedIndex > 0)
            {
                _searchList.Select(_selectProperties.GetSelectedProperties());
            }
            //SearchList.Select(SelectProperties.SelectedProperty.Name, SelectProperties.SelectedProperty.Type.GetGenericArguments()[0]);
            else
            {
                _searchList.ResetSelect();
            }

            _searchList.Search(SelectionPanel.BuildQuery(criteria));
            return _searchList.GetSelection();
        }


        private List<SearchInfo> BuildQuery(List<CriteriaLine> inputCriteria)
        {
            List<SearchInfo> criteria = new List<SearchInfo>();
            Type genericSearch = typeof(SearchInfo<>);
            foreach (CriteriaLine line in inputCriteria.Where(line => line.IsComplete()))
            {
                PropertyBox currentProperty = line.PropertySelect;
                Type searchType = genericSearch.MakeGenericType(line.PropertySelect.ParentType);
                SearchInfo newCriteria = Activator.CreateInstance(searchType) as SearchInfo;
                if (line == inputCriteria.First(line1 => line1.IsComplete()))
                {
                    newCriteria.Operator = QueryOperator.Or;
                }
                else
                {
                    newCriteria.Operator = QueryOperator.And;
                }

                criteria.Add(newCriteria);
                if (line.OrderByCriteria)
                {
                    if (line.OrderBySelect.Text == "Descending")
                    {
                        newCriteria.OrderByDescending = true;
                    }
                }

                while (currentProperty != null)
                {
                    
                    if (currentProperty.Child == null || currentProperty.Child.SelectedProperty == null)
                    {
                        if (currentProperty.SelectedProperty != null)
                        {
                            newCriteria.PropertyName = currentProperty.SelectedProperty.Name;
                        }

                        if (currentProperty.SelectedProperty != null && currentProperty.SelectedProperty.Type.IsGenericType && currentProperty.SelectedProperty.Type.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            newCriteria.Comparer = QueryComparer.Count;
                            newCriteria.Value = 0;
                            SearchInfo temp = newCriteria;
                            Type nextSearchType = genericSearch.MakeGenericType(currentProperty.SelectedProperty.Type.GetGenericArguments()[0]);
                            newCriteria = Activator.CreateInstance(nextSearchType) as SearchInfo;
                            temp.Next = newCriteria;
                            newCriteria.Previous = temp;
                            if (line.OrderByCriteria)
                            {
                                newCriteria.Comparer = QueryComparer.All;
                            }
                        }

                        if (newCriteria.Comparer != QueryComparer.All)
                        {
                            newCriteria.Comparer = SearchProperty.StringToComparer(line.ComparerSelect.Text);
                        }

                        if (currentProperty.SelectedProperty != null && currentProperty.SelectedProperty.Type == typeof(string))
                        {
                            if (newCriteria.Comparer == QueryComparer.Equals)
                            {
                                newCriteria.Comparer = QueryComparer.StringEquals;
                            }
                            else if (newCriteria.Comparer == QueryComparer.NotEqual)
                            {
                                newCriteria.Comparer = QueryComparer.StringNotEqual;
                            }
                        }

                        if (currentProperty.SelectedProperty != null && (currentProperty.SelectedProperty.Type == typeof(int) || currentProperty.SelectedProperty.Type == typeof(List<int>)))
                        {
                            newCriteria.Value = Convert.ToInt32(line.ValueSelect.Text);
                        }
                        else
                        {
                            newCriteria.Value = line.ValueSelect.Text;
                        }
                    }
                    else
                    {
                        newCriteria.Comparer = QueryComparer.Count;
                        newCriteria.PropertyName = currentProperty.SelectedProperty.Name;
                        SearchInfo temp = newCriteria;
                        Type nextSearchType;
                        if (currentProperty.Child.ParentType.IsGenericType)
                        {
                            nextSearchType = genericSearch.MakeGenericType(currentProperty.Child.ParentType.GetGenericArguments()[0]);
                        }
                        else
                        {
                            nextSearchType = genericSearch.MakeGenericType(currentProperty.Child.ParentType);
                        }

                        newCriteria = Activator.CreateInstance(nextSearchType) as SearchInfo;
                        temp.Next = newCriteria;
                        newCriteria.Previous = temp;
                    }
                    if (newCriteria.Previous != null)
                    {
                        newCriteria.Operator = QueryOperator.Or;
                    }

                    currentProperty = currentProperty.Child;
                }
            }
            return criteria;
        }

        private void listResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (listResults.SelectedItem is DwarfObject) Browser.LoadPageControl(listResults.SelectedItem);
        }

        private void dgResults_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //Browser.LoadPageControl(dgResults.SelectedRows[0].DataBoundItem);
        }

        private void dgResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                object navigateTo = dgResults.Rows[e.RowIndex].DataBoundItem;
                if (navigateTo.GetType() == typeof(HistoricalFigureLink))
                {
                    navigateTo = (navigateTo as HistoricalFigureLink).HistoricalFigure;
                }
                else if (navigateTo.GetType() == typeof(SiteLink))
                {
                    navigateTo = (navigateTo as SiteLink).Site;
                }

                Browser.Navigate(ControlOption.Html, navigateTo);
            }
        }

        private void dgResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Type objectType = dgResults.Rows[e.RowIndex].DataBoundItem.GetType();
            string column = dgResults.Columns[e.ColumnIndex].HeaderText;

            if (e.Value != null && e.Value.GetType().IsEnum)
            {
                e.Value = e.Value.GetDescription();
            }

            if (objectType == typeof(HistoricalFigure))
            {
                HistoricalFigure hf = dgResults.Rows[e.RowIndex].DataBoundItem as HistoricalFigure;
                if (column == "Kills")
                {
                    e.Value = hf.NotableKills.Count;
                }
                else if (column == "Abductions")
                {
                    e.Value = hf.Abductions.Count;
                }
                else if (column == "Battles")
                {
                    e.Value = hf.Battles.Count;
                }
                else if (column == "Beast Attacks")
                {
                    e.Value = hf.BeastAttacks.Count;
                }
                else if (column == "Age")
                {
                    int age;
                    string deathYear;
                    if (hf.DeathYear == -1)
                    {
                        age = World.Events.Last().Year - hf.BirthYear;
                        deathYear = "Present";
                    }
                    else
                    {
                        age = hf.DeathYear - hf.BirthYear;
                        deathYear = hf.DeathYear.ToString();
                    }

                    e.Value = age + " (" + hf.BirthYear + " - " + deathYear + ")";
                }
                else if (column == "Events")
                {
                    e.Value = hf.FilteredEvents.Count + " / " + hf.Events.Count;
                }
            }
            else if (objectType == typeof(Entity))
            {
                Entity entity = dgResults.Rows[e.RowIndex].DataBoundItem as Entity;
                if (column == "Name")
                {
                    e.Value = entity.ToString();
                    if (entity.IsCiv)
                    {
                        e.Value += " <Civ>";
                    }

                    e.Value += " (" + entity.Race + ")";
                }
                else if (column == "Sites")
                {
                    e.Value = entity.CurrentSites.Count;
                }
                else if (column == "Lost Sites")
                {
                    e.Value = entity.LostSites.Count;
                }
                else if (column == "Population")
                {
                    e.Value = entity.Populations.Sum(population => population.Count);
                }
                else if (column == "Wars")
                {
                    e.Value = entity.Wars.Count;
                }
                else if (column == "Wins : Losses")
                {
                    e.Value = entity.WarVictories + " / " + entity.WarLosses;
                }
                else if (column == "Kills : Deaths")
                {
                    e.Value = entity.WarKills + " / " + entity.WarDeaths;
                }
                else if (column == "Events")
                {
                    e.Value = entity.FilteredEvents.Count + " / " + entity.Events.Count;
                }
            }
            else if (objectType == typeof(Site))
            {
                Site site = dgResults.Rows[e.RowIndex].DataBoundItem as Site;
                if (column == "Owner") {
                    if (e.Value == null)
                    {
                        e.Value = "";
                    }
                    else
                    {
                        e.Value = site.CurrentOwner;
                        if (site.CurrentOwner is Entity)
                        {
                            e.Value += " (" + ((Entity)site.CurrentOwner).Race + ")";
                        }
                    }
                }
                else if (column == "Previous Owners")
                {
                    e.Value = site.PreviousOwners.Count;
                }
                else if (column == "Deaths")
                {
                    e.Value = site.Deaths.Count;
                }
                else if (column == "Warfare")
                {
                    e.Value = site.Warfare.Count;
                }
                else if (column == "Population")
                {
                    e.Value = site.Populations.Sum(population => population.Count);
                }
                else if (column == "Beast Attacks")
                {
                    e.Value = site.BeastAttacks.Count;
                }
                else if (column == "Events")
                {
                    e.Value = site.FilteredEvents.Count + " / " + site.Events.Count;
                }
            }
            else if (objectType == typeof(WorldRegion))
            {
                WorldRegion region = dgResults.Rows[e.RowIndex].DataBoundItem as WorldRegion;
                if (column == "Battles")
                {
                    e.Value = region.Battles.Count;
                }
                else if (column == "Deaths")
                {
                    e.Value = region.Deaths.Count;
                }
                else if (column == "Events")
                {
                    e.Value = region.FilteredEvents.Count + " / " + region.Events.Count;
                }
            }
            else if (objectType == typeof(UndergroundRegion))
            {
                UndergroundRegion uregion = dgResults.Rows[e.RowIndex].DataBoundItem as UndergroundRegion;
                if (column == "Events")
                {
                    e.Value = uregion.FilteredEvents.Count + " / " + uregion.Events.Count;
                }
            }
            else if (objectType == typeof(War))
            {
                War war = dgResults.Rows[e.RowIndex].DataBoundItem as War;
                //if (column == "Battles") e.Value = war.Battles;
                if (column == "Length")
                {
                    e.Value = war.StartYear + " - ";
                    if (war.EndYear == -1)
                    {
                        e.Value += "Present";
                    }
                    else
                    {
                        e.Value += war.EndYear.ToString();
                    }

                    e.Value += " (" + war.Length + ")";
                }
                else if (column == "Attacker")
                {
                    e.Value = war.Attacker + " (" + war.Attacker.Race + ")";
                }
                else if (column == "Defender")
                {
                    e.Value = war.Defender + " (" + war.Defender.Race + ")";
                }
                else if (column == "Kills")
                {
                    e.Value = war.DefenderDeathCount + " / " + war.AttackerDeathCount;
                }
                else if (column == "Victories")
                {
                    e.Value = war.AttackerBattleVictories.Count + " / " + war.DefenderBattleVictories.Count;
                }
                else if (column == "Sites Lost")
                {
                    e.Value = war.DefenderConquerings.Count(conquering => conquering.Notable) + " / " + war.AttackerConquerings.Count(conquering => conquering.Notable);
                }
                else if (column == "Events")
                {
                    e.Value = war.FilteredEvents.Count + " / " + war.AllEvents.Count;
                }
            }
            else if (objectType == typeof(Battle))
            {
                Battle battle = dgResults.Rows[e.RowIndex].DataBoundItem as Battle;
                if (column == "Attacker")
                {
                    e.Value = battle.Attacker + " (" + battle.Attacker.Race + ")";
                }
                else if (column == "Defender")
                {
                    e.Value = battle.Defender + " (" + battle.Defender.Race + ")";
                }
                else if (column == "Deaths")
                {
                    e.Value = battle.DeathCount;
                }
                else if (column == "Remaining")
                {
                    e.Value = battle.AttackersRemainingCount + " / " + battle.DefendersRemainingCount;
                }
                else if (column == "Conquering")
                {
                    if (battle.Conquering == null)
                    {
                        e.Value = "";
                    }
                    else
                    {
                        e.Value = battle.Conquering.ToString();
                    }
                }
                else if (column == "Events")
                {
                    e.Value = battle.FilteredEvents.Count + " / " + battle.AllEvents.Count;
                }
            }
            else if (objectType == typeof(SiteConquered))
            {
                SiteConquered conquering = dgResults.Rows[e.RowIndex].DataBoundItem as SiteConquered;
                if (column == "Name")
                {
                    e.Value = conquering.ToString();
                }
                else if (column == "Deaths")
                {
                    e.Value = conquering.Deaths.Count;
                }
                else if (column == "Events")
                {
                    e.Value = conquering.FilteredEvents.Count + " / " + conquering.AllEvents.Count;
                }
            }
            else if (objectType == typeof(BeastAttack))
            {
                BeastAttack attack = dgResults.Rows[e.RowIndex].DataBoundItem as BeastAttack;
                if (column == "Name")
                {
                    e.Value = attack.ToString();
                }
                else if (column == "Deaths")
                {
                    e.Value = attack.Deaths.Count;
                }
                else if (column == "Events")
                {
                    e.Value = attack.FilteredEvents.Count + " / " + attack.AllEvents.Count;
                }
            }
            else if (objectType == typeof(Artifact))
            {
                Artifact artifact = dgResults.Rows[e.RowIndex].DataBoundItem as Artifact;
                if (column == "Events")
                {
                    e.Value = artifact.Events.Count;
                }
            }

            //if (objectType.BaseType == typeof(WorldObject) && column == "Events")
            //    e.Value = (dgResults.Rows[e.RowIndex].DataBoundItem as WorldObject).Events.Count;
            //if (objectType.BaseType == typeof(EventCollection) && column == "Events")
            //    e.Value = (dgResults.Rows[e.RowIndex].DataBoundItem as EventCollection).AllEvents.Count;

            dgResults.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 1).ToString();
        }

        private void dgResults_MouseEnter(object sender, EventArgs e)
        {
            dgResults.Focus();
        }

        private void btnMapResults_Click(object sender, EventArgs e)
        {
            if (dgResults.Rows.Count > 0)
            {
                Browser.Navigate(ControlOption.Map, _results);
            }
        }
    }

    public class SearchControl : PageControl
    {
        public QueryControl QueryControl;
        public SearchControl(DwarfTabControl browser)
        {
            TabControl = browser;
            Title = "Advanced Search";
        }

        public override Control GetControl()
        {
            if (QueryControl == null)
            {
                QueryControl = new QueryControl(TabControl.World, TabControl)
                {
                    Dock = DockStyle.Fill
                };
            }
            return QueryControl;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                QueryControl.Dispose();
            }
        }

        public override void Refresh()
        {

        }

    }
}

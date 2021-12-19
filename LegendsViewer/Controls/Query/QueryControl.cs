using System;
using System.Collections;
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
        private SearchList _searchList;
        private PropertyBox _selectProperties;
        public DwarfTabControl Browser;
        public World World;
        private List<object> _results;
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
            _selectProperties?.Remove();

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
                case "Structures":
                    _searchList = new SearchList<Structure>(World.Structures);
                    _selectProperties.ParentType = typeof(Structure);
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
                case "Rampages":
                    _searchList = new SearchList<BeastAttack>(World.BeastAttacks);
                    _selectProperties.ParentType = typeof(BeastAttack);
                    break;
                case "Raids":
                    _searchList = new SearchList<Raid>(World.EventCollections.OfType<Raid>().ToList());
                    _selectProperties.ParentType = typeof(Raid);
                    break;
                case "Artifacts":
                    _searchList = new SearchList<Artifact>(World.Artifacts);
                    _selectProperties.ParentType = typeof(Artifact);
                    break;
                case "Written Content":
                    _searchList = new SearchList<WrittenContent>(World.WrittenContents);
                    _selectProperties.ParentType = typeof(WrittenContent);
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
        }

        private void SelectSubListChanged(object sender, EventArgs e)
        {
            if (_selectProperties.SelectedIndex > 0)
            {
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
                btnMapResults.Visible = selectedType == typeof(Site) || selectedType == typeof(Battle);
            }
            else
            {
                btnMapResults.Visible = _selectProperties.ParentType == typeof(Site) || _selectProperties.ParentType == typeof(Battle);
            }
        }

        private void PanelResized(object sender, EventArgs e)
        {
            SelectionPanel.Top = SelectList.Bottom + 3;
            SearchPanel.Top = SelectionPanel.Visible ? SelectionPanel.Bottom : SelectList.Bottom + 3;

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

            var columns = AppHelpers.GetColumns(_selectProperties.SelectedProperty == null
                ? _selectProperties.ParentType
                : _selectProperties.SelectedProperty.Type);

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
        }

        public List<object> Search(CriteriaLine criteria = null)
        {
            if (_selectProperties.SelectedIndex > 0)
            {
                _searchList.Select(_selectProperties.GetSelectedProperties());
            }
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
                newCriteria.Operator = line == inputCriteria.First(line1 => line1.IsComplete()) ? QueryOperator.Or : QueryOperator.And;

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

                        if (currentProperty.SelectedProperty?.Type.IsGenericType == true && currentProperty.SelectedProperty.Type.GetGenericTypeDefinition() == typeof(List<>))
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

                        newCriteria.Value = currentProperty.SelectedProperty != null && (currentProperty.SelectedProperty.Type == typeof(int) || currentProperty.SelectedProperty.Type == typeof(List<int>))
                            ? Convert.ToInt32(line.ValueSelect.Text)
                            : (object)line.ValueSelect.Text;
                    }
                    else
                    {
                        newCriteria.Comparer = QueryComparer.Count;
                        newCriteria.PropertyName = currentProperty.SelectedProperty.Name;
                        SearchInfo temp = newCriteria;
                        Type nextSearchType = currentProperty.Child.ParentType.IsGenericType
                            ? genericSearch.MakeGenericType(currentProperty.Child.ParentType.GetGenericArguments()[0])
                            : genericSearch.MakeGenericType(currentProperty.Child.ParentType);
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
            Type objectType = dgResults.Rows[e.RowIndex].DataBoundItem?.GetType();
            if (objectType == null)
            {
                return;
            }

            if (e.Value != null)
            {
                if (e.Value.GetType().IsEnum)
                {
                    e.Value = e.Value.GetDescription();
                }
                else if (e.Value is IList list)
                {
                    e.Value = list.Count;
                }
            }

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

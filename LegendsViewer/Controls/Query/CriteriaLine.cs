using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LegendsViewer.Controls.Query
{
    public class CriteriaLine : Control
    {
        public static int LineHeight = 21;
        public ComboBox QueryOperatorSelect = new ComboBox();
        public PropertyBox PropertySelect = new PropertyBox();
        public ComboBox ComparerSelect = new ComboBox();
        public ComboBox ValueSelect = new ComboBox();
        public ComboBox OrderBySelect = new ComboBox();
        public Button Insert = new Button();
        public Button Remove = new Button();
        public bool OrderByCriteria;
        public bool SelectCriteria;
        public bool SearchCriteria;

        public CriteriaLine(bool select, bool search, bool order)
        {
            SelectCriteria = select;
            SearchCriteria = search;
            OrderByCriteria = order;

            //BackColor = Color.White;

            Insert.Text = "Insert"; Insert.Width = 40; Insert.Height = 19; Insert.FlatStyle = FlatStyle.Flat; Insert.Font = new Font("Arial", 6.5f);
            Insert.FlatAppearance.BorderSize = 0;

            Remove.Text = "Remove"; Remove.Width = 51; Remove.Height = 19; Remove.FlatStyle = FlatStyle.Flat; Remove.Font = new Font("Arial", 6.5f);
            Remove.FlatAppearance.BorderSize = 0;

            QueryOperatorSelect.Items.AddRange(new object[] { QueryOperator.And, QueryOperator.Or });
            QueryOperatorSelect.Width = 45;
            QueryOperatorSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            QueryOperatorSelect.SelectedIndex = 0;
            QueryOperatorSelect.SelectedIndexChanged += delegate
            {
                GetValueOptions();
                object value = ValueSelect.SelectedItem;
                (Parent as CriteriaPanel).UpdateValueSelects(this);
                ValueSelect.SelectedItem = value;
            };
            //QueryOperatorSelect.Visible = false;

            PropertySelect.SelectedIndexChanged += OnPropertyChange;
            //PropertySelect.FlatStyle = FlatStyle.Flat;

            ComparerSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            ComparerSelect.Width = 115;
            ComparerSelect.ForeColor = Color.Green;
            //ComparerSelect.FlatStyle = FlatStyle.Popup;
            //ComparerSelect.Font = new System.Drawing.Font("MS Sans Serif", 8, FontStyle.Bold);
            ComparerSelect.FormattingEnabled = true;
            ComparerSelect.Format += delegate (object sender, ListControlConvertEventArgs e)
            {
                e.Value = SearchProperty.ComparerToString((QueryComparer)e.Value);
            };
            GetComparers();

            //ValueSelect.FlatStyle = FlatStyle.Flat;
            ValueSelect.Width = 175;
            ValueSelect.TextChanged += delegate
            {
                (Parent as CriteriaPanel).UpdateValueSelects(this);
            };
            ValueSelect.SelectedIndexChanged += delegate
            {
                (Parent as CriteriaPanel).UpdateValueSelects(this);
            };
            ValueSelect.FormattingEnabled = true;
            ValueSelect.Format += delegate (object sender, ListControlConvertEventArgs e)
            {
                //if (e.ListItem.GetType() == typeof(DeathCause))
                e.Value = e.ListItem.GetDescription();
            };

            OrderBySelect.Items.AddRange(new object[] { "Ascending", "Descending" });
            OrderBySelect.Width = 83;
            OrderBySelect.DropDownStyle = ComboBoxStyle.DropDownList;
            OrderBySelect.SelectedIndex = 0;
            //OrderBySelect.FlatStyle = FlatStyle.Flat;

            if (OrderByCriteria)
            {
                Controls.AddRange(new Control[] { PropertySelect, OrderBySelect, Insert, Remove });
            }
            else
            {
                Controls.AddRange(new Control[] { QueryOperatorSelect, PropertySelect, ComparerSelect, ValueSelect, Insert, Remove });
            }

            Height = LineHeight;
        }

        private void OnPropertyChange(object sender, EventArgs e)
        {
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            ResizeSelf();
        }

        public void ResizeSelf()
        {
            QueryOperatorSelect.Location = new Point(0, 0);
            PropertySelect.Location = new Point(QueryOperatorSelect.Right + 3, 0);
            ComparerSelect.Location = new Point(PropertySelect.GetRightSide() + 3, 0);
            ValueSelect.Location = new Point(ComparerSelect.Right + 3, 0);

            if (OrderByCriteria && Controls.Contains(ValueSelect))
            {
                OrderBySelect.Location = new Point(ValueSelect.Right + 3, 0);
            }
            else if (OrderByCriteria && Controls.Contains(ComparerSelect))
            {
                OrderBySelect.Location = new Point(ComparerSelect.Right + 3, 0);
            }
            else if (OrderByCriteria)
            {
                OrderBySelect.Location = new Point(PropertySelect.GetRightSide() + 3, 0);
            }

            Insert.Location = OrderByCriteria ? new Point(OrderBySelect.Right + 15, 0) : new Point(ValueSelect.Right + 15, 0);

            Remove.Location = new Point(Insert.Right + 3, 0);

            Width = Remove.Right;
            if (Parent != null)
            {
                (Parent as CriteriaPanel).AutoResize();
            }
        }

        public void GetComparers()
        {
            if (OrderByCriteria)
            {
                if (PropertySelect.Child?.SelectedProperty != null && PropertySelect.ContainsList() && !PropertySelect.ContainsListLast())
                {
                    ComparerSelect.SelectedIndexChanged += ComparerChanged;
                    Controls.AddRange(new Control[] { ComparerSelect, ValueSelect });
                    ValueSelect.Items.Clear();
                    ValueSelect.Text = "";
                }
                else
                {
                    ComparerSelect.SelectedIndexChanged += null;
                    Controls.Remove(ComparerSelect);
                    Controls.Remove(ValueSelect);
                }
            }
            ComparerSelect.Items.Clear();
            Type propertyType = PropertySelect.GetLowestPropertyType();
            List<QueryComparer> comparers = SearchProperty.GetComparers(propertyType);
            if (OrderByCriteria && PropertySelect.Child != null && (propertyType == typeof(int) || propertyType == typeof(double) || propertyType.IsGenericType))
            {
                comparers.AddRange(new List<QueryComparer> { QueryComparer.Min, QueryComparer.Max, QueryComparer.Average, QueryComparer.Sum });
            }

            ComparerSelect.Items.AddRange(comparers.Cast<object>().ToArray());
            //foreach (QueryComparer comparer in comparers)
            //    ComparerSelect.Items.Insert(SearchProperty.ComparerToString(comparer));
            if (comparers.Count > 0)
            {
                ComparerSelect.SelectedIndex = 0;
            }
        }

        private void ComparerChanged(object sender, EventArgs e)
        {
            if (ComparerSelect.SelectedIndex >= 0)
            {
                QueryComparer comparer = (QueryComparer)ComparerSelect.SelectedItem;
                if (comparer == QueryComparer.Min || comparer == QueryComparer.Max || comparer == QueryComparer.Average || comparer == QueryComparer.Sum)
                {
                    Controls.Remove(ValueSelect);
                }
                else if (!Controls.Contains(ValueSelect) && PropertySelect.Child?.SelectedProperty != null && !PropertySelect.ContainsListLast())
                {
                    Controls.Add(ValueSelect);
                }
            }
        }

        public void GetValueOptions()
        {
            object previousSelection = ValueSelect.SelectedItem;
            ValueSelect.Items.Clear();
            ValueSelect.DropDownStyle = ComboBoxStyle.DropDown;
            Type selectedType = PropertySelect.GetLowestPropertyType();
            SearchProperty selected = PropertySelect.GetLowestProperty();
            if (selected == null)
            {
                return;
            }

            if (selectedType == typeof(bool))
            {
                ValueSelect.Items.Add(true);
                ValueSelect.Items.Add(false);
            }
            //else if (selectedType == typeof(DeathCause))
            //{
            //ValueSelect.Items.AddRange(Enum.GetValues(typeof(DeathCause)).Cast<object>().ToArray());
            //    ValueSelect.Items.AddRange(World.DeathCauses.Cast<object>().ToArray());
            //}
            //else if (selectedType == typeof(SiteConqueredType))
            //{
            //    ValueSelect.Items.AddRange(Enum.GetValues(typeof(SiteConqueredType)).Cast<object>().OrderBy(type => type.GetDescription()).ToArray());
            //}
            //else if (selectedType == typeof(BattleOutcome))
            //{
            //    ValueSelect.Items.AddRange(Enum.GetValues(typeof(BattleOutcome)).Cast<object>().OrderBy(outcome => outcome.GetDescription()).ToArray());
            //}
            //else if (selectedType == typeof(HFState))
            //{
            //    ValueSelect.Items.AddRange(Enum.GetValues(typeof(HFState)).Cast<object>().OrderBy(state => state.GetDescription()).ToArray());
            //}
            else //if (!selected.Type.IsGenericType)// && selected.Type != typeof(int) && selected.Type != typeof(double))// && PropertySelect.GetLowestProperty().Name != "Name")
            {
                IEnumerable<object> options = SelectCriteria ? (Parent.Parent as QueryControl).SearchSelection(this) : (Parent.Parent as QueryControl).Search(this);
                SearchInfo available = BuildSearchInfo(true);
                if (available != null)
                {
                    options = available.Select(options);
                    options = options.Where(o => o != null).GroupBy(option => option).Select(option => option.Key);
                    options = options.FirstOrDefault() != null && (options.First().GetType() == typeof(int) || options.First().GetType() == typeof(double))
                        ? options.OrderBy(option => option)
                        : (IEnumerable<object>)options.OrderBy(option => option.GetDescription()).ToList();

                    ValueSelect.Items.AddRange(options.ToArray());
                }
            }

            if (selectedType == typeof(bool) || selectedType.IsEnum)
            {
                ValueSelect.DropDownStyle = ComboBoxStyle.DropDownList;
                if (ValueSelect.Items.Count > 0)
                {
                    ValueSelect.SelectedIndex = 0;
                }
            }

            if (PropertySelect.GetLowestProperty().Name == "Name")
            {
                ValueSelect.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                ValueSelect.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            else
            {
                ValueSelect.AutoCompleteMode = AutoCompleteMode.None;
            }

            if (previousSelection != null && ValueSelect.Items.Contains(previousSelection))
            {
                ValueSelect.SelectedItem = previousSelection;
            } (Parent as CriteriaPanel).UpdateValueSelects(this);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            OnLocationChanged(e);
            base.OnControlAdded(e);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            OnLocationChanged(e);
            base.OnControlRemoved(e);
        }

        public bool IsComplete()
        {
            if (!OrderByCriteria)
            {
                return PropertySelect.SelectedIndex >= 0 && ComparerSelect.SelectedIndex >= 0 && ValueSelect.Text != "";
            }

            bool complete = PropertySelect.SelectedIndex >= 0 && (PropertySelect.GetLowestPropertyType().IsGenericType || PropertySelect.GetLowestPropertyType() == typeof(int) || PropertySelect.GetLowestPropertyType() == typeof(double) || PropertySelect.GetLowestPropertyType() == typeof(string) || PropertySelect.GetLowestPropertyType().IsEnum);
            if (Controls.Contains(ComparerSelect) && Controls.Contains(ValueSelect))
            {
                complete = complete && ComparerSelect.SelectedIndex >= 0 && ValueSelect.Text != "";
            }

            return complete;
        }

        public SearchInfo BuildSearchInfo(bool gettingValuesOnly = false)
        {
            if (!OrderByCriteria && !(PropertySelect.SelectedIndex >= 0 && ComparerSelect.SelectedIndex >= 0))
            {
                return null;
            }

            if (OrderByCriteria && Controls.Contains(ComparerSelect) && ComparerSelect.SelectedIndex < 0)
            {
                return null;
            }

            SearchInfo criteria = SearchInfo.Make(PropertySelect.ParentType);

            criteria.Operator = (QueryOperator)QueryOperatorSelect.SelectedItem;

            //build property
            PropertySelect.SetupSearchProperties(criteria);

            //build comparer
            if (OrderByCriteria)
            {
                if (OrderBySelect.Text == "Descending")
                {
                    criteria.OrderByDescending = true;
                }

                if (Controls.Contains(ComparerSelect))
                {
                    criteria.SetupOrderByComparers((QueryComparer)ComparerSelect.SelectedItem);
                }
                else if (PropertySelect.GetLowestPropertyType().IsGenericType)
                {
                    criteria.SetupOrderByComparers(QueryComparer.All);
                }
            }
            else
            {
                criteria.SetupComparers((QueryComparer)ComparerSelect.SelectedItem);//  SearchProperty.StringToComparer(ComparerSelect.Text));
            }

            //build value
            if (PropertySelect.GetLowestPropertyType() == typeof(int) || PropertySelect.GetLowestPropertyType() == typeof(List<int>) || PropertySelect.GetLowestPropertyType().IsGenericType)
            {
                if (Controls.Contains(ValueSelect) && !gettingValuesOnly)
                {
                    try
                    {
                        criteria.SetupValue(Convert.ToInt32(ValueSelect.Text));
                    }
                    catch { return null; }
                }
                else
                {
                    criteria.SetupValue(0);
                }
            }
            else if (PropertySelect.GetLowestPropertyType() == typeof(double) || PropertySelect.GetLowestPropertyType() == typeof(List<double>))
            {
                if (Controls.Contains(ValueSelect) && !gettingValuesOnly)
                {
                    try
                    {
                        criteria.SetupValue(double.Parse(ValueSelect.Text));// Convert.ToDouble(ValueSelect.Text));
                    }
                    catch { return null; }
                }
            }
            else if (ValueSelect.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                criteria.SetupValue(ValueSelect.SelectedItem);
            }
            else
            {
                criteria.SetupValue(ValueSelect.Text);
            }

            return criteria;
        }
    }
}

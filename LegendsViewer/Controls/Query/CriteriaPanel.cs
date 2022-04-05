using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LegendsViewer.Controls.Query
{
    public class CriteriaPanel : Panel
    {
        public List<CriteriaLine> Criteria = new List<CriteriaLine>();
        public int CriteriaStartLocation;
        public Type CriteriaType;
        public Button Add = new Button();
        public bool OrderByCriteria;
        public bool SelectCriteria;
        public bool SearchCriteria;

        public CriteriaPanel()
        {
            Add.Text = "Add Criteria"; Add.Width = 90; Add.Height = 19; Add.FlatStyle = FlatStyle.Flat; Add.Font = new Font("Arial", 6.5f);
            Add.Click += AddClick;
            Add.Visible = false;
            //Insert.Location = new Point(10, CriteriaStartLocation + 3);
            Controls.Add(Add);
            DoubleBuffered = true;
        }

        private void AddClick(object sender, EventArgs e)
        {
            AddNew(Criteria.IndexOf((sender as Button)?.Parent as CriteriaLine));
        }
        public void AddNew(int index = -1)
        {
            CriteriaLine criteria = new CriteriaLine(SelectCriteria, SearchCriteria, OrderByCriteria);
            if (index >= 0)
            {
                //index++;
                for (int i = index; i < Criteria.Count; i++)
                {
                    CriteriaLine shiftCriteria = Criteria[i];
                    shiftCriteria.Location = new Point(shiftCriteria.Location.X, shiftCriteria.Location.Y + shiftCriteria.Height);
                }
                criteria.Location = new Point(3, CriteriaStartLocation + CriteriaLine.LineHeight * index);
                Criteria.Insert(index, criteria);
            }
            else
            {
                criteria.Location = new Point(3, CriteriaStartLocation + CriteriaLine.LineHeight * Criteria.Count);
                Criteria.Add(criteria);
            }
            criteria.Remove.Click += RemoveClick;
            criteria.Insert.Click += AddClick;

            if (CriteriaType != null)
            {
                criteria.PropertySelect.ParentType = CriteriaType;
            }

            Controls.Add(criteria);
            Add.Location = new Point(10, Criteria.Last().Bottom + 3);
            Add.Visible = true;
            Criteria[0].QueryOperatorSelect.Visible = false;
            if (Criteria.Count > 1)
            {
                Criteria[1].QueryOperatorSelect.Visible = true;
            }

            AutoResize();
        }

        public void RemoveClick(object sender, EventArgs e)
        {
            Remove((sender as Button)?.Parent as CriteriaLine);
        }
        public void Remove(CriteriaLine criteria)
        {
            int index = Criteria.IndexOf(criteria);
            //if (index == 0 && Criteria.Count == 1) return;
            for (int i = index; i < Criteria.Count; i++)
            {
                CriteriaLine shiftCritera = Criteria[i];
                shiftCritera.Location = new Point(shiftCritera.Location.X, shiftCritera.Location.Y - CriteriaLine.LineHeight);
            }
            Criteria.Remove(criteria);
            Controls.Remove(criteria);
            if (Criteria.Count == 0)
            {
                AddNew();
            }

            Criteria[0].QueryOperatorSelect.Visible = false;
            Criteria[0].QueryOperatorSelect.SelectedItem = QueryOperator.And;
            Add.Location = new Point(10, Criteria.Last().Bottom + 3);
            AutoResize();
            UpdateValueSelects(index);
        }

        public void Clear()
        {
            foreach (CriteriaLine criteria in Criteria)
            {
                Controls.Remove(criteria);
            }

            Criteria.Clear();
            AutoResize();
        }

        public void AutoResize()
        {
            int panelHeight = 0;
            int panelWidth = 0;
            IEnumerator selectionEnumerator = Controls.GetEnumerator();
            while (selectionEnumerator.MoveNext())
            {
                if ((selectionEnumerator.Current as Control)?.Bottom > panelHeight)
                {
                    panelHeight = (selectionEnumerator.Current as Control)?.Bottom ?? 0;
                }

                if ((selectionEnumerator.Current as Control)?.Right > panelWidth)
                {
                    panelWidth = (selectionEnumerator.Current as Control)?.Right ?? 0;
                }
            }
            Height = panelHeight + 3;
            Width = panelWidth + 3;
        }

        public void UpdateValueSelects(CriteriaLine criteriaAfter)
        {
            foreach (CriteriaLine line in Criteria.Where(criteria => Criteria.IndexOf(criteria) > Criteria.IndexOf(criteriaAfter) || Criteria.IndexOf(criteria) < 0))
            {
                line.GetValueOptions();
            }
            if (SelectCriteria)
            {
                (Parent as QueryControl)?.SearchPanel.UpdateAllValueSelects();
            }
        }

        public void UpdateValueSelects(int index)
        {
            foreach (CriteriaLine line in Criteria.Where(criteria => Criteria.IndexOf(criteria) > index - 1))
            {
                line.GetValueOptions();
            }

            if (SelectCriteria)
            {
                (Parent as QueryControl)?.SearchPanel.UpdateAllValueSelects();
            }
        }

        public void UpdateAllValueSelects()
        {
            foreach (CriteriaLine line in Criteria)
            {
                line.GetValueOptions();
            }

            if (SelectCriteria)
            {
                (Parent as QueryControl)?.SearchPanel.UpdateAllValueSelects();
            }
        }

        public List<SearchInfo> BuildQuery(CriteriaLine breakCriteria = null)
        {
            List<SearchInfo> query = new List<SearchInfo>();

            if (breakCriteria?.QueryOperatorSelect.Text == "OR")
            {
                for (int i = Criteria.IndexOf(breakCriteria); i >= 0; i--)
                {
                    if (Criteria[i].QueryOperatorSelect.Text == "AND" || i == 0)
                    {
                        breakCriteria = Criteria[i];
                        break;
                    }
                }
            }

            foreach (CriteriaLine line in Criteria.Where(line => line.IsComplete() || line == breakCriteria))
            {
                if (line == breakCriteria)
                {
                    break;
                }

                SearchInfo criteria = line.BuildSearchInfo();
                if (criteria != null)
                {
                    query.Add(criteria);
                    //criteria.Operator = QueryOperator.And;
                }
            }
            if (query.Count > 0)
            {
                query[0].Operator = QueryOperator.Or;
            }

            return query;
        }
    }
}

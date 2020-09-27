using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LegendsViewer.Controls.Query
{
    public abstract class SearchList
    {
        public SearchList SubList;
        public abstract Type GetListType();
        public abstract Type GetMainListType();
        public abstract void Select(List<PropertyInfo> properties);
        public abstract void SetupList<TU>(List<TU> mainList, List<PropertyInfo> properties);
        public abstract void SubListSearch(List<SearchInfo> searchCriteria);
        public abstract void ResetSelect();
        public abstract void Search(List<SearchInfo> searchCriteria);
        public abstract void OrderBy(List<SearchInfo> orderCriteria);
        public abstract List<object> GetResults();
        public abstract List<object> GetSelection();
    }

    public class SearchList<T> : SearchList
    {
        public List<T> List;
        public List<T> BaseList;
        public List<PropertyInfo> SubListProperties { get; set; }

        public SearchList()
        {
            List = new List<T>();
        }

        public SearchList(List<T> list) : this()
        {
            BaseList = list;
        }

        public override Type GetListType()
        {
            if (SubList != null)
            {
                return SubList.GetListType();
            }

            return typeof(T);
        }

        public override Type GetMainListType()
        {
            return typeof(T);
        }

        public override void Select(List<PropertyInfo> properties)
        {
            SubListProperties = properties;
            Type genericSearchList = typeof(SearchList<>);
            Type subListSearchList;
            Type selectProperty = properties.Last().PropertyType;
            if (selectProperty.IsGenericType)
            {
                subListSearchList = genericSearchList.MakeGenericType(selectProperty.GetGenericArguments()[0]);
            }
            else
            {
                subListSearchList = genericSearchList.MakeGenericType(selectProperty);
            }

            SubList = Activator.CreateInstance(subListSearchList) as SearchList;
            SubList.SetupList(List, properties);
        }

        public override void SetupList<TU>(List<TU> mainList, List<PropertyInfo> properties)
        {
            BaseList = new List<T>();
            IQueryable selection = mainList.AsQueryable();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsGenericType)
                {
                    selection = mainList.AsQueryable().SelectMany(property.Name);
                }
                else
                {
                    selection = mainList.AsQueryable().Select(property.Name);
                }
            }

            IEnumerator list = selection.GetEnumerator();
            while (list.MoveNext())
            {
                BaseList.Add((T)list.Current);
            }

            List = BaseList;
        }

        private string GetSelectString(List<PropertyInfo> properties)
        {
            string selectString = "";
            foreach (PropertyInfo property in properties)
            {
                selectString += property.Name;
                if (!(properties.Last() == property))
                {
                    selectString += ".";
                }
            }
            return selectString;
        }

        public override void ResetSelect()
        {
            SubList = null;
        }

        public override void SubListSearch(List<SearchInfo> searchCriteria)
        {
            if (SubList != null)
            {
                SubList.SubListSearch(searchCriteria);
            }
            else
            {
                Search(searchCriteria);
            }
        }

        public override void Search(List<SearchInfo> searchCriteria)
        {
            try
            {
                Expression<Func<T, bool>> predicate;
                if (searchCriteria.Count > 0)
                {
                    predicate = t => false;
                }
                else
                {
                    predicate = t => true;
                }

                foreach (SearchInfo criteria in searchCriteria)
                {
                    var where = criteria.GetPredicateExpression() as Expression<Func<T, bool>>;
                    predicate = criteria.Operator == QueryOperator.And
                        ? predicate.And(where)
                        : predicate.Or(where);
                }
                var compiled = predicate.Compile();
                var search = BaseList.Where(compiled);
                List = search.ToList();
            }
            catch (Exception)
            {
                List = new List<T>();
            }
            SubList?.SetupList(List, SubListProperties);
        }

        public override void OrderBy(List<SearchInfo> orderCriteria)
        {
            if (SubList != null)
            {
                SubList.OrderBy(orderCriteria);
            }
            else
            {
                for (int i = orderCriteria.Count - 1; i >= 0; i--)
                {
                    SearchInfo criteria = orderCriteria[i];
                    if (criteria.Next == null || !criteria.ContainsListProperties())
                    {
                        if (criteria.OrderByDescending && criteria.PropertyString() != "Value")
                        {
                            List = List.AsQueryable().OrderBy(criteria.PropertyString() + " DESC").ToList();
                        }
                        else if (criteria.PropertyName == "Value")
                        {
                            if (criteria.OrderByDescending)
                            {
                                List = List.OrderByDescending(value => value).ToList();
                            }
                            else
                            {
                                List = List.OrderBy(value => value).ToList();
                            }
                        }
                        else
                        {
                            List = List.AsQueryable().OrderBy(criteria.PropertyString()).ToList();
                        }
                    }
                    else if (criteria.ContainsListPropertyLast() || criteria.Next.Comparer == QueryComparer.All)
                    {
                        if (criteria.OrderByDescending)
                        {
                            List = List.AsQueryable().OrderBy(criteria.PropertyString() + ".Count DESC").ToList();
                        }
                        else
                        {
                            List = List.AsQueryable().OrderBy(criteria.PropertyString() + ".Count").ToList();
                        }
                    }
                    else
                    {
                        Expression<Func<T, bool>> predicate = t => false;

                        if (criteria.OrderByDescending)
                        {
                            switch (criteria.Comparer)
                            {
                                case QueryComparer.Min: List = List.OrderByDescending(t => criteria.Select(t).Min()).ToList(); break;
                                case QueryComparer.Max: List = List.OrderByDescending(t => criteria.Select(t).Max()).ToList(); break;
                                case QueryComparer.Average:
                                    List = List.OrderByDescending(t => criteria.Select(t).Select(t1 => Convert.ToDouble(t1)).AverageOrZero()).ToList();
                                    //List = List.OrderByDescending(t => criteria.Select(t)
                                    //Expression<Func<T, bool>> notEmpty = t => criteria.Select(t).Any();
                                    //List = List.Where(notEmpty.Compile()).OrderByDescending(t => criteria.Select(t).Select(t1 => Convert.ToDouble(t1)).Average()).ToList();
                                    break;
                                case QueryComparer.Sum: List = List.OrderByDescending(t => criteria.Select(t).Select(t1 => Convert.ToDouble(t1)).Sum()).ToList(); break;
                                default: List = List.OrderByDescending(t => criteria.GetCount(t)).ToList(); break;
                            }
                        }
                        else
                        {
                            switch (criteria.Comparer)
                            {
                                case QueryComparer.Min: List = List.OrderBy(t => criteria.Select(t).Min()).ToList(); break;
                                case QueryComparer.Max: List = List.OrderBy(t => criteria.Select(t).Max()).ToList(); break;
                                case QueryComparer.Average: List = List.OrderBy(t => criteria.Select(t).Select(t1 => Convert.ToDouble(t1)).AverageOrZero()).ToList(); break;
                                case QueryComparer.Sum: List = List.OrderBy(t => criteria.Select(t).Select(t1 => Convert.ToDouble(t1)).Sum()).ToList(); break;
                                default: List = List.OrderBy(t => criteria.GetCount(t)).ToList(); break;
                            }
                        }

                    }
                }
            }
        }

        public override List<object> GetResults()
        {
            if (SubList != null)
            {
                return SubList.GetResults();
            }

            return List.Cast<object>().ToList();
        }

        public override List<object> GetSelection()
        {
            return List.Cast<object>().ToList();
        }
    }

}

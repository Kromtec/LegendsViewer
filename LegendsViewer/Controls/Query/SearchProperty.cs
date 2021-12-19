using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using LegendsViewer.Controls.Query.Attributes;
using LegendsViewer.Legends;
using LegendsViewer.Legends.EventCollections;
using LegendsViewer.Legends.Events;

namespace LegendsViewer.Controls.Query
{
    public class SearchProperty
    {
        public string Name;
        public string Description;
        public bool IsList;
        public bool IsSelectable;
        public Type Type;
        public List<SearchProperty> SubProperties = new List<SearchProperty>();
        public SearchProperty(string name, Type type) : this(name, name, type)
        {
        }

        public SearchProperty(string name, string description, Type type, bool selectable = false)
        {
            Name = name;
            Description = description;
            Type = type;
            if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(List<>))
            {
                IsList = true;
            }
            IsSelectable = selectable;
        }

        public override string ToString()
        {
            return Description;
        }

        public void SetSubProperties()
        {
            SubProperties = GetProperties(Type, true);
        }

        public static List<SearchProperty> GetProperties(Type searchType, bool noSubProperties = false)
        {
            Type nonGenericSearchType = searchType.IsGenericType && searchType != typeof(List<int>) && searchType != typeof(List<string>)
                ? searchType.GetGenericArguments()[0]
                : searchType;
            List<SearchProperty> searchProperties = getSearchPropertiesByType(nonGenericSearchType);

            if (searchType == typeof(List<int>))
            {
                searchProperties = new List<SearchProperty> { new SearchProperty("Value", typeof(int)) };
            }
            else if (searchType == typeof(List<string>))
            {
                searchProperties = new List<SearchProperty> { new SearchProperty("Value", typeof(string)) };
            }

            if (nonGenericSearchType.BaseType == typeof(WorldObject))
            {
                searchProperties.Add(new SearchProperty(nameof(WorldObject.Events), typeof(List<WorldEvent>)));
                searchProperties.Add(new SearchProperty(nameof(WorldObject.FilteredEvents), "Events (Filtered)", typeof(List<WorldEvent>)));
            }
            if (nonGenericSearchType.BaseType == typeof(EventCollection))
            {
                searchProperties.Add(new SearchProperty(nameof(EventCollection.AllEvents), "Events", typeof(List<WorldEvent>)));
                searchProperties.Add(new SearchProperty(nameof(EventCollection.FilteredEvents), "Events (Filtered)", typeof(List<WorldEvent>)));
            }

            if (nonGenericSearchType.BaseType == typeof(WorldEvent))
            {
                searchProperties.Add(new SearchProperty(nameof(WorldEvent.Year), typeof(int)));
            }

            foreach (SearchProperty property in searchProperties)
            {
                if (!noSubProperties)
                {
                    property.SetSubProperties();
                }
            }

            return searchProperties;
        }

        private static List<SearchProperty> getSearchPropertiesByType(Type type)
        {
            return type
                .GetProperties()
                .Where(pi => pi.GetCustomAttributes(typeof(AllowAdvancedSearchAttribute), false).FirstOrDefault() != null)
                .Select(getSearchPropertyByPropertyInfo)
                .ToList();
        }

        private static SearchProperty getSearchPropertyByPropertyInfo(PropertyInfo propertyInfo)
        {
            var allowAdvancedSearchAttribute = propertyInfo
                .GetCustomAttributes(typeof(AllowAdvancedSearchAttribute), false)
                .OfType<AllowAdvancedSearchAttribute>().FirstOrDefault();
            var name = propertyInfo.Name;
            var description = allowAdvancedSearchAttribute?.Description;
            var type = propertyInfo.PropertyType;
            var isSelectable = allowAdvancedSearchAttribute?.IsSelectable ?? false;
            return new SearchProperty(name, description ?? name, type, isSelectable);
        }

        public static List<QueryComparer> GetComparers(Type type)
        {
            List<QueryComparer> comparers = new List<QueryComparer>();
            if (type == null)
            {
                return comparers;
            }

            if (type == typeof(string))
            {
                comparers = new List<QueryComparer> { QueryComparer.Equals, QueryComparer.Contains, QueryComparer.StartsWith, QueryComparer.EndsWith, QueryComparer.NotEqual, QueryComparer.NotContains, QueryComparer.NotStartsWith, QueryComparer.NotEndsWith };
            }
            else if (type == typeof(int) || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) || type == typeof(double))
            {
                comparers = new List<QueryComparer> { QueryComparer.GreaterThan, QueryComparer.LessThan, QueryComparer.Equals };
            }
            else if (type == typeof(bool) || type.IsEnum)
            {
                comparers = new List<QueryComparer> { QueryComparer.Equals, QueryComparer.NotEqual };
            }

            return comparers;
        }

        public static string ComparerToString(QueryComparer comparer)
        {
            switch (comparer)
            {
                case QueryComparer.Contains: return "Contains";
                case QueryComparer.EndsWith: return "Ends With";
                case QueryComparer.Equals: return "Equals";
                case QueryComparer.NotEqual: return "Doesn't Equal";
                case QueryComparer.GreaterThan: return "Greater Than";
                case QueryComparer.Is: return "Is";
                case QueryComparer.LessThan: return "Less Than";
                case QueryComparer.StartsWith: return "Starts With";
                case QueryComparer.NotStartsWith: return "Doesn't Start With";
                case QueryComparer.NotEndsWith: return "Doesn't End With";
                case QueryComparer.NotContains: return "Doesn't Contain";
                default: return comparer.ToString();
            }
        }

        public static QueryComparer StringToComparer(string comparer)
        {
            switch (comparer)
            {
                case "Contains": return QueryComparer.Contains;
                case "Ends With": return QueryComparer.EndsWith;
                case "Equals": return QueryComparer.Equals;
                case "Greater Than": return QueryComparer.GreaterThan;
                case "Is": return QueryComparer.Is;
                case "Less Than": return QueryComparer.LessThan;
                case "Starts With": return QueryComparer.StartsWith;
                case "Doesn't Equal": return QueryComparer.NotEqual;
                default: return QueryComparer.All;
            }
        }
    }
}

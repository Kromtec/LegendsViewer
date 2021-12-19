using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LegendsViewer.Controls.Query
{
    public static class ExpressionExtensions
    {
        public static Expression AddNullCheck(this Expression expression)
        {
            if (expression is BinaryExpression binaryExpression && binaryExpression.Left is BinaryExpression leftExpression &&
                leftExpression.Left is MemberExpression binaryMemberExpression)
            {
                return CombineNullCheckWithCurrentExpression(expression, binaryMemberExpression);
            }

            if (expression is MethodCallExpression methodCallExpressionCurrent && methodCallExpressionCurrent.Object != null)
            {
                if (methodCallExpressionCurrent.Object is MethodCallExpression methodCallExpression && methodCallExpression.Object != null &&
                    methodCallExpression.Object is MemberExpression memberExpressionParent && memberExpressionParent.Expression != null)
                {
                    return CombineNullCheckWithCurrentExpression(expression, memberExpressionParent);
                }
                if (methodCallExpressionCurrent.Object is MemberExpression memberExpression && memberExpression.Expression != null)
                {
                    return CombineNullCheckWithCurrentExpression(expression, memberExpression);
                }
            }

            return expression is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression unaryMemberExpression && unaryMemberExpression.Expression != null
                ? CombineNullCheckWithCurrentExpression(expression, unaryMemberExpression)
                : expression;
        }

        private static Expression CombineNullCheckWithCurrentExpression(Expression expression, MemberExpression memberExpression)
        {
            var parentExpression = memberExpression.Expression;
            var parentExpressionNullCheck =
                Expression.NotEqual(parentExpression, Expression.Constant(null, parentExpression.Type));
            var addedParentNullCheck = Expression.AndAlso(parentExpressionNullCheck, expression);
            return addedParentNullCheck.AddNullCheck();
        }
    }

    public abstract class SearchInfo
    {
        public SearchInfo Previous, Next;
        public QueryOperator Operator { get; set; }
        public QueryComparer Comparer { get; set; }
        public object Value { get; set; }
        public string PropertyName { get; set; }
        public bool OrderByDescending;
        public abstract Type GetSearchType();
        public abstract Expression GetPredicateExpression();
        public abstract Expression GetTypeAs(Expression property);
        public abstract int GetCount(object item);
        public abstract Expression GetComparer(Expression property);
        public abstract List<object> Select(object baseObject);

        public static SearchInfo Make(Type type)
        {
            Type genericSearchInfo = typeof(SearchInfo<>);
            Type searchType = type.IsGenericType
                ? genericSearchInfo.MakeGenericType(type.GetGenericArguments()[0])
                : genericSearchInfo.MakeGenericType(type);
            return Activator.CreateInstance(searchType) as SearchInfo;
        }
        public void SetupComparers(QueryComparer comparer)
        {
            if (Next != null)
            {
                Comparer = GetSearchType().GetProperty(PropertyName).PropertyType.IsGenericType ? QueryComparer.Count : QueryComparer.Property;

                Next.SetupComparers(comparer);
            }
            else
            {
                if (comparer == QueryComparer.Equals || comparer == QueryComparer.NotEqual)
                {
                    if (PropertyName != "Value" && GetSearchType().GetProperty(PropertyName).PropertyType == typeof(string)
                      || PropertyName == "Value" && GetSearchType() == typeof(string))
                    {
                        if (comparer == QueryComparer.Equals)
                        {
                            comparer = QueryComparer.StringEquals;
                        }

                        if (comparer == QueryComparer.NotEqual)
                        {
                            comparer = QueryComparer.StringNotEqual;
                        }
                    }

                    if (PropertyName != "Value" && GetSearchType().GetProperty(PropertyName).PropertyType == typeof(bool))
                    {
                        comparer = QueryComparer.Is;
                    }

                }
                if (PropertyName != "Value" && GetSearchType().GetProperty(PropertyName).PropertyType.IsGenericType)
                {
                    switch (comparer)
                    {
                        case QueryComparer.Equals: comparer = QueryComparer.ListEquals; break;
                        case QueryComparer.GreaterThan: comparer = QueryComparer.ListGreaterThan; break;
                        case QueryComparer.LessThan: comparer = QueryComparer.ListLessThan; break;
                    }
                    Next = Make(GetSearchType().GetProperty(PropertyName).PropertyType);
                    Next.Operator = QueryOperator.Or;
                    Next.Previous = this;
                    Next.Comparer = QueryComparer.All;
                }

                Comparer = comparer;
            }
        }

        public void SetupOrderByComparers(QueryComparer comparer)
        {
            if (Next != null)
            {
                if (comparer == QueryComparer.Min || comparer == QueryComparer.Max || comparer == QueryComparer.Average || comparer == QueryComparer.Sum)
                {
                    Comparer = comparer;
                    Next.Comparer = Comparer;
                }

                if (!GetSearchType().GetProperty(PropertyName).PropertyType.IsGenericType)
                {
                    Comparer = QueryComparer.Property;
                }

                Next.SetupOrderByComparers(comparer);
            }
            else if (comparer == QueryComparer.All)
            {
                Comparer = QueryComparer.All;
                Next = Make(GetSearchType().GetProperty(PropertyName).PropertyType);
                Next.Operator = QueryOperator.Or;
                Next.Previous = Next;
                Next.Comparer = QueryComparer.All;
            }
            else
            {
                if (comparer == QueryComparer.Equals || comparer == QueryComparer.NotEqual)
                {
                    bool propertyIsString = PropertyName != "Value"
                        ? GetSearchType().GetProperty(PropertyName).PropertyType == typeof(string)
                        : Previous.GetSearchType().GetProperty(Previous.PropertyName).PropertyType.GetGenericArguments()[0] == typeof(string);
                    if (propertyIsString && comparer == QueryComparer.Equals)
                    {
                        comparer = QueryComparer.StringEquals;
                    }

                    if (propertyIsString && comparer == QueryComparer.NotEqual)
                    {
                        comparer = QueryComparer.StringNotEqual;
                    }
                }

                Comparer = comparer;
            }
        }

        public void SetupValue(object value)
        {
            if (Next != null)
            {
                Next.SetupValue(value);
            }
            else
            {
                if (Comparer == QueryComparer.All)
                {
                    Previous.Value = value;
                }

                if (Comparer == QueryComparer.Is && value.Equals(false))
                {
                    Comparer = QueryComparer.IsNot;
                }
                else
                {
                    Value = value;
                }
            }
        }

        public string PropertyString()
        {
            if (Next == null)
            {
                return PropertyName;
            }

            return Next.PropertyName == null ? PropertyName : PropertyName + "." + Next.PropertyString();
        }

        public bool ContainsListProperties()
        {
            if (GetSearchType().GetProperty(PropertyName).PropertyType.IsGenericType)
            {
                return true;
            }

            return Next != null && Next.ContainsListProperties();
        }

        public bool ContainsListPropertyLast()
        {
            var propertyInfo = GetSearchType().GetProperty(PropertyName);
            if (propertyInfo.PropertyType.IsGenericType && Next?.Comparer == QueryComparer.All)
            {
                return true;
            }

            if (propertyInfo.PropertyType.IsGenericType && Next != null)
            {
                return false;
            }

            if (propertyInfo.PropertyType.IsGenericType && Next == null)
            {
                return true;
            }

            return Next != null && Next.ContainsListPropertyLast();
        }
    }

    public class SearchInfo<T> : SearchInfo
    {
        public override Type GetSearchType()
        {
            return typeof(T);
        }

        public List<object> Search(List<T> dataset)
        {
            List<T> results = new List<T>();
            Expression<Func<T, bool>> predicate = GetPredicate();
            var compiled = predicate.Compile();
            results = dataset.Where(compiled).ToList();
            return results as List<object>;
        }

        public override Expression GetPredicateExpression()
        {
            return GetPredicate();
        }
        public Expression<Func<T, bool>> GetPredicate()
        {
            ParameterExpression e = Expression.Parameter(typeof(T));
            Expression<Func<T, bool>> predicate = t => false;

            Expression comparer;
            if (PropertyName == null || PropertyName == "Value")
            {
                comparer = GetComparer(e);
            }
            else
            {
                Expression property = Expression.Property(e, PropertyName);
                comparer = GetComparer(property);
            }

            LambdaExpression whereArg = Expression.Lambda(comparer, e);

            var exp = whereArg as Expression<Func<T, bool>>;

            predicate = Previous != null
                ? Operator == QueryOperator.And
                            ? predicate.And(exp)
                            : predicate.Or(exp)
                : exp;

            return predicate;
        }

        public Expression<Func<T, double>> GetValuePredicate()
        {
            ParameterExpression e = Expression.Parameter(typeof(T));
            Expression<Func<T, double>> predicate = t => 0;

            Expression comparer;
            if (PropertyName == null || PropertyName == "Value")
            {
                comparer = GetComparer(e);
            }
            else
            {
                Expression property = Expression.Property(e, PropertyName);
                comparer = GetComparer(property);
            }

            LambdaExpression valueArg = Expression.Lambda(comparer, e);
            return valueArg as Expression<Func<T, double>>;
        }

        public override Expression GetComparer(Expression property)
        {
            Expression comparer;
            MethodInfo methodInfo = null;
            switch (Comparer)
            {
                case QueryComparer.GreaterThan: comparer = Expression.GreaterThan(property, Expression.Constant(Value)); break;
                case QueryComparer.LessThan: comparer = Expression.LessThan(property, Expression.Constant(Value)); break;
                case QueryComparer.All: comparer = Expression.Constant(Equals(1, 1)); break;
                case QueryComparer.Is: comparer = Expression.IsTrue(property); break;
                case QueryComparer.IsNot: comparer = Expression.IsFalse(property); break;
                case QueryComparer.StartsWith:
                case QueryComparer.NotStartsWith:
                case QueryComparer.EndsWith:
                case QueryComparer.NotEndsWith:
                case QueryComparer.StringEquals:
                    methodInfo = GetMethodInfo();

                    comparer = Expression.Call(property, methodInfo, Expression.Constant(Value), Expression.Constant(StringComparison.CurrentCultureIgnoreCase));
                    if (Comparer == QueryComparer.NotEndsWith || Comparer == QueryComparer.NotStartsWith)
                    {
                        comparer = Expression.Not(comparer);
                    }
                    break;
                case QueryComparer.StringNotEqual:
                    methodInfo = GetMethodInfo();
                    comparer = Expression.Not(Expression.Call(property, methodInfo, Expression.Constant(Value), Expression.Constant(StringComparison.CurrentCultureIgnoreCase))); break;
                case QueryComparer.Contains:
                case QueryComparer.NotContains:
                    MethodInfo toLower = typeof(string).GetMethod("ToLower", new Type[] { });
                    property = Expression.Call(property, toLower);
                    methodInfo = GetMethodInfo();
                    comparer = Expression.Call(property, methodInfo, Expression.Constant((Value as string).ToLower()));
                    if (Comparer == QueryComparer.NotContains)
                    {
                        comparer = Expression.Not(comparer);
                    }

                    break;
                case QueryComparer.Equals:
                    _ = GetMethodInfo();
                    comparer = Expression.Equal(property, Expression.Constant(Value)); break;
                case QueryComparer.NotEqual:
                    _ = GetMethodInfo();
                    comparer = Expression.Not(Expression.Equal(property, Expression.Constant(Value))); break;
                case QueryComparer.Count:
                case QueryComparer.ListGreaterThan:
                case QueryComparer.ListLessThan:
                case QueryComparer.ListEquals:
                    methodInfo = GetMethodInfo();
                    methodInfo = methodInfo.MakeGenericMethod(Next.GetSearchType());
                    property = Next.GetTypeAs(property);
                    Expression listPredicate = Next.GetPredicateExpression();
                    comparer = Expression.Call(methodInfo, property, listPredicate);
                    switch (Comparer)
                    {
                        case QueryComparer.Count: comparer = Expression.GreaterThan(comparer, Expression.Constant(0)); break;
                        case QueryComparer.ListGreaterThan: comparer = Expression.GreaterThan(comparer, Expression.Constant(Convert.ToInt32(Value))); break;
                        case QueryComparer.ListLessThan: comparer = Expression.LessThan(comparer, Expression.Constant(Convert.ToInt32(Value))); break;
                        case QueryComparer.ListEquals: comparer = Expression.Equal(comparer, Expression.Constant(Convert.ToInt32(Value))); break;
                    }
                    break;
                case QueryComparer.Property:
                    comparer = Expression.Property(property, Next.PropertyName);
                    comparer = Next.GetComparer(comparer);
                    break;
                case QueryComparer.Min:
                    methodInfo = GetMethodInfo();
                    comparer = Expression.Call(property, methodInfo, property);
                    break;
                default:
                    throw new ArgumentException("No comparison supported for " + Comparer);
            }
            comparer = comparer.AddNullCheck();
            return comparer;
        }

        public override Expression GetTypeAs(Expression property)
        {
            return Expression.TypeAs(property, typeof(IEnumerable<T>));
        }

        private MethodInfo GetMethodInfo()
        {
            List<Type> types = new List<Type>();
            MethodInfo methodInfo = null;
            switch (Comparer)
            {
                case QueryComparer.Contains:
                case QueryComparer.NotContains:
                    types.Add(typeof(string));
                    methodInfo = typeof(string).GetMethod("Contains", types.ToArray()); break;
                case QueryComparer.EndsWith:
                case QueryComparer.NotEndsWith:
                    types.Add(typeof(string));
                    types.Add(typeof(StringComparison));
                    methodInfo = typeof(string).GetMethod("EndsWith", types.ToArray()); break;
                case QueryComparer.StartsWith:
                case QueryComparer.NotStartsWith:
                    types.Add(typeof(string));
                    types.Add(typeof(StringComparison));
                    methodInfo = typeof(string).GetMethod("StartsWith", types.ToArray()); break;
                case QueryComparer.Equals:
                case QueryComparer.NotEqual:
                    if (PropertyName == null || PropertyName == "Value")
                    {
                        types.Add(typeof(T));
                    }
                    else
                    {
                        types.Add(typeof(T).GetProperty(PropertyName).PropertyType);
                    }

                    methodInfo = types.First().GetMethod("Equals", types.ToArray()); break;
                case QueryComparer.StringEquals:
                case QueryComparer.StringNotEqual:
                    methodInfo = typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(StringComparison) }); break;
                case QueryComparer.Count:
                case QueryComparer.ListEquals:
                case QueryComparer.ListGreaterThan:
                case QueryComparer.ListLessThan:
                    types.Add(typeof(Func<T, bool>));
                    methodInfo = Next != null
                        ? typeof(Enumerable).GetMethods().Single(method => method.Name == "Count"
                            && method.IsStatic && method.GetParameters().Length == 2)
                        : typeof(Enumerable).GetMethods().Single(method => method.Name == "Count"
                            && method.IsStatic && method.GetParameters().Length == 1);

                    break;
                case QueryComparer.Min:
                    methodInfo = typeof(Enumerable).GetMethods().Single(method => method.Name == "Min" && method.IsStatic && method.GetParameters().Length == 2 && method.GetGenericArguments().Length == 2);
                    break;
            }

            return methodInfo == null ? throw new ArgumentException("No method info supported for " + Comparer) : methodInfo;
        }

        public override int GetCount(object item)
        {
            int count = 0;
            if (Previous == null)
            {
                List<T> itemList = new List<T> { (T)item };
                IEnumerator selectedList;
                selectedList = itemList.AsQueryable().Select(PropertyName).GetEnumerator();

                while (selectedList.MoveNext())
                {
                    count += Next.GetCount(selectedList.Current);
                }
            }
            else
            {
                List<T> itemList = item.GetType().IsGenericType ? item as List<T> : new List<T> { (T)item };
                count = itemList.Count(GetPredicate().Compile());
            }
            return count;
        }

        public override List<object> Select(object baseObject)
        {
            if (baseObject.GetType().IsGenericType)
            {
                List<T> baseList = baseObject.GetType().GetGenericArguments()[0] == typeof(object)
                    ? (baseObject as List<object>).OfType<T>().ToList()
                    : baseObject as List<T>;
                if (PropertyName == "Value")
                {
                    return baseList.Cast<object>().ToList();
                }

                baseList = baseList.Where(i => i != null).ToList();

                if (GetSearchType().GetProperty(PropertyName).PropertyType.IsGenericType)
                {
                    return Next == null || Next.Comparer == QueryComparer.All
                        ? baseList.AsQueryable().Select(PropertyName + ".Count").Cast<object>().ToList()
                        : Next.Select(baseList.AsQueryable().SelectMany(PropertyName).Cast<object>().ToList());
                }

                return Next == null
                    ? baseList.AsQueryable().Select(PropertyName).Cast<object>().ToList()
                    : Next.Select(baseList.AsQueryable().Select(PropertyName).Cast<object>().ToList());
            }
            PropertyInfo property = baseObject.GetType().GetProperty(PropertyName);
            return property.PropertyType.IsGenericType
                ? (Next?.Select(property.GetValue(baseObject, null)))
                : new List<object> { property.GetValue(baseObject, null) };
        }
    }
}

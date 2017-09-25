using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FaunaDB.Query;
using FaunaDB.Types;

namespace FaunaDB.Extensions
{
    public static class FaunaQueryParser
    {
        public static Expr[] ToExprArray<T>(this IEnumerable<T> collection) => collection.Select(a => a as Expr).ToArray();

        public static Expr Parse(Expr selector, Expression expr)
        {
            if (!(expr is MethodCallExpression)) return selector;
            var methodExpr = (MethodCallExpression) expr;

            var argsArr = methodExpr.Arguments;
            var next = argsArr[0];
            var rest = Parse(selector, next);
            var args = argsArr.Skip(1).Take(argsArr.Count - 1).ToArray();

            var method = methodExpr.Method;
            Expr current;

            switch (method.Name)
            {
                case "Select":
                    var arg = args[0];
                    var propInfo = GetPropertyInfo((dynamic) arg) as PropertyInfo;
                    var nameAttr = propInfo.GetCustomAttribute<FaunaFieldAttribute>();
                    var propName = nameAttr?.Name ?? $"data.{propInfo.Name}";
                    current = Language.Select(Language.Arr(propName.Split('.').ToExprArray()), rest);
                    break;
                case "Map":
                    current = Map(rest, (dynamic) args[0]);
                    break;
                case "Paginate":
                    var fromRef = ((ConstantExpression) args[0]).Value.ToString();
                    var sortDirection = (ListSortDirection)((ConstantExpression)args[1]).Value;
                    var size = (int) ((ConstantExpression) args[2]).Value;
                    var ts = (DateTime) ((ConstantExpression) args[3]).Value;
                    current = sortDirection == ListSortDirection.Ascending
                        ? Language.Paginate(rest, ts: Language.Date(ts.ToUnixTimeStamp()), after: fromRef, size: size)
                        : Language.Paginate(rest, ts: Language.Date(ts.ToUnixTimeStamp()), before: fromRef, size: size);
                    break;
                default:
                    throw new ArgumentException($"Unsupported method {method}.");
            }

            return current;
        }

        private static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var type = typeof(TSource);

            if (!(propertyLambda.Body is MemberExpression member))
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a method, not a property.");

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a field, not a property.");

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType ?? throw new InvalidOperationException()))
                throw new ArgumentException(
                    $"Expresion '{propertyLambda}' refers to a property that is not from type {type}.");

            return propInfo;
        }
    }
}
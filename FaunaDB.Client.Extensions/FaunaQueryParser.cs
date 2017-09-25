using System;
using System.Collections;
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
                    var arg = args[0] as Expression<Func<string, string>>;
                    if (arg.Body is MethodCallExpression exp)
                    {
                        if (exp.Method.Name == "Concat")
                        {
                            
                        }
                        var methodInfo = exp.Method;
                        var attribute = methodInfo.GetCustomAttribute<DbFunctionAttribute>();
                        if(attribute == null) throw new ArgumentException($"Unknown method {methodInfo.Name} in select statement.", nameof(expr));
                        var methodName = attribute.Name ?? methodInfo.Name;
                        var functionRef = Language.Ref(methodName); //Not sure this works, documentation doesn't list it as of yet
                        var arguments = new List<Expr>();
                        foreach (var argument in exp.Arguments)
                        {
                            switch (argument)
                            {
                                case ConstantExpression cexp:
                                    arguments.Add(cexp.ToFaunaObjOrPrimitive());
                                    continue;
                                case MemberExpression mexp:
                                    var type = arg.Type.GetGenericArguments()[0];
                                    var info = (PropertyInfo) mexp.Member;
                                    if (type != info.ReflectedType &&
                                        !type.IsSubclassOf(info.ReflectedType ?? throw new InvalidOperationException()))
                                        throw new ArgumentException(
                                            $"Expresion '{mexp}' refers to a property that is not from type {type}.");

                                    var nameAttr2 = info.GetCustomAttribute<FaunaFieldAttribute>();
                                    var attrName = typeof(IReferenceType).IsAssignableFrom(type)
                                        ? nameAttr2?.Name
                                        : $"data.{nameAttr2?.Name}";
                                    var memberName = attrName ?? $"data.{info.Name}";
                                    //TODO: Await further info about UDFs to finish this.
                                    continue;
                            }
                        }
                        current = Language.Call(functionRef, arguments.ToArray());
                        break;
                    }
                    var propInfo = GetPropertyInfo((dynamic) arg) as PropertyInfo;
                    var nameAttr = propInfo.GetCustomAttribute<FaunaFieldAttribute>();
                    var propName = nameAttr?.Name ?? $"data.{propInfo.Name}";
                    var refAttr = propInfo.GetCustomAttribute<ReferenceAttribute>();
                    if (refAttr == null)
                        current = Language.Select(Language.Arr(propName.Split('.').ToExprArray()), rest);
                    else
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(propInfo.PropertyType))
                            current = Language.Map(
                                Language.Select(Language.Arr(propName.Split('.').ToExprArray()), rest),
                                item => Language.Map(item, @ref => Language.Get(@ref)));
                        else
                            current = Language.Map(
                                Language.Select(Language.Arr(propName.Split('.').ToExprArray()), rest),
                                @ref => Language.Get(@ref));
                    }
                    break;
                case "Map":
                    current = Language.Map(rest, (dynamic) args[0]);
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
                case "GetAll":
                    current = Language.Map(rest, @ref => Language.Get(@ref));
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
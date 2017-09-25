using System;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace FaunaDB.Extensions
{
    public static class QueryableExtensions
    {
        internal static readonly MethodInfo PaginateMethodInfo =
            typeof(QueryableExtensions).GetTypeInfo().GetDeclaredMethod(nameof(Paginate));

        public static IFaunaQueryable<T> Paginate<T>(this IFaunaQueryable<T> source, string fromRef = null,
            ListSortDirection sortDirection = ListSortDirection.Ascending, int size = 16, DateTime? timeStamp = null) where T : IEnumerable
        {
            return new FaunaQueryableData<T>(source.Provider, Expression.Call(
                instance: null,
                method: PaginateMethodInfo.MakeGenericMethod(typeof(T)),
                arguments: new [] { source.Expression,
                Expression.Constant(fromRef),
                Expression.Constant(sortDirection),
                Expression.Constant(size),
                Expression.Constant(timeStamp) }
            ));
        }
    }
}
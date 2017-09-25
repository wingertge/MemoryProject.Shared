using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using FaunaDB.Client;
using FaunaDB.Query;
using static FaunaDB.Query.Language;

namespace FaunaDB.Extensions
{
    public static class FaunaClientExtensions
    {
        private static Expr[] ObjToArray(object obj)
        {
            return obj.GetType().Name.StartsWith("Tuple")
                ? obj.GetType().GetProperties().Select(a => a.GetValue(obj).ToFaunaObjOrPrimitive()).ToArray()
                : new[] { obj.ToFaunaObjOrPrimitive() };
        }

        public static IQueryable<T> Query<T>(this FaunaClient client, Expression<Func<T, bool>> index)
        {
            if (!(index.Body is BinaryExpression binary)) throw new ArgumentException("Index selector must be ==.");

            var constant = binary.Left is ConstantExpression constExp ? constExp : (ConstantExpression) binary.Right;
            var indexSelector = binary.Right is MemberExpression mExp ? mExp : (MemberExpression)binary.Left;
            var propInfo = indexSelector.GetPropertyInfo();
            var indexAttr = propInfo.GetCustomAttribute<IndexedAttribute>();
            if(indexAttr == null) throw new ArgumentException("Can't use unindexed property as selector!", nameof(index));
            var args = ObjToArray(constant.Value);
            var indexName = indexAttr.Name;

            return client.Query<T>(indexName, args);
        }

        public static IQueryable<T> Query<T>(this FaunaClient client, params Expression<Func<T, bool>>[] indexes)
        {
            var matchExprs = new List<Expr>();

            foreach (var index in indexes)
            {
                if (!(index.Body is BinaryExpression binary)) throw new ArgumentException("Index selector must be ==.");

                var constant = binary.Left is ConstantExpression constExp ? constExp : (ConstantExpression)binary.Right;
                var indexSelector = binary.Right is MemberExpression mExp ? mExp : (MemberExpression)binary.Left;
                var propInfo = indexSelector.GetPropertyInfo();
                var indexAttr = propInfo.GetCustomAttribute<IndexedAttribute>();
                if (indexAttr == null) throw new ArgumentException("Can't use unindexed property as selector!", nameof(index));
                var args = ObjToArray(constant.Value);
                var indexName = indexAttr.Name;
                matchExprs.Add(Match(Index(indexName), args));
            }

            return new FaunaQueryableData<T>(client, Map(Union(matchExprs.ToArray()), @ref => Language.Get(@ref)));
        }

        public static IQueryable<T> Query<T>(this FaunaClient client, Expression<Func<T, object>> index,
            params Expr[] args)
        {
            if(!(index.Body is MemberExpression member)) throw new ArgumentException("Index selector must be a member.");

            var propInfo = member.GetPropertyInfo();
            var indexAttr = propInfo.GetCustomAttribute<IndexedAttribute>();
            if (indexAttr == null) throw new ArgumentException("Can't use unindexed property as selector!", nameof(index));
            var indexName = indexAttr.Name;

            return client.Query<T>(indexName, args);
        }

        public static IQueryable<T> Query<T>(this FaunaClient client, string index, params Expr[] args)
        {
            return new FaunaQueryableData<T>(client, Map(Match(Index(index), args), @ref => Language.Get(@ref)));
        }

        public static IQueryable<T> Query<T>(this FaunaClient client, string @ref)
        {
            return new FaunaQueryableData<T>(client, Language.Get(Ref(@ref)));
        }

        public static async Task<T> Create<T>(this FaunaClient client, T obj)
        {
            var objType = obj.GetType();
            var className = string.Concat(objType.Name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
            var result = await client.Query(Language.Create(Ref(Class(className), 1), Obj("data", obj.ToFaunaObj())));
            return obj is IReferenceType ? result.To<T>().Value : result.To<FaunaResult<T>>().Value.Data;
        }

        public static Task<T> Update<T>(this FaunaClient client, T obj) where T : IReferenceType
        {
            return client.Update(obj, obj.Id);
        }

        public static async Task<T> Update<T>(this FaunaClient client, T obj, string id)
        {
            var result = await client.Query(Language.Update(Ref(id), obj.ToFaunaObj()));
            return obj is IReferenceType ? result.To<T>().Value : result.To<FaunaResult<T>>().Value.Data;
        }

        public static Task<T> Upsert<T>(this FaunaClient client, T obj) where T : IReferenceType
        {
            return client.Upsert<T>(obj, obj.Id);
        }

        public static async Task<T> Upsert<T>(this FaunaClient client, T obj, string id)
        {
            var objType = obj.GetType();
            var className = string.Concat(objType.Name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
            var result = await client.Query(If(Exists(id),
                Language.Update(id, obj.ToFaunaObj()),
                Language.Create(Ref(Class(className), 1), obj.ToFaunaObj())));
            return obj is IReferenceType ? result.To<T>().Value : result.To<FaunaResult<T>>().Value.Data;
        }

        public static Task Delete(this FaunaClient client, IReferenceType obj)
        {
            return client.Delete(obj.Id);
        }

        public static Task Delete(this FaunaClient client, string id)
        {
            return client.Query(Language.Delete(Ref(id)));
        }

        public static async Task<T> Get<T>(this FaunaClient client, string @ref)
        {
            var result = await client.Query(Language.Get(Ref(@ref)));
            return typeof(IReferenceType).IsAssignableFrom(typeof(T))
                ? result.To<T>().Value
                : result.To<FaunaResult<T>>().Value.Data;
        }
    }
}
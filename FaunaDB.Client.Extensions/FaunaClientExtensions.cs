using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FaunaDB.Client;
using FaunaDB.Query;
using static FaunaDB.Query.Language;

namespace FaunaDB.Extensions
{
    public static class FaunaClientExtensions
    {
        public static IQueryable<T> Query<T>(this FaunaClient client, string index, params Expr[] args)
        {
            return new FaunaQueryableData<T>(client, Map(Match(Index(index), args), @ref => Get(@ref)));
        }

        public static IQueryable<T> Query<T>(this FaunaClient client, string @ref)
        {
            return new FaunaQueryableData<T>(client, Get(Ref(@ref)));
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
    }
}
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
        public static IFaunaQueryable<T> Query<T>(this FaunaClient client, string index, params Expr[] args)
        {
            return new FaunaQueryableData<T>(client, Match(Index(index), args));
        }

        public static IFaunaQueryable<T> Query<T>(this FaunaClient client, string @ref)
        {
            return new FaunaQueryableData<T>(client, Get(Ref(@ref)));
        }

        public static Task Create(this FaunaClient client, object obj)
        {
            var objType = obj.GetType();
            var className = string.Concat(objType.Name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
            return client.Query(Language.Create(Ref(Class(className), 1), Obj("data", obj.ToFaunaObj())));
        }

        public static Task Update(this FaunaClient client, IReferenceType obj)
        {
            return client.Update(obj, obj.Id);
        }

        public static Task Update(this FaunaClient client, object obj, string id)
        {
            return client.Query(Language.Update(Ref(id), obj.ToFaunaObj()));
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
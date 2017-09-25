using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FaunaDB.Client;
using FaunaDB.Query;

namespace FaunaDB.Extensions
{
    public class FaunaQueryProvider : IQueryProvider
    {
        private readonly FaunaClient _client;
        private readonly Expr _selector;

        public FaunaQueryProvider(FaunaClient client, Expr selector)
        {
            _client = client;
            _selector = selector;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = expression.Type;
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(FaunaQueryableData<>).MakeGenericType(elementType), this, expression);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new FaunaQueryableData<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            throw new InvalidOperationException("Can't execute non generic Fauna query.");
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var result = _client.Query(FaunaQueryParser.Parse(_selector, expression)).Result;
            return result.To<TResult>().Value;
        }
    }
}
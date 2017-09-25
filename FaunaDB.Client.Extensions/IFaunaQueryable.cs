using System.Linq;

namespace FaunaDB.Extensions
{
    /// <summary>
    /// Custom IQueryable definition due to unique nature of FaunaDB Query language and capabilities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
    public interface IFaunaQueryable<T> : IQueryable<T>
    {
        
    }
}
using System;
using System.Deployment.Internal;
using FaunaDB.Query;

namespace FaunaDB.Extensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexedAttribute : Attribute
    {
        public Type TargetType { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Stubs used in query building
    /// </summary>
    /// <typeparam name="T1">The type of the first composite parameter.</typeparam>
    /// <typeparam name="T2">The type of the second composite parameter.</typeparam>
    public class CompositeIndex<T1, T2> : IEquatable<Tuple<T1, T2>>
    {
        public bool Equals(Tuple<T1, T2> other)
        {
            throw new NotImplementedException();
        }
    }

    public class CompositeIndex<T1, T2, T3> : IEquatable<Tuple<T1, T2, T3>>
    {
        public bool Equals(Tuple<T1, T2, T3> other)
        {
            throw new NotImplementedException();
        }
    }

    public class CompositeIndex<T1, T2, T3, T4> : IEquatable<Tuple<T1, T2, T3, T4>>
    {
        public bool Equals(Tuple<T1, T2, T3, T4> other)
        {
            throw new NotImplementedException();
        }
    }

    public class CompositeIndex<T1, T2, T3, T4, T5> : IEquatable<Tuple<T1, T2, T3, T4, T5>>
    {
        public bool Equals(Tuple<T1, T2, T3, T4, T5> other)
        {
            throw new NotImplementedException();
        }
    }

    public class CompositeIndex<T1, T2, T3, T4, T5, T6> : IEquatable<Tuple<T1, T2, T3, T4, T5, T6>>
    {
        public bool Equals(Tuple<T1, T2, T3, T4, T5, T6> other)
        {
            throw new NotImplementedException();
        }
    }
}
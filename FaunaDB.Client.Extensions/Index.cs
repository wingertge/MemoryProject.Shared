﻿using System;
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
    public class CompositeIndex<T1, T2> : Tuple<T1, T2>
    {
        public CompositeIndex(T1 item1, T2 item2) : base(item1, item2) {}
    }

    public class CompositeIndex<T1, T2, T3> : Tuple<T1, T2, T3>
    {
        public CompositeIndex(T1 item1, T2 item2, T3 item3) : base(item1, item2, item3) { }
    }

    public class CompositeIndex<T1, T2, T3, T4> : Tuple<T1, T2, T3, T4>
    {
        public CompositeIndex(T1 item1, T2 item2, T3 item3, T4 item4) : base(item1, item2, item3, item4) { }
    }

    public class CompositeIndex<T1, T2, T3, T4, T5> : Tuple<T1, T2, T3, T4, T5>
    {
        public CompositeIndex(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) : base(item1, item2, item3, item4, item5) { }
    }

    public class CompositeIndex<T1, T2, T3, T4, T5, T6> : Tuple<T1, T2, T3, T4, T5, T6>
    {
        public CompositeIndex(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) : base(item1, item2, item3, item4, item5, item6) { }
    }
}
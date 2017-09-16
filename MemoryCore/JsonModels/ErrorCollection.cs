using System.Collections.Generic;

namespace MemoryCore.JsonModels
{
    /// <summary>
    /// Semantic type for an error dictionary. Contents should be "field": "error"
    /// </summary>
    public class ErrorCollection : Dictionary<string, string>
    {
    }
}
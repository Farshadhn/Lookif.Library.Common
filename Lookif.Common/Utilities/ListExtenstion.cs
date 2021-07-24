using System.Collections.Generic;

namespace Lookif.Library.Common.Utilities
{
    public static class ListExtenstion
    {
        public static List<T> GetClone<T>(this List<T> source)
        {
            return source.GetRange(0, source.Count);
        }
    }
}
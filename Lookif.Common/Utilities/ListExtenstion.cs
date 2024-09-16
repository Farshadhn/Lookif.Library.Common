using System.Collections.Generic;
using System.Linq;

namespace Lookif.Library.Common.Utilities;

public static class ListExtenstion
{
    public static List<T> GetClone<T>(this List<T> source)
    {
        return source.GetRange(0, source.Count);
    }
    public static IEnumerable<T> NeverNull<T>(this IEnumerable<T> value)
    {
        return value ?? Enumerable.Empty<T>();
    }
    public static IEnumerable<T> NeverNullConcat<T>(this IEnumerable<T> first, IEnumerable<T> second)
    {
        first ??= Enumerable.Empty<T>();
        second ??= Enumerable.Empty<T>();
        return first.Concat(second);
    }
}
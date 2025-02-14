using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lookif.Library.Common.Utilities;
public static class IQueryableExtension
{
    public static async Task<ImmutableArray<TSource>> ToImmutableArrayAsync<TSource>(
   [NotNull] this IQueryable<TSource> source,
   CancellationToken cancellationToken = default)
    {
        var builder = ImmutableArray.CreateBuilder<TSource>();
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
            builder.Add(element);

        return builder.ToImmutable();
    }
}

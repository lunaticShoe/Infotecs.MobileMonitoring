using System.Collections.Generic;
using System.Linq;

namespace Infotecs.MobileMonitoring.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
    {
        return list is null || !list.Any();
    }
}

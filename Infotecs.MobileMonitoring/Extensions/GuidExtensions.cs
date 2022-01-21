using System;

namespace Infotecs.MobileMonitoring.Extensions;

public static class GuidExtensions
{
    public static bool IsNullOrEmpty(this Guid? guid)
        => guid is null || guid == Guid.Empty;
}

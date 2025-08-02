using Microsoft.EntityFrameworkCore;
using Zorro.Data.Interfaces;

namespace Zorro.Extensions;

public static class CoreExtensions
{
    public static string ToReabableValue(this Enums.Environment environment)
    {
        return ZorroDI.ENVIRONMENT_VALUES[(int)environment];
    }

    public static TValue GetValueByEnvironment<TValue>(
        this Enums.Environment environment,
        IDictionary<string, TValue> mapping
    )
    {
        return mapping[environment.ToReabableValue()];
    }

    public static IQueryable<TModel> NoTrackingInclude<TModel>(
        this IQueryable<TModel> set,
        string[] inclusion
    )
        where TModel : class, IEntity
    {
        IQueryable<TModel>? queryableSet = set
            .IgnoreAutoIncludes()
            .AsNoTracking();

        foreach (var include in inclusion)
        {
            queryableSet = queryableSet.Include(include);
        }

        return queryableSet.AsSplitQuery();
    }

    public static int GetContentBasedHashCode<TKey, TValue>(
        this IDictionary<TKey, TValue>? dict,
        int nullHashCode = 0
    )
        where TKey : notnull
    {
        if (dict is null)
            return nullHashCode;

        unchecked
        {
            int hash = 17;
            foreach (var pair in dict.OrderBy(p => p.Key))
            {
                hash *= 23 + (pair.Key?.GetHashCode() ?? 0);
                hash *= 23 + (pair.Value?.GetHashCode() ?? 0);
            }
            return hash;
        }
    }
}
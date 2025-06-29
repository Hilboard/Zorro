namespace Zorro.Query.Essentials;

public static class ForEachQuery
{
    public static (QueryContext, IEnumerable<TExit>) ForEach<TItem, TEntry, TExit>(
        this (QueryContext context, TEntry) carriage,
        TItem[] items,
        Func<TItem, (QueryContext, TItem), (QueryContext, TExit)> expression
    )
    {
        if (items.Length == 0)
        {
            return (carriage.context, Array.Empty<TExit>());
        }

        IList<TExit> results = new List<TExit>();
        foreach (var item in items)
        {
            results.Add(expression.Invoke(item, (carriage.context, item)).Item2);
        }

        return (carriage.context, results);
    }
}
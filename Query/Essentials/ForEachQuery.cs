namespace Zorro.Query.Essentials;

public static class ForEachQuery
{
    public static ArgHttpQueryContext<IEnumerable<TReturn>> ForEach<TItem, TReturn>(
        this HttpQueryContext context,
        IEnumerable<TItem> items,
        Func<TItem, ArgHttpQueryContext<TItem>, ArgHttpQueryContext<TReturn>> expression
    )
    {
        if (items.Count() == 0)
        {
            return context.PassArg(Enumerable.Empty<TReturn>());
        }

        IEnumerable<TReturn> results = Enumerable.Empty<TReturn>();
        foreach(TItem item in items)
        {
            results = results.Append(expression(item, context.PassArg(item)).arg);
        }

        context.TryLogElapsedTime(nameof(ForEachQuery));
        return context.PassArg(results);
    }

    public static HttpQueryContext ForEach<TItem>(
        this HttpQueryContext context,
        IEnumerable<TItem> items,
        Func<TItem, ArgHttpQueryContext<TItem>, HttpQueryContext> expression
    )
    {
        if (items.Count() == 0)
        {
            return context;
        }

        foreach (TItem item in items)
        {
            expression(item, context.PassArg(item));
        }

        context.TryLogElapsedTime(nameof(ForEachQuery));
        return context;
    }
}
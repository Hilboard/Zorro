namespace Zorro.Query.Essentials;

public static class ForEachQuery
{
    public static ArgQueryContext<IEnumerable<TReturn>> ForEach<TItem, TReturn>(
        this QueryContext context,
        IEnumerable<TItem> items,
        Func<TItem, ArgQueryContext<TItem>, ArgQueryContext<TReturn>> expression
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

        return context.PassArg(results);
    }

    public static QueryContext ForEach<TItem>(
        this QueryContext context,
        IEnumerable<TItem> items,
        Func<TItem, ArgQueryContext<TItem>, QueryContext> expression
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

        return context;
    }
}
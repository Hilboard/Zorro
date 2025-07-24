namespace Zorro.Query.Essentials;

public static class IfQuery
{
    public static QueryContext If<TEntry>(
        this ArgQueryContext<TEntry> context,
        bool condition,
        Action<ArgQueryContext<TEntry>> expression
    )
    {
        if (condition)
        {
            expression.Invoke(context);
        }
        return context;
    }

    public static QueryContext If<TEntry>(
        this ArgQueryContext<TEntry> context,
        Func<TEntry?, bool> predicate,
        Action<ArgQueryContext<TEntry>> expression
    )
    {
        if (predicate(context.arg))
        {
            expression.Invoke(context);
        }
        return context;
    }
}
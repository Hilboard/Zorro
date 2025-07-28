namespace Zorro.Query.Essentials;

public static class IfQuery
{
    public static QueryContext If(
        this QueryContext context,
        bool condition,
        Action<QueryContext> expression
    )
    {
        if (condition)
        {
            expression.Invoke(context);
        }

        context.TryLogElapsedTime(nameof(IfQuery));

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

        context.TryLogElapsedTime(nameof(IfQuery));

        return context;
    }
}
namespace Zorro.Query.Essentials;

public static class IfQuery
{
    public static HttpQueryContext If(
        this HttpQueryContext context,
        bool condition,
        Action<HttpQueryContext> expression
    )
    {
        if (condition)
        {
            expression.Invoke(context);
        }

        context.TryLogElapsedTime(nameof(IfQuery));

        return context;
    }

    public static HttpQueryContext If<TEntry>(
        this ArgHttpQueryContext<TEntry> context,
        Func<TEntry?, bool> predicate,
        Action<ArgHttpQueryContext<TEntry>> expression
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
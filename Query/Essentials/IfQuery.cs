namespace Zorro.Query.Essentials;

public static class IfQuery
{
    public static (QueryContext, TEntry?) If<TEntry>(
        this (QueryContext context, TEntry? value) carriage,
        bool condition,
        Func<(QueryContext, TEntry?), (QueryContext, object? value)> expression
    )
    {
        if (condition)
        {
            expression.Invoke(carriage);
        }
        return (carriage.context, carriage.value);
    }

    public static (QueryContext, TEntry?) If<TEntry>(
        this (QueryContext context, TEntry? value) carriage,
        Func<TEntry?, bool> predicate,
        Func<(QueryContext, TEntry?), (QueryContext, object? value)> expression
    )
    {
        if (predicate(carriage.value))
        {
            expression.Invoke(carriage);
        }
        return (carriage.context, carriage.value);
    }
}
namespace Zorro.Query.Essentials;

public static class ExecuteQuery
{
    public static (QueryContext, TReturn) Execute<TReturn, TEntry>(
        this (QueryContext context, TEntry entry) carriage,
        Func<TEntry, TReturn> function
    )
    {
        return (carriage.context, function(carriage.entry));
    }

    public static (QueryContext, TReturn) Execute<TReturn>(
        this ANY_TUPLE carriage,
        Func<TReturn> function
    )
    {
        return (carriage.context, function());
    }

    public static ANY_TUPLE Execute(
        this ANY_TUPLE carriage,
        Action function
    )
    {
        function.Invoke();
        return (carriage.context, null);
    }
}
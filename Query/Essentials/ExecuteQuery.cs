namespace Zorro.Query.Essentials;

public static class ExecuteQuery
{
    public static ArgQueryContext<TReturn> Execute<TEntry, TReturn>(
        this ArgQueryContext<TEntry> context,
        Func<TEntry, TReturn> function
    )
    {
        return context.PassArg(function(context.arg));
    }

    public static ArgQueryContext<TReturn> Execute<TReturn>(
        this QueryContext context,
        Func<TReturn> function
    )
    {
        return context.PassArg(function());
    }

    public static QueryContext Execute(
        this QueryContext context,
        Action function
    )
    {
        function();
        return context;
    }
}
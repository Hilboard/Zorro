namespace Zorro.Query.Essentials;

public static class ExecuteQuery
{
    public static ArgHttpQueryContext<TReturn> Execute<TEntry, TReturn>(
        this ArgHttpQueryContext<TEntry> context,
        Func<TEntry, TReturn> function
    )
    {
        TReturn returnValue = function(context.arg);
        context.TryLogElapsedTime(nameof(ExecuteQuery));
        return context.PassArg(returnValue);
    }

    public static ArgHttpQueryContext<TReturn> Execute<TReturn>(
        this HttpQueryContext context,
        Func<TReturn> function
    )
    {
        TReturn returnValue = function();
        context.TryLogElapsedTime(nameof(ExecuteQuery));
        return context.PassArg(returnValue);
    }

    public static HttpQueryContext Execute(
        this HttpQueryContext context,
        Action function
    )
    {
        function();
        context.TryLogElapsedTime(nameof(ExecuteQuery));
        return context;
    }
}
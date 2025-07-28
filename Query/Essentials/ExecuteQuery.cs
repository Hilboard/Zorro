namespace Zorro.Query.Essentials;

public static class ExecuteQuery
{
    public static ArgQueryContext<TReturn> Execute<TEntry, TReturn>(
        this ArgQueryContext<TEntry> context,
        Func<TEntry, TReturn> function
    )
    {
        TReturn returnValue = function(context.arg);
        context.TryLogElapsedTime(nameof(ExecuteQuery));
        return context.PassArg(returnValue);
    }

    public static ArgQueryContext<TReturn> Execute<TReturn>(
        this QueryContext context,
        Func<TReturn> function
    )
    {
        TReturn returnValue = function();
        context.TryLogElapsedTime(nameof(ExecuteQuery));
        return context.PassArg(returnValue);
    }

    public static QueryContext Execute(
        this QueryContext context,
        Action function
    )
    {
        function();
        context.TryLogElapsedTime(nameof(ExecuteQuery));
        return context;
    }
}
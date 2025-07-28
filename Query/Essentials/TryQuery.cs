namespace Zorro.Query.Essentials;

public static class TryQuery
{
    public static ArgQueryContext<TReturn> Try<TEntry, TReturn>(
        this ArgQueryContext<TEntry> context,
        Func<ArgQueryContext<TEntry>, ArgQueryContext<TReturn>> expression
    )
        where TReturn : notnull
    {
        TReturn returnValue; 

        try
        {
            returnValue = expression.Invoke(context).arg;
        }
        catch 
        {
            returnValue = default!;
        }

        context.TryLogElapsedTime(nameof(TryQuery));

        return context.PassArg(returnValue);
    }

    public static QueryContext Try<TEntry>(
        this ArgQueryContext<TEntry> context,
        Func<QueryContext, QueryContext> expression
    )
    {
        try
        {
            expression.Invoke(context);
        }
        catch { }

        context.TryLogElapsedTime(nameof(TryQuery));

        return context;
    }
}
namespace Zorro.Query.Essentials;

public static class TryQuery
{
    public static ArgQueryContext<TReturn> Try<TEntry, TReturn>(
        this ArgQueryContext<TEntry> context,
        Func<ArgQueryContext<TEntry>, ArgQueryContext<TReturn>> expression
    )
        where TReturn : notnull
    {
        try
        {
            return expression.Invoke(context);
        }
        catch 
        {
            return context.PassArg(default(TReturn)!);
        }
    }

    public static QueryContext Try<TEntry>(
        this ArgQueryContext<TEntry> context,
        Func<QueryContext, QueryContext> expression
    )
    {
        try
        {
            return expression.Invoke(context);
        }
        catch
        {
            return context;
        }
    }
}
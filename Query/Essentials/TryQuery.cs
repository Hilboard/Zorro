namespace Zorro.Query.Essentials;

public static class TryQuery
{
    public static ArgHttpQueryContext<TReturn> Try<TEntry, TReturn>(
        this ArgHttpQueryContext<TEntry> context,
        Func<ArgHttpQueryContext<TEntry>, ArgHttpQueryContext<TReturn>> expression
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

    public static HttpQueryContext Try<TEntry>(
        this ArgHttpQueryContext<TEntry> context,
        Func<HttpQueryContext, HttpQueryContext> expression
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
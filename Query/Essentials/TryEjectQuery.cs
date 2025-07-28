namespace Zorro.Query.Essentials;

public static class TryEjectQuery
{
    public static ArgQueryContext<TReturn> TryEject<TEntry, TReturn>(
        this ArgQueryContext<TEntry> context,
        Func<ArgQueryContext<TEntry>, ArgQueryContext<TReturn>> expression, out TReturn value
    )
        where TReturn : notnull
    {
        try
        {
            value = expression(context).arg;
        }
        catch
        {
            value = default!;
        }

        context.TryLogElapsedTime(nameof(TryEjectQuery));

        return context.PassArg(value);
    }

    public static ArgQueryContext<TReturn> TryEject<TReturn>(
        this QueryContext context,
        Func<QueryContext, ArgQueryContext<TReturn>> expression, out TReturn value
    )
        where TReturn : notnull
    {
        try
        {
            value = expression(context).arg;
        }
        catch
        {
            value = default!;
        }

        context.TryLogElapsedTime(nameof(TryEjectQuery));

        return context.PassArg(value);
    }
}
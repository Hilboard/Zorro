namespace Zorro.Query.Essentials;

public static class TryEjectQuery
{
    public static ArgQueryContext<TEject> TryEject<TEject>(
        this QueryContext context,
        Func<QueryContext, ArgQueryContext<TEject>> convertor, out TEject value
    )
        where TEject : notnull
    {
        try
        {
            value = convertor.Invoke(context).arg;
        }
        catch
        {
            value = default!;
        }

        context.TryLogElapsedTime(nameof(TryEjectQuery));
        return context.PassArg(value);
    }

    public static ArgQueryContext<TEject> TryEject<TEject>(
        this QueryContext context,
        TEject newValue, out TEject value
    )
        where TEject : notnull
    {
        try
        {
            value = newValue;
        }
        catch
        {
            value = default!;
        }

        context.TryLogElapsedTime(nameof(TryEjectQuery));
        return context.PassArg(value);
    }

    public static ArgQueryContext<TEject> TryEject<TEject>(
        this ArgQueryContext<TEject> context,
        out TEject value
    )
        where TEject : notnull
    {
        try
        {
            value = context.arg;
        }
        catch
        {
            value = default!;
        }

        context.TryLogElapsedTime(nameof(TryEjectQuery));
        return context.PassArg(value);
    }
}
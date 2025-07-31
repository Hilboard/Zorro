namespace Zorro.Query.Essentials;

public static class TryEjectQuery
{
    public static ArgHttpQueryContext<TEject> TryEject<TEject>(
        this HttpQueryContext context,
        Func<HttpQueryContext, ArgHttpQueryContext<TEject>> convertor, out TEject value
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

    public static ArgHttpQueryContext<TEject> TryEject<TEject>(
        this HttpQueryContext context,
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

    public static ArgHttpQueryContext<TEject> TryEject<TEject>(
        this ArgHttpQueryContext<TEject> context,
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
namespace Zorro.Query.Essentials;

public static class EjectQuery
{
    public static ArgHttpQueryContext<TEject> Eject<TEject>(
        this HttpQueryContext context,
        Func<HttpQueryContext, ArgHttpQueryContext<TEject>> convertor, out TEject value
    )
        where TEject : notnull
    {
        value = convertor.Invoke(context).arg;
        context.TryLogElapsedTime(nameof(EjectQuery));
        return context.PassArg(value);
    }

    public static ArgHttpQueryContext<TEject> Eject<TEject>(
        this HttpQueryContext context,
        TEject newValue, out TEject value
    )
        where TEject : notnull
    {
        value = newValue;
        context.TryLogElapsedTime(nameof(EjectQuery));
        return context.PassArg(value);
    }

    public static ArgHttpQueryContext<TEject> Eject<TEject>(
        this ArgHttpQueryContext<TEject> context,
        out TEject value
    )
        where TEject : notnull
    {
        value = context.arg;
        context.TryLogElapsedTime(nameof(EjectQuery));
        return context.PassArg(value);
    }
}
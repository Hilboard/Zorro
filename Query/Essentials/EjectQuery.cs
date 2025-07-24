namespace Zorro.Query.Essentials;

public static class EjectQuery
{
    public static ArgQueryContext<TEject> Eject<TEject>(
        this QueryContext context,
        Func<QueryContext, ArgQueryContext<TEject>> convertor, out TEject value
    )
    {
        value = convertor.Invoke(context).arg;
        return context.PassArg(value);
    }

    public static ArgQueryContext<TEject> Eject<TEject>(
        this QueryContext context,
        TEject newValue, out TEject value
    )
    {
        value = newValue;
        return context.PassArg(value);
    }

    public static ArgQueryContext<TEject> Eject<TEject>(
        this ArgQueryContext<TEject> context,
        out TEject value
    )
    {
        value = context.arg;
        return context.PassArg(value);
    }
}
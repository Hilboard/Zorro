namespace Zorro.Query.Essentials;

public static class EjectQuery
{
    public static (QueryContext, TEject) Eject<TEntry, TEject>(
        this (QueryContext context, TEntry?) carriage,
        Func<(QueryContext, TEntry?), (QueryContext, TEject value)> expression, out TEject value
    )
    {
        value = expression.Invoke(carriage).value;
        return (carriage.context, value);
    }

    public static (QueryContext, TEject) Eject<TEntry, TEject>(
        this (QueryContext context, TEntry) carriage,
        TEject newValue, out TEject value
    )
    {
        value = newValue;
        return (carriage.context, value);
    }

    public static (QueryContext, TEject) Eject<TEject>(
        this (QueryContext context, TEject value) carriage,
        out TEject value
    )
    {
        value = carriage.value;
        return (carriage.context, value);
    }
}
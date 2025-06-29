namespace Zorro.Query.Essentials;

public static class TryEjectQuery
{
    public static (QueryContext, TExit?) TryEject<TEntry, TExit>(
        this (QueryContext context, TEntry?) carriage,
        Func<(QueryContext, TEntry?), (QueryContext, TExit? value)> expression, out TExit? value
    )
    {
        try
        {
            var exitCarriage = expression(carriage);
            value = exitCarriage.value;
            return exitCarriage;
        }
        catch
        {
            value = default;
            return (carriage.context, value);
        }
    }
}
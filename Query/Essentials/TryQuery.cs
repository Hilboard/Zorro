namespace Zorro.Query.Essentials;

public static class TryQuery
{
    public static (QueryContext, TExit?) Try<TEntry, TExit>(
        this (QueryContext context, TEntry?) carriage,
        Func<(QueryContext, TEntry?), (QueryContext, TExit?)> expression
    )
    {
        try
        {
            return expression.Invoke(carriage);
        }
        catch 
        {
            return (carriage.context, default(TExit));
        }
    }
}
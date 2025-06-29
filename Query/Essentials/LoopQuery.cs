namespace Zorro.Query.Essentials;

public static class LoopQuery
{
    public static (QueryContext, IEnumerable<TExit>) Loop<TEntry, TExit>(
        this (QueryContext context, TEntry) carriage,
        int loopCount,
        Func<int, (QueryContext, int), (QueryContext, TExit)> expression
    )
    {
        IList<TExit> results = new List<TExit>();
        int i = 0;
        for (; i < loopCount; i++)
        {
            results.Add(expression.Invoke(i, (carriage.context, i)).Item2);
        }

        return (carriage.context, results);
    }
}
namespace Zorro.Query.Essentials;

public static class LoopQuery
{
    public static ArgQueryContext<IEnumerable<TReturn>> Loop<TReturn>(
        this QueryContext context,
        int loopCount,
        Func<int, QueryContext, ArgQueryContext<TReturn>> expression
    )
    {
        IEnumerable<TReturn> results = Enumerable.Empty<TReturn>();
        int i = 0;
        for (; i < loopCount; i++)
        {
            results = results.Append(expression.Invoke(i, context).arg);
        }

        return context.PassArg(results);
    }
}
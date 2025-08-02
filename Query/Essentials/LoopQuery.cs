namespace Zorro.Query.Essentials;

public static class LoopQuery
{
    public static ArgHttpQueryContext<IEnumerable<TReturn>> Loop<TReturn>(
        this HttpQueryContext context,
        int loopCount,
        Func<int, HttpQueryContext, ArgHttpQueryContext<TReturn>> expression
    )
    {
        IEnumerable<TReturn> results = Enumerable.Empty<TReturn>();
        int i = 0;
        for (; i < loopCount; i++)
        {
            results = results.Append(expression.Invoke(i, context).arg);
        }

        context.TryLogElapsedTime(nameof(LoopQuery));
        return context.PassArg(results);
    }

    public static HttpQueryContext Loop(
        this HttpQueryContext context,
        int loopCount,
        Func<int, HttpQueryContext, HttpQueryContext> expression
    )
    {
        int i = 0;
        for (; i < loopCount; i++)
        {
            expression.Invoke(i, context);
        }

        context.TryLogElapsedTime(nameof(LoopQuery));
        return context;
    }
}
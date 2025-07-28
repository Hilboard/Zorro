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

        context.TryLogElapsedTime(nameof(LoopQuery));

        return context.PassArg(results);
    }

    public static QueryContext Loop(
        this QueryContext context,
        int loopCount,
        Func<int, QueryContext, QueryContext> expression
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
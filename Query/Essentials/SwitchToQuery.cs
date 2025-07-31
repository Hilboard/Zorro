namespace Zorro.Query.Essentials;

public static class SwitchToQuery
{
    public static ArgHttpQueryContext<TEntry> SwitchTo<TEntry>(this HttpQueryContext context, TEntry focusObject)
    {
        context.TryLogElapsedTime(nameof(SwitchToQuery));
        return context.PassArg(focusObject);
    }
}
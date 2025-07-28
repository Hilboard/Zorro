namespace Zorro.Query.Essentials;

public static class SwitchToQuery
{
    public static ArgQueryContext<TEntry> SwitchTo<TEntry>(this QueryContext context, TEntry focusObject)
    {
        context.TryLogElapsedTime(nameof(SwitchToQuery));
        return context.PassArg(focusObject);
    }
}
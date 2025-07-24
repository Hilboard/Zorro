namespace Zorro.Query.Essentials;

public static class SwitchToQuery
{
    public static ArgQueryContext<TEntry> SwitchTo<TEntry>(this QueryContext context, TEntry focusObject)
    {
        return context.PassArg(focusObject);
    }
}
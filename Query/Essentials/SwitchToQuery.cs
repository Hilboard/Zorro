namespace Zorro.Query.Essentials;

public static class SwitchToQuery
{
    public static (QueryContext, TEntry) SwitchTo<TEntry>(this ANY_TUPLE carriage, TEntry focusObject)
    {
        return (carriage.context, focusObject);
    }
}
namespace Zorro.Query.Essentials;

public static class SetInclusionValueQuery
{
    public static QueryContext SetInclusion(this QueryContext context, string variableName, bool? value = true)
    {
        context.inclusion.TryAdd(variableName, value);

        context.TryLogElapsedTime(nameof(SetInclusionValueQuery));

        return context;
    }
}
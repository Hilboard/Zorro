namespace Zorro.Query.Essentials;

public static class SetInclusionValueQuery
{
    public static HttpQueryContext SetInclusion(this HttpQueryContext context, string variableName, bool? value = true)
    {
        context.inclusion.TryAdd(variableName, value);

        context.TryLogElapsedTime(nameof(SetInclusionValueQuery));

        return context;
    }
}
namespace Zorro.Query.Essentials;

public static class SetInclusionValueQuery
{
    public static QueryContext SetInclusion(this QueryContext context, string variableName, bool? value = true)
    {
        if (context.inclusion.ContainsKey(variableName) is false)
        {
            context.inclusion.Add(variableName, value);
        }
        else
        {
            context.inclusion[variableName] = value;
        }

        return context;
    }
}
namespace Zorro.Query.Essentials;

public static class SetInclusionValueQuery
{
    public static ANY_TUPLE SetInclusion(this ANY_TUPLE carriage, string variableName, bool? value = true)
    {
        if (carriage.context.inclusionContext.ContainsKey(variableName) is false)
        {
            carriage.context.inclusionContext.Add(variableName, value);
        }
        else
        {
            carriage.context.inclusionContext[variableName] = value;
        }

        return carriage;
    }
}
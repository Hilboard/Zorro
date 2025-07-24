using Zorro.Middlewares;

namespace Zorro.Query.Essentials;

public static class ThrowQuery
{
    public static QueryContext Throw<TEntry>(this QueryContext context, QueryException exception)
    {
        throw exception;
    }
}
using Zorro.Middlewares;

namespace Zorro.Query.Essentials;

public static class ThrowQuery
{
    public static QueryContext Throw(this QueryContext context, QueryException exception)
    {
        context.TryLogElapsedTime(nameof(ThrowQuery));
        throw exception;
    }
}
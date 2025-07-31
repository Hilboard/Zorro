using Zorro.Middlewares;

namespace Zorro.Query.Essentials;

public static class ThrowQuery
{
    public static HttpQueryContext Throw(this HttpQueryContext context, QueryException exception)
    {
        context.TryLogElapsedTime(nameof(ThrowQuery));
        throw exception;
    }
}
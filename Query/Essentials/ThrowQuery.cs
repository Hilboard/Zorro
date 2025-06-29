using Zorro.Middlewares;

namespace Zorro.Query.Essentials;

public static class ThrowQuery
{
    public static ANY_TUPLE Throw<TEntry>(this (QueryContext, TEntry?) carrige, QueryException exception)
    {
        throw exception;
    }
}
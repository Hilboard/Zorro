using Microsoft.AspNetCore.Mvc;

namespace Zorro.Query;

public static class EndAndReturnQuery
{
    public static IActionResult EndAndReturn(this ANY_TUPLE input)
    {
        if (input.lastValue is not null)
            return new OkObjectResult(input.lastValue);
        else
            return new OkResult();
    }

    public static IActionResult EndAndReturn<TEntry>(
        this (QueryContext context, TEntry entry) input,
        Func<TEntry, object?> returnValueBuilder
    )
    {
        return new OkObjectResult(returnValueBuilder(input.entry));
    }

    public static IActionResult EndAndReturn(this ANY_TUPLE input, object? returnValue)
    {
        return new OkObjectResult(returnValue);
    }
}

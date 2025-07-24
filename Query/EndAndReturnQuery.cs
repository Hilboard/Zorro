using Microsoft.AspNetCore.Mvc;

namespace Zorro.Query;

public static class EndAndReturnQuery
{
    public static IActionResult EndAndReturn<TEntry>(
        this ArgQueryContext<TEntry> context
    )
    {
        if (context.arg is not null)
            return new OkObjectResult(context.arg);
        else
            return new OkResult();
    }

    public static IActionResult EndAndReturn<TEntry>(
        this ArgQueryContext<TEntry> context,
        Func<TEntry, object?> convertor
    )
    {
        if (context.arg is not null)
            return new OkObjectResult(convertor(context.arg));
        else
            return new OkResult();
    }

    public static IActionResult EndAndReturn(this QueryContext input, object? returnValue)
    {
        return new OkObjectResult(returnValue);
    }

    public static IActionResult EndAndReturn(this QueryContext input)
    {
        return new OkResult();
    }
}
